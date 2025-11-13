using ECS_training.Core;
using System.Numerics;

namespace ECS_training.Components
{
    public struct Transform2D : IComponentData
    {
        public Vector2 Position;
        //public Vector3 Rotation;
        public Vector2 Scale;
    }
}
