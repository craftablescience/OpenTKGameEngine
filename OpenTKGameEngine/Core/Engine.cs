using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Input;
using OpenTKGameEngine.Render;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace OpenTKGameEngine.Core  {
	public class Engine : GameWindow
	{
		public Color4 ClearColor { get; set; } = Color4.Black;
		public static PerspectiveCamera Camera { get; private set; }
		private string _fmodPath;
		public World World;
		public InputRegistry InputRegistry { get; private set; }
		public double ElapsedTime { get; private set; }
		private bool _firstMove = true;
		private Vector2 _lastPos;
		private SplashScreen _splashScreen;
		private SplashScreenPhase _splashScreenPhase = SplashScreenPhase.EngineLogo;
		private string _splashPath;
		private bool _loadCameraControls;
		private float _cameraSpeed = 1.5f;
		public float CameraSpeed
		{
			get => _cameraSpeed;
			set => _cameraSpeed = value;
		}
		private float _cameraSensitivity = 0.2f;
		public float CameraSensitivity
		{
			get => _cameraSensitivity;
			set => _cameraSensitivity = value;
		}

		public Engine(string[] args, string fmodPath, string title = "GameWindow", Vector2i? size = null, string splashPath = "EngineAssets/icon.png", string iconPath = null, bool loadCameraControls = true) : base(GameWindowSettings.Default, SetNativeWindowSettingsOnInit(title, size, iconPath))
		{
			_fmodPath = fmodPath;
			_splashPath = splashPath;
			_loadCameraControls = loadCameraControls;
		}

		private static NativeWindowSettings SetNativeWindowSettingsOnInit(string title, Vector2i? size, string iconPath)
		{
			iconPath ??= "EngineAssets/icon.png";
			var settings = new NativeWindowSettings
			{
				Title = title,
				WindowBorder = WindowBorder.Fixed,
				Icon = LoadIconFromImage(Image.Load<Rgba32>(iconPath)),
				StartFocused = true
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
			GL.Enable(EnableCap.DepthTest);
			_splashScreen = new SplashScreen("EngineAssets/Splashes/engine.png", "EngineAssets/Splashes/fmod.png", _splashPath);
			base.OnLoad();
		}

		public new virtual void Load()
		{
			// overwrite in inherited classes
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			ElapsedTime += e.Time;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			switch (_splashScreenPhase)
			{
				case SplashScreenPhase.EngineLogo:
				case SplashScreenPhase.FMOD:
				case SplashScreenPhase.GameLogo:
				case SplashScreenPhase.LoadAssets:
				{
					_splashScreen.Render(e.Time, _splashScreenPhase);
					break;
				}
				case SplashScreenPhase.LoadComplete:
				{
					World.Render(Size.X, Size.Y, e.Time);
					Render(e.Time);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(e), "Invalid splashscreen phase");
			}
			Context.SwapBuffers();
			base.OnUpdateFrame(e);
		}
		
		public virtual void Render(double time)
		{
			// overwrite in inherited classes
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			if (_splashScreenPhase == SplashScreenPhase.LoadComplete)
			{
				if (!IsFocused) return;
				if (_loadCameraControls)
				{
					var mouse = MouseState;
					if (_firstMove)
					{
						_lastPos = new Vector2(mouse.X, mouse.Y);
						_firstMove = false;
					}
					else
					{
						float deltaX = mouse.X - _lastPos.X;
						float deltaY = mouse.Y - _lastPos.Y;
						_lastPos = new Vector2(mouse.X, mouse.Y);
						Camera.Yaw += deltaX * CameraSensitivity;
						Camera.Pitch -= deltaY * CameraSensitivity;
					}
				}
				World.Update(e.Time);
				InputRegistry.UpdateInput(this, e.Time, KeyboardState);
				Update();
			}
			else
			{
				_splashScreenPhase = ElapsedTime switch
				{
					< 3 => SplashScreenPhase.EngineLogo,
					< 6 => SplashScreenPhase.FMOD,
					< 9 => SplashScreenPhase.GameLogo,
					_ => SplashScreenPhase.LoadAssets
				};
				switch (_splashScreenPhase)
				{
					case SplashScreenPhase.EngineLogo:
					case SplashScreenPhase.FMOD:
					case SplashScreenPhase.GameLogo:
					{
						_splashScreen.Render(e.Time, _splashScreenPhase);
						break;
					}
					case SplashScreenPhase.LoadAssets:
					{
						Camera = new PerspectiveCamera(Vector3.UnitZ, Size.X / (float) Size.Y);
						CursorGrabbed = true;
						World = new World();
						World.Load(_fmodPath);
						InputRegistry = new InputRegistry();
						if (_loadCameraControls)
							LoadCameraControls();
						Load();
						ResetView();
						_splashScreenPhase = SplashScreenPhase.LoadComplete;
						break;
					}
					case SplashScreenPhase.LoadComplete:
					default:
						throw new ArgumentOutOfRangeException(nameof(e), "Invalid splashscreen phase");
				}
			}
			base.OnUpdateFrame(e);
		}

		public virtual void Update()
		{
			// overwrite in inherited classes
		}

		public void LoadCameraControls()
		{
			InputRegistry.BindKey(Keys.W,     (engine, time) => Camera.Position += Camera.Front * engine.CameraSpeed * (float)time, InputType.Continuous);
			InputRegistry.BindKey(Keys.S,     (engine, time) => Camera.Position -= Camera.Front * engine.CameraSpeed * (float)time, InputType.Continuous);
			InputRegistry.BindKey(Keys.A,     (engine, time) => Camera.Position -= Camera.Right * engine.CameraSpeed * (float)time, InputType.Continuous);
			InputRegistry.BindKey(Keys.D,     (engine, time) => Camera.Position += Camera.Right * engine.CameraSpeed * (float)time, InputType.Continuous);
			InputRegistry.BindKey(Keys.Space, (engine, time) => Camera.Position += Camera.Up    * engine.CameraSpeed * (float)time, InputType.Continuous);
			InputRegistry.BindKey(Keys.C,     (engine, time) => Camera.Position -= Camera.Up    * engine.CameraSpeed * (float)time, InputType.Continuous);
		}

		public void ResizeWindow(int newWidth, int newHeight, WindowState state)
		{
			Size = new Vector2i(newWidth, newHeight);
			WindowState = state;
			OnResize(new ResizeEventArgs(newWidth, newHeight));
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			ResetView();
			base.OnResize(e);
		}

		private void ResetView()
		{
			GL.Viewport(0, 0, Size.X, Size.Y);
			if (Camera != null)
				Camera.AspectRatio = Size.X / (float) Size.Y;
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			Camera.Fov -= e.OffsetY;
			base.OnMouseWheel(e);
		}

		protected override void OnUnload()
		{
			World.Unload();
			UnLoad();
			base.OnUnload();
		}
		
		public virtual void UnLoad()
		{
			// overwrite in inherited classes
		}
	}
}