using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Goober.Tests.Utils.ConfigurationJsonUtils
{
    internal class JsonConfigurationToDictionaryParser
    {
        private JsonConfigurationToDictionaryParser() { }

        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new();

        private string _currentPath;
        private JsonTextReader _reader;

        public static Dictionary<string, string> Parse(Stream input)
        {
            return new JsonConfigurationToDictionaryParser().ParseStream(input);
        }

        [MemberNotNull(nameof(_reader))]
        private Dictionary<string, string> ParseStream(Stream input)
        {
            _data.Clear();
            _reader = new JsonTextReader(new StreamReader(input));
            _reader.DateParseHandling = DateParseHandling.None;
            var jsonConfig = JObject.Load(_reader);

            VisitJObject(jsonConfig);

            var result = new Dictionary<string, string>(_data);

            return result;
        }

        private void VisitJObject(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitJProperty(property);
                ExitContext();
            }
        }

        private void VisitJProperty(JProperty property)
        {
            VisitJToken(property.Value);
        }

        private void VisitJToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;

                case JTokenType.Array:
                    VisitJArray(token.Value<JArray>());
                    break;

                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Bytes:
                case JTokenType.Raw:
                case JTokenType.Null:
                    VisitPrimitive(token.Value<JValue>());
                    break;

                default:
                    throw new FormatException(
                        $"Unsupported JSON token '{_reader.TokenType}' was found." +
                        $" Path '{_reader.Path}', line {_reader.LineNumber}" +
                        $" position {_reader.LinePosition}.");
            }
        }

        private void VisitJArray(JArray array)
        {
            for (int index = 0; index < array.Count; index++)
            {
                EnterContext(index.ToString());
                VisitJToken(array[index]);
                ExitContext();
            }
        }

        private void VisitPrimitive(JValue data)
        {
            var key = _currentPath;

            if (_data.ContainsKey(key))
            {
                throw new FormatException($"A duplicate key '{key}' was found.");
            }
            _data[key] = data.ToString(CultureInfo.InvariantCulture);
        }

        [MemberNotNull(nameof(_currentPath))]
        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        [MemberNotNull(nameof(_currentPath))]
        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}
