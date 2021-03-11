using OpenTK.Graphics.OpenGL;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Render.Shader;

namespace Demo {
	internal class Demo : Engine
	{
		private readonly float[] _vertices =
		{
			// positions        // colors
			1.0f, -1.0f, 0.0f,  1.0f, 0.0f, 0.0f,  // bottom right
			-1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,  // bottom left
			-1.0f,  1.0f, 0.0f, 0.0f, 0.0f, 1.0f,  // top left
			1.0f,  1.0f, 0.0f,  0.0f, 0.0f, 1.0f,  // top right
		};
		private readonly uint[] _indices = {
			0, 1, 3,   // first triangle
			1, 2, 3    // second triangle
		};
		private int _vertexBufferId;
		private int _elementBufferId;
		private int _vertexArrayId;
		private UnlitShader _triangleShader;
		
		private Demo(string[] args) : base(args, "Demo")
		{
		}
		
		private static void Main(string[] args)
		{
			var engine = new Demo(args);
			engine.Run();
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
			_triangleShader = new UnlitShader("shaders/demo1.vert", "shaders/demo1.frag");
			_triangleShader.Use();
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("position"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("position"));
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("color"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("color"));
		}

		public override void Update()
		{
			_triangleShader.Use();
			GL.BindVertexArray(_vertexArrayId);
			GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
		}

		public override void UnLoad()
		{
			_triangleShader.Dispose();
		}
	}
}