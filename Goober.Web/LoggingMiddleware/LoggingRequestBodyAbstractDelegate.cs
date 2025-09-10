using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Goober.Web.LoggingMiddleware
{
	public abstract class LoggingRequestBodyAbstractDelegate
	{
		public long MaxContentLength { get; set; }

		public HttpContext HttpContext { get; set; }
		public MemoryStream RequestBodyMemoryStream { get; set; }
		public byte[] RequestBodyBytes { get; set; }
		private string _requestBodyParsed { get; set; }

		public override string ToString()
		{
			if (_requestBodyParsed != null)
			{
				return _requestBodyParsed;
			}

			string ret = null;

			if (RequestBodyBytes != null && RequestBodyBytes.Length > 0)
			{
				ret = ParseBody(RequestBodyBytes, HttpContext);
			}

			_requestBodyParsed = ret ?? string.Empty;

			return _requestBodyParsed;
		}

		protected abstract string ParseBody(byte[] requestBodyBytes, HttpContext httpContext);
	}
}
