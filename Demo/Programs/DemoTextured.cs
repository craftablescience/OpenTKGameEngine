using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Render;

namespace Demo
{
    internal class DemoTextured : Engine
	{
		private readonly float[] _vertices =
		{
			// positions        // uvs
			1.0f, -1.0f, 0.0f,  1.0f, 0.0f,  // bottom right
			-1.0f, -1.0f, 0.0f, 0.0f, 0.0f,  // bottom left
			-1.0f,  1.0f, 0.0f, 0.0f, 1.0f,  // top left
			1.0f,  1.0f, 0.0f,  1.0f, 1.0f,  // top right
		};
		private readonly uint[] _indices =
		{
			0, 1, 3,   // first triangle
			1, 2, 3    // second triangle
		};
		private int _vertexBufferId;
		private int _elementBufferId;
		private int _vertexArrayId;
		private Shader _triangleShader;
		private Texture _crate;

		public DemoTextured(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Textured", new Vector2i(1600, 900), "Assets/Textures/splash.png")
		{
		}

		public override void Load()
		{
			_vertexBufferId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
			_vertexArrayId = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayId);
			_elementBufferId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferId);
			GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
			_crate = Texture.LoadFromFile("Assets/Textures/container.png", false);
			_triangleShader = new UnlitShader("Assets/Shaders/demotextured.vert", "Assets/Shaders/demotextured.frag");
			_triangleShader.SetInt("texture0", 0);
			_triangleShader.Use();
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("position"), 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("position"));
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("textureCoords"), 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("textureCoords"));
		}

		public override void Render()
		{
			GL.BindVertexArray(_vertexArrayId);
			_crate.Use(TextureUnit.Texture0);
			_triangleShader.Use();
			GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
		}

		public override void UnLoad()
		{
			_triangleShader.Dispose();
		}
	}
}