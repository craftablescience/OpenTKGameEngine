using System.Linq;

namespace Demo {
	internal static class Demo
	{
		private static void Main(string[] args)
		{
			string[] acceptableInput = {"0", "1", "2", "3", "4", "5"};
				System.Console.WriteLine("Color Plane (0)\nTexture Plane (1)\nCube (2)\nCube Physics (3)\nOBJ Loader (4)\n2D Sound (5)");
			string i;
			while (!acceptableInput.Contains(i = System.Console.In.ReadLine()))
			{
				System.Console.WriteLine("0, 1, 2, 3, 4, or 5 please.");
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
				case "5":
				{
					var engine = new Demo2DSound(args);
					engine.Run();
					return;
				}
			}
		}
	}
}