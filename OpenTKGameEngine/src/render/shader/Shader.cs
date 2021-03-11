using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKGameEngine.Render.Shader
{
    public abstract class Shader : IDisposable
    {
        protected int Handle { get; }
        private readonly Dictionary<string, int> _uniformLocations;

        protected Shader()
        {
            Handle = GL.CreateProgram();
            
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();
            for (var i = 0; i < numberOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
        }
        
        public void Use()
        {
            GL.UseProgram(Handle);
        }
        
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }
        
        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
            if (code != (int) All.True)
            {
                throw new Exception($"Error occurred while linking program {program}:\n{GL.GetProgramInfoLog(program)}");
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
            if (code != (int) All.True)
            {
                throw new Exception($"Error occurred while compiling shader {shader}:\n{GL.GetShaderInfoLog(shader)}");
            }
        }

        private protected static string LoadShaderFile(string path)
        {
            using var reader = new StreamReader(path, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        
        private protected static int CreateShader(string shader, ShaderType shaderType)
        {
            int shaderId = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderId, shader);
            CompileShader(shaderId);
            string infoLogVert = GL.GetShaderInfoLog(shaderId);
            if (infoLogVert != string.Empty)
                Console.WriteLine(infoLogVert);
            return shaderId;
        }
        
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
        
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

        public abstract void Dispose();
    }
}