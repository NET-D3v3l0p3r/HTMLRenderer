using HTMLParser.StringKernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Parser.CSS
{
    public class Style
    {
        public string Selector { get; set; }
        public string Content { get; set; }
        public List<Property> Properties = new List<Property>();
        
        public void Parse()
        {
            string[] s = Content.Split(';');
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals(""))
                    continue;

                string[] pair = s[i].Split(':');
                
                string name = pair[0];
                string value = pair[1];
                Properties.Add(new Property()
                {
                    PropertyName = name,
                    Value = value
                });
            }
        }

        private static string _CancelCriterion = "#;a;b;c;d;e;f;g;h;i;j;k;l;m;n;o;p;*;q;r;s;t;u;v;w;x;y;z;A;B;C;D;E;F;G;H;I;J;K;L;M;N;O;P;Q;R;S;T;U;V;W;X;Y;Z;<;-;0;1;2;3;4;5;6;7;8;9;.;};);#;";
        public static List<Style> Parse(string rawCSS)
        {
            List<Style> styles = new List<Style>();

            #region FORMATTING
            rawCSS = rawCSS.Replace('\r', ' ').Replace('\n', ' ');

            SearchKernel sk = new SearchKernel("#({,[" + _CancelCriterion + "])~§");
            var m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");


            sk = new SearchKernel("#(},[[" + _CancelCriterion + "]])~§/r");
            m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");


            sk = new SearchKernel("#(},[" + _CancelCriterion + "])~§");
            m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");


            sk = new SearchKernel("#({,[[" + _CancelCriterion + "]])~§/r");
            m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");

            sk = new SearchKernel("#(:,[[" + _CancelCriterion + "]])~§/r");
            m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");

            sk = new SearchKernel("#(:,[[" + _CancelCriterion + "]])~§");
            m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");

            sk = new SearchKernel("#(;,[[" + _CancelCriterion + "]])~§");
            m = sk.Search(rawCSS);
            rawCSS = m[0].GetText().Replace("§", "");

            char[] rawArray = rawCSS.ToCharArray();
            #endregion

            string selectorName = "";
            bool isLocked = false;
            int index = 0;

            for (int i = 0; i < rawArray.Length; i++)
            {
                char c = rawArray[i];
                switch (isLocked)
                {
                    case true:

                        if (c == '}')
                        {
                            sk = new SearchKernel("$({,})~" + index);
                            m = sk.Search(rawCSS);


                            styles.Add(new Style()
                            {
                                Selector = selectorName,
                                Content = m[0].GetText()
                            });

                            styles[styles.Count - 1].Parse();

                            selectorName = "";
                            isLocked = false;
                            continue;
                        }

                        break;

                    case false:
                        if (c == '{')
                        {
                            index = i;
                            isLocked = true;
                            continue;
                        }
                        selectorName += c;
                        break;
                }
            }

            return styles;
        }
    }
}
