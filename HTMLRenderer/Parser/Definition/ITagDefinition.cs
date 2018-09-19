using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public interface ITagDefinition
    {
        Document Document { get; set; }
        DOMObject HtmlTag { get; set; }
        Dictionary<string, Delegate> Events { get; set; }

        void Initialize(Document document);
        void PrepareCSS();
        void Update();
        void Render();
        void SetPosition();

    }
}
