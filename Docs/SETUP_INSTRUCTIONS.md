# Watermelon Merge Template - Setup Instructions

## Starting Point

You have the scripts and nothing else. This guide walks you through setting up the Unity scene from scratch so the game is fully playable with 3 fruit types (Cherry, Strawberry, Grape) before students touch any code.

**Time estimate:** 30-40 minutes

---

## Step 1: Open the Project

1. Open Unity Hub
2. Click **Open** and navigate to the WatermelonTemplate folder
3. Unity 6000.0.63f1 should be selected (or your matching version)
4. Open the project — it will compile the scripts automatically
5. Open `Assets/_Project/Scenes/GameScene.unity`

---

## Step 2: Camera Setup

1. Select **Main Camera** in the Hierarchy
2. In the Inspector:
   - **Projection:** Orthographic
   - **Size:** 6
   - **Position:** (0, 0, -10)
   - **Background Color:** Dark color of your choice (e.g., #1A1A2E)

---

## Step 3: Create the Container

1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **Container**
3. Set **Position** to (0, -1, 0)
4. Click **Add Component** → search for **ContainerSetup** → add it
5. In the ContainerSetup Inspector, verify defaults:
   - Width: 6
   - Height: 8
   - Wall Thickness: 0.5
   - Wall Color: your choice (default dark gray works)
6. Press **Play** briefly to verify — you should see 3 walls forming a box with an open top
7. Stop Play

---

## Step 4: Create the Managers GameObject

1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **Managers**
3. Set **Position** to (0, 0, 0)
4. Click **Add Component** → search for **GameManager** → add it
5. Click **Add Component** → search for **FruitFactory** → add it
6. Click **Add Component** → search for **ScoreManager** → add it

Leave the Inspector references empty for now — we'll wire them up after creating everything.

---

## Step 5: Create the DropController

1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **DropController**
3. Set **Position** to (0, 0, 0)
4. Click **Add Component** → search for **DropController** → add it
5. In the DropController Inspector, verify defaults:
   - Drop Cooldown: 0.5
   - Drop Line Y: 4
   - Min X: -2.5
   - Max X: 2.5
   - Max Drop Tier: **2** (default — only Cherry/Strawberry/Grape exist at start)
   - Keyboard Aim Speed: 5

---

## Step 6: Create the Canvas and UI

1. In the Hierarchy, right-click → **UI** → **Canvas**
2. The Canvas and EventSystem will be created automatically
3. Select the **Canvas** and set:
   - **Canvas Scaler** → UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: 1920 x 1080

### Score Text
4. Right-click on **Canvas** → **UI** → **Legacy** → **Text**
5. Rename it to **ScoreText**
6. In the Rect Transform:
   - Anchor: Top-Left
   - Position: (120, -40, 0)
   - Width: 200, Height: 50
7. In the Text component:
   - Text: "Score: 0"
   - Font Size: 28
   - Color: White
   - Alignment: Left

### Game Over Text
8. Right-click on **Canvas** → **UI** → **Legacy** → **Text**
9. Rename it to **GameOverText**
10. In the Rect Transform:
    - Anchor: Center
    - Position: (0, 0, 0)
    - Width: 400, Height: 200
11. In the Text component:
    - Text: "Game Over!"
    - Font Size: 36
    - Color: Red or White
    - Alignment: Center
12. **Uncheck the GameObject's active checkbox** (top-left of Inspector) — it starts hidden

### Highest Fruit Text (Optional)
13. Right-click on **Canvas** → **UI** → **Legacy** → **Text**
14. Rename it to **HighestFruitText**
15. In the Rect Transform:
    - Anchor: Top-Right
    - Position: (-120, -40, 0)
    - Width: 200, Height: 50
16. In the Text component:
    - Text: "Best: --"
    - Font Size: 24
    - Color: White
    - Alignment: Right

---

## Step 7: Create the Circle Sprite

Fruits need a circle sprite. You can create one or use Unity's built-in:

### Option A: Use Unity's Built-in Circle
1. In any SpriteRenderer's Sprite field, click the circle picker (dot icon)
2. Search for **Knob** — this is a built-in circle sprite that ships with Unity UI
3. Remember this sprite name — you'll use it for all fruit prefabs

### Option B: Create Your Own
1. In any image editor, create a white circle on transparent background (128x128 px is fine)
2. Save as PNG
3. Drag into `Assets/_Project/Sprites/` folder (create the folder first)
4. In Import Settings: Texture Type → **Sprite (2D and UI)**, Pixels Per Unit → 128
5. Click **Apply**

---

## Step 8: Create the Cherry Prefab

1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **Cherry**
3. Set **Position** to (0, 0, 0)

### Add Components:
4. **Add Component** → **Sprite Renderer**
   - Sprite: Assign the circle sprite from Step 7
   - Color: Leave white (the script sets color at runtime)
   - Sorting Order: 0

5. **Add Component** → **Rigidbody 2D**
   - Body Type: Dynamic
   - Gravity Scale: 1
   - Mass: 1
   - Linear Drag: 0.5
   - Angular Drag: 0.5
   - Collision Detection: Continuous

6. **Add Component** → **Circle Collider 2D**
   - Radius: 0.5
   - **Is Trigger: UNCHECKED** (must be false for OnCollisionEnter2D)

7. **Add Component** → search for **Cherry** → add the Cherry script

### Save as Prefab:
8. Create the folder `Assets/_Project/Prefabs/Fruits/` if it doesn't exist
9. Drag the **Cherry** GameObject from the Hierarchy into `Assets/_Project/Prefabs/Fruits/`
10. A prefab file appears — this is your Cherry prefab
11. **Delete the Cherry GameObject from the Hierarchy** (the prefab is saved)

---

## Step 9: Create the Strawberry Prefab

**Shortcut: Duplicate the Cherry prefab and modify it.**

1. In `Assets/_Project/Prefabs/Fruits/`, select the Cherry prefab
2. **Ctrl+D** to duplicate it
3. Rename the duplicate to **Strawberry**
4. Double-click **Strawberry** to open the prefab editor
5. In the Inspector:
   - **Remove** the Cherry script component (right-click → Remove Component)
   - **Add Component** → search for **Strawberry** → add it
6. Click the **back arrow** (top-left of Hierarchy) to exit prefab editor
7. That's it — the Strawberry script sets its own size and color at runtime

---

## Step 10: Create the Grape Prefab

1. Duplicate the Cherry prefab again → rename to **Grape**
2. Open the prefab → remove Cherry script → add **Grape** script
3. Exit prefab editor

---

## Step 11: Assign Prefabs to FruitFactory

1. Select the **Managers** GameObject in the Hierarchy
2. Find the **Fruit Factory** component in the Inspector
3. You'll see **Fruit Prefabs** — an array with 11 slots (Element 0 through Element 10)
4. Drag the prefabs into the correct slots:
   - **Element 0:** Cherry prefab
   - **Element 1:** Strawberry prefab
   - **Element 2:** Grape prefab
   - Elements 3-10: Leave empty (students fill these as they create fruits)

---

## Step 12: Wire Up Inspector References

### On the Managers GameObject:

**GameManager component:**
- Fruit Factory: Drag the **Managers** GameObject into this slot (it has the FruitFactory component)
- Drop Controller: Drag the **DropController** GameObject
- Score Manager: Drag the **Managers** GameObject (it has the ScoreManager component)
- Game Over Line Y: 4.5
- Game Over Delay: 2

**ScoreManager component:**
- Score Text: Drag **ScoreText** from the Canvas
- Game Over Text: Drag **GameOverText** from the Canvas
- Highest Fruit Text: Drag **HighestFruitText** from the Canvas (if you created it)

### On the DropController GameObject:

**DropController component:**
- Fruit Factory: Drag the **Managers** GameObject
- Game Manager: Drag the **Managers** GameObject
- Max Drop Tier: Set to **2** (only 3 fruits exist — Cherry=0, Strawberry=1, Grape=2)

---

## Step 13: Test!

1. Press **Play**
2. **You should see:**
   - A box container with 3 walls
   - A fruit floating at the top following your mouse
   - Score text in the top-left

3. **Test dropping:**
   - Click or press Space to drop a fruit
   - Move mouse or use A/D keys to aim
   - Fruits should fall and stack

4. **Test merging:**
   - Drop two Cherries near each other — they should merge into a Strawberry
   - Drop two Strawberries — they should merge into a Grape
   - Score should increase on each merge

5. **Test game over:**
   - Keep dropping fruits until they stack above the red line
   - After 2 seconds, "Game Over" text should appear
   - Press R to restart

---

## Step 14: Physics Tuning (Optional)

If fruits feel too bouncy, too heavy, or stack poorly:

### In Project Settings → Physics 2D:
- **Velocity Iterations:** 8 (default is 3 — increase for better stacking)
- **Position Iterations:** 8 (increase for more stable contacts)
- **Gravity:** (0, -9.81) is default. Try (0, -6) for a floatier feel.

### On Fruit Prefabs:
- **Linear Drag:** Increase for slower settling (try 0.5-1.0)
- **Mass:** Keep at 1 for all fruits (mass doesn't affect falling speed in Unity)
- **Collision Detection:** Keep Continuous to prevent fast fruits passing through walls

### Create a Physics Material 2D (Optional):
1. Right-click in Assets → **Create** → **2D** → **Physics Material 2D**
2. Name it **FruitMaterial**
3. Set Friction: 0.4, Bounciness: 0.1
4. Assign it to each fruit prefab's CircleCollider2D → Material slot
5. Optionally assign to ContainerSetup's Wall Material slot for wall friction too

---

## Final Hierarchy Should Look Like:

```
Main Camera
Canvas
  ScoreText
  GameOverText
  HighestFruitText (optional)
  EventSystem
Container                    [ContainerSetup]
  LeftWall    (auto-created at runtime)
  RightWall   (auto-created at runtime)
  BottomWall  (auto-created at runtime)
Managers                     [GameManager, FruitFactory, ScoreManager]
DropController               [DropController]
```

## Final Project Window Should Look Like:

```
Assets/
  _Project/
    Animations/
    Audio/
      Music/
      SFX/
    Fonts/
    Input/
      InputSystem_Actions.inputactions
    Materials/
    Prefabs/
      Fruits/
        Cherry.prefab
        Strawberry.prefab
        Grape.prefab
    Scenes/
      GameScene.unity
    Scripts/
      Fruits/        (Fruit.cs + 11 derived classes)
      Managers/      (GameManager.cs, FruitFactory.cs, ScoreManager.cs)
      Player/        (DropController.cs)
      Utils/         (ContainerSetup.cs)
      README.md
    Sprites/
```

---

## Preparing for Students

Before distributing to students:

1. **Verify Max Drop Tier** — DropController should have Max Drop Tier = 2 (only 3 starter fruits)
2. **Verify FruitFactory** — Only slots 0-2 should have prefabs assigned
3. **Verify the game plays correctly** — Drop, merge, score, game over all work
4. **Save the scene** (Ctrl+S)
5. Commit to git

Students will increase Max Drop Tier as they add more fruits (up to 4 once Dekopon exists).

---

## Quick Reference: Student Prefab Creation

When students create a new fruit class (e.g., Orange), they need to:

1. Complete the TODO in `Orange.cs`
2. Duplicate an existing fruit prefab
3. Open the duplicate, swap the script to **Orange**
4. Rename the prefab to **Orange**
5. Drag it into FruitFactory slot **3**
6. Test — Orange should appear with correct size/color and merge with other Oranges
