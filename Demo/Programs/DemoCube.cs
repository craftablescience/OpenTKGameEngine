using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Input;
using OpenTKGameEngine.Render;

namespace Demo
{
    internal class DemoCube : Engine
	{
		private readonly float[] _vertices =
		{
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
			0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
			-0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

			-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			-0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			-0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f
		};
		private int _vertexBufferId;
		private int _vertexArrayId;
		private Shader _triangleShader;
		private Texture _crate;

		public DemoCube(string[] args) : base(args, "Binaries/fmod.dll", "Demo - Cube", new Vector2i(1600, 900), "Assets/Textures/splash.png")
		{
		}

		public override void Load()
		{
			_vertexBufferId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
			_vertexArrayId = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayId);
			_crate = Texture.LoadFromFile("Assets/Textures/container.png");
			_triangleShader = new UnlitShader("Assets/Shaders/democube.vert", "Assets/Shaders/democube.frag");
			_triangleShader.SetInt("texture0", 0);
			_triangleShader.Use();
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("position"), 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("position"));
			GL.VertexAttribPointer(_triangleShader.GetAttribLocation("textureCoords"), 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(_triangleShader.GetAttribLocation("textureCoords"));
			InputRegistry.BindKey(Keys.Escape, (_, _) => DestroyWindow(), InputType.OnPressed);
		}

		public override void Render(double time)
		{
			GL.BindVertexArray(_vertexArrayId);
			_crate.Use(TextureUnit.Texture0);
			_triangleShader.Use();
			var model = Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(ElapsedTime * 16));
			_triangleShader.SetMatrix4("model", model);
			_triangleShader.SetMatrix4("view", Camera.GetViewMatrix());
			_triangleShader.SetMatrix4("projection", Camera.GetProjectionMatrix());
			GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
		}

		public override void UnLoad()
		{
			_triangleShader.Dispose();
		}
	}
}