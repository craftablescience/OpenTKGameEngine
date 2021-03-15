using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;

namespace OpenTKGameEngine.Input
{
    public class KeyBind
    {
        public Keys Key { get; }
        public InputType Type { get; }
        public Action<Engine,double> Action { get; }
        
        public KeyBind(Keys key, Action<Engine,double> action, InputType type)
        {
            Key = key;
            Action = action;
            Type = type;
        }
    }
}