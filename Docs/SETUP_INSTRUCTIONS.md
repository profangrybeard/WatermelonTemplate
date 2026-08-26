# Merge Template - Setup Instructions

## Starting Point

You have the scripts, a configured camera, and a **Managers** GameObject. This guide walks you
through building out the rest of the Unity scene so the game is fully playable with 3 object
types (TierZero, TierOne, TierTwo) before students touch any code.

**Time estimate:** 25-35 minutes

> **This setup is required before the game will run.** `GameScene.unity` ships with only a
> camera (Step 2, done) and a Managers object (Step 4, done) -- no walls, no sprites, no
> prefabs, no DropController. Pressing Play on a fresh clone shows an empty screen. That is
> expected; work through the steps below first.

**Already done -- verify only:** Step 2 (camera) and Step 4 (Managers object).
**Still to do:** Steps 3, 5, 6, 7-10 (prefabs) and Step 11 (wiring).

### Project configuration (already done -- verify only)

| Setting | Value | Where |
|---|---|---|
| Unity version | `6000.5.9f1` | `ProjectSettings/ProjectVersion.txt` |
| Render pipeline | URP 17.5.0, **2D renderer** (`Renderer2DData`) | `Assets/_Project/Render/` |
| Active Input Handling | **Input System Package (New)** only | Player settings |
| Input System | 1.20.0 | `Packages/manifest.json` |

**Active Input Handling is New-only on purpose.** Legacy `Input.GetKeyDown` /
`Input.mousePosition` will not compile. Input is bound through `InputAction` fields on
`DropController` and `GameManager` -- there is no `.inputactions` asset and no `PlayerInput`
component to wire up. If a student pastes tutorial code that fails to compile, that is the
setting doing its job; see `Docs/TROUBLESHOOTING.md` issues 15 and 16.

---

## Step 1: Open the Project

1. Open Unity Hub
2. Click **Open** and navigate to the project folder
3. Unity 6000.5.9f1 should be selected (or your matching version)
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

## Step 3: Create the Container Walls

The container is made of 3 manually placed GameObjects with BoxCollider2D components. No script is needed.

### LeftWall
1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **LeftWall**
3. Set **Position** to (-3.25, 0, 0)
4. Click **Add Component** → add **Box Collider 2D**
   - Size: (0.5, 10)
5. Optionally add a **Sprite Renderer** for visibility (assign a square sprite, set color to dark gray)

### RightWall
1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **RightWall**
3. Set **Position** to (3.25, 0, 0)
4. Click **Add Component** → add **Box Collider 2D**
   - Size: (0.5, 10)
5. Optionally add a **Sprite Renderer** for visibility

### BottomWall
1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **BottomWall**
3. Set **Position** to (0, -5.25, 0)
4. Click **Add Component** → add **Box Collider 2D**
   - Size: (7, 0.5)
5. Optionally add a **Sprite Renderer** for visibility

Press **Play** briefly to verify objects do not fall through the floor, then **Stop Play**.

---

## Step 4: The Managers GameObject — already in the scene

**This step is done.** `GameScene.unity` ships with a **Managers** GameObject at (0, 0, 0)
carrying both **GameManager** and **MergeObjectFactory**. Select it in the Hierarchy and
confirm:

| Component | Expected state |
|---|---|
| GameManager | Merge Object Factory: `None`, Drop Controller: `None` (wired in Step 11) |
| GameManager → Input | *Restart* action, Button, bound to `<Keyboard>/r` |
| MergeObjectFactory | Object Prefabs: 5 empty slots |

The references are deliberately empty — Step 11 wires them once everything exists. The
Restart binding comes from the script, so there is nothing to configure there either.

> **Do not add a second GameManager.** If you create another one by hand, `MergeObject` looks
> the manager up with `FindAnyObjectByType<GameManager>()` and may find the wrong one — merges
> would then score against a manager that nothing else references, and the score you see would
> never change.

<details>
<summary>If the Managers object is missing (someone deleted it), rebuild it</summary>

1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **Managers**
3. Set **Position** to (0, 0, 0)
4. Click **Add Component** → search for **GameManager** → add it
5. Click **Add Component** → search for **MergeObjectFactory** → add it

Leave the object/factory references empty.

</details>

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
   - Max Drop Tier: **2** (default — only TierZero/TierOne/TierTwo exist at start)
6. Under **Input**, confirm two actions are already present:
   - **Aim** — Value / Vector2, bound to `<Mouse>/position`
   - **Drop** — Button, bound to `<Mouse>/leftButton`

   These defaults come from the script, so they are filled in the moment you add the
   component. If either binding list is empty, input will silently do nothing — see
   `Docs/TROUBLESHOOTING.md` issue 16.

---

## Step 6: Create the Circle Sprite

Merge objects need a circle sprite. You can create one or use Unity's built-in:

### Option A: Use Unity's Built-in Circle
1. In any SpriteRenderer's Sprite field, click the circle picker (dot icon)
2. Search for **Knob** — this is a built-in circle sprite that ships with Unity UI
3. Remember this sprite name — you'll use it for all object prefabs

### Option B: Create Your Own
1. In any image editor, create a white circle on transparent background (128x128 px is fine)
2. Save as PNG
3. Drag into `Assets/_Project/Sprites/` folder (create the folder first)
4. In Import Settings: Texture Type → **Sprite (2D and UI)**, Pixels Per Unit → 128
5. Click **Apply**

---

## Step 7: Create the TierZero Prefab

1. In the Hierarchy, right-click → **Create Empty**
2. Rename it to **TierZero**
3. Set **Position** to (0, 0, 0)

### Add Components:
4. **Add Component** → **Sprite Renderer**
   - Sprite: Assign the circle sprite from Step 6
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

7. **Add Component** → search for **TierZero** → add the TierZero script

### Save as Prefab:
8. Create the folder `Assets/_Project/Prefabs/MergeObjects/` if it doesn't exist
9. Drag the **TierZero** GameObject from the Hierarchy into `Assets/_Project/Prefabs/MergeObjects/`
10. A prefab file appears — this is your TierZero prefab
11. **Delete the TierZero GameObject from the Hierarchy** (the prefab is saved)

---

## Step 8: Create the TierOne Prefab

**Shortcut: Duplicate the TierZero prefab and modify it.**

1. In `Assets/_Project/Prefabs/MergeObjects/`, select the TierZero prefab
2. **Ctrl+D** to duplicate it
3. Rename the duplicate to **TierOne**
4. Double-click **TierOne** to open the prefab editor
5. In the Inspector:
   - **Remove** the TierZero script component (right-click → Remove Component)
   - **Add Component** → search for **TierOne** → add it
6. Click the **back arrow** (top-left of Hierarchy) to exit prefab editor
7. That's it — the TierOne script sets its own size and color at runtime

---

## Step 9: Create the TierTwo Prefab

1. Duplicate the TierZero prefab again → rename to **TierTwo**
2. Open the prefab → remove TierZero script → add **TierTwo** script
3. Exit prefab editor

---

## Step 10: Assign Prefabs to MergeObjectFactory

1. Select the **Managers** GameObject in the Hierarchy
2. Find the **Merge Object Factory** component in the Inspector
3. You'll see **Object Prefabs** — an array with 5 slots (Element 0 through Element 4)
4. Drag the prefabs into the correct slots:
   - **Element 0:** TierZero prefab
   - **Element 1:** TierOne prefab
   - **Element 2:** TierTwo prefab
   - Elements 3-4: Leave empty (students fill these as they create new classes)

---

## Step 11: Wire Up Inspector References

### On the Managers GameObject:

**GameManager component:**
- Merge Object Factory: Drag the **Managers** GameObject into this slot (it has the MergeObjectFactory component)
- Drop Controller: Drag the **DropController** GameObject
- Game Over Line Y: 4.5
- Game Over Delay: 2

### On the DropController GameObject:

**DropController component:**
- Merge Object Factory: Drag the **Managers** GameObject
- Game Manager: Drag the **Managers** GameObject
- Max Drop Tier: Set to **2** (only 3 objects exist — TierZero=0, TierOne=1, TierTwo=2)

---

## Step 12: Test!

1. Press **Play**
2. **You should see:**
   - A box container with 3 walls
   - An object floating at the top following your mouse

3. **Test dropping:**
   - Click to drop an object
   - Move mouse to aim
   - Objects should fall and stack

4. **Test merging:**
   - Drop two TierZero objects near each other — they should merge into a TierOne
   - Drop two TierOne objects — they should merge into a TierTwo
   - Score should appear in the Console (Debug.Log) on each merge

5. **Test game over:**
   - Keep dropping objects until they stack above the game over line
   - After 2 seconds, game over should log to Console
   - Press R to restart

---

## Step 13: Physics Tuning (Optional)

If objects feel too bouncy, too heavy, or stack poorly:

### In Project Settings → Physics 2D:
- **Velocity Iterations:** 8 (default is 3 — increase for better stacking)
- **Position Iterations:** 8 (increase for more stable contacts)
- **Gravity:** (0, -9.81) is default. Try (0, -6) for a floatier feel.

### On Object Prefabs:
- **Linear Drag:** Increase for slower settling (try 0.5-1.0)
- **Mass:** Keep at 1 for all objects (mass doesn't affect falling speed in Unity)
- **Collision Detection:** Keep Continuous to prevent fast objects passing through walls

### Create a Physics Material 2D (Optional):
1. Right-click in Assets → **Create** → **2D** → **Physics Material 2D**
2. Name it **MergeObjectMaterial**
3. Set Friction: 0.4, Bounciness: 0.1
4. Assign it to each object prefab's CircleCollider2D → Material slot
5. Optionally assign to the container wall BoxCollider2D Material slots for wall friction too

---

## Final Hierarchy Should Look Like:

```
Main Camera
LeftWall                     [BoxCollider2D]
RightWall                    [BoxCollider2D]
BottomWall                   [BoxCollider2D]
Managers                     [GameManager, MergeObjectFactory]
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
      InputSystem_Actions.inputactions   (Unity's stock sample — no script uses it)
    Materials/
    Render/
      New Universal Render Pipeline Asset.asset
      New Universal Render Pipeline Asset_Renderer.asset
    Prefabs/
      MergeObjects/
        TierZero.prefab
        TierOne.prefab
        TierTwo.prefab
    Scenes/
      GameScene.unity
    Scripts/
      MergeObjects/  (MergeObject.cs + derived classes)
      Managers/      (GameManager.cs, MergeObjectFactory.cs)
      Player/        (DropController.cs)
      README.md
    Sprites/
```

---

## Preparing for Students

Before distributing to students:

1. **Verify Max Drop Tier** — DropController should have Max Drop Tier = 2 (only 3 starter objects)
2. **Verify MergeObjectFactory** — Only slots 0-2 should have prefabs assigned
3. **Verify the game plays correctly** — Drop, merge, score in Console, game over all work
4. **Verify input** — mouse aims, left click drops, and `R` restarts after game over
5. **Save the scene** (Ctrl+S)
6. Commit to git

Students will increase Max Drop Tier as they add more objects.

---

## Quick Reference: Student Prefab Creation

When students create a new derived class, they need to:

1. Create the new `.cs` file with a class that inherits from `MergeObject`
2. Duplicate an existing object prefab
3. Open the duplicate, swap the script to the new class
4. Rename the prefab to match the class name
5. Drag it into the correct `MergeObjectFactory` slot
6. Test — the new object should appear with correct size/color and merge with others of the same type

Our Business is Fun
