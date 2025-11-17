using ECS_training.Core;
using System.Numerics;

namespace ECS_training.Components
{
    public struct Gravity : IComponentData
    {
        public Vector2 Force;
    }
}
