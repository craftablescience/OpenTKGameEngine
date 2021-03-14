using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Render;

namespace Demo
{
    internal class DemoObjLoader : Engine
    {
        public DemoObjLoader(string[] args) : base(args, "Binaries/fmod.dll", "Demo - OBJ Loader", new Vector2i(1600, 900), "Assets/Textures/splash.png")
        {
        }

        public override void Load()
        {
            InputRegistry.BindKey(Keys.Escape, (_, _) => DestroyWindow());
            Camera.Position = new Vector3(0, 0, 5);
            var mesh1 = new StaticTexturedMesh("Assets/Textures/container.png");
            mesh1.LoadObj("Assets/Models/demo.obj");
            mesh1.BakeMesh();
            var mesh2 = new StaticTexturedMesh("Assets/Textures/container.png");
            mesh2.LoadObj("Assets/Models/deformedcube.obj");
            mesh2.BakeMesh();
            World.AddMesh(mesh1, new Vector3(0,0,0));
            World.AddMesh(mesh2, new Vector3(0,3,0));
            World.AddMesh(mesh2, new Vector3(0,-3,0));
            World.AddMesh(mesh2, new Vector3(3,0,0));
            World.AddMesh(mesh2, new Vector3(-3,0,0));
        }
    }
}