using Recs.Core;
using System.Numerics;

namespace ECS_Demo.Components
{
    public struct Gravity : IComponentData
    {
        public Vector2 Force;
    }
}
