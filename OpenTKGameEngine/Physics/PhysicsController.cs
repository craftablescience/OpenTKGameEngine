using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;
using OpenTK.Mathematics;
using Vector3 = BulletSharp.Math.Vector3;

namespace OpenTKGameEngine.Physics
{
    public class PhysicsController
    {
        public DiscreteDynamicsWorld World { get; set; }
        private readonly CollisionDispatcher _dispatcher;
        private readonly DbvtBroadphase _broadphase;
        private readonly List<CollisionShape> _collisionShapes = new();
        private readonly CollisionConfiguration _collisionConf;

        public PhysicsController()
        {
            _collisionConf = new DefaultCollisionConfiguration();
            _dispatcher = new CollisionDispatcher(_collisionConf);
            _broadphase = new DbvtBroadphase();
            World = new DiscreteDynamicsWorld(_dispatcher, _broadphase, null, _collisionConf)
            {
                Gravity = new Vector3(0, -10, 0)
            };
        }

        public virtual void Update(float elapsedTime)
        {
            World.StepSimulation(elapsedTime);
        }

        public void UnloadPhysics()
        {
            for (int i = World.NumConstraints - 1; i >= 0; i--)
            {
                var constraint = World.GetConstraint(i);
                World.RemoveConstraint(constraint);
                constraint.Dispose();
            }
            for (int i = World.NumCollisionObjects - 1; i >= 0; i--)
            {
                var obj = World.CollisionObjectArray[i];
                if (obj is RigidBody body)
                {
                    body.MotionState?.Dispose();
                }
                World.RemoveCollisionObject(obj);
                obj.Dispose();
            }
            foreach (var shape in _collisionShapes)
                shape.Dispose();
            _collisionShapes.Clear();
            World.Dispose();
            _broadphase.Dispose();
            _dispatcher?.Dispose();
            _collisionConf.Dispose();
        }

        public RigidBody CreateRigidBody(float mass, Matrix4 startTransform, CollisionShape shape)
        {
            _collisionShapes.Add(shape);
            bool isDynamic = (mass != 0.0f);
            var localInertia = Vector3.Zero;
            if (isDynamic)
                shape.CalculateLocalInertia(mass, out localInertia);
            var motionState = new DefaultMotionState(OpenTkMatrixToBulletMatrix(startTransform));
            var body = new RigidBody(new RigidBodyConstructionInfo(mass, motionState, shape, localInertia));
            World.AddRigidBody(body);
            return body;
        }

        public static Matrix OpenTkMatrixToBulletMatrix(Matrix4 startTransform)
        {
            return new(
                startTransform.M11, startTransform.M12, startTransform.M13, startTransform.M14,
                startTransform.M21, startTransform.M22, startTransform.M23, startTransform.M24,
                startTransform.M31, startTransform.M32, startTransform.M33, startTransform.M34,
                startTransform.M41, startTransform.M42, startTransform.M43, startTransform.M44);
        }
        
        public static Matrix4 BulletMatrixToOpenTkMatrix(Matrix startTransform)
        {
            return new(
                startTransform.M11, startTransform.M12, startTransform.M13, startTransform.M14,
                startTransform.M21, startTransform.M22, startTransform.M23, startTransform.M24,
                startTransform.M31, startTransform.M32, startTransform.M33, startTransform.M34,
                startTransform.M41, startTransform.M42, startTransform.M43, startTransform.M44);
        }
    }
}