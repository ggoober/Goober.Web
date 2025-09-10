namespace Indusoft.Web.Angular.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Indusoft.Web.ProgramUtils.RunWebhost<Startup>(args: args,
                nlogConfigFileName: "config/nlog.config");
        }
    }
}
