using HTMLParser.Parser;
using HTMLParser.Parser.CSS;
using HTMLParser.StringKernel;
using Jurassic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLParser
{
    public static class Global
    {
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static ScriptEngine JSExecutor { get; private set; }

        public static HashSet<string> HtmlTags = new HashSet<string>()
            {
                "html",
                "head",
                "title",
                "body",
                "tbody",
                "form",
                "td",
                "tr",
                "table",
                "h1",
                "a",
                "p",
                "div",
                "strong",
                "em",
                "img",
                "link",
                "button",
                "script",
                "style",
                "span"
            };
        public static HashSet<string> UniqueTags = new HashSet<string>()
        {
            "p",
            "h1"
        };

        public static Dictionary<string, int> DocumentTags = new Dictionary<string, int>()
        {
            { "html", 0 },
            { "head", 1 },
            { "body", 2 }
        };
        public static Dictionary<string, Style> PreDefinedStyles { get; private set; }

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
    
            JSExecutor = new ScriptEngine();

            bool executedCommand = false;
            JSExecutor.SetGlobalFunction("alert", new Action<string>((string parameter) =>
            {
                if (executedCommand) return;
                executedCommand = true;
                SendKeys.Send("%");
                MessageBox.Show(parameter);
                executedCommand = false;
            }));
            JSExecutor.SetGlobalFunction("Date", new Func<string>(() =>
            {
                return DateTime.Now.ToString();
            }));
            JSExecutor.SetGlobalFunction("print", new Action<string>((string parameters) =>
            {
                Console.WriteLine(parameters);

            }));

            PreDefinedStyles = new Dictionary<string, Style>();
            PreDefinedStyles.Add("h1", new Style()
            {
                Properties = new List<Property>(new Property[]
                {
                    new Property()
                    {
                        PropertyName = "font-size",
                        Value = "20"
                    }
                })
            });
        }
        public static void InitializeJScript(Document document)
        {
            JSExecutor.SetGlobalValue("document", document);
        }



        public static dynamic CreateInstance(string className, params object[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name.ToUpper().Equals((className + "Definition").ToUpper()));
            if (type == null)
                return null;

            return Activator.CreateInstance(type, args);
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> 
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
