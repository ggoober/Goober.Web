using Goober.Web;

namespace Goober.WebApi.FileExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProgramUtils.RunWebhost<Startup>(args);
        }
    }
}
