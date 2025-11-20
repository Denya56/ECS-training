using ECS_Demo.Components;
using Recs.Core;
using Recs.Systems;
using System.Numerics;
using static Recs.EcsConfig;

namespace ECS_Demo.Systems
{
    public class RenderingSystem : ECSSystem
    {
        [RequireComponent] public Transform2D Transform;
        [RequireComponent] public Rendarable Rendarable;

        public readonly List<RenderCommand> Commands = new List<RenderCommand>(MAX_ENTITIES);
        protected override void UpdateInternal(float dt)
        {
            Commands.Clear();
            foreach (Entity entity in entities)
            {
                ref var transform = ref Coordinator.Instance.GetComponent<Transform2D>(entity);
                ref var color = ref Coordinator.Instance.GetComponent<Rendarable>(entity);

                if (Coordinator.Instance.HasComponent<Square>(entity))
                {
                    ref var square = ref Coordinator.Instance.GetComponent<Square>(entity);
                    Commands.Add(new RenderCommand
                    {
                        Position = transform.Position,
                        Scale = transform.Scale,
                        Color = color.Color,
                        Size = square.SideLength,
                        Command = RenderCommand.CommandType.DrawRectangle
                    });
                }
                else if (Coordinator.Instance.HasComponent<Circle>(entity))
                {
                    ref var circle = ref Coordinator.Instance.GetComponent<Circle>(entity);
                    Commands.Add(new RenderCommand
                    {
                        Position = transform.Position,
                        Scale = transform.Scale,
                        Color = color.Color,
                        Size = circle.Radius,
                        Command = RenderCommand.CommandType.DrawCircle
                    });
                }
            }
        }
        public struct RenderCommand
        {
            public Vector2 Position { get; internal set; }
            public Vector2 Scale { get; internal set; }
            public Vector4 Color { get; internal set; }
            public int Size { get; internal set; }
            public CommandType Command { get; internal set; }

            public enum CommandType
            {
                DrawRectangle,
                DrawCircle
            }
        }
    }
}
