using OpenTK.Mathematics;

namespace OpenTKGameEngine.Utility
{
    public struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3? Color { get; set; }
        public Vector2? Uv { get; set; }
    }
}