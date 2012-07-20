using System;
using System.Threading.Tasks;

namespace AssertExLib.Internal
{
    public static class Recorder
    {
        /// <summary>
        /// Records any exception which is thrown by the given code.
        /// </summary>
        /// <param name="code">The code which may thrown an exception.</param>
        /// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
        public static Exception Exception(ThrowsDelegate code)
        {
            try
            {
                code();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// Records any exception which is thrown by the given code that has
        /// a return value. Generally used for testing property accessors.
        /// </summary>
        /// <param name="code">The code which may thrown an exception.</param>
        /// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
        public static Exception Exception(ThrowsDelegateWithReturn code)
        {
            try
            {
                code();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// Records any exception which is thrown by the given code that returns a task.
        /// </summary>
        /// <param name="task">The task which may thrown an exception</param>
        /// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
        public static AggregateException Exception(Task task)
        {
            try
            {
                task.Wait();
                return null;
            }
            catch (AggregateException ex)
            {
                return ex;
            }
        }
    }
}
