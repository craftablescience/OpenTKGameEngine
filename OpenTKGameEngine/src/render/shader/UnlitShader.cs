using System;
using OpenTK.Graphics.OpenGL;

namespace OpenTKGameEngine.Render.Shader
{
    public class UnlitShader : Shader
    {
        public UnlitShader(string vertexPath, string fragmentPath)
        {
            int vertexShader = CreateShader(LoadShaderFile(vertexPath), ShaderType.VertexShader);
            int fragmentShader = CreateShader(LoadShaderFile(fragmentPath), ShaderType.FragmentShader);
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        #region Dispose of shader
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            GL.DeleteProgram(Handle);
            _disposedValue = true;
        }

        ~UnlitShader() => Dispose(false);

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}