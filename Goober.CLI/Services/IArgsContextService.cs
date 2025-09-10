using System.Collections.Generic;

namespace Goober.CLI.Services
{
    public interface IArgsContextService
    {
        IEnumerable<string> Args { get; }
    }
}
