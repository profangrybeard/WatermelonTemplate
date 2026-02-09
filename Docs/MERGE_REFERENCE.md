# Merge Reference Guide

How to create your own merge objects in the Merge game. Use this when creating derived classes, setting up prefabs, and assigning MergeObjectFactory slots.

---

## Merge Chain Overview

```
TierZero + TierZero = TierOne
         |
TierOne + TierOne = TierTwo
         |
TierTwo + TierTwo = (your next tier)
         |
... and so on ...
         |
Final Tier + Final Tier = (nothing -- max tier!)
```

Two objects of the **same tier** collide and produce **one object of the next tier** at their midpoint. The final tier in the chain cannot merge further (`GetMergeResultTier()` returns `-1`).

---

## Provided Example Tiers

These three classes are given as complete reference examples. Study the pattern before creating your own.

| Tier | Class | objectName | objectSize | Points | Color (RGBA) | Color Description | Merge Result |
|---:|---|---|---:|---:|---|---|---|
| 0 | TierZero | TierZero | 0.50 | 1 | `(0.85, 0.12, 0.15, 1.0)` | Bright red | TierOne (1) |
| 1 | TierOne | TierOne | 0.65 | 3 | `(0.95, 0.30, 0.35, 1.0)` | Pink-red | TierTwo (2) |
| 2 | TierTwo | TierTwo | 0.80 | 6 | `(0.55, 0.27, 0.68, 1.0)` | Purple | Your tier 3 |

**Pattern notes:**
- Size increases gradually with each tier
- Points increase following a triangular number pattern (1, 3, 6, 10, 15, 21, ...)
- All alpha values are 1.0 (fully opaque)
- Each class sets exactly 5 fields: `tier`, `objectName`, `pointValue`, `objectSize`, `objectColor`

---

## Creating Your Own Merge Object Class

Follow this pattern for every new tier you add. Use the provided examples (TierZero, TierOne, TierTwo) as reference.

### Step 1: Create the C# Script

1. In `Assets/_Project/Scripts/MergeObjects/`, create a new C# script
2. Name it after your tier (e.g., `TierThree.cs`)
3. Write the class following this pattern:

```csharp
using UnityEngine;

public class TierThree : MergeObject
{
    protected override void Awake()
    {
        tier = 3;                                       // Must be unique -- next after TierTwo
        objectName = "TierThree";                       // Display name
        pointValue = 10;                                // Higher tiers = more points
        objectSize = 1.0f;                              // Bigger than previous tier
        objectColor = new Color(1.0f, 0.65f, 0.0f);    // Your chosen color (RGBA 0-1)

        base.Awake();   // REQUIRED -- caches component references
    }
}
```

**Important rules:**
- Your class MUST inherit from `MergeObject` (the `: MergeObject` part)
- Your `Awake()` MUST be `protected override void Awake()`
- You MUST call `base.Awake()` as the LAST line
- Your `tier` value MUST be unique and sequential (no gaps, no duplicates)

### Step 2: Create the Final Tier (Special Case)

Your highest tier needs one extra override to stop the merge chain:

```csharp
public class MyFinalTier : MergeObject
{
    protected override void Awake()
    {
        tier = 4;                                       // Whatever your final tier number is
        objectName = "MyFinalTier";
        pointValue = 15;
        objectSize = 1.15f;
        objectColor = new Color(1.0f, 0.80f, 0.20f);

        base.Awake();
    }

    // This override tells the merge system: stop here, no further merging
    public override int GetMergeResultTier()
    {
        return -1;
    }
}
```

Only the LAST tier in your chain needs `GetMergeResultTier()`. All others use the default (`tier + 1`).

---

## Prefab Setup Instructions

Follow these steps for **each** new class you create. Repeat this process for every new tier.

### Step 1: Create an Empty GameObject
1. In the Unity Hierarchy, right-click and select **Create Empty**
2. Rename it to match your class name (e.g., `TierThree`)

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
   - Collision Detection: `Continuous` (recommended for fast-moving small objects)
2. Click **Add Component** and add **Circle Collider 2D**
   - Radius: `0.5` (the script's `objectSize` will scale the entire transform)

### Step 4: Add Your Script
1. Click **Add Component** and search for your class name (e.g., `TierThree`)
2. Add the script
3. **Important:** Add the **specific derived class** (TierThree), not the base `MergeObject` class

### Step 5: Save as Prefab
1. Navigate to `Assets/_Project/Prefabs/MergeObjects/` in the Project window
2. Drag the GameObject from the **Hierarchy** into that folder
3. This creates a prefab asset
4. Delete the instance from the Hierarchy (the prefab in the Project window is what matters)

### Step 6: Assign to MergeObjectFactory
1. Select the GameObject in the scene that has the **MergeObjectFactory** component
2. In the Inspector, find the **Object Prefabs** array
3. If the array is not big enough, increase its size
4. Drag your new prefab into the correct slot (the slot index MUST match the tier value)

### Verification
After completing all steps, press **Play** and test:
- Does the object appear when dropped? (If not: check that the script is attached and `base.Awake()` is called)
- Does it have the correct color and size? (If not: check your `objectColor` and `objectSize` values)
- Does it merge with another of the same type? (If not: check that the `tier` value is correct and the prefab is in the right MergeObjectFactory slot)
- Does the merge produce the correct next object? (If not: check that the next tier's prefab is also assigned)

---

## How to Expand the MergeObjectFactory Array

The `MergeObjectFactory` component has a single array called **Object Prefabs** with 5 slots by default (indices 0-4). Each slot must contain the prefab for the corresponding tier.

```
Object Prefabs
  Size: 5 (expand as needed)
  +-----------------------------------------+
  | Element 0   [ TierZero Prefab       ]   |  <- Provided
  | Element 1   [ TierOne Prefab        ]   |  <- Provided
  | Element 2   [ TierTwo Prefab        ]   |  <- Provided
  | Element 3   [ (Your Tier 3 Prefab)  ]   |  <- Student-created
  | Element 4   [ (Your Tier 4 Prefab)  ]   |  <- Student-created
  +-----------------------------------------+
```

**To add more slots:**
1. Select the MergeObjectFactory in the Inspector
2. Change the **Size** field to a larger number
3. New empty slots will appear at the end
4. Drag your new prefabs into the appropriate slots

**Important:** The array index MUST match the object's `tier` value. If your class has `tier = 3`, its prefab goes in `Element 3`. A mismatch will cause the merge chain to break (merging two objects would spawn the wrong type).

**Missing prefab warnings:** If a slot is empty (None), the game will log a warning when a merge tries to create that tier:
```
MergeObjectFactory: No prefab assigned for tier X!
Create the class and prefab, then drag it into slot X in the Inspector.
```

This warning tells you exactly which slot needs a prefab. Follow the Prefab Setup Instructions above to fix it.

---

## Drop Rules

The `DropController` only spawns objects from **tier 0 through maxDropTier** (default: **2**, TierZero through TierTwo). Students increase this as they add more tiers. This is controlled by the `maxDropTier` field on the DropController Inspector.

Higher tiers (above maxDropTier) can **only** be created through merging. This ensures the player must build up through the merge chain to reach the final tier.

The drop tier is randomly selected each time: `Random.Range(0, maxDropTier + 1)`. All droppable tiers have equal probability.

Our Business is Fun
