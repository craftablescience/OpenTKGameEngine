using System;
using OpenTK.Mathematics;
using OpenTKGameEngine.Utility;

namespace OpenTKGameEngine.Render
{
    public class SplashScreen
    {
        private readonly StaticTexturedMesh _engineLogo;
        private readonly StaticTexturedMesh _fmodLogo;
        private readonly StaticTexturedMesh _gameLogo;
        private readonly Shader _shader;

        public SplashScreen(string engineLogoPath, string fmodLogoPath, string gameLogoPath)
        {
            var v0 = new TextureVertex(-1, -1, 0, 0, 0);
            var v1 = new TextureVertex(1, -1, 0, 1, 0);
            var v2 = new TextureVertex(-1, 1, 0, 0, 1);
            var v3 = new TextureVertex(1, 1, 0, 1, 1);
            _shader = new UnlitShader("EngineAssets/Shaders/textured_mesh.vert", "EngineAssets/Shaders/textured_mesh.frag");
            _shader.SetInt("texture0", 0);
            _engineLogo = new StaticTexturedMesh(engineLogoPath, _shader, "position", "textureCoords", true);
            _engineLogo.AddSquare(v0, v1, v2, v3);
            _engineLogo.BakeMesh();
            _fmodLogo = new StaticTexturedMesh(fmodLogoPath, _shader, "position", "textureCoords", true);
            _fmodLogo.AddSquare(v0, v1, v2, v3);
            _fmodLogo.BakeMesh();
            _gameLogo = new StaticTexturedMesh(gameLogoPath, _shader, "position", "textureCoords", true);
            _gameLogo.AddSquare(v0, v1, v2, v3);
            _gameLogo.BakeMesh();
        }

        public void Render(double time, SplashScreenPhase phase)
        {
            switch (phase)
            {
                case SplashScreenPhase.EngineLogo:
                {
                    _engineLogo.Render(time, Matrix4.Identity);
                    return;
                }
                case SplashScreenPhase.FMOD:
                {
                    _fmodLogo.Render(time, Matrix4.Identity);
                    return;
                }
                case SplashScreenPhase.GameLogo:
                {
                    _gameLogo.Render(time, Matrix4.Identity);
                    return;
                }
                case SplashScreenPhase.LoadAssets:
                    _gameLogo.Render(time, Matrix4.Identity);
                    return;
                case SplashScreenPhase.LoadComplete:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }
    }
}