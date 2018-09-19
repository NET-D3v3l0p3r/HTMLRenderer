using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class BrDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public BrDefinition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }

        public void Initialize(Document document)
        {
            Document = document;
            Document.ResetX();
            Document.NewLine();
            Document.Move(0, 1);

            HtmlTag.ProcessedTag = this;
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
