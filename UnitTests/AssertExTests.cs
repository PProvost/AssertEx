using System.Threading.Tasks;
using AssertExLib;
using AssertExLib.Exceptions;
using System;
using Xunit;
#if NET4
using TaskEx = System.Threading.Tasks.TaskEx;
#else
using TaskEx = System.Threading.Tasks.Task;
#endif

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
            var task = Task.Factory.StartNew(() => { throw new FooException(); });
            AssertEx.TaskThrows<FooException>(task);
        }

        [Fact]
        public void WhenTaskReturningResultThrows_DetectsInnerException()
        {
            var task = Task.Factory.StartNew<int>(() => { throw new FooException(); });
            AssertEx.TaskThrows<FooException>(task);
        }

        [Fact]
        public void WhenTaskReturningResultThrowsDifferent_WillItselfThrow()
        {
            var task = Task.Factory.StartNew<int>(() => { throw new FooException(); });
            Assert.Throws<AssertExException>(() => AssertEx.TaskThrows<BarException>(task));
        }

        [Fact]
        public void WhenTaskReturningResult_WillItselfThrow()
        {
            var task = TaskEx.FromResult(1);
            Assert.Throws<AssertExException>(() => AssertEx.TaskThrows<BarException>(task));
        }

        [Fact]
        public void WhenTaskThrowsDifferentException_IsIgnored()
        {
            var task = Task.Factory.StartNew<int>(() => { throw new BarException(); });
            AssertEx.TaskDoesNotThrow<FooException>(task);
        }
    }

    public class TaskDoesNotThrow
    {
        [Fact]
        public void WhenTaskReturnsResult_IsSuccessful()
        {
            var task = TaskEx.FromResult(1);
            AssertEx.TaskDoesNotThrow(task);
        }

        [Fact]
        public void ForGenericParameter_WhenTaskReturnsResult_IsSuccessful()
        {
            var task = TaskEx.FromResult(1);
            AssertEx.TaskDoesNotThrow<FooException>(task);
        }

        [Fact]
        public void ForAnyException_WillItselfThrow()
        {
            var task = Task.Factory.StartNew<int>(() => { throw new BarException(); });
            Assert.Throws<AssertExException>(() => AssertEx.TaskDoesNotThrow(task));
        }

        [Fact]
        public void ForMatchingException_WillItselfThrow()
        {
            var task = Task.Factory.StartNew<int>(() => { throw new FooException(); });
            Assert.Throws<AssertExException>(() => AssertEx.TaskDoesNotThrow<FooException>(task));
        }

        [Fact]
        public void WhereExceptionIsDifferent_IsSuccessful()
        {
            var task = Task.Factory.StartNew<int>(() => { throw new BarException(); });
            AssertEx.TaskDoesNotThrow<FooException>(task);
        }

        [Fact]
        public void UsingGenericType_WhenTaskReturnsResult_IsSuccessful()
        {
            var task = TaskEx.FromResult(1);
            AssertEx.TaskDoesNotThrow<FooException>(task);
        }
    }
}