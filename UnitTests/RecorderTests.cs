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

}
