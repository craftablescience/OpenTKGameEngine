using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Input;
using OpenTKGameEngine.Render;

namespace Demo
{
    internal class DemoCube : Engine
    {
	    private StaticTexturedMesh _mesh;
		
		public DemoCube(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Cube", new Vector2i(1600, 900), "Assets/Textures/splash.png")
		{
		}

		public override void Load()
		{
			_mesh = StaticTexturedMesh.GetCubeMesh(1f, "Assets/Textures/container.png");
			InputRegistry.BindKey(Keys.Escape, (_,_) => DestroyWindow(), InputType.OnPressed);
		}

		public override void Render(double time)
		{
			var model = Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(ElapsedTime * 16));
			_mesh.Render(time, model);
		}
    }
}