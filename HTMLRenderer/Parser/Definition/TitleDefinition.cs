using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class TitleDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public TitleDefinition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }

        public void Initialize(Document document)
        {
            Document = document;
            if (HtmlTag.Childs.Count == 0)
                return;
            Document.Renderer.Window.Invoke(new Action(() => { Document.Renderer.Window.Text = HtmlTag.Childs[0].Token.Content.ToString(); }));
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