using System;
using System.Collections.Generic;
using BulletSharp.Math;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Utility;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace OpenTKGameEngine.Render
{
    public class StaticTexturedMesh
    {
        public Texture Texture { get; private set; }
        private string _texturePath;
        private string _uniformPosition;
        private string _uniformTextureCoords;
        private int _vertexBufferId;
        private int _vertexArrayId;
        private int _elementBufferId;
        private static Shader _defaultShader;
        public Shader Shader { get; }
        private readonly List<TextureVertex> _vertices = new();
        public float[] VertexArrayCache { get; private set; } = Array.Empty<float>();
        private readonly List<uint> _indices = new();
        public uint[] IndexArrayCache { get; private set; } = Array.Empty<uint>();
        private uint _currentIndex;
        public bool Finalized;

        public StaticTexturedMesh(string texturePath) : this(texturePath, SetDefaultShader(), "position", "textureCoords")
        {
        }

        public StaticTexturedMesh(string texturePath, Shader shader, string uniformPosition, string uniformTextureCoords)
        {
            _texturePath = texturePath;
            _uniformPosition = uniformPosition;
            _uniformTextureCoords = uniformTextureCoords;
            Shader = shader;
            World.Register3DShader(Shader);
            Finalized = false;
        }

        private static Shader SetDefaultShader()
        {
            if (_defaultShader != null)
                return _defaultShader;
            _defaultShader = new UnlitShader("Assets/Shaders/textured_mesh.vert", "Assets/Shaders/textured_mesh.frag");
            _defaultShader.SetInt("texture0", 0);
            return _defaultShader;
        }

        public void AddVertex(TextureVertex vertex)
        {
            if (Finalized) throw new FieldAccessException("StaticTexturedMesh is finalized! Do not add more vertices");
            if (_vertices.Contains(vertex))
            {
                _indices.Add((uint) _vertices.IndexOf(vertex));
            }
            else
            {
                _vertices.Add(vertex);
                _indices.Add(_currentIndex);
                _currentIndex++;
            }
        }

        public void AddTriangle(TextureVertex vertex0, TextureVertex vertex1, TextureVertex vertex2)
        {
            AddVertex(vertex0);
            AddVertex(vertex1);
            AddVertex(vertex2);
        }
        
        public void AddSquare(TextureVertex vertex0, TextureVertex vertex1, TextureVertex vertex2, TextureVertex vertex3)
        {
            AddTriangle(vertex0, vertex1, vertex2);
            AddTriangle(vertex1, vertex2, vertex3);
        }

        public void CalculateVertexAndIndexArrays()
        {
            VertexArrayCache = new float[_vertices.Count * 5];
            for (var i = 0; i < _vertices.Count; i++)
            {
                var vertex = _vertices[i];
                VertexArrayCache[i*5] = vertex.Position.X;
                VertexArrayCache[i*5+1] = vertex.Position.Y;
                VertexArrayCache[i*5+2] = vertex.Position.Z;
                VertexArrayCache[i*5+3] = vertex.Uv.X;
                VertexArrayCache[i*5+4] = vertex.Uv.Y;
            }
            IndexArrayCache = new uint[_indices.Count];
            for (var i = 0; i < _indices.Count; i++)
            {
                IndexArrayCache[i] = _indices[i];
            }
            _vertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, VertexArrayCache.Length * sizeof(float), VertexArrayCache, BufferUsageHint.StaticDraw);
            _vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayId);
            _elementBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexArrayCache.Length * sizeof(uint), IndexArrayCache, BufferUsageHint.StaticDraw);
            Texture = Texture.LoadFromFile(_texturePath);
            Shader.Use();
            GL.VertexAttribPointer(Shader.GetAttribLocation(_uniformPosition), 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(Shader.GetAttribLocation(_uniformPosition));
            GL.VertexAttribPointer(Shader.GetAttribLocation(_uniformTextureCoords), 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(Shader.GetAttribLocation(_uniformTextureCoords));
            Finalized = true;
        }

        public void Render(double time, Matrix4 position)
        {
            GL.BindVertexArray(_vertexArrayId);
            Texture.Use(TextureUnit.Texture0);
            Shader.SetMatrix4("model", Matrix4.Identity * position);
            Shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, IndexArrayCache.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Unload()
        {
            Shader.Dispose();
        }

        public static StaticTexturedMesh GetCubeMesh(float size, string texturePath)
        {
            var mesh = new StaticTexturedMesh(texturePath);
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, -size/2f, 0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f, -size/2f, 1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f, size/2f, -size/2f, 1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f, size/2f, -size/2f, 1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, size/2f, -size/2f, 0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, -size/2f, 0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, size/2f, 0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f,  size/2f,  1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f,  size/2f,  1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f,  size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f,  size/2f,  0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f, -size/2f,  1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f,  size/2f,  0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f, -size/2f,  1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f,  size/2f,  0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f, -size/2f,  1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f, -size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f,  size/2f,  0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f, -size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f, -size/2f,  0.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f, -size/2f,  1.0f, 1.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(size/2f,  size/2f,  size/2f,  1.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f,  size/2f,  0.0f, 0.0f));
            mesh.AddVertex(new TextureVertex(-size/2f,  size/2f, -size/2f,  0.0f, 1.0f));
            mesh.CalculateVertexAndIndexArrays();
            return mesh;
        }
    }
}