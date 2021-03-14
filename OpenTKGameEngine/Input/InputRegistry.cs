using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKGameEngine.Core;

namespace OpenTKGameEngine.Input
{
    // Mostly "borrowed" from https://github.com/ENDERZOMBI102/Gamegine/blob/master/Gamegine/src/core/InputSystem.cs
    // Hurray open source
    public class InputRegistry
    {
        private readonly Dictionary<Keys, List<Action<Engine,double>>> _controls = new();

        public void BindKey(Keys key, Action<Engine,double> callback)
        {
            if (!_controls.ContainsKey(key))
                _controls[key] = new List<Action<Engine,double>>();
            _controls[key].Add(callback);
        }

        public void UpdateInput(Engine engine, double time, KeyboardState state)
        {
            foreach (var callback in _controls.Keys.SelectMany(key => _controls[key].Where(_ => state.IsKeyDown(key))))
            {
                callback.Invoke(engine, time);
            }
        }
    }
}