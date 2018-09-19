using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class BodyDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public Brush BackgroundColor { get; set; }

        public BodyDefinition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }



        public void Initialize(Document document)
        {
            Document = document;
            PrepareCSS();
            Render();
        }

        public void PrepareCSS()
        {
            if (HtmlTag.Styles == null)
                return;
 


            foreach (var style in HtmlTag.Styles)
            {
                foreach (var prop in style.Properties)
                {
                    switch (prop.PropertyName)
                    {
                        case "background-color":
                            Document.Renderer.RenderContext.BackgroundColor = Color.FromName(prop.Value);
                            BackgroundColor = new SolidBrush(Color.FromName(prop.Value));
                            break;
                    }
                }
            }

        }
        public void Render()
        {
            if (BackgroundColor == null) return;

            Document.Renderer.RenderContext.GDIGraphics.FillRectangle
             (BackgroundColor, new RectangleF(0, 0, Document.Renderer.Width, Document.Renderer.Height));
        }
        public void Update()
        {

        }
        public void SetPosition()
        {

        }
    }
}
