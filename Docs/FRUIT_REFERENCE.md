# Fruit Reference Guide

Quick-reference data for all 11 fruits in the Watermelon Merge game. Use this when creating derived classes, setting up prefabs, and assigning FruitFactory slots.

---

## Merge Chain

```
Cherry + Cherry = Strawberry
         |
Strawberry + Strawberry = Grape
         |
Grape + Grape = Orange
         |
Orange + Orange = Dekopon
         |
Dekopon + Dekopon = Apple
         |
Apple + Apple = Pear
         |
Pear + Pear = Peach
         |
Peach + Peach = Pineapple
         |
Pineapple + Pineapple = Melon
         |
Melon + Melon = Watermelon
         |
Watermelon + Watermelon = (nothing -- max tier!)
```

Two fruits of the **same tier** collide and produce **one fruit of the next tier** at their midpoint. Watermelon is the final tier and cannot merge further (`GetMergeResultTier()` returns `-1`).

---

## Complete Data Table

| Tier | Fruit | Size | Points | Color (RGBA) | Color Description | Merge Result | Session | Status |
|---:|---|---:|---:|---|---|---|---|---|
| 0 | Cherry | 0.50 | 1 | `(0.85, 0.12, 0.15, 1.0)` | Bright red | Strawberry (1) | 1 | Provided |
| 1 | Strawberry | 0.65 | 3 | `(0.95, 0.30, 0.35, 1.0)` | Pink-red | Grape (2) | 1 | Provided |
| 2 | Grape | 0.80 | 6 | `(0.55, 0.27, 0.68, 1.0)` | Purple | Orange (3) | 1 | Provided |
| 3 | Orange | 1.00 | 10 | `(1.00, 0.65, 0.00, 1.0)` | Orange | Dekopon (4) | 2 | Student TODO |
| 4 | Dekopon | 1.15 | 15 | `(1.00, 0.80, 0.20, 1.0)` | Yellow-orange | Apple (5) | 2 | Student TODO |
| 5 | Apple | 1.35 | 21 | `(0.85, 0.15, 0.20, 1.0)` | Dark red | Pear (6) | 2 | Student TODO |
| 6 | Pear | 1.50 | 28 | `(0.75, 0.85, 0.20, 1.0)` | Yellow-green | Peach (7) | 3 | Student TODO |
| 7 | Peach | 1.70 | 36 | `(1.00, 0.80, 0.70, 1.0)` | Peach | Pineapple (8) | 3 | Student TODO |
| 8 | Pineapple | 1.90 | 45 | `(0.95, 0.85, 0.15, 1.0)` | Yellow | Melon (9) | 3 | Student TODO |
| 9 | Melon | 2.15 | 55 | `(0.55, 0.85, 0.40, 1.0)` | Green | Watermelon (10) | 3 | Student TODO |
| 10 | Watermelon | 2.50 | 66 | `(0.20, 0.75, 0.20, 1.0)` | Dark green | None (-1) | 3 | Student TODO |

**Pattern notes:**
- Size increases gradually from 0.50 (Cherry) to 2.50 (Watermelon)
- Points increase following a triangular number pattern (1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66)
- Colors progress through a fruit-inspired spectrum: red, pink, purple, orange, yellow, dark red, yellow-green, peach, yellow, green, dark green
- All alpha values are 1.0 (fully opaque)

---

## Drop Rules

The `DropController` only spawns fruits from **tier 0 through maxDropTier** (default: **2**, Cherry through Grape). Students increase this as they add more fruits (up to 4 once Dekopon exists). This is controlled by the `maxDropTier` field on the DropController Inspector.

| Tier | Fruit | Can Be Dropped? |
|---:|---|---|
| 0 | Cherry | Yes |
| 1 | Strawberry | Yes |
| 2 | Grape | Yes |
| 3 | Orange | Yes |
| 4 | Dekopon | Yes |
| 5 | Apple | No -- merge only |
| 6 | Pear | No -- merge only |
| 7 | Peach | No -- merge only |
| 8 | Pineapple | No -- merge only |
| 9 | Melon | No -- merge only |
| 10 | Watermelon | No -- merge only |

Fruits at tier 5 and above can **only** be created through merging. This ensures the player must build up through the merge chain to reach Watermelon.

The drop tier is randomly selected each time: `Random.Range(0, maxDropTier + 1)`. All droppable tiers have equal probability.

---

## Prefab Setup Instructions

Follow these steps for **each** new fruit class you create. Repeat this process for every fruit (Orange, Dekopon, Apple, Pear, Peach, Pineapple, Melon, Watermelon).

### Step 1: Create an Empty GameObject
1. In the Unity Hierarchy, right-click and select **Create Empty**
2. Rename it to the fruit name (e.g., `Orange`)

### Step 2: Add a SpriteRenderer
1. With the GameObject selected, click **Add Component** in the Inspector
2. Search for and add **Sprite Renderer**
3. For the Sprite field, assign Unity's built-in circle sprite:
   - Click the small circle icon next to the Sprite field
   - Search for `Circle` or `Knob` in the sprite selector
   - Select any round sprite (the color will be overridden by the script)

### Step 3: Add Physics Components
1. Click **Add Component** and add **Rigidbody 2D**
   - Gravity Scale: `1` (default is fine)
   - Collision Detection: `Continuous` (recommended for fast-moving small fruits)
2. Click **Add Component** and add **Circle Collider 2D**
   - Radius: `0.5` (the script's `fruitSize` will scale the entire transform)

### Step 4: Add the Fruit Script
1. Click **Add Component** and search for the fruit's class name (e.g., `Orange`)
2. Add the script
3. **Important:** Add the **specific derived class** (Orange), not the base `Fruit` class

### Step 5: Save as Prefab
1. Create a folder for prefabs if one does not exist (e.g., `Assets/_Project/Prefabs/Fruits/`)
2. Drag the GameObject from the **Hierarchy** into that folder in the **Project** window
3. This creates a prefab asset
4. Delete the instance from the Hierarchy (the prefab in the Project window is what matters)

### Step 6: Assign to FruitFactory
1. Select the GameObject in the scene that has the **FruitFactory** component
2. In the Inspector, find the **Fruit Prefabs** array
3. Drag your new prefab into the correct slot (see FruitFactory Inspector Layout below)

### Verification
After completing all steps, press **Play** and test:
- Does the fruit appear when dropped? (If not: check that the script is attached and `base.Awake()` is called)
- Does it have the correct color and size? (If not: check your `fruitColor` and `fruitSize` values)
- Does it merge with another of the same type? (If not: check that the `tier` value is correct and the prefab is in the right FruitFactory slot)
- Does the merge produce the correct next fruit? (If not: check that the next tier's prefab is also assigned)

---

## FruitFactory Inspector Layout

The `FruitFactory` component has a single array called **Fruit Prefabs** with 11 slots (indices 0-10). Each slot must contain the prefab for the corresponding tier.

```
Fruit Prefabs
  Size: 11
  +-----------------------------------------+
  | Element 0   [ Cherry Prefab        ]    |  <- Provided
  | Element 1   [ Strawberry Prefab    ]    |  <- Provided
  | Element 2   [ Grape Prefab         ]    |  <- Provided
  | Element 3   [ Orange Prefab        ]    |  <- Session 2
  | Element 4   [ Dekopon Prefab       ]    |  <- Session 2
  | Element 5   [ Apple Prefab         ]    |  <- Session 2
  | Element 6   [ Pear Prefab          ]    |  <- Session 3
  | Element 7   [ Peach Prefab         ]    |  <- Session 3
  | Element 8   [ Pineapple Prefab     ]    |  <- Session 3
  | Element 9   [ Melon Prefab         ]    |  <- Session 3
  | Element 10  [ Watermelon Prefab    ]    |  <- Session 3
  +-----------------------------------------+
```

**Important:** The array index MUST match the fruit's `tier` value. If Orange has `tier = 3`, its prefab goes in `Element 3`. A mismatch will cause the merge chain to break (merging two Oranges would spawn the wrong fruit).

**Missing prefab warnings:** If a slot is empty (None), the game will log a warning when a merge tries to create that tier:
```
FruitFactory: No prefab assigned for tier X!
Create the fruit class and prefab, then drag it into slot X in the Inspector.
```

This warning tells you exactly which slot needs a prefab. Follow the Prefab Setup Instructions above to fix it.
