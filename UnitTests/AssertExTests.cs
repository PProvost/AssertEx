using AssertExLib;
using AssertExLib.Exceptions;
using System;
using Xunit;

public class AssertExTests
{
    class FooException : Exception { }

    public class DoesNotThrow
    {
        [Fact]
        public void DoesNotFailWhenThrowsDelegateDoesNotThrow()
        {
            ThrowsDelegate codeDelegate = () => { /* noop */ };
            Assert.DoesNotThrow(() => AssertEx.DoesNotThrow(codeDelegate));
        }

        [Fact]
        public void DoesNotFailWhenThrowsDelegateWithReturnDoesNotThrow()
        {
            ThrowsDelegateWithReturn codeDelegate = () => { return null; };
            Assert.DoesNotThrow(() => AssertEx.DoesNotThrow(codeDelegate));
        }

        [Fact]
        public void ThrowsAssertExExceptionWhenThrowsDelegateThrows()
        {
            ThrowsDelegate codeDelegate = () => { throw new FooException(); };
            Assert.Throws<AssertExException>(() => AssertEx.DoesNotThrow(codeDelegate));
        }
    }

    public class Throws
    {
        [Fact]
        public void GenericVersionThrowsAssertExExceptionWhenThrowsDelegateDoesNotThrow()
        {
            ThrowsDelegate codeDelegate = () => { /* noop */ };
            Assert.Throws<AssertExException>(() => AssertEx.Throws<FooException>(codeDelegate));
        }

        [Fact]
        public void GenericVersionDoesNotThrowWhenThrowsDelegateThrows()
        {
            ThrowsDelegate codeDelegate = () => { throw new FooException(); };
            Assert.DoesNotThrow(() => AssertEx.Throws<FooException>(codeDelegate));
        }

        [Fact]
        public void GenericVersionThrowsAssertExExceptionWhenThrowsDelegateWithReturnDoesNotThrow()
        {
            ThrowsDelegateWithReturn codeDelegate = () => { return null; };
            Assert.Throws<AssertExException>(() => AssertEx.Throws<FooException>(codeDelegate));
        }

        [Fact]
        public void GenericVersionDoesNotThrowWhenThrowsDelegateWithReturnThrows()
        {
            ThrowsDelegateWithReturn codeDelegate = () => { throw new FooException(); };
            Assert.DoesNotThrow(() => AssertEx.Throws<FooException>(codeDelegate));
        }

        [Fact]
        public void TypedVersionThrowsAssertExExceptionWhenThrowsDelegateDoesNotThrow()
        {
            ThrowsDelegate codeDelegate = () => { /* noop */ };
            Assert.Throws<AssertExException>(() => AssertEx.Throws(typeof(FooException), codeDelegate));
        }

        [Fact]
        public void TypedVersionDoesNotThrowWhenThrowsDelegateThrows()
        {
            ThrowsDelegate codeDelegate = () => { throw new FooException(); };
            Assert.DoesNotThrow(() => AssertEx.Throws(typeof(FooException), codeDelegate));
        }

    }
}
