using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class HRDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public Pen ForegroundColor { get; set; }
        public Brush BackgroundColor { get; set; }

        public HRDefinition(DOMObject tag)
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
            ForegroundColor = new Pen(Color.Gray);
            BackgroundColor = new SolidBrush(Document.Renderer.RenderContext.BackgroundColor);

            if (HtmlTag.Styles == null)
                return;

            foreach (var style in HtmlTag.Styles)
            {
                foreach (var prop in style.Properties)
                {
                    switch (prop.PropertyName)
                    {
                        case "color":
                            ForegroundColor = new Pen(Color.FromName(prop.Value));
                            break;
                        case "background-color":
                            BackgroundColor = new SolidBrush(Color.FromName(prop.Value));
                            break;
                    }
                }
            }
        }
        public void Render()
        {
            Document.ResetX();
            Document.NewLine();
            Document.Move(0, 5);

            Document.Renderer.RenderContext.GDIGraphics.FillRectangle
                (BackgroundColor, new RectangleF(Document.DocumentX, Document.DocumentY - 5, Document.DocumentX + 1980, 10));

            Document.Renderer.RenderContext.GDIGraphics.DrawLine(
                ForegroundColor, Document.DocumentX + 5, Document.DocumentY, Document.DocumentX + 1980, Document.DocumentY);

            Document.Move(0, 10);
        }
        public void Update()
        {

        }
        public void SetPosition()
        {
  
        }
    }
}
