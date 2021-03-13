using System;
using System.Collections.Generic;
using OpenTKGameEngine.Utility;

namespace OpenTKGameEngine.Render
{
    public class TexturedMesh
    {
        private readonly List<TextureVertex> _vertices = new();
        public float[] VertexArrayCache { get; private set; } = Array.Empty<float>();
        private readonly List<uint> _indices = new();
        public uint[] IndexArrayCache { get; private set; } = Array.Empty<uint>();
        private uint _currentIndex;

        public void AddVertex(TextureVertex vertex)
        {
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
            VertexArrayCache = Array.Empty<float>();
            foreach (var vertex in _vertices)
            {
                VertexArrayCache[^0] = vertex.Position.X;
                VertexArrayCache[^0] = vertex.Position.Y;
                VertexArrayCache[^0] = vertex.Position.Z;
                VertexArrayCache[^0] = vertex.Uv.X;
                VertexArrayCache[^0] = vertex.Uv.Y;
            }
            IndexArrayCache = Array.Empty<uint>();
            foreach (uint index in _indices)
            {
                IndexArrayCache[^0] = index;
            }
        }
    }
}