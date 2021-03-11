using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace OpenTKGameEngine.Render.Shader
{
    public class UnlitShader : IDisposable
    {
        private readonly int _handle;

        public UnlitShader(string vertexPath, string fragmentPath)
        {
            int vertexShader = CreateShader(LoadShaderFile(vertexPath), ShaderType.VertexShader);
            int fragmentShader = CreateShader(LoadShaderFile(fragmentPath), ShaderType.FragmentShader);
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);
            GL.LinkProgram(_handle);
            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private static int CreateShader(string shader, ShaderType shaderType)
        {
            int shaderId = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderId, shader);
            GL.CompileShader(shaderId);
            string infoLogVert = GL.GetShaderInfoLog(shaderId);
            if (infoLogVert != string.Empty)
                Console.WriteLine(infoLogVert);
            return shaderId;
        }

        private static string LoadShaderFile(string path)
        {
            using var reader = new StreamReader(path, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        
        public void Use()
        {
            GL.UseProgram(_handle);
        }
        
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }

        #region Dispose of shader
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteProgram(_handle);
                _disposedValue = true;
            }
        }

        ~UnlitShader() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}