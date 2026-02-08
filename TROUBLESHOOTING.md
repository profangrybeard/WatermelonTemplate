# Troubleshooting Guide -- GAME 220 Watermelon Merge Template

> Quick-reference for common student issues. Each entry lists the symptom, cause, and fix.

---

## Table of Contents

1. [Fruits Do Not Merge (Same Type)](#1-fruits-do-not-merge-same-type)
2. [Fruits Do Not Merge (Wrong Tier Value)](#2-fruits-do-not-merge-wrong-tier-value)
3. [Fruits Do Not Merge (Missing Collider or Rigidbody)](#3-fruits-do-not-merge-missing-collider-or-rigidbody)
4. [Fruits Fall Through the Floor](#4-fruits-fall-through-the-floor)
5. [New Fruit Type Does Not Appear in Game](#5-new-fruit-type-does-not-appear-in-game)
6. [Wrong Fruit Spawns After Merge](#6-wrong-fruit-spawns-after-merge)
7. [Fruit Has Wrong Size or Color](#7-fruit-has-wrong-size-or-color)
8. [Cannot Merge Watermelons Message](#8-cannot-merge-watermelons-message)
9. [base.Awake() Errors / NullReferenceException](#9-baseawake-errors--nullreferenceexception)
10. [Compiler Error: Does Not Implement Abstract Member](#10-compiler-error-does-not-implement-abstract-member-session-5)
11. [Game Over Triggers Too Quickly or Not At All](#11-game-over-triggers-too-quickly-or-not-at-all)
12. [Prefab Not Working After Adding Script](#12-prefab-not-working-after-adding-script)
13. [OnMerge Override Not Running](#13-onmerge-override-not-running-session-4)
14. [Multiple Fruits Spawning on Merge](#14-multiple-fruits-spawning-on-merge-double-merge-bug)

---
## 1. Fruits Do Not Merge (Same Type)

### Symptom
Two fruits of the same type (e.g., two Cherries) collide and stack on top of each other, but no merge happens. No error messages in the Console.

### Cause
The fruit prefab is missing one or more required components. The merge system requires:
- A script that inherits from Fruit (e.g., Cherry, Orange)
- A Rigidbody2D (required for OnCollisionEnter2D to fire)
- A CircleCollider2D (required for collision detection)

If the prefab has no Fruit-derived script, GetComponent<Fruit>() returns null in OnCollisionEnter2D and the merge check exits immediately.

### Fix
1. Select the fruit prefab in the Project window.
2. In the Inspector, verify it has ALL of the following components:
   - SpriteRenderer
   - Rigidbody2D (body type: Dynamic)
   - CircleCollider2D (NOT set as Trigger)
   - The correct fruit script (e.g., Cherry, Orange, Apple)
3. If any component is missing, click Add Component and add it.
4. Make sure the CircleCollider2D **Is Trigger** checkbox is **unchecked**. The merge system uses OnCollisionEnter2D, not OnTriggerEnter2D.

---

## 2. Fruits Do Not Merge (Wrong Tier Value)

### Symptom
Two fruits that look the same collide but do not merge. Or, two different fruit types unexpectedly merge with each other.

### Cause
The tier value is set incorrectly in one or more fruit classes. Merging only happens when both fruits have the same tier value (the check is GetTier() == otherFruit.GetTier()). If two Oranges have different tier values (e.g., one has tier = 3 and the other has tier = 4 due to a typo), they will not merge.

### Fix
1. Open the fruit script (e.g., Orange.cs).
2. Verify the tier value matches the expected value:
   - Cherry = 0, Strawberry = 1, Grape = 2, Orange = 3, Dekopon = 4, Apple = 5
   - Pear = 6, Peach = 7, Pineapple = 8, Melon = 9, Watermelon = 10
3. Make sure no two different fruit classes share the same tier value.
4. Add a Debug.Log in Awake() to print the tier at startup if unsure:

   ```csharp
   Debug.Log(fruitName + " tier = " + tier);
   ```

---

## 3. Fruits Do Not Merge (Missing Collider or Rigidbody)

### Symptom
Fruits overlap or pass through each other without any physics interaction. No collision events fire.

### Cause
Unity requires **both** colliding objects to have colliders, and **at least one** must have a Rigidbody2D, for OnCollisionEnter2D to fire. If either fruit is missing its CircleCollider2D or Rigidbody2D, collisions are not detected.

### Fix
1. Select each fruit prefab and confirm it has:
   - A CircleCollider2D (not set to Trigger)
   - A Rigidbody2D (body type: Dynamic)
2. If using a BoxCollider2D instead of CircleCollider2D, that will still work for collisions, but the shape may not match the round fruit sprite.
3. Confirm neither collider has Is Trigger checked. Trigger colliders use OnTriggerEnter2D, which the merge system does not use.

---
## 4. Fruits Fall Through the Floor

### Symptom
Fruits drop and fall through the bottom of the container, disappearing off-screen.

### Cause A: Container is missing or misconfigured
The ContainerSetup script creates walls at runtime. If the Container GameObject is missing from the scene, or if the ContainerSetup script is not attached, no walls are created.

### Fix A
1. Check the Hierarchy for a Container GameObject with the ContainerSetup script.
2. If missing, create a new empty GameObject, name it Container, and add the ContainerSetup script.
3. Make sure the Container is at position (0, 0, 0).

### Cause B: Fruit collider is set to Trigger
If the CircleCollider2D on the fruit prefab has Is Trigger checked, the fruit will not physically interact with the walls.

### Fix B
Select the fruit prefab, find the CircleCollider2D, and uncheck Is Trigger.

### Cause C: Layer collision matrix
If the fruits and container walls are on layers that do not collide with each other, physics interactions will be ignored.

### Fix C
Go to Edit > Project Settings > Physics 2D and check the Layer Collision Matrix. Make sure the layers used by fruits and walls are set to collide with each other.

---

## 5. New Fruit Type Does Not Appear in Game

### Symptom
A student wrote the code for a new fruit class (e.g., Orange.cs), but when two Grapes merge, nothing spawns. The Console shows a warning like:

   ```
   FruitFactory: No prefab assigned for tier 3!
   ```

### Cause
The student created the script and possibly the prefab, but did not assign the prefab to the correct slot in the FruitFactory Inspector.

### Fix
1. Confirm the fruit prefab exists in the Assets/Prefabs/ folder (or wherever prefabs are stored).
2. Select the FruitFactory GameObject in the scene Hierarchy.
3. In the Inspector, find the Fruit Prefabs array.
4. Drag the fruit prefab into the correct slot:
   - Slot 0 = Cherry, Slot 1 = Strawberry, Slot 2 = Grape
   - Slot 3 = Orange, Slot 4 = Dekopon, Slot 5 = Apple
   - Slot 6 = Pear, Slot 7 = Peach, Slot 8 = Pineapple
   - Slot 9 = Melon, Slot 10 = Watermelon
5. If the array size is less than 11, increase it to 11.

---

## 6. Wrong Fruit Spawns After Merge

### Symptom
Two Oranges merge, but instead of a Dekopon, a Cherry (or other wrong fruit) appears.

### Cause
The prefabs are assigned to the wrong slots in the FruitFactory array. The merge system calls CreateFruitAtPosition(nextTier, position) where nextTier = currentTier + 1. If slot 4 has the Cherry prefab instead of the Dekopon prefab, a Cherry will spawn.

### Fix
1. Select the FruitFactory GameObject.
2. Carefully verify every slot in the Fruit Prefabs array matches the correct tier:
   - Slot 0 = Cherry prefab
   - Slot 1 = Strawberry prefab
   - Slot 2 = Grape prefab
   - Slot 3 = Orange prefab
   - Slot 4 = Dekopon prefab
   - Slot 5 = Apple prefab
   - Slot 6 = Pear prefab
   - Slot 7 = Peach prefab
   - Slot 8 = Pineapple prefab
   - Slot 9 = Melon prefab
   - Slot 10 = Watermelon prefab
3. A common mistake is dragging a prefab into the wrong slot, or having duplicate prefabs.

---
## 7. Fruit Has Wrong Size or Color

### Symptom
A fruit appears in the game but is the wrong color (e.g., white) or the wrong size (e.g., default scale of 1).

### Cause A: Awake() is empty or incomplete
The student created the class with the correct inheritance but forgot to set the fruitSize and/or fruitColor fields in Awake(). The base class defaults are fruitSize = 1f and fruitColor = Color.white.

### Fix A
Open the fruit script and verify all five fields are set in Awake():
- tier
- fruitName
- pointValue
- fruitSize
- fruitColor

### Cause B: base.Awake() called before setting values
If the student calls base.Awake() at the top of their Awake() method instead of the bottom, the component references are cached before the values are set. While ApplyFruitProperties() runs in Start() (so size/color are usually fine), it is best practice to set fields first.

### Fix B
Move base.Awake() to the END of the derived Awake() method, after all field assignments.

### Cause C: Color values out of range
Unity Color constructor expects float values between 0.0f and 1.0f. If a student uses values like 255 (thinking of the 0-255 byte range), the color will be overbright white.

### Fix C
Remind students that Unity uses the 0.0-1.0 float range for colors, not 0-255. To convert: divide each value by 255. For example, RGB(200, 50, 50) becomes:

   ```csharp
   fruitColor = new Color(200f / 255f, 50f / 255f, 50f / 255f);
   ```

---

## 8. Cannot Merge Watermelons Message

### Symptom
Two Watermelons collide but do not merge. The Console prints: "Maximum tier reached! Two Watermelons cannot merge."

### Cause
**This is expected behavior, not a bug.** Watermelon is the final fruit in the merge chain (tier 10). Its GetMergeResultTier() override returns -1, which tells the merge system that no further merge is possible.

### Why This Exists
The GameManager.MergeFruits() method checks GetMergeResultTier() before spawning a new fruit. When it receives -1, it logs the message and skips the merge. This prevents the system from trying to create a nonexistent tier 11 fruit.

### What to Tell Students
This message confirms that the Watermelon GetMergeResultTier() override is working correctly. It is a sign of success, not failure. If a student does NOT see this message when two Watermelons collide, it means they forgot to override GetMergeResultTier() in Watermelon.cs.

### If Students Want Watermelons to Do Something on Collision
They could add special handling inside OnCollisionEnter2D or add a score bonus in the GameManager. Note that OnMerge() on Watermelon will NOT be called during a Watermelon-Watermelon collision because the GetMergeResultTier() check short-circuits before the OnMerge() calls.

---
## 9. base.Awake() Errors / NullReferenceException

### Symptom A: NullReferenceException at runtime
The game runs but crashes or shows errors like:

   ```
   NullReferenceException: Object reference not set to an instance of an object
   Fruit.ApplyFruitProperties() (at Assets/Scripts/Fruits/Fruit.cs:...)
   ```

### Cause A: Forgot to call base.Awake()
The student overrode Awake() and set their field values, but forgot to call base.Awake() at the end. Without base.Awake(), the component references (rb, circleCollider, spriteRenderer) are never cached and remain null.

### Fix A
Add base.Awake() as the last line inside the derived Awake() method:

   ```csharp
   protected override void Awake()
   {
       tier = 3;
       fruitName = "Orange";
       pointValue = 10;
       fruitSize = 1.0f;
       fruitColor = new Color(1.0f, 0.65f, 0.0f);
   
       base.Awake();  // <-- Do not forget this line
   }
   ```

### Symptom B: Compiler error about base.Awake()
The student sees a compiler error when trying to call base.Awake().

### Cause B: Awake() is not marked as override
If the student wrote their method as:

   ```csharp
   void Awake()  // Missing: protected override
   ```

then base.Awake() will not compile because the method is not properly overriding the virtual base method.

### Fix B
Change the method signature to include both protected and override:

   ```csharp
   protected override void Awake()
   ```

---

## 10. Compiler Error: Does Not Implement Abstract Member (Session 5)

### Symptom
After the Session 5 abstract refactor, the compiler shows errors like:

   ```
   error CS0534: 'Orange' does not implement inherited abstract member
                 'Fruit.InitializeFruitProperties()'
   ```

### Cause
The Fruit base class now has an abstract method (InitializeFruitProperties()) that every derived class must implement. The student has not yet added the override in one or more derived classes.

### Fix
Add the InitializeFruitProperties() override to the class that is showing the error. Move the field assignments from Awake() into this new method:

   ```csharp
   protected override void InitializeFruitProperties()
   {
       tier = 3;
       fruitName = "Orange";
       pointValue = 10;
       fruitSize = 1.0f;
       fruitColor = new Color(1.0f, 0.65f, 0.0f);
   }
   ```

Once every derived class has this method, the Awake() override and base.Awake() call can be removed from the derived classes (since the base Awake() now calls InitializeFruitProperties() automatically).

### Common Mistake
Students sometimes add the method but forget the override keyword. Without override, the method does not satisfy the abstract contract and the compiler error persists.

---
## 11. Game Over Triggers Too Quickly or Not At All

### Symptom A: Game over triggers almost immediately
The game declares game over within seconds of starting, even though no fruits are near the top.

### Cause A: gameOverLineY is set too low
The GameManager.gameOverLineY value determines the Y position of the game over detection line. If this value is too low (e.g., 0 instead of 4.5), fruits will be above the line as soon as they are dropped.

### Fix A
1. Select the GameManager GameObject in the scene.
2. In the Inspector, find the gameOverLineY field.
3. Set it to a value near the top of the container (default is 4.5).
4. The red gizmo line in the Scene view shows where the game over line is -- use this to verify placement.

### Symptom B: Game over never triggers
Fruits stack far above the container and the game never ends.

### Cause B: gameOverLineY is set too high
If gameOverLineY is very high (e.g., 100), fruits will never reach it.

### Fix B
Set gameOverLineY to approximately 4.5 (just above the container opening).

### Cause C: gameOverDelay is too long
The game over does not trigger instantly. A fruit must remain above the line for gameOverDelay seconds (default 2 seconds). If this value is very high, the game over takes a long time to trigger.

### Fix C
Set gameOverDelay to 2 seconds (default). A value of 1-3 seconds is reasonable.

---

## 12. Prefab Not Working After Adding Script

### Symptom
The student created a fruit class and added it to a GameObject, but when that fruit is instantiated during a merge, it has no script behavior. It falls with physics but does not merge, has default white color, and default scale.

### Cause A: Script added to scene object but not saved to prefab
If the student adds the script to a GameObject in the scene but does not click "Apply" (or "Overrides > Apply All") to save changes back to the prefab, the prefab in the Project folder still lacks the script. When FruitFactory instantiates the prefab, it creates a copy of the unmodified prefab.

### Fix A
1. Select the fruit GameObject in the Hierarchy (should show a blue name if it is a prefab instance).
2. In the Inspector, click the Overrides dropdown at the top and select Apply All.
3. Alternatively, drag the GameObject from the Hierarchy onto the existing prefab in the Project folder to replace it.

### Cause B: Wrong script attached
The student may have attached the wrong script (e.g., Cherry instead of Orange) or attached the base Fruit class directly.

### Fix B
1. Select the prefab and check which script component is attached.
2. Remove the wrong script (right-click the component > Remove Component).
3. Add the correct script.

### Cause C: Prefab was created without any components
If the student created the prefab from an empty GameObject (no SpriteRenderer, no Rigidbody2D, no Collider, no script), the prefab will be non-functional.

### Fix C
Open the prefab (double-click it in Project folder) and add all required components:
1. SpriteRenderer (assign a circle sprite)
2. Rigidbody2D
3. CircleCollider2D
4. The correct fruit script (e.g., Orange)

---
## 13. OnMerge Override Not Running (Session 4)

### Symptom
The student wrote a custom OnMerge() method in their fruit class, but when that fruit merges, the custom Debug.Log message does not appear and the custom effects do not run. Instead, the default base class message appears.

### Cause A: Missing the override keyword
The student wrote:

   ```csharp
   public void OnMerge()  // WRONG: missing override
   {
       Debug.Log("Custom merge!");
   }
   ```

Without the override keyword, this method **hides** the base class version rather than **replacing** it. When the merge system calls OnMerge() through a Fruit reference (which is how OnCollisionEnter2D works), the base class version runs because the method is not properly overridden.

### Fix A
Add the override keyword:

   ```csharp
   public override void OnMerge()  // CORRECT: override keyword present
   {
       Debug.Log("Custom merge!");
   }
   ```

The compiler should also show warning CS0108 when override is missing. Encourage students to read compiler warnings.

### Cause B: Method signature mismatch
The student changed the return type, parameters, or access modifier:

   ```csharp
   private override void OnMerge()     // WRONG: should be public
   public override bool OnMerge()      // WRONG: should return void
   public override void OnMerge(int x) // WRONG: should have no parameters
   ```

### Fix B
The override must exactly match the base class signature:

   ```csharp
   public override void OnMerge()
   ```

### Cause C: Watermelon-specific issue
If the student added OnMerge() to Watermelon.cs specifically, note that it will never be called during a Watermelon-Watermelon collision. The GetMergeResultTier() check returns -1, which causes OnCollisionEnter2D to exit before reaching the OnMerge() calls. See [Issue 8](#8-cannot-merge-watermelons-message) for details.

---

## 14. Multiple Fruits Spawning on Merge (Double-Merge Bug)

### Symptom
When two fruits merge, two (or more) copies of the next fruit appear instead of one. Sometimes fruits also disappear unexpectedly.

### Cause A: hasMerged flag not working
The base class uses a private boolean hasMerged to prevent the same fruit from being claimed by multiple simultaneous collisions. If this flag is not being checked properly (e.g., if someone modified OnCollisionEnter2D), both fruits in a collision might each independently trigger a merge, resulting in two merge events.

### Fix A
Verify that the OnCollisionEnter2D method in Fruit.cs has not been modified. The critical lines are:

   ```csharp
   if (hasMerged || otherFruit.HasMerged()) return;
   ```

and:

   ```csharp
   SetMerged();
   otherFruit.SetMerged();
   ```

These lines must appear in this order: check first, then set. If a student accidentally deleted or reordered these lines, restore them from the original template.

### Cause B: Student added their own OnCollisionEnter2D
If a student added a second OnCollisionEnter2D method in their derived class, it may conflict with the base class version. In C#, a derived class method with the same name (without override) hides the base version.

### Fix B
Remove any OnCollisionEnter2D methods from derived classes. The merge detection logic should ONLY exist in the base Fruit class. Derived classes customize behavior through OnMerge() overrides, not by duplicating collision code.

### Cause C: Multiple Fruit scripts on one prefab
If a prefab accidentally has two Fruit-derived scripts attached (e.g., both Cherry and Orange on the same GameObject), each script independently runs OnCollisionEnter2D, causing duplicate merge processing.

### Fix C
Select the prefab and check the Inspector. Each fruit prefab should have exactly ONE script that inherits from Fruit. Remove any duplicate or incorrect scripts.

---

## Quick Reference: Complete Tier Table

Use this table to verify student values are correct.

| Tier | Class | fruitName | pointValue | fruitSize | fruitColor (R, G, B) |
|------|-------|-----------|------------|-----------|----------------------|
| 0 | Cherry | Cherry | 1 | 0.5f | (0.85, 0.12, 0.15) |
| 1 | Strawberry | Strawberry | 3 | 0.65f | (0.95, 0.30, 0.35) |
| 2 | Grape | Grape | 6 | 0.8f | (0.55, 0.27, 0.68) |
| 3 | Orange | Orange | 10 | 1.0f | (1.00, 0.65, 0.00) |
| 4 | Dekopon | Dekopon | 15 | 1.15f | (1.00, 0.80, 0.20) |
| 5 | Apple | Apple | 21 | 1.35f | (0.85, 0.15, 0.20) |
| 6 | Pear | Pear | 28 | 1.5f | (0.75, 0.85, 0.20) |
| 7 | Peach | Peach | 36 | 1.7f | (1.00, 0.80, 0.70) |
| 8 | Pineapple | Pineapple | 45 | 1.9f | (0.95, 0.85, 0.15) |
| 9 | Melon | Melon | 55 | 2.15f | (0.55, 0.85, 0.40) |
| 10 | Watermelon | Watermelon | 66 | 2.5f | (0.20, 0.75, 0.20) |

**Special override:** Watermelon.GetMergeResultTier() returns -1 (all others inherit the default: tier + 1).

---

## Quick Reference: Required Prefab Components

Every fruit prefab must have these four components:

| Component | Settings |
|---|---|
| SpriteRenderer | Any sprite (default circle works); color is overridden by script |
| Rigidbody2D | Body Type: Dynamic; Gravity Scale: 1 |
| CircleCollider2D | Is Trigger: unchecked |
| [FruitName] script | The correct derived class (Cherry, Orange, etc.) |

---

## Quick Reference: FruitFactory Array Slots

| Slot | Prefab |
|------|--------|
| 0 | Cherry |
| 1 | Strawberry |
| 2 | Grape |
| 3 | Orange |
| 4 | Dekopon |
| 5 | Apple |
| 6 | Pear |
| 7 | Peach |
| 8 | Pineapple |
| 9 | Melon |
| 10 | Watermelon |
