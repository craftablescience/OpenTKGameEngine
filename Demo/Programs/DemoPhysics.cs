using OpenTK.Mathematics;
using OpenTKGameEngine.Core;

namespace Demo
{
    internal class DemoPhysics : Engine
    {
        public DemoPhysics(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Physics", new Vector2i(1600, 900), "Assets/Textures/splash.png")
        {
        }

        public override void Load()
        {
            Camera.Position = new Vector3(0, -10, 10);
            World.AddCube(40.0f, false, new Vector3(0, -40, 0));
            World.AddCube(2.0f, true, new Vector3(0, 0, 0));
            World.AddCube(1.5f, true, new Vector3(0, 5, 1f));
            World.AddCube(2.5f, true, new Vector3(0, 10, 0));
            World.AddCube(2.0f, true, new Vector3(0, 15, 0));
        }
    }
}