using OpenTK.Graphics.OpenGL;
using OpenTKGameEngine.Render.Shader;

namespace OpenTKGameEngine.Render
{
    public enum MeshRenderType
    {
        Colored,
        Textured
    }
    
    public abstract class Mesh
    {
        private float[] _vertices;
        private uint[] _indices;
        private int _vertexBufferId;
        private int _vertexArrayId;
        private int _elementBufferId;
        private readonly BufferUsageHint _bufferUsageHint;
        public Shader.Shader Shader { get; }

        public Mesh(Shader.Shader shader, BufferUsageHint bufferUsageHint)
        {
            Shader = shader;
            _bufferUsageHint = bufferUsageHint;
        }

        public void OnLoad()
        {
            _vertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, _bufferUsageHint);
            _vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayId);
            _elementBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, _bufferUsageHint);
            Shader.Use();
            GL.VertexAttribPointer(Shader.GetAttribLocation("position"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(Shader.GetAttribLocation("position"));
            GL.VertexAttribPointer(Shader.GetAttribLocation("color"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(Shader.GetAttribLocation("color"));
        }

        public void Update()
        {
            Shader.Use();
            GL.BindVertexArray(_vertexArrayId);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void OnUnload()
        {
            Shader.Dispose();
        }
    }
}