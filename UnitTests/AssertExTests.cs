using System.Threading.Tasks;
using AssertExLib;
using AssertExLib.Exceptions;
using System;
using Xunit;

public class AssertExTests
{
    class FooException : Exception { }
    class BarException : Exception { }

    public class DoesNotThrow
    {
        [Fact]
        public void WhenDelegateDoesNotThrow_DoesNotThrow()
        {
            ThrowsDelegate codeDelegate = () => { /* noop */ };
            Assert.DoesNotThrow(() => AssertEx.DoesNotThrow(codeDelegate));
        }

        [Fact]
        public void WhenDelegateThrows_ThrowsCustomException()
        {
            ThrowsDelegate codeDelegate = () => { throw new FooException(); };
            Assert.Throws<AssertExException>(() => AssertEx.DoesNotThrow(codeDelegate));
        }

        [Fact]
        public void DoesNotFailWhenThrowsDelegateWithReturnDoesNotThrow()
        {
            ThrowsDelegateWithReturn codeDelegate = () => { return 1; };
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

    public class TaskThrows
    {
        [Fact]
        public void WhenTaskThrows_DetectsInnerException()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew(() => { throw new FooException(); });
            AssertEx.TaskThrows<FooException>(codeDelegate);
        }

        [Fact]
        public void WhenTaskReturningResultThrows_DetectsInnerException()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew<int>(() => { throw new FooException(); });
            AssertEx.TaskThrows<FooException>(codeDelegate);
        }

        [Fact]
        public void WhenTaskReturningResultThrowsDifferent_WillItselfThrow()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew<int>(() => { throw new FooException(); });
            Assert.Throws<AssertExException>(() => AssertEx.TaskThrows<BarException>(codeDelegate));
        }

        [Fact]
        public void WhenTaskReturningResult_WillItselfThrow()
        {
            TaskThrowsDelegate codeDelegate = () => Task.FromResult(1);
            Assert.Throws<AssertExException>(() => AssertEx.TaskThrows<BarException>(codeDelegate));
        }

        [Fact]
        public void WhenTaskThrowsDifferentException_IsIgnored()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew<int>(() => { throw new BarException(); });
            AssertEx.TaskDoesNotThrow<FooException>(codeDelegate);
        }
    }

    public class TaskDoesNotThrow
    {
        [Fact]
        public void WhenTaskReturnsResult_IsSuccessful()
        {
            TaskThrowsDelegate codeDelegate = () => Task.FromResult(1);
            AssertEx.TaskDoesNotThrow(codeDelegate);
        }

        [Fact]
        public void ForGenericParameter_WhenTaskReturnsResult_IsSuccessful()
        {
            TaskThrowsDelegate codeDelegate = () => Task.FromResult(1);
            AssertEx.TaskDoesNotThrow<FooException>(codeDelegate);
        }

        [Fact]
        public void ForAnyException_WillItselfThrow()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew<int>(() => { throw new BarException(); });
            Assert.Throws<AssertExException>(() => AssertEx.TaskDoesNotThrow(codeDelegate));
        }

        [Fact]
        public void ForMatchingException_WillItselfThrow()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew<int>(() => { throw new FooException(); });
            Assert.Throws<AssertExException>(() => AssertEx.TaskDoesNotThrow<FooException>(codeDelegate));
        }

        [Fact]
        public void WhereExceptionIsDifferent_IsSuccessful()
        {
            TaskThrowsDelegate codeDelegate = () => Task.Factory.StartNew<int>(() => { throw new BarException(); });
            AssertEx.TaskDoesNotThrow<FooException>(codeDelegate);
        }

        [Fact]
        public void UsingGenericType_WhenTaskReturnsResult_IsSuccessful()
        {
            TaskThrowsDelegate codeDelegate = () => Task.FromResult(1);
            AssertEx.TaskDoesNotThrow<FooException>(codeDelegate);
        }
    }
}