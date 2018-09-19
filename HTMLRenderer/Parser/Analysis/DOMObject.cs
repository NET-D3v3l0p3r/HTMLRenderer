using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HTMLParser.Parser.CSS;
using HTMLParser.Parser.Definition;
using HTMLParser.Rendering.Batching;
using HTMLParser.StringKernel;
using Jurassic;
using Jurassic.Library;

namespace HTMLParser.Parser.Analysis
{
    public class DOMObject
    {
        #region TagProperties
        public Document Document { get; private set; }

        public DOMObject Parent { get; set; }
        public List<Attribute> Attributes = new List<Attribute>();

        public List<Style> Styles { get; private set; }        
        #endregion

        #region Internal
        public List<DOMObject> Childs = new List<DOMObject>();
        public Token Token { get; private set; }
        public int Index { get; set; }

        public ITagDefinition ProcessedTag { get; set; }
        #endregion

        #region Private declarations
        private TagProcessor tagProcessor;
        private bool isProcessed;

        private void ParseAttributes()
        {
            if (Token == null)
                return;

            if (Token.Attribute.Equals(""))
                return;

            List<string> split = Token.Attribute.Split(' ').ToList();
            split.RemoveAll(p => p.Equals(""));
            string[] attributes = split.ToArray();


            for (int i = 0; i < attributes.Length; i++)
            {
                string attribute = attributes[i];
                string[] pair = attribute.Split('=');

                if (pair.Length < 2)
                    return;

                string attributeId = pair[0];
                string attributeValue = pair[1];


                Attributes.Add(new Attribute()
                {
                    AttributeName = attributeId,
                    Value = attributeValue.Replace("\"", "").Replace(">", "")
                });


            }

        }
        #endregion

        #region Public declarations

        public DOMObject(Document document)
        {
            Document = document;

            Styles = new List<Style>();
            tagProcessor = new TagProcessor(this);
        }

        public DOMObject Find(string id)
        {
            DOMObject tag = null;

            for (int i = 0; i < Attributes.Count; i++)
            {
                if (Attributes[i].AttributeName != null && Attributes[i].AttributeName.Equals("id"))
                    if (Attributes[i].Value.Equals(id))
                        return this;
            }


            for (int i = 0; i < Childs.Count; i++)
            {
                tag = Childs[i].Find(id);
                if (tag != null)
                    return tag;
            }


            return null;
        }
        public DOMObject[] GetChildByName(string name)
        {
            List<DOMObject> tags = new List<DOMObject>();
            for (int i = 0; i < Childs.Count; i++)
            {
                if (Childs[i].Token.Name.Equals(name))
                    tags.Add(Childs[i]);
            }

            return tags.ToArray();
        }

        public void Process()
        {
            ParseAttributes();

            for (int i = Index; i < Document.HtmlTokenizer.Tokens.Count; i++)
            {
                Token currentToken = Document.HtmlTokenizer.Tokens[i];
                if (currentToken.Type == Token.TokenType.BeginTag)
                {
                    DOMObject child = new DOMObject(Document)
                    {
                        Index = i + 1,
                        Parent = this,
                        Token = currentToken
                    };
                    Childs.Add(child);
                    if (!Global.HtmlTags.Contains(child.Token.Name))
                        continue;
                    child.Process();
                    i = Index;
                    continue;
                }

                if (currentToken.Type == Token.TokenType.Content || currentToken.Type == Token.TokenType.Special)
                {
                    currentToken.NameBuilder.Append("echo");
                    currentToken.ClearSpaces();
                    DOMObject child = new DOMObject(Document)
                    {
                        Index = i + 1,
                        Parent = this,
                        Token = currentToken
                    };
                    Childs.Add(child);
                    continue;
                }

                if (currentToken.Type == Token.TokenType.EndTag && Token.Name.Equals(currentToken.Name))
                {
                    Parent.Index = i;
                    return;
                }

            }
            if (Parent != null)
                Parent.Index = Document.HtmlTokenizer.Tokens.Count;


        }
        public void Run()
        {            
            if (isProcessed)
                return;
            if (Parent != null)
                Styles = new List<Style>(Parent.Styles);

            var currentStyle = Document.GetStyle(this);
            if (currentStyle != null)
                Styles.Add(currentStyle);

            tagProcessor.Run(Document);
            foreach (var tag in Childs)
                tag.Run();

            isProcessed = true;
        }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return Token.ToString();
        }
        #endregion
    }
}
