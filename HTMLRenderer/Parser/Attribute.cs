using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser
{
    public class Attribute
    {
        public string AttributeName;
        public string Value;

        public override string ToString()
        {
            return AttributeName;
        }
    }
}
