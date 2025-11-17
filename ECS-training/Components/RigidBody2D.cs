using ECS_training.Core;
using System.Numerics;

namespace ECS_training.Components
{
    public struct RigidBody2D : IComponentData
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
    }
}
