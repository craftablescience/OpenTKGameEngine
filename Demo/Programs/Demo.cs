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
			string[] acceptableInput = {"0", "1", "2", "3", "4"};
				System.Console.WriteLine("Color Plane (0) | Texture Plane (1) | Cube (2) | Cube Physics (3) | OBJ Loader (4)");
			string i;
			while (!acceptableInput.Contains(i = System.Console.In.ReadLine()))
			{
				System.Console.WriteLine("0, 1, 2, 3, or 4 please.");
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
				case "4":
				{
					var engine = new DemoObjLoader(args);
					engine.Run();
					return;
				}
			}
		}
	}
}