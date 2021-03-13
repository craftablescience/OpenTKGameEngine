using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Render;

namespace Demo {
	internal static class Demo
	{
		private static void Main(string[] args)
		{
			string[] acceptableInput = {"0", "1", "2", "3"};
				System.Console.WriteLine("Color Plane (0) | Texture Plane (1) | Cube (2) | Cube Physics (3)");
			string i;
			while (!acceptableInput.Contains(i = System.Console.In.ReadLine()))
			{
				System.Console.WriteLine("0, 1, 2, or 3 please.");
			}
			switch (i)
			{
				case "0":
				{
					var engine = new DemoColor(args);
					engine.Run();
					return;
				}
				case "1":
				{
					var engine = new DemoTextured(args);
					engine.Run();
					return;
				}
				case "2":
				{
					var engine = new DemoCube(args);
					engine.Run();
					return;
				}
				case "3":
				{
					var engine = new DemoPhysics(args);
					engine.Run();
					return;
				}
			}
		}
	}
	
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
		
		public DemoColor(string[] args) : base(args, "Demo - Colored", new Vector2i(1600, 900))
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
		}

		public override void Render()
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

		public DemoTextured(string[] args) : base(args, "Demo - Textured", new Vector2i(1600, 900))
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

		public DemoCube(string[] args) : base(args, "Demo - Cube", new Vector2i(1600, 900))
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
		}

		public override void Render()
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
	
	internal class DemoPhysics : Engine
	{
		public DemoPhysics(string[] args) : base(args, "Demo - Physics", new Vector2i(1600, 900))
		{
		}

		public override void Load()
		{
			Camera.Position = new Vector3(0, 0, -10);
			World.AddCube(10.0f, false, new Vector3(0, -20, 0));
			World.AddCube(2.0f, true, new Vector3(0, 0, 0));
			World.AddCube(1.5f, true, new Vector3(0, 6, -1.1f));
			World.AddCube(2.5f, true, new Vector3(0, 12, 0));
			World.AddCube(2.0f, true, new Vector3(0, 18, 0));
		}
	}
}