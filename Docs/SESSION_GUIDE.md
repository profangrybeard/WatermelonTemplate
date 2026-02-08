# Session Guide -- GAME 220 Watermelon Merge Template

> **Instructor Reference with Solution Code**
>
> This guide walks through every session of the Watermelon Merge project.
> Sessions 1-5 teach inheritance, derived classes, virtual/abstract methods, and polymorphism.
> Sessions 6-8 cover polish, building, and presentation.

---

## Table of Contents

1. [Session 1: Explore + Understand Base Class](#session-1-explore--understand-base-class)
2. [Session 2: First Derived Classes](#session-2-first-derived-classes)
3. [Session 3: Remaining Fruits + Polymorphism Deep Dive](#session-3-remaining-fruits--polymorphism-deep-dive)
4. [Session 4: Custom OnMerge() Overrides](#session-4-custom-onmerge-overrides)
5. [Session 5: Abstract Refactor (Stretch)](#session-5-abstract-refactor-stretch)
6. [Sessions 6-8: Polish, Build, Present](#sessions-6-8-polish-build-present)

---

## Session 1: Explore + Understand Base Class

### What Is Provided

When students open the project, the following is already complete and functional:

| File | Purpose |
|---|---|
| Fruit.cs | Base class with protected fields, virtual methods, merge detection, physics helpers |
| Cherry.cs | Complete derived class (tier 0) -- primary student reference |
| Strawberry.cs | Complete derived class (tier 1) -- second reference |
| Grape.cs | Complete derived class (tier 2) -- third reference |
| FruitFactory.cs | Creates any fruit by tier index; prefab array |
| GameManager.cs | Tracks active fruits, executes merges, detects game over |
| ScoreManager.cs | UI score display and game over text |
| DropController.cs | Player aiming and dropping via mouse/keyboard |
| ContainerSetup.cs | Generates box container with three walls at runtime |

### What Works When Pressing Play

- The container (three walls, open top) appears.
- Fruits spawn at the top. Player aims with mouse (or A/D keys) and drops with click (or Space).
- Cherry, Strawberry, and Grape are fully functional: they drop, stack, and merge.
- Two Cherries merge into a Strawberry. Two Strawberries merge into a Grape.
- Two Grapes attempt to merge into tier 3 (Orange), but since Orange has an empty Awake(), a warning appears in the Console: "FruitFactory: No prefab assigned for tier 3\!" if the prefab slot is unassigned, or the Orange spawns with default white color and scale 1 if the prefab exists but Awake() has no code.
- Score updates on merge. Game over triggers if fruit stays above the red line for too long. Press R to restart.

### What to Show Students

1. **Play the game** -- drop Cherries, merge them, and point out what happens at the Grape-to-Orange boundary.
2. **Open Fruit.cs** -- walk through the protected fields and explain why they are protected rather than private or public.
3. **Open Cherry.cs** side-by-side with Fruit.cs -- show the ": Fruit" syntax, the "override" keyword, and the "base.Awake()" call.
4. **Open Strawberry.cs and Grape.cs** -- ask students what is the same and what is different. (Answer: structure is identical; only values change.)
5. **Open Orange.cs** -- show the empty TODO scaffold and point out that the pattern is spelled out in the comments.

### Comprehension Questions

Use these during the session to check understanding. Have students answer in discussion or on paper.

1. **"In Cherry.cs, what does the colon in 'public class Cherry : Fruit' mean?"**
   - Expected: Cherry inherits from Fruit. Cherry gets all of Fruit's fields and methods automatically.

2. **"Why are the fields in Fruit.cs (tier, fruitName, pointValue, fruitSize, fruitColor) marked 'protected' instead of 'private'?"**
   - Expected: protected allows derived classes (Cherry, Grape, etc.) to access and set these fields. private would block them. public would let any script access them, which is too open.

3. **"What would happen if Cherry forgot to call base.Awake() at the end of its Awake() method?"**
   - Expected: The component references (rb, circleCollider, spriteRenderer) would never be cached. The fruit would not render, have no collider, and have no physics.

4. **"In Fruit.cs, the OnCollisionEnter2D method calls GetTier() on both fruits. Why does it call the Cherry version when two Cherries collide, even though OnCollisionEnter2D is written in the Fruit class?"**
   - Expected: Because GetTier() is virtual and Cherry inherits from Fruit. When the actual object is a Cherry, the correct derived version of the method runs. This is polymorphism / virtual method dispatch.

5. **"Look at DropController.cs. The variable 'currentFruit' is typed as Fruit, not Cherry or Grape. Why is this acceptable?"**
   - Expected: Because Fruit is the base class. A variable typed as the base class can hold any derived type. We do not need separate variables for each fruit type. This is polymorphism.

6. **"In GameManager.cs, activeFruits is a List<Fruit>. What types of objects can this list contain?"**
   - Expected: Any object whose class inherits from Fruit -- Cherry, Strawberry, Grape, Orange, and any future fruit class. The list holds them all through the base class reference.

7. **"How does FruitFactory.CreateFruit(int tier) know which fruit class to create?"**
   - Expected: It uses the fruitPrefabs array indexed by tier. Each prefab has the correct derived script attached (e.g., slot 0 has a prefab with the Cherry script). GetComponent<Fruit>() finds any derived script through the base class.

8. **"What does the 'virtual' keyword on Awake() in Fruit.cs mean? What would happen if it were removed?"**
   - Expected: virtual means derived classes are allowed to override this method. If removed, derived classes could not use override and their Awake methods would hide (not replace) the base version, leading to unexpected behavior.

---

## Session 2: First Derived Classes

### TODO Locations

| File | Path |
|---|---|
| Orange.cs | Assets/_Project/Scripts/Fruits/Orange.cs |
| Dekopon.cs | Assets/_Project/Scripts/Fruits/Dekopon.cs |
| Apple.cs | Assets/_Project/Scripts/Fruits/Apple.cs |

### Learning Goal

Students write their first derived classes by following the pattern established in Cherry, Strawberry, and Grape. Every class follows the same structure:

1. Override Awake()
2. Set the protected fields (tier, fruitName, pointValue, fruitSize, fruitColor)
3. Call base.Awake()

### Solution Code

#### Orange.cs (Complete Awake Method)

```csharp
public class Orange : Fruit
{
    protected override void Awake()
    {
        tier = 3;
        fruitName = "Orange";
        pointValue = 10;
        fruitSize = 1.0f;
        fruitColor = new Color(1.0f, 0.65f, 0.0f);

        base.Awake();
    }
}
```

#### Dekopon.cs (Complete Awake Method)

```csharp
public class Dekopon : Fruit
{
    protected override void Awake()
    {
        tier = 4;
        fruitName = "Dekopon";
        pointValue = 15;
        fruitSize = 1.15f;
        fruitColor = new Color(1.0f, 0.80f, 0.20f);

        base.Awake();
    }
}
```

#### Apple.cs (Complete Awake Method)

```csharp
public class Apple : Fruit
{
    protected override void Awake()
    {
        tier = 5;
        fruitName = "Apple";
        pointValue = 21;
        fruitSize = 1.35f;
        fruitColor = new Color(0.85f, 0.15f, 0.20f);

        base.Awake();
    }
}
```

### Prefab Setup Steps

After writing the code, students must create prefabs for each new fruit and register them with the FruitFactory.

1. **Create a new GameObject** in the scene Hierarchy (right-click > Create Empty). Name it "Orange".
2. **Add components** to the GameObject:
   - SpriteRenderer -- assign Unity default circle sprite (or the project fruit sprite).
   - Rigidbody2D -- leave defaults (Dynamic body type, gravity scale 1).
   - CircleCollider2D -- leave defaults.
   - Orange script -- drag the Orange.cs script onto the GameObject.
3. **Drag the GameObject** from the Hierarchy into the `Assets/_Project/Prefabs/Fruits/` folder to create a prefab.
4. **Delete the GameObject** from the scene (the prefab is saved in the project).
5. **Assign the prefab** to the FruitFactory:
   - Select the FruitFactory GameObject in the scene.
   - In the Inspector, find the "Fruit Prefabs" array.
   - Drag the Orange prefab into **slot 3** (index 3 = tier 3).
6. Repeat steps 1-5 for Dekopon (slot 4) and Apple (slot 5).

> **Tip for Students:** You can duplicate an existing fruit prefab (e.g., Cherry) and replace the script component to save time. Just make sure to swap the Cherry script for the correct new script.

### Verification Checklist

- [ ] Orange.cs compiles without errors
- [ ] Dekopon.cs compiles without errors
- [ ] Apple.cs compiles without errors
- [ ] Orange prefab exists with SpriteRenderer, Rigidbody2D, CircleCollider2D, and Orange script
- [ ] Dekopon prefab exists with the same four components and Dekopon script
- [ ] Apple prefab exists with the same four components and Apple script
- [ ] FruitFactory slots 3, 4, and 5 have the correct prefabs assigned
- [ ] Press Play: two Grapes merge into an Orange (orange-colored, scale 1.0)
- [ ] Two Oranges merge into a Dekopon (yellow-orange, scale 1.15)
- [ ] Two Dekopons merge into an Apple (dark red, scale 1.35)
- [ ] Score increases on each merge
- [ ] Console shows no warnings about missing prefabs for tiers 3-5

---

## Session 3: Remaining Fruits + Polymorphism Deep Dive

### TODO Locations

| File | Path | Tier |
|---|---|---|
| Pear.cs | Assets/_Project/Scripts/Fruits/Pear.cs | 6 |
| Peach.cs | Assets/_Project/Scripts/Fruits/Peach.cs | 7 |
| Pineapple.cs | Assets/_Project/Scripts/Fruits/Pineapple.cs | 8 |
| Melon.cs | Assets/_Project/Scripts/Fruits/Melon.cs | 9 |
| Watermelon.cs | Assets/_Project/Scripts/Fruits/Watermelon.cs | 10 (special) |

### Solution Code

#### Pear.cs

```csharp
public class Pear : Fruit
{
    protected override void Awake()
    {
        tier = 6;
        fruitName = "Pear";
        pointValue = 28;
        fruitSize = 1.5f;
        fruitColor = new Color(0.75f, 0.85f, 0.20f);

        base.Awake();
    }
}
```

#### Peach.cs

```csharp
public class Peach : Fruit
{
    protected override void Awake()
    {
        tier = 7;
        fruitName = "Peach";
        pointValue = 36;
        fruitSize = 1.7f;
        fruitColor = new Color(1.0f, 0.80f, 0.70f);

        base.Awake();
    }
}
```

#### Pineapple.cs

```csharp
public class Pineapple : Fruit
{
    protected override void Awake()
    {
        tier = 8;
        fruitName = "Pineapple";
        pointValue = 45;
        fruitSize = 1.9f;
        fruitColor = new Color(0.95f, 0.85f, 0.15f);

        base.Awake();
    }
}
```

#### Melon.cs

```csharp
public class Melon : Fruit
{
    protected override void Awake()
    {
        tier = 9;
        fruitName = "Melon";
        pointValue = 55;
        fruitSize = 2.15f;
        fruitColor = new Color(0.55f, 0.85f, 0.40f);

        base.Awake();
    }
}
```

#### Watermelon.cs (Special Case -- Two Overrides)

```csharp
public class Watermelon : Fruit
{
    protected override void Awake()
    {
        tier = 10;
        fruitName = "Watermelon";
        pointValue = 66;
        fruitSize = 2.5f;
        fruitColor = new Color(0.20f, 0.75f, 0.20f);

        base.Awake();
    }

    // Watermelon is the FINAL fruit in the merge chain.
    // Returning -1 tells OnCollisionEnter2D that no further merge is possible.
    public override int GetMergeResultTier()
    {
        return -1;
    }
}
```

### Watermelon Special Case: GetMergeResultTier() Override

Ask students before revealing the solution:

> "Every other fruit merges into the next tier by returning tier + 1. What should Watermelon return? There is no tier 11."

**Answer:** Watermelon overrides GetMergeResultTier() to return -1. The merge detection code in Fruit.OnCollisionEnter2D checks for this:

```csharp
if (GetMergeResultTier() \!= -1)
{
    // proceed with merge...
}
```

When two Watermelons collide, GetMergeResultTier() returns -1, so the merge is skipped and the Console logs: "Maximum tier reached\! Two Watermelons cannot merge."

This is the only fruit that needs a second override. It is a clear, focused example of overriding a virtual method to change behavior rather than just data.

### Polymorphism Walkthrough Script for Instructor

Use this walkthrough after students have completed their fruit classes and the game is fully playable with all 11 tiers.

#### Step 1: Show the Polymorphic Collection

Open GameManager.cs and point to the line that declares List<Fruit> activeFruits.

> "This is a List of Fruit. It is typed as the base class. But right now, this list contains Cherries, Strawberries, Grapes, Oranges, and every other fruit you created. The list does not care which specific type each fruit is."

#### Step 2: Run the Game and Pause

1. Play the game and drop several fruits. Let a few merges happen.
2. Pause the game.
3. In the Console, use Debug.Log calls (or the polymorphic query methods) to show:

> "Let me call GetActiveFruitCount() -- there are, say, 7 fruits. Now GetHighestFruitName() -- it returns Dekopon. The GameManager never checks 'is this a Cherry? is this a Dekopon?' It just calls GetFruitName() on each Fruit reference, and the correct derived version runs."

#### Step 3: Walk Through OnCollisionEnter2D

Open Fruit.cs and scroll to OnCollisionEnter2D. Read through it line by line.

Ask the class:

> "This collision code is written once, in the base class. How many fruit classes have their own copy of this code?"

**Answer:** Zero. Every fruit inherits it. One method handles all 55 possible same-tier collision pairs (11 types, each can collide with itself).

Ask:

> "When two Oranges collide, this code calls GetTier(). Which version of GetTier() runs -- the one in Fruit.cs or the one in Orange.cs?"

**Answer:** Trick question. Orange does not override GetTier(), so the base class version runs -- but it returns the value of the tier field, which Orange set to 3 in its Awake(). The important point: the correct value (3) is returned regardless.

Ask:

> "What about OnMerge()? Right now, which version runs for an Orange?"

**Answer:** The base class version (Debug.Log with the fruit name), because Orange has not overridden it. This sets up Session 4.

#### Step 4: The Power Demonstration

> "Imagine we add a brand new fruit -- say, Dragonfruit at tier 11. We write one new class with Awake() setting its values and GetMergeResultTier() returning -1. We update Watermelon's override to return 11 instead of -1. Without changing a single line in GameManager, FruitFactory, DropController, or Fruit.OnCollisionEnter2D, the new fruit works. That is the power of polymorphism."

### Verification Checklist

- [ ] All five .cs files compile without errors
- [ ] All five prefabs created with correct components
- [ ] FruitFactory slots 6-10 have the correct prefabs assigned
- [ ] Full merge chain works: Cherry -> Strawberry -> Grape -> Orange -> Dekopon -> Apple -> Pear -> Peach -> Pineapple -> Melon -> Watermelon
- [ ] Two Watermelons collide but do NOT merge; Console shows "Maximum tier reached" message
- [ ] Each fruit appears at the correct size (Watermelon visually much larger than Cherry)
- [ ] Each fruit has the correct color
- [ ] Score increases correctly throughout the chain

---

## Session 4: Custom OnMerge() Overrides

### Learning Goal

Students learn to override a virtual method to add unique behavior per fruit type. The base class OnMerge() just logs a message. Students replace it with creative custom effects: visual feedback, camera shakes, debug messages, color changes, or anything else they can imagine.

### Key Concept: Same Merge Code, Different Effects Per Fruit

Write this on the board or share on screen:

```
Fruit.OnCollisionEnter2D() calls OnMerge() on both fruits.
  -> If it is a Cherry, Cherry.OnMerge() runs.
  -> If it is a Pineapple, Pineapple.OnMerge() runs.
  -> If it is a Watermelon, Watermelon.OnMerge() runs.

The collision code NEVER checks which type of fruit it is.
It just calls OnMerge(), and polymorphism dispatches to the correct version.
```

### Teaching Discussion

Walk students through this scenario:

1. Open Fruit.cs and find OnCollisionEnter2D. Point to the two lines:
   ```csharp
   OnMerge();
   otherFruit.OnMerge();
   ```

2. Ask: "These two calls are on Fruit references. How does C# know to run the Cherry version when Cherries merge?"
   - Because OnMerge() is virtual in the base class and override in the derived class. The runtime looks at the actual object type, not the variable type.

3. Ask: "If a student forgets to write 'override' and just writes 'public void OnMerge()', what happens?"
   - The compiler issues a warning (CS0108: hides inherited member). The base class version runs instead of the student's version when called through a Fruit reference. The 'new' keyword would suppress the warning but would NOT fix the polymorphism problem.

4. Ask: "Do you need to call base.OnMerge() inside your override?"
   - Not required. It depends on whether you want the base behavior (the Debug.Log) to also run. If you want only your custom effect, skip it. If you want both, call base.OnMerge() first.

### Example Override Implementations

Give students these examples for inspiration. They should write their own, not copy these verbatim.

#### Example 1: Cherry -- Simple Log with Personality

```csharp
public class Cherry : Fruit
{
    protected override void Awake()
    {
        tier = 0;
        fruitName = "Cherry";
        pointValue = 1;
        fruitSize = 0.5f;
        fruitColor = new Color(0.85f, 0.12f, 0.15f);
        base.Awake();
    }

    public override void OnMerge()
    {
        Debug.Log("Pop! Two tiny cherries combined!");
    }
}
```

#### Example 2: Apple -- Scale Pulse Effect

```csharp
public class Apple : Fruit
{
    protected override void Awake()
    {
        tier = 5;
        fruitName = "Apple";
        pointValue = 21;
        fruitSize = 1.35f;
        fruitColor = new Color(0.85f, 0.15f, 0.20f);
        base.Awake();
    }

    public override void OnMerge()
    {
        // Quick scale-up effect right before the fruit is destroyed
        transform.localScale *= 1.5f;
        Debug.Log("Apple crunch! Merging into a Pear!");
    }
}
```

#### Example 3: Pineapple -- Color Flash

```csharp
public class Pineapple : Fruit
{
    protected override void Awake()
    {
        tier = 8;
        fruitName = "Pineapple";
        pointValue = 45;
        fruitSize = 1.9f;
        fruitColor = new Color(0.95f, 0.85f, 0.15f);
        base.Awake();
    }

    public override void OnMerge()
    {
        // Flash bright white before merging
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        Debug.Log("Tropical explosion! Pineapple is merging!");
    }
}
```

#### Example 4: Melon -- Spawn Visual Marker at Merge Position

```csharp
public class Melon : Fruit
{
    protected override void Awake()
    {
        tier = 9;
        fruitName = "Melon";
        pointValue = 55;
        fruitSize = 2.15f;
        fruitColor = new Color(0.55f, 0.85f, 0.40f);
        base.Awake();
    }

    public override void OnMerge()
    {
        Debug.Log("MASSIVE melon merge! Almost at Watermelon!");

        // Create a temporary visual marker at the merge location
        GameObject marker = new GameObject("MelonMergeMarker");
        marker.transform.position = transform.position;
        SpriteRenderer sr = marker.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.55f, 0.85f, 0.40f, 0.5f);
        // Destroy the marker after 0.5 seconds
        Destroy(marker, 0.5f);
    }
}
```

> **Important Note for Instructors:** Because OnCollisionEnter2D checks GetMergeResultTier() != -1 **before** calling OnMerge(), the Watermelon OnMerge() override will never actually execute during a Watermelon-Watermelon collision. This is worth discussing with students as an exercise in reading code flow. If students want a Watermelon collision effect, they would need to move the OnMerge() calls before the tier check, or handle it separately. This is an excellent design discussion opportunity.

### Verification: How to Confirm Overrides Work

1. **Console Output:** Each custom OnMerge() should produce a unique Debug.Log message. Play the game, merge fruits, and check the Console for the expected messages.
2. **Visual Effects:** If students added color changes or scale effects, these will be briefly visible right before the merging fruits are destroyed. The effect may flash for only one frame -- that is expected since Destroy() happens immediately after.
3. **Breakpoints:** Students comfortable with the debugger can set breakpoints inside their OnMerge() methods to confirm they are hit.
4. **Deliberate Test:** Temporarily comment out the Destroy() calls in GameManager.MergeFruits() to keep the fruits alive after merge. This makes visual effects easier to verify. Remember to uncomment afterward.

---

## Session 5: Abstract Refactor (Stretch)

> This is a stretch goal for advanced students or an optional class-wide exercise. It is not required for project completion.

### Learning Goal

Students understand the difference between virtual (optional override) and abstract (required override). They refactor Fruit.cs to enforce that every derived class MUST set its properties.

### Exact Changes to Make in Fruit.cs

#### Change 1: Make the Class Abstract

```csharp
// BEFORE:
public class Fruit : MonoBehaviour

// AFTER:
public abstract class Fruit : MonoBehaviour
```

#### Change 2: Add an Abstract Method for Property Initialization

Instead of making Awake() itself abstract (which would complicate the base.Awake() pattern), add a new abstract method that derived classes must implement:

```csharp
// Add this new abstract method declaration in the VIRTUAL METHODS section of Fruit.cs:

/// <summary>
/// Each derived class MUST implement this to set tier, fruitName,
/// pointValue, fruitSize, and fruitColor.
/// </summary>
protected abstract void InitializeFruitProperties();
```

#### Change 3: Update Awake() to Call the Abstract Method

```csharp
// BEFORE:
protected virtual void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    circleCollider = GetComponent<CircleCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}

// AFTER:
protected virtual void Awake()
{
    // Derived classes set their values here (abstract = they MUST implement it)
    InitializeFruitProperties();

    // Cache component references
    rb = GetComponent<Rigidbody2D>();
    circleCollider = GetComponent<CircleCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}
```

> **Note:** Awake() stays virtual so derived classes can still override it if needed. The new abstract method InitializeFruitProperties() is what forces each fruit to provide its values. Derived classes no longer need to override Awake() at all -- they implement InitializeFruitProperties() instead.

#### Alternative Approach: Make Awake() Abstract Directly

A simpler but more disruptive approach is to make Awake() itself abstract. This requires every derived class to handle both property-setting AND component caching:

```csharp
// Fruit.cs:
protected abstract void Awake();

// Then provide a helper for component caching:
protected void CacheComponents()
{
    rb = GetComponent<Rigidbody2D>();
    circleCollider = GetComponent<CircleCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}
```

The first approach (adding InitializeFruitProperties()) is recommended because it preserves the existing base.Awake() pattern students already understand.

### What Compiler Errors Appear and Why

After adding abstract to the class and adding the InitializeFruitProperties() method, every derived class that does NOT implement InitializeFruitProperties() will produce a compiler error:

```
error CS0534: 'Cherry' does not implement inherited abstract member 'Fruit.InitializeFruitProperties()'
error CS0534: 'Strawberry' does not implement inherited abstract member 'Fruit.InitializeFruitProperties()'
error CS0534: 'Grape' does not implement inherited abstract member 'Fruit.InitializeFruitProperties()'
error CS0534: 'Orange' does not implement inherited abstract member 'Fruit.InitializeFruitProperties()'
... (one error per derived class)
```

**Why:** An abstract method has no body -- it is a contract. The compiler enforces that every non-abstract class inheriting from Fruit must provide an implementation. If a student creates a new fruit class and forgets to implement InitializeFruitProperties(), the code will not compile. This is the safety net that virtual does not provide.

### Solution: Adding Override Methods in Derived Classes

Each derived class replaces its Awake() override with an InitializeFruitProperties() override. The values stay the same.

#### Cherry.cs (Refactored)

```csharp
public class Cherry : Fruit
{
    protected override void InitializeFruitProperties()
    {
        tier = 0;
        fruitName = "Cherry";
        pointValue = 1;
        fruitSize = 0.5f;
        fruitColor = new Color(0.85f, 0.12f, 0.15f);
    }

    // No need to override Awake() anymore.
    // Fruit.Awake() calls InitializeFruitProperties() automatically.
}
```

Apply the same pattern to all other derived classes: move the field assignments from Awake() into InitializeFruitProperties(), remove the Awake() override, and remove the base.Awake() call (since the base Awake() is no longer being overridden).

Watermelon still keeps its GetMergeResultTier() override:

```csharp
public class Watermelon : Fruit
{
    protected override void InitializeFruitProperties()
    {
        tier = 10;
        fruitName = "Watermelon";
        pointValue = 66;
        fruitSize = 2.5f;
        fruitColor = new Color(0.20f, 0.75f, 0.20f);
    }

    public override int GetMergeResultTier()
    {
        return -1;
    }
}
```

### Discussion Questions: Virtual vs Abstract

1. **"What is the key difference between virtual and abstract?"**
   - virtual: provides a default implementation; derived classes MAY override it.
   - abstract: provides NO implementation; derived classes MUST override it.

2. **"When would you choose virtual over abstract?"**
   - When the base class has a sensible default that most derived classes can use as-is. Example: GetMergeResultTier() defaults to tier + 1, which works for 10 out of 11 fruits.

3. **"When would you choose abstract over virtual?"**
   - When every derived class MUST provide its own implementation and there is no sensible default. Example: InitializeFruitProperties() -- every fruit has unique values, so there is no meaningful default.

4. **"Can you create an instance of an abstract class?"**
   - No. Writing new Fruit() would produce a compiler error. You can only create instances of concrete (non-abstract) derived classes like Cherry, Grape, etc. This makes sense: a generic Fruit with no specific values should not exist in the game.

5. **"We already had 11 working fruit classes before the refactor. What practical benefit did making Fruit abstract provide?"**
   - Safety for future development. If someone adds a 12th fruit class and forgets to implement InitializeFruitProperties(), the compiler catches the mistake immediately instead of the fruit silently appearing with default values at runtime.

---

## Sessions 6-8: Polish, Build, Present

### Session 6: Polish

Students refine their game and add personal touches:

- Custom OnMerge() effects (if not completed in Session 4)
- UI improvements (better fonts, layout, colors)
- Sound effects on merge (use AudioSource.PlayClipAtPoint())
- Particle effects on merge
- Custom fruit sprites (replace the default circle)
- Background art or theme customization
- Adjusted physics (gravity, bounce, friction via Physics Materials)
- Container size/shape adjustments
- Drop speed and cooldown tuning

### Session 7: Build

#### Build Checklist

- [ ] **Test the full merge chain** one final time (Cherry through Watermelon, 10 merges)
- [ ] **Verify Watermelon collision** does not crash or produce errors
- [ ] **Check the Console** for any remaining warnings or errors
- [ ] **Remove or disable debug Debug.Log calls** that are not part of OnMerge() effects
- [ ] **File > Build Settings**: add the game scene to "Scenes in Build"
- [ ] **Player Settings**:
  - Set Company Name and Product Name
  - Set the default resolution (e.g., 1920x1080 or 1280x720)
  - Choose a build target (Windows standalone or WebGL for itch.io)
- [ ] **Build and Run**: verify the built executable works outside the editor
- [ ] **Test on a second machine** if possible (catches missing assets or hard-coded paths)

### Session 8: Presentation

#### Presentation Talking Points

Each student (or team) presents their game. Suggested format: 3-5 minutes per student.

1. **Live Demo** -- Play the game and show the merge chain in action. Try to get at least one high-tier merge during the demo.

2. **Code Walkthrough** -- Open one of their derived classes and explain:
   - How it inherits from Fruit
   - What values they set in Awake() (or InitializeFruitProperties())
   - Their custom OnMerge() override (if they did Session 4)

3. **Inheritance Explanation** -- Answer one of these questions:
   - "How does the merge system work without checking which specific fruit type collided?"
   - "What does base.Awake() do and why is it important?"
   - "What would happen if we removed the virtual keyword from OnMerge() in Fruit.cs?"

4. **Polish Showcase** -- Show any custom effects, sounds, sprites, or UI additions.

5. **Reflection** -- What was the most challenging part? What would they add with more time?

#### Grading Rubric Suggestions

| Category | Points | Criteria |
|---|---|---|
| All 8 fruit classes complete | 40 | Correct tier, name, points, size, color; base.Awake() called |
| Prefabs and FruitFactory setup | 15 | All 11 prefabs assigned; full merge chain works |
| Watermelon override | 10 | GetMergeResultTier() returns -1; two Watermelons do not merge |
| OnMerge() overrides (Session 4) | 15 | At least 3 fruits have custom OnMerge() behavior |
| Polish and creativity | 10 | Visual/audio effects, custom sprites, UI improvements |
| Presentation and code explanation | 10 | Clear explanation of inheritance and polymorphism concepts |
