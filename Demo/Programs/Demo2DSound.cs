using FmodAudio;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;
using OpenTKGameEngine.Input;

namespace Demo
{
    public class Demo2DSound : Engine
    {
        private Channel _soundChannel;
        
        public Demo2DSound(string[] args) : base(args, "Binaries/fmod.dll", "Demo - 2D Sound", new Vector2i(1600, 900), "Assets/Textures/splash.png", loadCameraControls:false)
        {
        }

        public override void Load()
        {
            var sound = World.SoundController.Load2DSoundFromFile("Assets/Sounds/ElevatorMusic.ogg", true);
            _soundChannel = World.SoundController.PlaySound(sound);
            InputRegistry.BindKey(Keys.Escape, (_, _) => DestroyWindow(), InputType.OnPressed);
        }

        public override void Pause()
        {
            _soundChannel.Paused = true;
        }

        public override void Resume()
        {
            _soundChannel.Paused = false;
        }
    }
}