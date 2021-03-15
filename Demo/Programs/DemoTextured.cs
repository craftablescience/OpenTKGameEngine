#nullable enable
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Input;
using OpenTKGameEngine.Render;
using OpenTKGameEngine.Utility;

namespace Demo
{
    internal class DemoTextured : Engine
    {
	    private StaticTexturedMesh? _mesh;
		
		public DemoTextured(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Textured", new Vector2i(1600, 900), "Assets/Textures/splash.png", loadCameraControls:false)
		{
		}

		public override void Load()
		{
			InputRegistry.BindKey(Keys.Escape, (_, _) => DestroyWindow(), InputType.OnPressed);
			_mesh = new StaticTexturedMesh("Assets/Textures/container.png", is2d:true, genMipmaps:false);
			_mesh.AddSquare(
				new TextureVertex(-1, -1, 0, 0, 0),
				new TextureVertex(1, -1, 0, 1, 0),
				new TextureVertex(-1, 1, 0, 0, 1),
				new TextureVertex(1, 1, 0, 1, 1));
			_mesh.BakeMesh();
		}

		public override void Render(double time)
		{
			_mesh?.Render(time, Matrix4.Identity);
		}
	}
}