using System;
#if netstandard1_3
using System.Linq;
#endif
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Prometheus.Client
{
    internal static class TupleHelper<TTuple>
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
        where TTuple : struct, IEquatable<TTuple>
#endif
    {
        private static readonly int _size;
        private static readonly Func<IReadOnlyList<string>, TTuple> _parser;
        private static readonly Func<TTuple, IReadOnlyList<string>> _formatter;

        static TupleHelper()
        {
            _size = TupleHelper.GetSize<TTuple>();
            _formatter = TupleHelper.GenerateFormatter<TTuple>();
            _parser = TupleHelper.GenerateParser<TTuple>();
        }

        public static IReadOnlyList<string> ToArray(TTuple values)
        {
            return _formatter(values);
        }

        public static TTuple FromArray(IReadOnlyList<string> values)
        {
            if (values == null || values.Count == 0)
            {
                return default;
            }

            if (values.Count != _size)
            {
                throw new ArgumentException(nameof(values));
            }

            return _parser(values);
        }
    }

    internal static class TupleHelper
    {
        public static Type MakeValueTupleType(int len)
        {
            switch (len)
            {
                case 0:
                    return typeof(ValueTuple);
                case 1:
                    return typeof(ValueTuple<string>);
                case 2:
                    return typeof(ValueTuple<string, string>);
                case 3:
                    return typeof(ValueTuple<string, string, string>);
                case 4:
                    return typeof(ValueTuple<string, string, string, string>);
                case 5:
                    return typeof(ValueTuple<string, string, string, string, string>);
                case 6:
                    return typeof(ValueTuple<string, string, string, string, string, string>);
                case 7:
                    return typeof(ValueTuple<string, string, string, string, string, string, string>);
                default:
                    var genericType = typeof(ValueTuple<,,,,,,,>);
                    var typeArgs = new Type[8];
                    typeArgs.Fill(typeof(string), 0, 7);
                    typeArgs[7] = MakeValueTupleType(len - 7);
                    return genericType.MakeGenericType(typeArgs);
            }
        }

        public static int GetSize<TTuple>()
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
        where TTuple : struct, IEquatable<TTuple>
#endif
        {
#if HasITuple
            return default(TTuple).Length;
#else
            int GetTupleSize(Type tupleType)
            {
                var typeParams = tupleType.GenericTypeArguments;
                if (typeParams.Length == 8)
                {
                    return 7 + GetTupleSize(typeParams[7]);
                }

                return typeParams.Length;
            }

            return GetTupleSize(typeof(TTuple));
#endif
        }

        public static Func<TTuple, IReadOnlyList<string>> GenerateFormatter<TTuple>()
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
        where TTuple : struct, IEquatable<TTuple>
#endif
        {
            var size = GetSize<TTuple>();
            var valuesArg = Expression.Parameter(typeof(TTuple));
            var resultArray = Expression.Variable(typeof(string[]), "result");

            var methodBody = new List<Expression>
            {
                Expression.Assign(
                            resultArray,
                            Expression.NewArrayBounds(typeof(string), Expression.Constant(size)))
            };

            Expression getterTarget = valuesArg;
            var itemNumber = 0;
            for (var i = 1; i <= size; i++)
            {
                itemNumber++;
                methodBody.Add(
                    Expression.Assign(
                        Expression.ArrayAccess(resultArray, Expression.Constant(i - 1)),
                        Expression.PropertyOrField(getterTarget, $"Item{itemNumber}")
                    ));

                if (i % 7 == 0)
                {
                    itemNumber = 0;
                    getterTarget = Expression.PropertyOrField(getterTarget, "Rest");
                }
            }

            methodBody.Add(resultArray);

            return Expression.Lambda<Func<TTuple, IReadOnlyList<string>>>(
                Expression.Block(new[] { resultArray }, methodBody), valuesArg).Compile();
        }

        public static Func<IReadOnlyList<string>, TTuple> GenerateParser<TTuple>()
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
        where TTuple : struct, IEquatable<TTuple>
#endif
        {
            Expression BuildUpTuple(Type tupleType, ParameterExpression source, int offset)
            {
                if (tupleType == typeof(ValueTuple))
                    return Expression.New(tupleType);

                var tupleSize = tupleType.GenericTypeArguments.Length;
                var args = new Expression[tupleSize];

                for (var i = 0; i < tupleSize; i++)
                {
                    if (i == 7) // it's TRest
                        args[i] = BuildUpTuple(tupleType.GenericTypeArguments[i], source, offset + 7);
                    else
                        args[i] = Expression.Property(source, "Item", Expression.Constant(offset + i));
                }

                return Expression.New(GetCtor(tupleType, tupleType.GenericTypeArguments), args);
            }

            var valuesArg = Expression.Parameter(typeof(IReadOnlyList<string>));
            var resultType = typeof(TTuple);
            var tupleCreateExpr = BuildUpTuple(resultType, valuesArg, 0);

            return Expression.Lambda<Func<IReadOnlyList<string>, TTuple>>(tupleCreateExpr, valuesArg).Compile();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Fill<T>(this IList<T> target, T value, int offset, int size)
        {
            for (var i = offset; i < size; i++)
            {
                target[i] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ConstructorInfo GetCtor(Type tupleType, Type[] parameters)
        {
#if netstandard1_3
            return tupleType.GetTypeInfo().DeclaredConstructors.Single();
#else
            return tupleType.GetConstructor(parameters);
#endif
        }
    }
}
