using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFileHelperMapping
{
    public class Mapping
    {
        public string Name { get; set; }
        public bool IgnoreEmptyLines { get; set; }
        public string Delimiter { get; set; }
        public int IgnoreFirstLines { get; set; }
        public int IgnoreLastLines { get; set; }
        public Field[] Fields { get; set; }
    }

    public sealed class Field
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Type { get; set; }
    }
}
