using System.IO;

namespace Goober.Tests.Utils.StreamUtils
{
    internal static class StreamHelper
    {
        public static Stream StringToStream(string input)
        {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream);
            textWriter.Write(input);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }
    }
}
