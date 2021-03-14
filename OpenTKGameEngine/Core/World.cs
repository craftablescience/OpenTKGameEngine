using System.Collections.Generic;
using BulletSharp;
using OpenTK.Mathematics;
using OpenTKGameEngine.Physics;
using OpenTKGameEngine.Render;
using OpenTKGameEngine.Sound;

namespace OpenTKGameEngine.Core
{
    public class World
    {
        public PhysicsController PhysicsController { get; protected set; }
        public SoundController SoundController { get; protected set; }
        private readonly List<PhysicsObject> _physicsObjects = new();
        private readonly Dictionary<StaticTexturedMesh,List<Vector3>> _staticTexturedMeshes = new();
        private static readonly List<Shader> Shaders = new();

        public void AddCube(float size, bool dynamic, Vector3 position)
        {
            _physicsObjects.Add(new PhysicsObject(
                StaticTexturedMesh.GetCubeMesh(size, "EngineAssets/icon.png"),
                dynamic ? 1f : 0f,
                position,
                new BoxShape(size / 2f),
                PhysicsController));
        }

        public void AddMesh(StaticTexturedMesh mesh, Vector3 position)
        {
            if (_staticTexturedMeshes.ContainsKey(mesh))
                _staticTexturedMeshes[mesh].Add(position);
            else
                _staticTexturedMeshes.Add(mesh, new List<Vector3>(new []{position}));
        }
        
        public void AddMesh(StaticTexturedMesh mesh, List<Vector3> position)
        {
            if (_staticTexturedMeshes.ContainsKey(mesh))
                _staticTexturedMeshes[mesh] = position;
            else
                _staticTexturedMeshes.Add(mesh, position);
        }

        public static void Register3DShader(Shader shader)
        {
            Shaders.Add(shader);
        }

        public void Load(string fmodPath)
        {
            PhysicsController = new PhysicsController();
            SoundController = new SoundController(fmodPath, 1f, 1f, 1f);
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
                foreach (var position in _staticTexturedMeshes[mesh])
                {
                    mesh.Render(time, Matrix4.CreateTranslation(position));
                }
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