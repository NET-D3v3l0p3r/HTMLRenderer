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

using System.Drawing;

namespace HTMLParser.Rendering.Batching
{
    public class RenderContext
    {
        public GraphicsDevice GraphicsDevice { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public System.Drawing.Color BackgroundColor { get; set; }

        public Bitmap StaticBackground { get; private set; }
        public Graphics GDIGraphics { get; private set; }

        public Dictionary<Size, Bitmap> TextureAtlantes { get; private set; }

        private Texture2D _internalStaticTexture;
        public RenderContext(GraphicsDevice device, int width, int height)
        {
            GraphicsDevice = device;

            Width = width;
            Height = height;

            StaticBackground = new Bitmap(Width, Height);
            GDIGraphics = Graphics.FromImage(StaticBackground);

            BackgroundColor = System.Drawing.Color.White;
            GDIGraphics.Clear(BackgroundColor);
        }

        public void SetBackgroundColor(System.Drawing.Color color)
        {
            BackgroundColor = color;
        }

        public void DrawHardwareAccelerated(SpriteBatch sBatch)
        {
            if (_internalStaticTexture == null) return;
            sBatch.Draw(_internalStaticTexture, new Vector2(0, 0), Microsoft.Xna.Framework.Color.White);
        }

        public void Reset()
        {
            GDIGraphics.Clear(System.Drawing.Color.White);
        }

        public void Create()
        {
            _internalStaticTexture = StaticBackground.ToTexture2D(GraphicsDevice);
        }

    }
}
