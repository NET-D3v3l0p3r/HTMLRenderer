using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.Analysis
{
    public class HtmlTokenizer
    {
        public StringBuilder RawHtmlSource { get; private set; }
        public List<Token> Tokens { get; private set; }

        public HtmlTokenizer(string rawHtmlSrc)
        {
            RawHtmlSource = new StringBuilder("<html>" + rawHtmlSrc + "</html>;");
            Tokens = new List<Token>();
        }

        public void Tokenize()
        {
            bool possibleTagFound = false;
            bool foundString = false;
            bool letterFound = true;
            bool endFound = false;

            Token currentToken = new Token();


            for (int i = 0; i < RawHtmlSource.Length - 1; i++)
            {
                char c = RawHtmlSource[i];
                char c1 = RawHtmlSource[i + 1];


                switch (possibleTagFound)
                {
                    case true:
                        currentToken.Content.Append(c);

                        if (c == '"')
                            foundString = !foundString;


                        if (!foundString && c == '/')
                            endFound = true;
                        if (!foundString && char.IsLetter(c))
                            letterFound = true;

                        if (endFound && c == '>' && letterFound)
                        {
                            if (currentToken.Content[0] == '<' && currentToken.Content[1] == '/')
                                currentToken.Type = Token.TokenType.EndTag;
                            else currentToken.Type = Token.TokenType.Special;
                        }
                        else if (!endFound && !foundString && letterFound && c == '>')
                            currentToken.Type = Token.TokenType.BeginTag;

                        if (currentToken.Type == Token.TokenType.Content || currentToken.Type == Token.TokenType.Default)
                            continue;

                        currentToken.ProcessToken();
                        Tokens.Add(currentToken);
                        currentToken = new Token()
                        {
                            Type = Token.TokenType.Content
                        };

                        possibleTagFound = false;
                        letterFound = false;
                        endFound = false;

                        break;
                    case false:

                        currentToken.Content.Append(c);

                        if (!foundString && c == '<')
                        {
                            if (c1 != '/' && !char.IsLetter(c1))
                                continue;

                            if (currentToken.Type == Token.TokenType.Content && (char.IsLetter(c1) || c1 == '/') && currentToken.Content.Length > 0)
                            {
                                currentToken.Content.Remove(currentToken.Content.Length - 1, 1);
                                if (currentToken.Content.Length > 0 && !string.IsNullOrWhiteSpace(currentToken.Content.ToString()))
                                    Tokens.Add(currentToken);

                                currentToken = new Token();
                                currentToken.Content.Append("<");
                            }
                            possibleTagFound = true;
                        }


                        break;
                }
            }

            if (currentToken.Content.Length > 0 && !string.IsNullOrWhiteSpace(currentToken.Content.ToString()))
                Tokens.Add(currentToken);

        }
        public void Analyze()
        {
            Token endToken = null;

            for (int i = 0; i < Tokens.Count; i++)
            {
                Token currentToken = Tokens[i];
                if (currentToken.Type == Token.TokenType.Content || currentToken.Type == Token.TokenType.Default) continue;

                string name = currentToken.Name;

                if (endToken != null)
                {
                    Tokens.Insert(i, endToken);
                    endToken = null;
                    continue;
                }
                if (currentToken.Type == Token.TokenType.BeginTag)
                    if (Global.UniqueTags.Contains(name))
                    {
                        endToken = new Token()
                        {
                            Type = Token.TokenType.EndTag,
                            Content = new StringBuilder("</" + name + ">;"),
                            NameBuilder = new StringBuilder(name)
                        };
                    }

            }
        }
    }
}
