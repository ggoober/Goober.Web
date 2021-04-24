namespace Goober.Tests.Utils
{
    public class AssemblyUtils
    {
        public static string GetAssemlbyName<T>()
        {
            return typeof(T).Assembly.GetName().Name;
        }
    }
}
