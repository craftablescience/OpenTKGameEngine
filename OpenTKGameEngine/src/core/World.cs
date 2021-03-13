using System.Collections.Generic;
using OpenTKGameEngine.Physics;
using OpenTKGameEngine.Utility;

namespace OpenTKGameEngine.Core
{
    public class World
    {
        public PhysicsController PhysicsController;
        private readonly List<WorldObject> _worldObjects = new();
        
        public void Load()
        {
            PhysicsController = new PhysicsController();
        }

        public void Update(double time)
        {
            PhysicsController.Update((float)time);
        }

        public void Render(double time)
        {
        }

        public void Unload()
        {
            PhysicsController.UnloadPhysics();
        }
    }
}