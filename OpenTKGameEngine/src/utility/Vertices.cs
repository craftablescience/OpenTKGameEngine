using OpenTK.Mathematics;

namespace OpenTKGameEngine.Utility
{
    public enum VertexType
    {
        Vertex,
        TextureVertex,
        ColorVertex
    }
    
    public struct Vertex
    {
        public Vector3 Position { get; set; }
    }

    public struct TextureVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 Uv { get; set; }
    }

    public struct ColorVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Color { get; set; }
    }
}