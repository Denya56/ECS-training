using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Systems;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using static ESC_training.Config;
using Transform2D = ESC_training.Components.Transform2D;

var coordinator = new Coordinator();

coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody2D>();
coordinator.RegisterComponent<Transform2D>();
coordinator.RegisterComponent<Rendarable>();
coordinator.RegisterComponent<Square>();
coordinator.RegisterComponent<Circle>();

var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
physicsSystem.Coordinator = coordinator;

var renderingSystem = coordinator.RegisterSystem<RenderingSystem>();
renderingSystem.Coordinator = coordinator;

var physicsSignature = new Signature();
physicsSignature.AddComponent(coordinator.GetComponentType<Gravity>());
physicsSignature.AddComponent(coordinator.GetComponentType<RigidBody2D>());
physicsSignature.AddComponent(coordinator.GetComponentType<Transform2D>());
coordinator.SetSystemSignature<PhysicsSystem>(physicsSignature);

var renderingSignature = new Signature();
renderingSignature.AddComponent(coordinator.GetComponentType<Rendarable>());
renderingSignature.AddComponent(coordinator.GetComponentType<Transform2D>());
coordinator.SetSystemSignature<RenderingSystem>(renderingSignature);

var entities = new List<Entity>(MAX_ENTITIES);
var rand = new Random();

for (int i = 0; i < MAX_ENTITIES; i++)
{
    var entity = coordinator.CreateEntity();

    coordinator.AddComponent(entity, new Gravity
    {
        Force = new Vector2(0f, 5f + (float)rand.NextDouble() * 10f)   
    });

    coordinator.AddComponent(entity, new RigidBody2D
    {
        Velocity = Vector2.Zero,
        Acceleration = Vector2.Zero
    });

    float scale = (float)(3 + rand.NextDouble() * 2);
    coordinator.AddComponent(entity, new Transform2D
    {
        Position = new Vector2(
            (float)(rand.NextDouble() * WINDOW_WIDTH),
            (float)(-rand.NextDouble() * 300 - 100)
        ),

        Scale = new Vector2(scale, scale)
    });

    coordinator.AddComponent(entity, new Rendarable
    {
        Color = new Vector4(
            (float)rand.NextDouble(),
            (float)rand.NextDouble(),
            (float)rand.NextDouble(),
            1f
        )
    });

    if (rand.NextDouble() > 0.5)
    {
        coordinator.AddComponent(entity, new Square
        {
            SideLength = (int)(5 + rand.NextDouble() * 10)
        });
    }
    else
    {
        coordinator.AddComponent(entity, new Circle
        {
            Radius = (int)(5 + rand.NextDouble() * 10)
        });
    }
    entities.Add(entity);
}

Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Raylib Window");
var swTotal = Stopwatch.StartNew();
List<int> fpsHistory = new();

while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();

    var swPhysics = Stopwatch.StartNew();
    physicsSystem.Update(dt);
    swPhysics.Stop();

    var swRender = Stopwatch.StartNew();
    renderingSystem.Update(dt);
    swRender.Stop();

    int fps = Raylib.GetFPS();
    fpsHistory.Add(fps);
    float avgFps = (float)fpsHistory.Average();

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.DarkGray);

    foreach (var command in renderingSystem.Commands)
    {
        if (command.Command == RenderingSystem.RenderCommand.CommandType.DrawRectangle)
        {
            Raylib.DrawRectangle(
                (int)command.Position.X,
                (int)command.Position.Y,
                command.Size * (int)command.Scale.X,
                command.Size * (int)command.Scale.Y,
                Raylib.ColorFromNormalized(command.Color));
        }
        else if (command.Command == RenderingSystem.RenderCommand.CommandType.DrawCircle)
        {
            Raylib.DrawCircle(
                (int)command.Position.X,
                (int)command.Position.Y,
                command.Size * command.Scale.X,
                Raylib.ColorFromNormalized(command.Color));
        }
    }

    long totalMemory = GC.GetTotalMemory(false);

    string stats = $"FPS: {fps} (Avg: {avgFps:F1})\n" +
                   $"Physics: {swPhysics.Elapsed.TotalMilliseconds:F2} ms\n" +
                   $"Rendering: {swRender.Elapsed.TotalMilliseconds:F2} ms\n" +
                   $"Memory: {totalMemory / 1024.0 / 1024.0:F2} MB\n" +
                   $"Entities: {entities.Count}";

    Raylib.DrawText(stats, 10, 30, 20, Color.White);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();