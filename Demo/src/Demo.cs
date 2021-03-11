using OpenTK.Graphics.OpenGL;
using OpenTKGameEngine;
using OpenTKGameEngine.Render;

namespace Demo {
	class Demo : Engine
	{
		private float[] _vertices = {
			-0.5f, -0.5f, 0.0f, //Bottom-left vertex
			0.5f, -0.5f, 0.0f,  //Bottom-right vertex
			0.0f,  0.5f, 0.0f   //Top vertex
		};
		private int _vertexBufferId;
		private int _vertexArrayId;
		private SimpleShader _triangleShader;
		
		private Demo() : base("Demo")
		{
		}
		
		private static void Main(string[] args)
		{
			var engine = new Demo();
			engine.Run();
		}

		public override void Load()
		{
			_vertexBufferId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
			_vertexArrayId = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayId);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);
			_triangleShader = new SimpleShader("shaders/triangle.vert", "shaders/triangle.frag");
			_triangleShader.Use();
		}

		public override void Update()
		{
			_triangleShader.Use();
			GL.BindVertexArray(_vertexArrayId);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
		}

		public override void UnLoad()
		{
			_triangleShader.Dispose();
		}
	}
}