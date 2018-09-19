using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Forms.Controls;
using HTMLParser;
using HTMLParser.Rendering.Batching;
using HTMLParser.Parser;
using System.Diagnostics;
using System.IO;
using HTMLParser.Parser.Definition;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;

namespace HTMLParser.Rendering
{
    public class Muktashif : DrawWindow
    {
        public Form Window { get; private set; }

        public RenderContext RenderContext { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public Document CurrentDocument { get; private set; }
        
        public List<ITagDefinition> UpdateRoutine { get; private set; }
        public List<ITagDefinition> RenderRoutine { get; private set; }
        
        public static double FPS { get; private set; }

        public Muktashif(Form frm, int width, int height)
        {
            Window = frm;
            Width = width;
            Height = height;
        }

        protected override void Initialize()
        {
            base.Initialize();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            UpdateRoutine = new List<ITagDefinition>();
            RenderRoutine = new List<ITagDefinition>();

            Global.Initialize(this.GraphicsDevice);
            RenderContext = new RenderContext(GraphicsDevice, 1980, 1080);
        }
       
        protected override void Draw()
        {
            base.Draw();
            GraphicsDevice.Clear(Color.DarkGray);

            for (int i = 0; i < UpdateRoutine.Count; i++)
                UpdateRoutine[i].Update();
            for (int i = 0; i < RenderRoutine.Count; i++)
                RenderRoutine[i].Render();

            SpriteBatch.Begin();
            RenderContext.DrawHardwareAccelerated(SpriteBatch);
            SpriteBatch.End();
        }


        public void Navigate(string file)
        {
            UpdateRoutine.Clear();
            RenderRoutine.Clear();

            RenderContext.Reset();
            CurrentDocument = new Document(this, File.ReadAllText(file));
            Global.InitializeJScript(CurrentDocument);
            CurrentDocument.ProcessRendering();

        }
    }
}
