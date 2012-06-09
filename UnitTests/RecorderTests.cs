using System.Threading.Tasks;
using AssertExLib;
using AssertExLib.Internal;
using System;
using Xunit;

public class RecorderTests
{
    class FooException : Exception { }

    [Fact]
    public void RecorderCollectsExceptionThrownByThrowsDelegate()
    {
        ThrowsDelegate codeDelegate = () => { throw new FooException(); };
        var result = Recorder.Exception(codeDelegate);
        Assert.IsAssignableFrom<FooException>(result);
    }

    [Fact]
    public void RecorderReturnsNullWhenThrowsDelegateDoesNotThrow()
    {
        ThrowsDelegate codeDelegate = () => { /* noop */ };
        var result = Recorder.Exception(codeDelegate);
        Assert.Null(result);
    }

    [Fact]
    public void RecorderCollectsExceptionThrownByThrowsDelegateWithReturn()
    {
        ThrowsDelegateWithReturn codeDelegate = () => { throw new FooException(); };
        var result = Recorder.Exception(codeDelegate);
        Assert.IsAssignableFrom<FooException>(result);
    }

    [Fact]
    public void RecorderReturnsNullWhenThrowsDelegateWithReturnDoesNotThrow()
    {
        ThrowsDelegateWithReturn codeDelegate = () => { return null; };
        var result = Recorder.Exception(codeDelegate);
        Assert.Null(result);
    }

    [Fact]
    public void RecorderCollectsExceptionThrownByTaskThrowsDelegate()
    {
        var task = Task.Factory.StartNew(() => { throw new FooException(); });
        TaskThrowsDelegate codeDelegate = () => task;
        var result = Recorder.Exception(codeDelegate);
        Assert.IsAssignableFrom<AggregateException>(result);
    }

    [Fact]
    public void RecorderReturnsNullWhenTaskDoesNotThrow()
    {
        var task = Task.FromResult(1);
        TaskThrowsDelegate codeDelegate = () => task;
        var result = Recorder.Exception(codeDelegate);
        Assert.Null(result);
    }
}
