using OpenTK.Mathematics;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Render;

namespace Demo
{
    public class Demo2DSound : Engine
    {
        public Demo2DSound(string[] args) : base(args, "Binaries/fmod.dll", "Demo - 2D Sound", new Vector2i(1600, 900), "Assets/Textures/splash.png")
        {
        }

        public override void Load()
        {
            var sound = World.SoundController.Load2DSoundFromFile("Assets/Sounds/ElevatorMusic.ogg", true);
            World.SoundController.PlaySound(sound, true);
        }
    }
}