# ESC-training: A C# ECS Framework

A simple Entity-Component-System (ECS) framework in C# for learning game architecture, inspired by [Austin Morlan's C++ ECS framework](https://austinmorlan.com/posts/entity_component_system/).

---

## Features

* Core ECS components: **Entities, Components, Systems**, and a **Coordinator**.
* Example systems for **physics** and **rendering**, demonstrated in a 2D demo using **Raylib2D**.
* Components like `Transform2D`, `RigidBody2D`, `Gravity`, `Renderable`.
* Handles thousands of entities efficiently.
* Uses contiguous component arrays for memory-efficient storage and fast updates.

---

## Usage Examples

### Register Components

```csharp
coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody2D>();
coordinator.RegisterComponent<Transform2D>();
coordinator.RegisterComponent<Renderable>();
coordinator.RegisterComponent<Square>();
coordinator.RegisterComponent<Circle>();
```

### Register Systems

```csharp
var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
physicsSystem.Coordinator = coordinator;

var physicsSignature = new Signature();
physicsSignature.AddComponent(coordinator.GetComponentType<Gravity>());
physicsSignature.AddComponent(coordinator.GetComponentType<RigidBody2D>());
physicsSignature.AddComponent(coordinator.GetComponentType<Transform2D>());
coordinator.SetSystemSignature<PhysicsSystem>(physicsSignature);
```

### Create Entities and Add Components

```csharp
var entity = coordinator.CreateEntity();

coordinator.AddComponent(entity, new Gravity { Force = new Vector2(0f, 9.8f) });
coordinator.AddComponent(entity, new RigidBody2D { Velocity = Vector2.Zero });
coordinator.AddComponent(entity, new Transform2D { Position = new Vector2(0, 0), Scale = Vector2.One });
coordinator.AddComponent(entity, new Renderable { Color = new Vector4(1f, 0f, 0f, 1f) });
```

### Main Loop

```csharp
while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();

    physicsSystem.Update(dt);
    renderingSystem.Update(dt);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.DarkGray);

    foreach (var command in renderingSystem.Commands)
    {
        if (command.Command == RenderCommand.CommandType.DrawRectangle)
            Raylib.DrawRectangle((int)command.Position.X, (int)command.Position.Y,
                                 command.Size, command.Size,
                                 Raylib.ColorFromNormalized(command.Color));
    }

    Raylib.EndDrawing();
}
```


https://github.com/user-attachments/assets/a3f0c054-438c-496c-afc1-9352339b94d2

