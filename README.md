# **Fallin'**

This project is a functional prototype of a **Bullet Hell** game featuring infinite falling mechanics, developed to demonstrate the implementation and efficiency of the **Object Pooling** design pattern in Unity.

---

## ** Game Description**
The player controls a triangle (GrayBox) that must survive endless waves of obstacles falling from the top and launched diagonally from the sides. The objective is to survive as long as possible through incremental difficulty levels.

* **Movement:** WASD or Arrow Keys.
* **Core Mechanic:** Dodge incoming projectiles.
* **Progression:** Waves change every 20 seconds, increasing spawn frequency and complexity.

---

## ** Technical Implementation**

### **1. Object Pooling (Core Requirement)**
The official Unity API `UnityEngine.Pool` was utilized to manage the lifecycle of obstacles.
* **Class:** `ObjectPool.cs`
* **Optimization:** Instead of using `Instantiate()` and `Destroy()`, which cause CPU spikes and trigger the *Garbage Collector*, the game uses a pool of pre-instantiated objects that are toggled via `OnGet` and `OnRelease`.
* **Configuration:**
    * `DefaultCapacity`: 20 objects.
    * `MaxSize`: 100 objects.

### **2. Event-Driven Architecture**
To maintain decoupled and clean code, a static `EventManager` was implemented to communicate different systems without direct references:
* `OnPlayerHit`: Notifies the `LevelManager` to subtract lives.
* `OnWaveChanged`: Synchronizes the `UIManager` and the Spawner to update difficulty and UI elements.
* `OnGameOver`: Halts game logic and triggers the results screen.

### **3. Spawning Logic & Difficulty**
The `LevelManager` acts as the master controller:
* **Weighted Probability:** Obstacles have a higher probability of appearing at the top (initially 70%). This weight decreases in later waves to favor lateral attacks.
* **Anti-Repetition:** The system prevents spawning twice in the same spot consecutively to ensure a varied distribution of projectiles.
* **Wave Transitions:** A 5-second "calm" period is triggered before each wave to clear the screen, followed by a 2.5-second UI message to prepare the player.

---

## ** Project Structure**
```text
Assets/
 ├── Scripts/
 │    ├── EventManager.cs    # Decoupled communication (Actions)
 │    ├── LevelManager.cs    # Game state, Waves, and Spawning logic
 │    ├── ObjectPool.cs      # Implementation of UnityEngine.Pool API
 │    ├── PlayerController.cs # Movement and screen boundaries
 │    ├── FallingObject.cs   # Movement logic and return-to-pool behavior
 │    └── UIManager.cs       # HUD management and Game Over screen
 ├── Prefabs/
 │    └── Obstacle.prefab    # Pooled object with 2D Colliders
 └── Scenes/
      └── MainGame.unity     # Principal game scene
```
## Demo
You can watch a video of the game here:

[![Demo del Juego](https://img.youtube.com/vi/cpqur9hwO9s/0.jpg)](https://youtu.be/cpqur9hwO9s)

[Ver video en YouTube](https://youtu.be/cpqur9hwO9s)
