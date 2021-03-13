using OpenTK.Mathematics;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Render;

namespace Demo
{
    internal class DemoObjLoader : Engine
    {
        public DemoObjLoader(string[] args) : base(args, "Demo - OBJ Loader", new Vector2i(1600, 900))
        {
        }

        public override void Load()
        {
            Camera.Position = new Vector3(0, 0, -10);
            var mesh = new StaticTexturedMesh("Assets/Textures/container.png");
            mesh.LoadObj("Assets/Models/demo.obj");
            mesh.CalculateVertexAndIndexArrays();
            World.AddMesh(mesh, new Vector3(0,0,0));
        }
    }
}