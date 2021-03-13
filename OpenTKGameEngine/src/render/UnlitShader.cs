using System;
using OpenTK.Graphics.OpenGL;

namespace OpenTKGameEngine.Render
{
    public class UnlitShader : Shader
    {
        private readonly string _vertexPath;
        private readonly string _fragmentPath;
        
        public UnlitShader(string vertexPath, string fragmentPath)
        {
            _vertexPath = vertexPath;
            _fragmentPath = fragmentPath;
            int vertexShader = CreateShader(LoadShaderFile(vertexPath), ShaderType.VertexShader);
            int fragmentShader = CreateShader(LoadShaderFile(fragmentPath), ShaderType.FragmentShader);
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            LinkProgram(Handle);
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public override void Reload()
        {
            GL.DeleteProgram(Handle);
            int vertexShader = CreateShader(LoadShaderFile(_vertexPath), ShaderType.VertexShader);
            int fragmentShader = CreateShader(LoadShaderFile(_fragmentPath), ShaderType.FragmentShader);
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            LinkProgram(Handle);
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