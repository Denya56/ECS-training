using Recs.Core;
using System.Numerics;

namespace ECS_Demo.Components
{
    public struct Transform2D : IComponentData
    {
        public Vector2 Position;
        //public Vector3 Rotation;
        public Vector2 Scale;
    }
}
