using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGameEngine.Core  {
	public class Engine : GameWindow
	{
		public Color4 ClearColor { get; set; } = Color4.Black;

		public Engine(string[] args, string title = "GameWindow", Vector2i? size = null) : base(GameWindowSettings.Default, SetNativeWindowSettingsOnInit(title, size))
		{
		}

		private static NativeWindowSettings SetNativeWindowSettingsOnInit(string title, Vector2i? size)
		{
			var settings = new NativeWindowSettings
			{
				Title = title,
				WindowBorder = WindowBorder.Fixed,
			};
			if (size.HasValue)
				settings.Size = size.Value;
			return settings;
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

		protected override void OnUpdateFrame(FrameEventArgs evt)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			// ---
			
			KeyboardState input = KeyboardState;

			if (input.IsKeyDown(Keys.Escape))
			{
				DestroyWindow();
			}
			
			Update();
			// ---
			Context.SwapBuffers();
			base.OnUpdateFrame(evt);
		}
		
		public virtual void Update()
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