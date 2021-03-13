using FmodAudio;

namespace OpenTKGameEngine.Sound
{
    public class SoundController
    {
        public FmodSystem FmodSystem { get; }
        public float DopplerScale { get; }
        public float DistanceFactor { get; }
        public float RollerScale { get; }
        public float MaxDistance { get; }
        public float MinDistance { get; }

        public SoundController(string fmodPath, float dopplerScale, float distanceFactor, float rollerScale)
        {
            DopplerScale = dopplerScale;
            DistanceFactor = distanceFactor;
            MaxDistance = distanceFactor * 8192f;
            MinDistance = distanceFactor * 0.5f;
            RollerScale = rollerScale;
            Fmod.SetLibraryLocation(fmodPath);
            FmodSystem = Fmod.CreateSystem();
            FmodSystem.Init(32);
            FmodSystem.Set3DSettings(dopplerScale, distanceFactor, rollerScale);
        }

        public Sound Load2DSoundFromFile(string path, bool loops = false)
        {
            return new()
            {
                FModSound = FmodSystem.CreateSound(path, Mode._2D | (loops? Mode.Loop_Normal : Mode.Loop_Off))
            };
        }
        
        public Sound Load3DSoundFromFile(string path, bool loops = false)
        {
            var sound = new Sound()
            {
                FModSound = FmodSystem.CreateSound(path, Mode._3D | (loops? Mode.Loop_Normal : Mode.Loop_Off))
            };
            sound.FModSound.Set3DMinMaxDistance(MinDistance, MaxDistance);
            return sound;
        }

        public void PlaySound(Sound sound, bool master = false)
        {
            if (master)
                FmodSystem.PlaySound(sound.FModSound);
            else
            {
                // todo: figure out channels
            }
        }
    }
}