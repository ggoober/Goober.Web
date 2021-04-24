using System;
using System.Collections.Generic;

namespace Goober.WebApi.Example.Models
{
    public class GetResponse
    {
        public int IntValue { get; set; }
        public float FloatValue { get; set; }
        public string StringValue { get; set; }
        public DateTime DateValue { get; set; }
        public ExampleEnum EnumValue { get; set; }
        public List<int> IntList { get; set; } = new List<int>();
    }
}
