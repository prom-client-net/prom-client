using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Prometheus.Client
{
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

        public static int GetHashCode<TTuple>(TTuple values)
#if HasITuple
            where TTuple : struct, ITuple, IEquatable<TTuple>
#else
            where TTuple : struct, IEquatable<TTuple>
#endif
        {
            return TupleHelperTyped<TTuple>.GetTupleHashCode(values);
        }

        public static int GetHashCode(IReadOnlyList<string> values)
        {
            var result = 0;
            foreach (var value in values)
            {
                if(value == null)
                    throw new ArgumentException("Label value cannot be empty");

                result = HashCode.Combine(result, value.GetHashCode());
            }

            return result;
        }

        public static IReadOnlyList<string> ToArray<TTuple>(TTuple values)
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
            where TTuple : struct, IEquatable<TTuple>
#endif
        {
            return TupleHelperTyped<TTuple>.ToArray(values);
        }

        public static TTuple FromArray<TTuple>(IReadOnlyList<string> values)
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
            where TTuple : struct, IEquatable<TTuple>
#endif
        {
            return TupleHelperTyped<TTuple>.FromArray(values);
        }

        internal static Func<TTuple, TAggregate, Func<string, int, TAggregate, TAggregate>, TAggregate> MakeReducer<TTuple, TAggregate>()
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
        where TTuple : struct, IEquatable<TTuple>
#endif
        {
            var size = GetSize<TTuple>();
            var values = Expression.Parameter(typeof(TTuple), "values");
            var result = Expression.Parameter(typeof(TAggregate), "aggregated");
            var aggregator = Expression.Parameter(typeof(Func<string, int, TAggregate, TAggregate>), "fn");

            var methodBody = new List<Expression>();

            Expression getterTarget = values;
            var itemNumber = 0;
            for (var i = 1; i <= size; i++)
            {
                itemNumber++;
                methodBody.Add(
                    Expression.Assign(
                        result,
                        Expression.Invoke(
                                aggregator,
                                Expression.PropertyOrField(getterTarget, $"Item{itemNumber}"),
                                Expression.Constant(i - 1),
                                result)));

                if (i % 7 == 0)
                {
                    itemNumber = 0;
                    getterTarget = Expression.PropertyOrField(getterTarget, "Rest");
                }
            }

            methodBody.Add(result);

            return Expression.Lambda<Func<TTuple, TAggregate, Func<string, int, TAggregate, TAggregate>, TAggregate>>(
                Expression.Block(new ParameterExpression[0], methodBody), values, result, aggregator).Compile();
        }

        internal static Func<IReadOnlyList<string>, TTuple> GenerateParser<TTuple>()
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

                return Expression.New(tupleType.GetConstructor(tupleType.GenericTypeArguments), args);
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

        private static class TupleHelperTyped<TTuple>
#if HasITuple
        where TTuple : struct, ITuple, IEquatable<TTuple>
#else
            where TTuple : struct, IEquatable<TTuple>
#endif
        {
            private static readonly int _size;
            private static readonly Func<IReadOnlyList<string>, TTuple> _parser;
            private static readonly Func<TTuple, string[], Func<string, int, string[], string[]>, string[]> FormatReducer;
            private static readonly Func<TTuple, int, Func<string, int, int, int>, int> HashCodeReducer;

            static TupleHelperTyped()
            {
                _size = TupleHelper.GetSize<TTuple>();
                FormatReducer = TupleHelper.MakeReducer<TTuple, string[]>();
                HashCodeReducer = TupleHelper.MakeReducer<TTuple, int>();
                _parser = TupleHelper.GenerateParser<TTuple>();
            }

            public static IReadOnlyList<string> ToArray(TTuple values)
            {
                return FormatReducer(values, new string[_size], (item, index, aggregated) =>
                {
                    aggregated[index] = item;
                    return aggregated;
                });
            }

            public static int GetTupleHashCode(TTuple values)
            {
                return HashCodeReducer(values, 0, (item, _, aggregated) =>
                {
                    if(item == null)
                        throw new ArgumentException("Label value cannot be empty");
                    return HashCode.Combine(aggregated, item.GetHashCode());
                });
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
    }
}
