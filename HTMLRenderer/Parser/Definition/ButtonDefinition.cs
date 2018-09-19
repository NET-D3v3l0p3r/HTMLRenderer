using HTMLParser.Parser.Analysis;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Definition
{
    public class ButtonDefinition : ITagDefinition
    {
        public Document Document { get; set; }
        public DOMObject HtmlTag { get; set; }
        public Dictionary<string, Delegate> Events { get; set; }

        public RectangleF ClientRectangle { get; set; }
        public bool IsPressed { get; set; }

        public ButtonDefinition(DOMObject tag)
        {
            Events = new Dictionary<string, Delegate>();
            HtmlTag = tag;
        }

        public void Initialize(Document document)
        {
            Document = document;

            PrepareCSS();
            Render();

            Events.Add("onclick", new Action(() =>
            {
                if (ClientRectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && !IsPressed)
                {
                    Global.JSExecutor.CallGlobalFunction(HtmlTag.Attributes.Find(p => p.AttributeName.Equals("onclick")).Value.Replace("(", "").Replace(")", ""));
                    IsPressed = true;
                }
            }));
            Events.Add("onrelease", new Action(() =>
            {
                IsPressed = false;
            }));

            Document.AddUpdateLogic(this);

        }
        public void PrepareCSS()
        {
            HtmlTag.GetChildByName("echo")[0].Run();

            DOMObject child = HtmlTag.GetChildByName("echo")[0];
            EchoDefinition echoDefinition = child.ProcessedTag as EchoDefinition;

            ClientRectangle = new System.Drawing.RectangleF(
                Document.LastDocumentX, Document.LastDocumentY,
                echoDefinition.MeasuredStringLength.Width, echoDefinition.MeasuredStringLength.Height);
        }
        public void Render()
        {
            Document.Renderer.RenderContext.GDIGraphics.DrawRectangle(
                System.Drawing.Pens.Black, Rectangle.Truncate(ClientRectangle));


        }
        public void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                Events["onclick"].DynamicInvoke();
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                Events["onrelease"].DynamicInvoke();

        }
        public void SetPosition()
        {

        }
    }
}