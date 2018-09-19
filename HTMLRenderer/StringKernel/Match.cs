using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.StringKernel
{
    public class Match
    {
        public StringBuilder Text = new StringBuilder();

        public int StartIndex { get; set; }
        public int Length { get; set; }

        private string _text = null;
        public string GetText()
        {
            if (_text != null)
                return _text;
            return _text = Text.ToString();
        }

    }
}
