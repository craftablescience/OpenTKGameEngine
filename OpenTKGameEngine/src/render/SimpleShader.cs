
using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace OpenTKGameEngine.Render
{
    public class SimpleShader : IDisposable
    {
        private readonly int _handle;

        public SimpleShader(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource;
            string fragmentShaderSource;

            if (vertexPath != null)
            {
                using (var reader = new StreamReader(vertexPath, Encoding.UTF8))
                {
                    vertexShaderSource = reader.ReadToEnd();
                }
            }
            else
            {
                vertexShaderSource = "#version 330 core\nlayout (location = 0) in vec3 aPosition;\n\nvoid main()\n{\n    gl_Position = vec4(aPosition, 1.0);\n}";
            }
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            string infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVert != String.Empty)
                Console.WriteLine(infoLogVert);

            if (fragmentPath != null)
            {
                using (var reader = new StreamReader(fragmentPath, Encoding.UTF8))
                {
                    fragmentShaderSource = reader.ReadToEnd();
                }
            }
            else
            {
                fragmentShaderSource = "#version 330 core\nout vec4 FragColor;\n\nvoid main()\n{\n    FragColor = vec4(1.0f, 1.0f, 1.0f, 1.0f);\n}";
            }
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFrag != String.Empty)
                Console.WriteLine(infoLogFrag);
            
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);
            GL.LinkProgram(_handle);
            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
        
        public void Use()
        {
            GL.UseProgram(_handle);
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

        ~SimpleShader()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}