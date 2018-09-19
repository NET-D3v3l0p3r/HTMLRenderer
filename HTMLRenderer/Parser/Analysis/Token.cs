using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Analysis
{
    public class Token
    {
        public enum TokenType
        {
            BeginTag,
            Content,
            EndTag,
            Special,
            Default
        };

        public TokenType Type { get; set; }
        public StringBuilder Content { get; set; }

        public StringBuilder NameBuilder { get; set; }
        public StringBuilder AttributeBuilder { get; set; }

        private string name;
        public string Name
        {
            get
            {
                if (name == null)
                    return name = NameBuilder.ToString();
                return name;
            }
        }

        private string attribute;
        public string Attribute
        {
            get
            {
                if (attribute == null)
                    return attribute = AttributeBuilder.ToString();
                return attribute;
            }
        }

        public Token()
        {
            Type = TokenType.Default;

            Content = new StringBuilder();
            NameBuilder = new StringBuilder();
            AttributeBuilder = new StringBuilder();
        }

        public void ProcessToken()
        {
            Content.Append(";");

            bool parsingString = false;
            bool parseName = false;
            bool parseAttribute = false;

            for (int i = 0; i < Content.Length - 1; i++)
            {
                char c = Content[i];
                char c1 = Content[i + 1];
                if (c == '"')
                    parsingString = !parsingString;

                if (!parsingString && (c == '<' || c == '/') && char.IsLetter(c1))
                    parseName = true;

                if (parseName)
                {
                    if (c == ' ')
                        parseAttribute = true;
                    if (parseAttribute)
                        AttributeBuilder.Append(c);
                    else
                    {
                        if (!parsingString && c == '/' || c == '<' || c == '>')
                            continue;
                        NameBuilder.Append(c);
                    }
                }
            }



        }
        public void ClearSpaces()
        {
            Content.Append(";");
            Content = Content.Replace("\r", "").Replace("\t", "").Replace("\n", " ");
            StringBuilder sb = new StringBuilder();

            bool letterFound = false;

            for (int i = 0; i < Content.Length - 1; i++)
            {
                char c = Content[i];

                if (c != ' ')
                    letterFound = true;

                if (letterFound && c == ' ')
                {
                    sb.Append(' ');
                    letterFound = false;
                }
                else if(c != ' ')
                    sb.Append(c);
            }

            Content = sb;
        }

        public override string ToString()
        {
            return "[" + Type + "]" + " " + Content.ToString();
        }
    }
}
