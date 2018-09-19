using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class StyleDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public StyleDefinition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }

        public void Initialize(Document document)
        {
            Document = document;
            if (HtmlTag.Childs.Count == 0)
                return;
            foreach (var style in CSS.Style.Parse(HtmlTag.Childs[0].Token.Content.ToString()))
                Document.DocumentStyles.Add(style);
            HtmlTag.Childs.Clear();
        }
        public void PrepareCSS() { }
        public void Render()
        {

        }
        public void Update()
        {

        }
        public void SetPosition()
        {

        }
    }
}