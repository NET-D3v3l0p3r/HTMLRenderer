using HTMLParser.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class EchoDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public Font Font { get; set; }

        public Brush ForegroundColor { get; set; }
        public Brush BackgroundColor { get; set; }

        public SizeF MeasuredStringLength { get; set; }

        public RectangleF ClientRectangle { get; set; }

        public EchoDefinition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }
 
        public void Initialize(Document document)
        {
            Document = document;
            HtmlTag.ProcessedTag = this;

            PrepareCSS();
            Render();
        }

        public void PrepareCSS()
        {
            ForegroundColor = new SolidBrush(Color.Black);
            BackgroundColor = new SolidBrush(Document.Renderer.RenderContext.BackgroundColor);

            Font = new Font("Arial", 12);

            if (HtmlTag.Styles == null)
                return;

            foreach (var style in HtmlTag.Styles)
            {
                foreach (var prop in style.Properties)
                {
                    switch (prop.PropertyName)
                    {
                        case "color":
                            ForegroundColor = new SolidBrush(Color.FromName(prop.Value));
                            break;
                        case "background-color":
                            BackgroundColor = new SolidBrush(Color.FromName(prop.Value));
                            break;
                        case "font-size":
                            Font = new Font("Arial", int.Parse(prop.Value));
                            break;
                    }
                }
            }
       
        }
        public void Render()
        {
            string s = HtmlTag.Token.Content.ToString();

            MeasuredStringLength = Document.Renderer.RenderContext.GDIGraphics.MeasureString(s, Font);
            MeasuredStringLength = new SizeF(MeasuredStringLength.Width, MeasuredStringLength.Height);

            ClientRectangle = new RectangleF(Document.DocumentX, Document.DocumentY, MeasuredStringLength.Width, MeasuredStringLength.Height);
            Document.Renderer.RenderContext.GDIGraphics.FillRectangle
                (BackgroundColor, ClientRectangle);

            Document.Renderer.RenderContext.GDIGraphics.DrawString
                (s, Font, ForegroundColor, Document.DocumentX, Document.DocumentY);

            Document.CurrentCharHeight = (int)MeasuredStringLength.Height;

            SetPosition();

        }
        public void Update()
        {
             
        }
        public void SetPosition()
        {
            Document.Move((int)MeasuredStringLength.Width, 0);
            HtmlTag.Parent.ProcessedTag?.SetPosition();
        }
    }
}
