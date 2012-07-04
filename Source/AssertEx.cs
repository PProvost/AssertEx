using System;
using System.Linq;
using System.Threading.Tasks;
using AssertExLib.Internal;
using AssertExLib.Exceptions;

namespace AssertExLib
{
    public delegate void ThrowsDelegate();
    public delegate object ThrowsDelegateWithReturn();
    public delegate Task TaskThrowsDelegate();

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

        public static T TaskThrows<T>(TaskThrowsDelegate testCode) where T : Exception
        {
            var exception = Recorder.Exception(testCode);

            if (exception == null)
                throw new AssertExException("AssertExtensions.Throws failed. No exception occurred.");

            var exceptionsMatching = exception.InnerExceptions.OfType<T>().ToList();

            if (!exceptionsMatching.Any())
                throw new AssertExException(String.Format("AssertExtensions.Throws failed. Incorrect exception {0} occurred.", exception.GetType().Name), exception);

            return exceptionsMatching.First();
        }

        public static void TaskDoesNotThrow(TaskThrowsDelegate testCode)
        {
            var exception = Recorder.Exception(testCode);

            if (exception == null)
                return;

            // TODO: better message
            throw new AssertExException(String.Format("AssertExtensions.TaskDoesNotThrow failed. Incorrect exception {0} occurred.", exception.GetType().Name), exception);
        }

        public static void TaskDoesNotThrow<T>(TaskThrowsDelegate testCode) where T : Exception
        {
            var exception = Recorder.Exception(testCode);

            if (exception == null)
                return;

            var exceptionsMatching = exception.InnerExceptions.OfType<T>().ToList();

            if (!exceptionsMatching.Any())
                return;

            // TODO: better message
            throw new AssertExException(String.Format("AssertExtensions.Throws failed. Incorrect exception {0} occurred.", exception.GetType().Name), exception);
        }
    }
}

