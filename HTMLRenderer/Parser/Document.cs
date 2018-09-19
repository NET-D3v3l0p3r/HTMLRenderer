
using HTMLParser.Parser.Analysis;
using HTMLParser.Parser.CSS;
using HTMLParser.Parser.Definition;
using HTMLParser.Rendering;
using HTMLParser.StringKernel;
using Jurassic;
using Jurassic.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLParser.Parser
{
    public class Document : ObjectInstance
    {
        public static HtmlTokenizer HtmlTokenizer { get; private set; }
        public static DOMObject DOMModel { get; private set; }

        public static string[] DocumentStrings;

        public Muktashif Renderer { get; set; }

        public int LastDocumentX { get; private set; }
        public int LastDocumentY { get; private set; }

        public int DocumentX { get; private set; }
        public int DocumentY { get; private set; }

        public int CurrentCharHeight { get; set; }

        public List<Style> DocumentStyles { get; private set; }

        public Document(Muktashif renderer, string raw)
            :base(Global.JSExecutor)
        {
            DocumentStyles = new List<Style>();

            Renderer = renderer;
            PopulateFunctions();

            Stopwatch sw = new Stopwatch();

            sw.Start();
            HtmlTokenizer = new HtmlTokenizer(raw);
            HtmlTokenizer.Tokenize();
            HtmlTokenizer.Analyze();

            DOMModel = new DOMObject(this);
            DOMModel.Process();

            sw.Stop();

            Console.WriteLine(sw.Elapsed.TotalMilliseconds + " ms.");

        }

        #region Rendering
        public void ProcessRendering()
        {
            Task.Factory.StartNew(() =>
            {
                DOMModel.Run();

                Renderer.RenderContext.Create();
            });
        }

        public void AddUpdateLogic(ITagDefinition definition)
        {
            Renderer.UpdateRoutine.Add(definition);
        }
        public void AddRenderLogic(ITagDefinition definition)
        {
            Renderer.RenderRoutine.Add(definition);
        }

        public void ResetX()
        {
            DocumentX = 0;
        }
        public void ResetY()
        {
            DocumentY = 0;
        }
        public void Move(int valX, int valY)
        {
            LastDocumentX = DocumentX;
            LastDocumentY = DocumentY;

            DocumentX += valX;
            DocumentY += valY;
        }
        public void NewLine()
        {
            Move(0, (int)(CurrentCharHeight * 1));
            CurrentCharHeight = 0;
        }

        public Style GetStyle(DOMObject tag)
        {
            foreach (var style in DocumentStyles)
            {
                char[] selectorArray = style.Selector.ToCharArray();
                char op = selectorArray[0];
                Attribute t = null;

                switch (op)
                {
                    case '.':
                        string className = style.Selector.Split('.')[1];
                        t = tag.Attributes.Find(p => p.AttributeName.Equals("class"));
                        if (t != null && t.Value.Equals(className))
                            return style;
                        break;

                    case '#':
                        string id = style.Selector.Split('#')[1];
                        t = tag.Attributes.Find(p => p.AttributeName.Equals("id"));
                        if (t != null && t.Value.Equals(id))
                            return style;
                        break;

                    default:
                        if (char.IsLetter(op))
                        {
                            SearchKernel _searchKernel = new SearchKernel("~(,| |>|+|~)");
                            var m = _searchKernel.Search(style.Selector);

                            string[] ab = style.Selector.Split(new string[] { m[0].GetText() }, StringSplitOptions.RemoveEmptyEntries);

                            switch (m[0].GetText())
                            {
                                case ",":
                                    for (int i = 0; i < ab.Length; i++)
                                    {
                                        if (tag.Token.Name.Contains(ab[i]))
                                            return style;
                                    }
                                    break;
                                case " ":
                                    break;
                                case ">":
                                    break;
                                case "+":
                                    break;
                                case "~":
                                    break;
                                default:
                                    if (tag.Token.Name.Contains(m[0].GetText()))
                                        return style;
                                    break;
                            }
                        }

                        break;
                }
            }

            return null;
        }
        #endregion


        #region JavascriptMethods
        [JSFunction(Name = "getElementById")]
        public static DOMObject GetElementById(string id)
        {
            DOMObject tag = null;

            for (int i = 0; i < DOMModel.Childs.Count; i++)
            {
                tag = DOMModel.Childs[i].Find(id);
                if (tag != null)
                    return tag;
            }
            

            return null;
        }
        #endregion

    }

}
