using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace OpenTKGameEngine.Core  {
	public class Engine : GameWindow
	{
		public Color4 ClearColor { get; set; } = Color4.Black;

		public Engine(string[] args, string title = "GameWindow", Vector2i? size = null, string iconPath = null) : base(GameWindowSettings.Default, SetNativeWindowSettingsOnInit(title, size, iconPath))
		{
		}

		private static NativeWindowSettings SetNativeWindowSettingsOnInit(string title, Vector2i? size, string iconPath)
		{
			iconPath ??= "Assets/icon.png";
			var settings = new NativeWindowSettings
			{
				Title = title,
				WindowBorder = WindowBorder.Fixed,
				Icon = LoadIconFromImage(Image.Load<Rgba32>(iconPath))
			};
			if (size.HasValue)
				settings.Size = size.Value;
			return settings;
		}

		private static WindowIcon LoadIconFromImage(Image<Rgba32> image)
		{
			var pixels = new List<byte>(4 * image.Width * image.Height);
			for (var y = 0; y < image.Height; y++) {
				var row = image.GetPixelRowSpan(y);
				for (var x = 0; x < image.Width; x++)
				{
					pixels.Add(row[x].R);
					pixels.Add(row[x].G);
					pixels.Add(row[x].B);
					pixels.Add(row[x].A);
				}
			}
			return new WindowIcon(new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, pixels.ToArray()));
		}
		
		protected override void OnLoad()
		{
			GL.ClearColor(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A);
			Load();
			base.OnLoad();
		}

		public new virtual void Load()
		{
			// overwrite in inherited classes
		}

		protected override void OnRenderFrame(FrameEventArgs evt)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			// ---
			
			KeyboardState input = KeyboardState;

			if (input.IsKeyDown(Keys.Escape))
			{
				DestroyWindow();
			}
			
			Render();
			// ---
			Context.SwapBuffers();
			base.OnUpdateFrame(evt);
		}
		
		public virtual void Render()
		{
			// overwrite in inherited classes
		}
		
		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, Size.X, Size.Y);
			base.OnResize(e);
		}

		protected override void OnUnload()
		{
			UnLoad();
			base.OnUnload();
		}
		
		public virtual void UnLoad()
		{
			// overwrite in inherited classes
		}
	}
}