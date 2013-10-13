#if NET4
using System.Threading.Tasks;

namespace UnitTests.NET4
{
    public class TaskEx
    {
        public static Task<T> FromResult<T>(T result)
        {
            return Task.Factory.StartNew(() => result);
        }
    }
}
#endif