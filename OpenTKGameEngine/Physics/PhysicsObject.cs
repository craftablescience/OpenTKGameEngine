using BulletSharp;
using OpenTK.Mathematics;
using OpenTKGameEngine.Render;
using OpenTKGameEngine.Utility;

namespace OpenTKGameEngine.Physics
{
    public class PhysicsObject
    {
        public RigidBody RigidBody { get; }
        public StaticTexturedMesh StaticTexturedMesh { get; }
        public bool IsDynamic;

        public PhysicsObject(StaticTexturedMesh mesh, float mass, Vector3 position, CollisionShape collisionShape, PhysicsController physicsController)
        {
            StaticTexturedMesh = mesh;
            RigidBody = physicsController.CreateRigidBody(mass, Matrix4.CreateTranslation(position), collisionShape);
            IsDynamic = mass != 0f;
        }

        public void Render(double time)
        {
            StaticTexturedMesh.Render(time, PhysicsController.BulletMatrixToOpenTkMatrix(RigidBody.MotionState.WorldTransform));
        }

        public void Unload()
        {
            StaticTexturedMesh.Unload();
        }
    }
}