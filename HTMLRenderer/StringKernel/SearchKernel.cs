using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.StringKernel
{
    public class SearchKernel
    {
        public string Pattern { get; set; }
        public char[] PatternArray { get; set; }

        public SearchKernel(string pattern)
        {
            Pattern = pattern;
            PatternArray = Pattern.ToCharArray();
        }

            /*
             * 
             * $(",") - between c, c
             * ?(abcd,abcd)~i - between string, string
             * §(abcdefg) after abcdefg
             * §p(abcdefg) before abcdefg
             * #(<,[a,b,c,...])
             * ~(xyz) check whether abcdefg contains xyz and return containing
             */

        public Match[] Search(string text)
        {
            List<Match> matches = new List<Match>();

            char _SearchMethod = PatternArray[0];
            bool isLocked = false;

            char[] rawText = text.ToCharArray();

            Match _currentMatch = null;
            int index = 0;

            switch (_SearchMethod)
            {
                case '$':

                    var p = Pattern.Split('(')[1].Split(')')[0].Split(',');

                    var c0 = p[0].ToCharArray()[0];
                    var c1 = p[1].ToCharArray()[0];

                    p = Pattern.Split('~');
                    if (p.Length > 1)
                        index = int.Parse(p[1]);
                    

                    for (int i = index; i < text.Length; i++)
                    {
                        char c = text[i];

                        if (!isLocked && c == c0)
                        {
                            _currentMatch = new Match();
                            _currentMatch.StartIndex = i;
                            isLocked = true;

                            continue;
                        }
                        if (c == c1)
                        {
                            isLocked = false;
                            if (_currentMatch != null)
                                _currentMatch.Length = _currentMatch.Text.Length;

                            matches.Add(_currentMatch);
                            _currentMatch = null;
                        }

                        if (!isLocked)
                            continue;


                        _currentMatch.Text.Append(c);
                    }

                    break;

                case '?':
                    var p1 = Pattern.Split('(')[1].Split(')')[0].Split(',');

                    var a0 = p1[0];
                    var a1 = p1[1];

           
                    string name = "";
                    p1 = Pattern.Split('~');
                    if (p1.Length > 1)
                    {
                        if (p1[1].Contains("/"))
                            name = p1[1].Split('/')[1];
                        index = int.Parse(p1[1].Split('/')[0]);
                    }
                    int optionalCounter = 0;

                    for (int i = index; i < text.Length; i++)
                    {
                        if (!isLocked)
                        {
                            if (i + a0.Length > text.Length)
                                break;

                            string seq = text.Substring(i, a0.Length);
                            if (a0.Equals(seq))
                            {
                                _currentMatch = new Match();
                                _currentMatch.StartIndex = i;
                                i += a0.Length - 1;
                                isLocked = true;
                                continue;
                            }

                        }
                        else
                        {
                            if (i + a1.Length > text.Length)
                                break;

                            string seq1 = text.Substring(i, a1.Length);
                            string seq = "";
                            //text.Substring(i, a0.Length);

                            if (i + a0.Length > text.Length)
                                seq = text.Substring(i, text.Length - i);
                            else seq = text.Substring(i, name.Length);

                            if (name.Equals(seq))
                                optionalCounter++;

                            if (a1.Equals(seq1))
                            {
                                if (optionalCounter == 0)
                                {
                                    isLocked = false;
                                    if (_currentMatch.Text != null)
                                        _currentMatch.Length = _currentMatch.Text.Length;
                                    matches.Add(_currentMatch);
                                    i += a1.Length - 1;
                                    _currentMatch = null;


                                    continue;
                                }
                                else optionalCounter--;

                            }

                            _currentMatch.Text.Append(text[i]);

                        }
                    }


                    break;

                case '§':

                    break;

                case 'p':

                    break;

                case '#':

                    p = Pattern.Split('(')[1].Split(')')[0].Split(',');

                    string beginTag = p[0];
                    var crit = p[1].Replace("[", "").Replace("]", "").Split(';');
                    string[] abortCriterion = new string[crit.Length + 1];
                    abortCriterion[0] = ";";
                    Array.Copy(crit, 0, abortCriterion, 1, crit.Length);

                    p1 = Pattern.Split('~');

                    string replaceTag = p1[1];

                    bool foundTag = false;

                    p1 = Pattern.Split('/');
                    bool r = p1.Length > 1;

                    if (!r)
                        for (int i = 0; i < text.Length; i++)
                        {
                            string c = text[i] + "";
                            switch (foundTag)
                            {
                                case true:
                                    for (int j = 0; j < abortCriterion.Length; j++)
                                    {
                                        if (c.Equals(abortCriterion[j]))
                                            foundTag = false;
                                    }
                                    if (!foundTag)
                                        continue;

                                    rawText[i] = replaceTag.ToCharArray()[0];

                                    break;

                                case false:

                                    if (beginTag.Equals(c))
                                        foundTag = true;


                                    break;
                            }
                        }
                    else for (int i = text.Length - 1; i > 0; i--)
                        {
                            string c = text[i] + "";


                            switch (foundTag)
                            {
                                case true:
                                    for (int j = 0; j < abortCriterion.Length; j++)
                                    {
                                        if (c.Equals(abortCriterion[j]))
                                            foundTag = false;
                                    }
                                    if (!foundTag)
                                        continue;

                                    rawText[i] = replaceTag.ToCharArray()[0];

                                    break;

                                case false:

                                    if (beginTag.Equals(c))
                                        foundTag = true;


                                    break;
                            }


                        }

                    Match m = new Match()
                    {
                        Text = new StringBuilder(new string(rawText))
                    };
                    matches.Add(m);

                    break;

                case '~':
                    p = Pattern.Split('(')[1].Split(')');
                    crit = p[0].Split('|');

                    foreach (var c in crit)
                    {
                        for (int i = 0; i < text.Length; i += c.Length)
                        {
                            string s = text.Substring(i, c.Length);
                            if (s.Equals(c))
                                matches.Add(new Match()
                                {
                                    Text = new StringBuilder(s)
                                });
                        }
                    }

                    if (matches.Count == 0)
                        matches.Add(new Match()
                        {
                            Text = new StringBuilder(text)
                        });
                    break;
            }

            return matches.ToArray();


        }
    }
}
