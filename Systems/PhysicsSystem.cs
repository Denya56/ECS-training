using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Entities;


namespace ESC_training.Systems
{
    internal class PhysicsSystem : System
    {
        public Coordinator coordinator { get; set; }
        public void Update(float dt)
        {
            foreach (Entity entity in entities)
            {
                var rigidBody = coordinator.GetComponent<RigidBody>(entity);
                var transform = coordinator.GetComponent<Transform>(entity);
                var gravity = coordinator.GetComponent<Gravity>(entity);

                transform.Position = rigidBody.Velocity * dt;
                rigidBody.Velocity = gravity.Force * dt;
            }
        }
    }
}
