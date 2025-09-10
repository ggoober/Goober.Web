using System;
using System.Collections.Generic;

namespace Goober.CLI.Services.Implementation
{
    public class ArgsContextService : IArgsContextService
    {
        private string[] _args = Array.Empty<string>();

        public ArgsContextService()
        {
        }

        public ArgsContextService(string[] args)
        {
            SetArgs(args);
        }

        public IEnumerable<string> Args => _args;

        protected void SetArgs(string[] args)
        {
            if (args is null || args.Length == 0)
            {
                return;
            }

            var arr = new string[args.Length];
            Array.Copy(args, arr, args.Length);
            _args = arr;
        }
    }
}
