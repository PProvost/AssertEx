using System;
using AssertExLib.Internal;
using AssertExLib.Exceptions;

namespace AssertExLib
{
    public delegate void ThrowsDelegate();
    public delegate object ThrowsDelegateWithReturn();

    public static class AssertEx
    {
        public static void DoesNotThrow(ThrowsDelegate testCode)
        {
            var ex = Recorder.Exception(testCode);
            if (ex != null)
                throw new AssertExException("DoesNotThrow failed.", ex);
        }

        public static void DoesNotThrow(ThrowsDelegateWithReturn testCode)
        {
            var ex = Recorder.Exception(testCode);
            if (ex != null)
                throw new AssertExException("DoesNotThrow failed.", ex);
        }

        public static T Throws<T>(ThrowsDelegate testCode) where T : Exception
        {
            return (T)Throws(typeof(T), testCode);
        }

        public static T Throws<T>(ThrowsDelegateWithReturn testCode) where T : Exception
        {
            return (T)Throws(typeof(T), testCode);
        }

        public static Exception Throws(Type exceptionType, ThrowsDelegate testCode)
        {
            var exception = Recorder.Exception(testCode);

            if (exception == null)
                throw new AssertExException("AssertExtensions.Throws failed. No exception occurred.");

            if (!exceptionType.Equals(exception.GetType()))
                throw new AssertExException(String.Format("AssertExtensions.Throws failed. Incorrect exception {0} occurred.", exception.GetType().Name), exception);

            return exception;
        }

        public static Exception Throws(Type exceptionType, ThrowsDelegateWithReturn testCode)
        {
            var exception = Recorder.Exception(testCode);

            if (exception == null)
                throw new AssertExException("AssertExtensions.Throws failed. No exception occurred.");

            if (!exceptionType.Equals(exception.GetType()))
                throw new AssertExException(String.Format("AssertExtensions.Throws failed. Incorrect exception {0} occurred.", exception.GetType().Name), exception);

            return exception;
        }
    }
}

