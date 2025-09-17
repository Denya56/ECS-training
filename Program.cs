// ===== ECS setup =====
using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Entities;
using ESC_training.Systems;
using System.Numerics;
using static ESC_training.Config;

/*
var entityManager = new EntityManager();
var componentManager = new ComponentManager();

// Register components
componentManager.RegisterComponent<Transform>();
componentManager.RegisterComponent<RigidBody>();
componentManager.RegisterComponent<Gravity>();

// Create entities
Entity player = entityManager.CreateEntity();
Entity enemy = entityManager.CreateEntity();

// Add components
componentManager.AddComponent(player, new Transform { Position = new Vector3(0, 0, 0), Rotation = Vector3.Zero, Scale = Vector3.One });
componentManager.AddComponent(player, new RigidBody { Velocity = Vector3.Zero, Acceleration = Vector3.Zero });

componentManager.AddComponent(enemy, new Transform { Position = new Vector3(10, 0, 0), Rotation = Vector3.Zero, Scale = Vector3.One });
componentManager.AddComponent(enemy, new Gravity { Force = new Vector3(0, -9.81f, 0) });

// ===== Access and modify components via ref =====
ref var playerTransform = ref componentManager.GetComponent<Transform>(player);
Console.WriteLine($"Player initial position: {playerTransform.Position}");
playerTransform.Position += new Vector3(1, 0, 0);
Console.WriteLine($"Player new position: {playerTransform.Position}");

ref var enemyGravity = ref componentManager.GetComponent<Gravity>(enemy);
Console.WriteLine($"Enemy gravity force: {enemyGravity.Force}");
enemyGravity.Force += new Vector3(0, -1, 0);
Console.WriteLine($"Enemy new gravity force: {enemyGravity.Force}");

// ===== Entity destruction demo =====
entityManager.DestroyEntity(enemy);
componentManager.EntityDestroyed(enemy);

try
{
    componentManager.GetComponent<Gravity>(enemy);
}
catch (Exception ex)
{
    Console.WriteLine($"Accessing destroyed component throws: {ex.Message}");
}*/


var coordinator = new Coordinator();

coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody>();
coordinator.RegisterComponent<Transform>();

var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
physicsSystem.coordinator = coordinator;

var signature = new Signature();
signature.AddComponent(coordinator.GetComponentType<Gravity>());
signature.AddComponent(coordinator.GetComponentType<RigidBody>());
signature.AddComponent(coordinator.GetComponentType<Transform>());

coordinator.SetSystemSignature<PhysicsSystem>(signature);


var entities = new List<Entity>(MAX_ENTITIES);
var rand = new Random();

for (int i = 0; i < MAX_ENTITIES; i++)
{
    var entity = coordinator.CreateEntity();

    coordinator.AddComponent(entity, new Gravity
    {
        Force = new Vector3(0.0f, (float)(-rand.NextDouble() * 9.0 - 1.0), 0.0f)
    });

    coordinator.AddComponent(entity, new RigidBody
    {
        Velocity = Vector3.Zero,
        Acceleration = Vector3.Zero
    });

    float positionX = (float)(rand.NextDouble() * 200.0 - 100.0);
    float positionY = (float)(rand.NextDouble() * 200.0 - 100.0);
    float positionZ = (float)(rand.NextDouble() * 200.0 - 100.0);

    float rotationX = (float)(rand.NextDouble() * 3.0);
    float rotationY = (float)(rand.NextDouble() * 3.0);
    float rotationZ = (float)(rand.NextDouble() * 3.0);

    float scale = (float)(rand.NextDouble() * 2.0 + 3.0);

    coordinator.AddComponent(entity, new Transform
    {
        Position = new Vector3(positionX, positionY, positionZ),
        Rotation = new Vector3(rotationX, rotationY, rotationZ),
        Scale = new Vector3(scale, scale, scale)
    });

    entities.Add(entity);
}

// Main loop
float dt = 0.016f; // start with ~60 FPS
var stopwatch = System.Diagnostics.Stopwatch.StartNew();


while (!quit)
{
    var startTime = stopwatch.Elapsed;

    // Update physics
    physicsSystem.Update(dt);

    // Simulate rendering time (optional)
    // Thread.Sleep(1);

    var stopTime = stopwatch.Elapsed;
    dt = (float)(stopTime - startTime).TotalSeconds;

    // For demo purposes, quit after a few iterations
    if (stopwatch.Elapsed.TotalSeconds > 5.0)
        quit = true;
}

Console.WriteLine("Simulation finished.");