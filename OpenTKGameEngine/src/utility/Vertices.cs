using OpenTK.Mathematics;

namespace OpenTKGameEngine.Utility
{
    public readonly struct Vertex
    {
        public Vertex(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }
        public Vector3 Position { get; }
    }

    public readonly struct TextureVertex
    {
        public TextureVertex(float x, float y, float z, float u, float v)
        {
            Position = new Vector3(x, y, z);
            Uv = new Vector2(u, v);
        }
        public Vector3 Position { get; }
        public Vector2 Uv { get; }
    }
}