using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Prometheus.Client.Tests;

public class ValueObserverExtensionsTests
{
    [Fact]
    public void ObserveDuration_Action_ObservesCalled()
    {
        var observer = Substitute.For<IValueObserver>();
        var called = false;

        observer.ObserveDuration(() => called = true);

        Assert.True(called);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Action_MethodException_StillObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        Assert.Throws<InvalidOperationException>(() =>
            observer.ObserveDuration(() => throw new InvalidOperationException("boom")));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Func_ReturnsResultAndObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        var result = observer.ObserveDuration(() => 42);

        Assert.Equal(42, result);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Func_MethodException_StillObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        Assert.Throws<InvalidOperationException>(() =>
            observer.ObserveDuration<int>(() => throw new InvalidOperationException("boom")));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_ObservesCalled()
    {
        var observer = Substitute.For<IValueObserver>();
        var called = false;

        await observer.ObserveDurationAsync(() =>
        {
            called = true;
            return Task.CompletedTask;
        });

        Assert.True(called);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_MethodException_StillObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            observer.ObserveDurationAsync(() => Task.FromException(new InvalidOperationException("boom"))));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Func_ReturnsResultAndObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        var result = await observer.ObserveDurationAsync(() => Task.FromResult(42));

        Assert.Equal(42, result);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Func_MethodException_StillObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            observer.ObserveDurationAsync(() => Task.FromException<int>(new InvalidOperationException("boom"))));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Action_WithHandler_ObservesOnSuccess()
    {
        var observer = Substitute.For<IValueObserver>();
        var called = false;

        observer.ObserveDuration(() => called = true, _ => { });

        Assert.True(called);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Func_WithHandler_ReturnsResultAndObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        var result = observer.ObserveDuration(() => 42, _ => { });

        Assert.Equal(42, result);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_WithHandler_ObservesOnSuccess()
    {
        var observer = Substitute.For<IValueObserver>();
        var called = false;

        await observer.ObserveDurationAsync(() =>
        {
            called = true;
            return Task.CompletedTask;
        }, _ => { });

        Assert.True(called);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Func_WithHandler_ReturnsResultAndObserves()
    {
        var observer = Substitute.For<IValueObserver>();

        var result = await observer.ObserveDurationAsync(() => Task.FromResult(42), _ => { });

        Assert.Equal(42, result);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Action_NullHandler_ObservesDirectly()
    {
        var observer = Substitute.For<IValueObserver>();

        observer.ObserveDuration(() => { }, null);

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Action_NullHandler_ObserveException_Throws()
    {
        var observer = Substitute.For<IValueObserver>();
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(new InvalidOperationException("observe"));

        Assert.Throws<InvalidOperationException>(() =>
            observer.ObserveDuration(() => { }, null));
    }

    [Fact]
    public void ObserveDuration_Func_NullHandler_ObservesDirectly()
    {
        var observer = Substitute.For<IValueObserver>();

        var result = observer.ObserveDuration(() => 42, null);

        Assert.Equal(42, result);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_NullHandler_ObservesDirectly()
    {
        var observer = Substitute.For<IValueObserver>();

        await observer.ObserveDurationAsync(() => Task.CompletedTask, null);

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Func_NullHandler_ObservesDirectly()
    {
        var observer = Substitute.For<IValueObserver>();

        var result = await observer.ObserveDurationAsync(() => Task.FromResult(42), null);

        Assert.Equal(42, result);
        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Action_MethodException_StillObservesWithHandler()
    {
        var observer = Substitute.For<IValueObserver>();

        Assert.Throws<InvalidOperationException>(() =>
            observer.ObserveDuration(() => throw new InvalidOperationException("method"), _ => { }));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Action_ObserveException_PassedToHandler()
    {
        var observer = Substitute.For<IValueObserver>();
        var observeEx = new InvalidOperationException("observe");
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(observeEx);

        Exception handledException = null;
        observer.ObserveDuration(() => { }, ex => handledException = ex);

        Assert.Same(observeEx, handledException);
    }

    [Fact]
    public void ObserveDuration_Func_MethodException_StillObservesWithHandler()
    {
        var observer = Substitute.For<IValueObserver>();

        Assert.Throws<InvalidOperationException>(() =>
            observer.ObserveDuration<int>(() => throw new InvalidOperationException("method"), _ => { }));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public void ObserveDuration_Func_ObserveException_PassedToHandler_ResultReturned()
    {
        var observer = Substitute.For<IValueObserver>();
        var observeEx = new InvalidOperationException("observe");
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(observeEx);

        Exception handledException = null;
        var result = observer.ObserveDuration(() => 42, ex => handledException = ex);

        Assert.Equal(42, result);
        Assert.Same(observeEx, handledException);
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_MethodException_StillObservesWithHandler()
    {
        var observer = Substitute.For<IValueObserver>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            observer.ObserveDurationAsync(
                () => Task.FromException(new InvalidOperationException("method")),
                _ => { }));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_ObserveException_PassedToHandler()
    {
        var observer = Substitute.For<IValueObserver>();
        var observeEx = new InvalidOperationException("observe");
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(observeEx);

        Exception handledException = null;
        await observer.ObserveDurationAsync(() => Task.CompletedTask, ex => handledException = ex);

        Assert.Same(observeEx, handledException);
    }

    [Fact]
    public async Task ObserveDurationAsync_Func_MethodException_StillObservesWithHandler()
    {
        var observer = Substitute.For<IValueObserver>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            observer.ObserveDurationAsync(
                () => Task.FromException<int>(new InvalidOperationException("method")),
                _ => { }));

        observer.Received(1).Observe(Arg.Is<double>(d => d >= 0));
    }

    [Fact]
    public async Task ObserveDurationAsync_Func_ObserveException_PassedToHandler_ResultReturned()
    {
        var observer = Substitute.For<IValueObserver>();
        var observeEx = new InvalidOperationException("observe");
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(observeEx);

        Exception handledException = null;
        var result = await observer.ObserveDurationAsync(() => Task.FromResult(42), ex => handledException = ex);

        Assert.Equal(42, result);
        Assert.Same(observeEx, handledException);
    }

    [Fact]
    public void ObserveDuration_Action_BothThrow_MethodExceptionPropagates_HandlerCalled()
    {
        var observer = Substitute.For<IValueObserver>();
        var observeEx = new InvalidOperationException("observe");
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(observeEx);

        Exception handledException = null;
        Assert.Throws<InvalidOperationException>(() =>
            observer.ObserveDuration(() => throw new InvalidOperationException("method"), ex => handledException = ex));

        Assert.Same(observeEx, handledException);
    }

    [Fact]
    public async Task ObserveDurationAsync_Action_BothThrow_MethodExceptionPropagates_HandlerCalled()
    {
        var observer = Substitute.For<IValueObserver>();
        var observeEx = new InvalidOperationException("observe");
        observer.When(o => o.Observe(Arg.Any<double>())).Throw(observeEx);

        Exception handledException = null;
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            observer.ObserveDurationAsync(
                () => Task.FromException(new InvalidOperationException("method")),
                ex => handledException = ex));

        Assert.Same(observeEx, handledException);
    }
}

