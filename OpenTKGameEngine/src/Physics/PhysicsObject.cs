using BulletSharp;
using OpenTK.Mathematics;
using OpenTKGameEngine.Render;
using OpenTKGameEngine.Utility;

namespace OpenTKGameEngine.Physics
{
    public class PhysicsObject : WorldObject
    {
        public RigidBody RigidBody { get; }
        public TexturedMesh TexturedMesh { get; }
        public bool IsDynamic;

        public PhysicsObject(TexturedMesh mesh, float mass, Vector3 position, CollisionShape collisionShape, PhysicsController physicsController)
        {
            TexturedMesh = mesh;
            RigidBody = physicsController.CreateRigidBody(mass, Matrix4.CreateTranslation(position), collisionShape);
            IsDynamic = mass != 0f;
        }
    }
}