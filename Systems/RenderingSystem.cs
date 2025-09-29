using ESC_training.Components;
using ESC_training.Core;
using System.Numerics;
using Transform2D = ESC_training.Components.Transform2D;
using static ESC_training.Config;

namespace ESC_training.Systems
{
    internal class RenderingSystem : System
    {
        public List<RenderCommand> Commands = new List<RenderCommand>(MAX_ENTITIES);
        public override void Update(float dt)
        {
            // move to System base class?
            if (Coordinator == null)
                throw new InvalidOperationException("Coordinator must be assigned before calling Update.");

            Commands.Clear();
            foreach (Entity entity in entities)
            {
                ref var transform = ref Coordinator.GetComponent<Transform2D>(entity);
                ref var color = ref Coordinator.GetComponent<Rendarable>(entity);

                if (Coordinator.HasComponent<Square>(entity))
                {
                    ref var square = ref Coordinator.GetComponent<Square>(entity);
                    Commands.Add(new RenderCommand
                    {
                        Position = transform.Position,
                        Scale = transform.Scale,
                        Color = color.Color,
                        Size = square.SideLength,
                        Command = RenderCommand.CommandType.DrawRectangle
                    });
                }
                else if (Coordinator.HasComponent<Circle>(entity))
                {
                    ref var circle = ref Coordinator.GetComponent<Circle>(entity);
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
