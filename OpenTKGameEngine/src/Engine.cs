using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGameEngine  {
	public class Engine : GameWindow
	{
		public Color4 ClearColor { get; set; } = Color4.Black;

		public Engine(string title = "GameWindow") : base(GameWindowSettings.Default, SetNativeWindowSettingsOnInit(title))
		{
		}

		private static NativeWindowSettings SetNativeWindowSettingsOnInit(string title)
		{
			var settings = new NativeWindowSettings
			{
				Title = title
			};
			return settings;
		}
		
		protected override void OnLoad()
		{
			GL.ClearColor(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A);
			Load();
			base.OnLoad();
		}

		public virtual void Load()
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