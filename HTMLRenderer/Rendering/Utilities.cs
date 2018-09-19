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
using System.IO;

namespace HTMLParser.Rendering
{
    public static class Utilities
    {
        private static MemoryStream _memoryStream = new MemoryStream();

        public static Texture2D ToTexture2D(this Bitmap bmp, GraphicsDevice g)
        {
            _memoryStream.SetLength(0);
            bmp.Save(_memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return Texture2D.FromStream(g, _memoryStream);
        }
    }
}
