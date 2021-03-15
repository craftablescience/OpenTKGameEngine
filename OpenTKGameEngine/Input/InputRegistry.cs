using System;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;

namespace OpenTKGameEngine.Input
{
    public class InputRegistry
    {
        public readonly List<KeyBind> KeyBinds = new();

        public void BindKey(Keys key, Action<Engine,double> callback, InputType type)
        {
            KeyBinds.Add(new KeyBind(key, callback, type));
        }
        
        public void BindKey(KeyBind keyBind)
        {
            KeyBinds.Add(keyBind);
        }

        public void UpdateInput(Engine engine, double time, KeyboardState state)
        {
            foreach (var keyBind in KeyBinds)
            {
                switch (keyBind.Type)
                {
                    case InputType.Continuous:
                    {
                        if (state.IsKeyDown(keyBind.Key))
                            keyBind.Action.Invoke(engine, time);
                        break;
                    }
                    case InputType.OnPressed:
                    {
                        if (state.IsKeyPressed(keyBind.Key))
                            keyBind.Action.Invoke(engine, time);
                        break;
                    }
                    case InputType.OnReleased:
                    {
                        if (state.IsKeyReleased(keyBind.Key))
                            keyBind.Action.Invoke(engine, time);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(keyBind.Type), "KeyBind does not have valid InputType value");
                }
            }
        }
    }
}