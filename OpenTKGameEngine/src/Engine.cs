using Microsoft.VisualBasic.CompilerServices;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGameEngine  {
	public class Engine : GameWindow
	{
		public Engine(string title = "Game Window", WindowIcon icon = null, bool startFullscreen = false, bool startVisible = true, double renderFrequency = 0.0, double updateFrequency = 0.0, bool multithreaded = false) : base(SetGameWindowSettingsOnInit(renderFrequency, updateFrequency, multithreaded), SetNativeWindowSettingsOnInit(title, icon, startFullscreen, startVisible))
		{
		}

		private static GameWindowSettings SetGameWindowSettingsOnInit(double renderFrequency, double updateFrequency, bool multithreaded)
		{
			var settings = new GameWindowSettings
			{
				RenderFrequency = renderFrequency,
				UpdateFrequency = updateFrequency,
				IsMultiThreaded = multithreaded
			};
			return settings;
		}

		private static NativeWindowSettings SetNativeWindowSettingsOnInit(string title, WindowIcon icon, bool startFullscreen, bool startVisible)
		{
			var settings = new NativeWindowSettings
			{
				Title = title,
				Icon = icon,
				IsFullscreen = startFullscreen,
				StartVisible = startVisible
			};
			return settings;
		}

		protected override void OnUpdateFrame(FrameEventArgs evt)
		{
			KeyboardState input = KeyboardState;

			if (input.IsKeyDown(Keys.Escape))
			{
				DestroyWindow();
			}
			
			base.OnUpdateFrame(evt);
		}
	}
}