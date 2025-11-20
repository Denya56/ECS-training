using Recs.Core;
using System.Numerics;

namespace ECS_Demo.Components
{
    public struct RigidBody2D : IComponentData
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
    }
}
