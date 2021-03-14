using OpenTK.Mathematics;
using System;

namespace OpenTKGameEngine.Render
{
    public class Camera
    {
        private Vector3 _front = -Vector3.UnitZ;
        private float _pitch;
        private float _yaw = -MathHelper.PiOver2;
        private float _fov = MathHelper.PiOver2;
        public Vector3 Position { get; set; }
        public float AspectRatio { private get; set; }
        public Vector3 Front => _front;
        public Vector3 Up { get; private set; } = Vector3.UnitY;
        public Vector3 Right { get; private set; } = Vector3.UnitX;
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                float angle = MathHelper.Clamp(value, -89.0f, 89.0f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                float angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public bool IsPerspective { get; set; }
        
        public Camera(Vector3 position, float aspectRatio, bool isPerspective = true)
        {
            Position = position;
            AspectRatio = aspectRatio;
            IsPerspective = isPerspective;
        }
        
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, Up);
        }
        
        public Matrix4 GetProjectionMatrix()
        {
            return IsPerspective?
                Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 8192f)
                : Matrix4.CreateOrthographic(1600, 900, 0.01f, 8192f);
        }
        
        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _front = Vector3.Normalize(_front);
            Right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, _front));
        }
    }
}