# 🧁 Cupcake Crush

A candy-themed match-3 puzzle game built from scratch with Unity and C#. Features an infinitely scrolling level map, a lock/progression system, and parametric level design for a complete game loop.

![Opening Screen](Screenshots/opening_screen.png)

## 🎮 Gameplay

Drag cupcakes to swap them with a neighbor, lining up 3 or more of the same color to pop them. When cupcakes pop, the ones above fall down to fill the gaps, and new cupcakes drop in from the top. Each level has its own target score and move limit — reach the goal before running out of moves to unlock the next level.

## ✨ Features

- **Match-3 mechanics:** Drag-to-swap input, horizontal/vertical match detection, cascade resolution, and board collapse/refill algorithms
- **Infinite level map:** Zigzag-arranged level buttons generated endlessly as the camera scrolls
- **Lock & progression system:** Persistent progress via PlayerPrefs; completing a level unlocks the next
- **Parametric level design:** Target score, move limit, rows/columns, and color count configurable per level
- **Win/lose flow:** Result-based Win/Lose screens with "next level" and "retry" options
- **Procedural asset generation:** All visuals (cupcakes, background, buttons, screens) generated through code
- **Android build:** Compiled and tested on a mobile device

## 🖼️ Screenshots

| Level Map | Gameplay | Win Screen |
|:---:|:---:|:---:|
| ![Map](Screenshots/map.png) | ![Gameplay](Screenshots/gameplay.png) | ![Win](Screenshots/win.png) |

## 🛠️ Built With

- **Unity 6** (2D)
- **C#**
- **PlayerPrefs** — cross-scene data and progress persistence
- Grid-based board architecture, manager (separation of concerns) pattern

## 🚀 Getting Started

1. Clone this repository
2. Open it via Unity Hub (**Add → select the project**, Unity 6)
3. Press Play from the `SampleScene` (main menu)

## 📸 Development Notes

The match-3 board is stored in a 2D array; each cupcake knows its own grid coordinates, while board logic is centralized in a single manager. Level buttons, cupcakes, and HUD elements are generated procedurally at runtime.

---

*Built as a learning / portfolio project.* 🧁
