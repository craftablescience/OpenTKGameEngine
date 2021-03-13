using System.Collections.Generic;
using BulletSharp;
using OpenTK.Mathematics;
using OpenTKGameEngine.Physics;
using OpenTKGameEngine.Render;

namespace OpenTKGameEngine.Core
{
    public class World
    {
        public PhysicsController PhysicsController;
        private readonly List<PhysicsObject> _physicsObjects = new();
        private readonly Dictionary<StaticTexturedMesh,Vector3> _staticTexturedMeshes = new();
        private static readonly List<Shader> Shaders = new();

        public void AddCube(float size, bool dynamic, Vector3 position)
        {
            _physicsObjects.Add(new PhysicsObject(
                StaticTexturedMesh.GetCubeMesh(size, "Assets/icon.png"),
                dynamic ? 1f : 0f,
                position,
                new BoxShape(size / 2f),
                PhysicsController));
        }

        public void AddMesh(StaticTexturedMesh mesh, Vector3 position)
        {
            _staticTexturedMeshes.Add(mesh, position);
        }

        public static void Register3DShader(Shader shader)
        {
            Shaders.Add(shader);
        }

        public void Load()
        {
            PhysicsController = new PhysicsController();
        }

        public void Update(double time)
        {
            PhysicsController.Update((float)time);
        }

        public void Render(double time)
        {
            foreach (var shader in Shaders)
            {
                shader.SetMatrix4("view", Engine.Camera.GetViewMatrix());
                shader.SetMatrix4("projection", Engine.Camera.GetProjectionMatrix());
            }
            foreach (var mesh in _staticTexturedMeshes.Keys)
            {
                mesh.Render(time, Matrix4.CreateTranslation(_staticTexturedMeshes[mesh]));
            }
            foreach (var physicsObject in _physicsObjects)
            {
                physicsObject.Render(time);
            }
        }

        public void Unload()
        {
            foreach (var physicsObject in _physicsObjects)
            {
                physicsObject.Unload();
            }
            PhysicsController.UnloadPhysics();
        }
    }
}