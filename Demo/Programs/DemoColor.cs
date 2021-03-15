using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Input;
using OpenTKGameEngine.Render;

namespace Demo
{
    internal class DemoColor : Engine
	{
		private readonly float[] _vertices =
		{
			// positions        // colors
			1.0f, -1.0f, 0.0f,  1.0f, 0.0f, 0.0f,  // bottom right
			-1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,  // bottom left
			-1.0f,  1.0f, 0.0f, 0.0f, 0.0f, 1.0f,  // top left
			1.0f,  1.0f, 0.0f,  0.0f, 0.0f, 1.0f,  // top right
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
		
		public DemoColor(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Colored", new Vector2i(1600, 900), "Assets/Textures/splash.png")
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
			_triangleShader = new UnlitShader("Assets/Shaders/democolor.vert", "Assets/Shaders/democolor.frag");
			_triangleShader.Use();
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("position"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("position"));
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("color"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("color"));
			InputRegistry.BindKey(Keys.Escape, (_, _) => DestroyWindow(), InputType.OnPressed);
		}

		public override void Render(double time)
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