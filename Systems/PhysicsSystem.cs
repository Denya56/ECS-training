using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Entities;


namespace ESC_training.Systems
{
    internal class PhysicsSystem : System
    {
        public Coordinator coordinator { get; set; }

        public override void Update(float dt)
        {
            if (coordinator == null)
                throw new InvalidOperationException("Coordinator must be assigned before calling Update.");

            foreach (Entity entity in entities)
            {
                ref var rigidBody = ref coordinator.GetComponent<RigidBody>(entity);
                ref var transform = ref coordinator.GetComponent<Transform>(entity);
                var gravity = coordinator.GetComponent<Gravity>(entity);

                transform.Position += rigidBody.Velocity * dt;
                rigidBody.Velocity += gravity.Force * dt;
            }
        }
    }
}
