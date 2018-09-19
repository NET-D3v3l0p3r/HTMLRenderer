using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser
{
    public class TagProcessor
    {
        public DOMObject Tag { get; private set; }
        public TagProcessor (DOMObject tag)
        {
            Tag = tag;
        }

        public void Run(Document document)
        {
            if (Tag.Token != null)
                Global.CreateInstance(Tag.Token.Name, Tag)?.Initialize(document);
        }
    }
}
