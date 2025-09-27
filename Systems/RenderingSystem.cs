using ESC_training.Components;
using ESC_training.Core;
using Raylib_cs;
using Transform2D = ESC_training.Components.Transform2D;

namespace ESC_training.Systems
{
    internal class RenderingSystem : System
    {
        public override void Update(float dt)
        {
            if (Coordinator == null)
                throw new InvalidOperationException("Coordinator must be assigned before calling Update.");

            foreach (Entity entity in entities)
            {
                ref var transform = ref Coordinator.GetComponent<Transform2D>(entity);
                ref var color = ref Coordinator.GetComponent<Rendarable>(entity);

                int x = (int)transform.Position.X;
                int y = (int)transform.Position.Y;

                if (Coordinator.HasComponent<Square>(entity))
                {
                    ref var square = ref Coordinator.GetComponent<Square>(entity);
                    Raylib.DrawRectangle(x, y, (int)(square.SideLength * transform.Scale.X), (int)(square.SideLength * transform.Scale.Y), Raylib.ColorFromNormalized(color.Color));
                }
                else if (Coordinator.HasComponent<Circle>(entity))
                {
                    ref var circle = ref Coordinator.GetComponent<Circle>(entity);
                    Raylib.DrawCircle(x, y, circle.Radius * transform.Scale.X, Raylib.ColorFromNormalized(color.Color));
                }
            }
        }
    }
}
