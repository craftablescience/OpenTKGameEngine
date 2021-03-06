using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Input;

namespace Demo
{
    internal class DemoPhysics : Engine
    {
        public DemoPhysics(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Physics", new Vector2i(1600, 900), "Assets/Textures/splash.png")
        {
        }

        public override void Load()
        {
            InputRegistry.BindKey(Keys.Escape, (_, _) => DestroyWindow(), InputType.OnPressed);
            Camera.Position = new Vector3(0, -10, 10);
            const string tex = "Assets/Textures/container.png";
            World.AddCube(40.0f, false, new Vector3(0, -40, 0), tex);
            World.AddCube(2.0f, true, new Vector3(0, 0, 0), tex);
            World.AddCube(1.5f, true, new Vector3(0, 5, 1f), tex);
            World.AddCube(2.5f, true, new Vector3(0, 10, 0), tex);
            World.AddCube(2.0f, true, new Vector3(0, 15, 0), tex);
            InputRegistry.BindKey(Keys.E, (_, _) => World.AddCube(3.0f, true, Camera.Position, tex), InputType.OnPressed);
        }
    }
}