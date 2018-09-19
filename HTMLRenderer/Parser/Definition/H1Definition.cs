using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class H1Definition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public H1Definition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }

        public void Initialize(Document document)
        {
            Document = document;
            PrepareCSS();
            Render();

            Document.ResetX();
            Document.NewLine();
            
            HtmlTag.ProcessedTag = this;

        }
        public void PrepareCSS()
        {
            HtmlTag.Styles.Add(Global.PreDefinedStyles["h1"]);
        }
        public void Render()
        {
            
        }
        public void Update()
        {

        }
        public void SetPosition()
        {
            Document.ResetX();
            Document.Move(0, 40);
        }
    }
}
