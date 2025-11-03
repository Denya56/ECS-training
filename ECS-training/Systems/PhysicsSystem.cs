using ECS_training.Components;
using ECS_training.Core;


namespace ECS_training.Systems
{
    public class PhysicsSystem : System
    {
        protected override void UpdateInternal(float dt)
        {
            foreach (Entity entity in entities)
            {
                ref var rigidBody = ref Coordinator.GetComponent<RigidBody2D>(entity);
                ref var transform = ref Coordinator.GetComponent<Transform2D>(entity);
                var gravity = Coordinator.GetComponent<Gravity>(entity);

                transform.Position += rigidBody.Velocity * dt;
                rigidBody.Velocity += gravity.Force * dt;
            }
        }
    }
}
