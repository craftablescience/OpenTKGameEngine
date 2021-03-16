using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OpenTKGameEngine.Render
{
    public class SkyboxTexture
    {
        private string _front;
        private string _back;
        private string _up;
        private string _down;
        private string _left;
        private string _right;
        protected int Handle { get; }

        private SkyboxTexture(int handle)
        {
            Handle = handle;
        }
        
        public static SkyboxTexture LoadFromFile(string front, string back, string up, string down, string left, string right) => new (
            CreateTexture(Image.Load<Rgba32>(front), Image.Load<Rgba32>(back), Image.Load<Rgba32>(up), Image.Load<Rgba32>(down), Image.Load<Rgba32>(left), Image.Load<Rgba32>(right)));

        public static SkyboxTexture LoadFromImage(Image<Rgba32> front, Image<Rgba32> back, Image<Rgba32> left, Image<Rgba32> right, Image<Rgba32> up, Image<Rgba32> down) => new(
            CreateTexture(front, back, up, down, left, right));

        private static int CreateTexture(Image<Rgba32> front, Image<Rgba32> back, Image<Rgba32> left, Image<Rgba32> right, Image<Rgba32> up, Image<Rgba32> down)
        {
            int width = front.Width;
            int height = front.Height;
            if (width != height)
                throw new ArgumentException("Cubemap image must be sized as a power of 2", nameof(width));
            foreach (var image in new[] {front, back, left, right, up, down})
            {
                if (image.Height != height || image.Width != width)
                    throw new ArgumentException("Cubemap images must all be the same size", nameof(image));
            }
            
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, handle);
            front.Mutate(x => x.Flip(FlipMode.Vertical));
            back.Mutate(x => x.Flip(FlipMode.Vertical));
            left.Mutate(x => x.Flip(FlipMode.Vertical));
            right.Mutate(x => x.Flip(FlipMode.Vertical));
            up.Mutate(x => x.Flip(FlipMode.Vertical));
            down.Mutate(x => x.Flip(FlipMode.Vertical));
            
            var pixels = new List<byte>(6 * 4 * front.Width * front.Height);
            /* Order is:
             * +X - right
             * -X - left
             * +Y - up
             * -Y - down
             * +Z - front
             * -Z - back
             */
            for (var y = 0; y < height; y++)
            {
                var row = right.GetPixelRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            for (var y = 0; y < height; y++)
            {
                var row = left.GetPixelRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            for (var y = 0; y < height; y++)
            {
                var row = up.GetPixelRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            for (var y = 0; y < height; y++)
            {
                var row = down.GetPixelRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            for (var y = 0; y < height; y++)
            {
                var row = front.GetPixelRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            for (var y = 0; y < height; y++)
            {
                var row = back.GetPixelRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }

            GL.TexImage2D(TextureTarget.TextureCubeMap, 0, PixelInternalFormat.Rgba, front.Width, front.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int) TextureWrapMode.ClampToEdge);
            return handle;
        }

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureCubeMap, Handle);
        }
    }
}