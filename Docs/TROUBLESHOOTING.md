# Troubleshooting Guide -- GAME 220 Merge Template

> Quick-reference for common student issues. Each entry lists the symptom, cause, and fix.

---

## Table of Contents

1. [Objects Do Not Merge (Same Type)](#1-objects-do-not-merge-same-type)
2. [Objects Do Not Merge (Wrong Tier Value)](#2-objects-do-not-merge-wrong-tier-value)
3. [Objects Do Not Merge (Missing Collider or Rigidbody)](#3-objects-do-not-merge-missing-collider-or-rigidbody)
4. [Objects Fall Through the Floor](#4-objects-fall-through-the-floor)
5. [New Object Type Does Not Appear in Game](#5-new-object-type-does-not-appear-in-game)
6. [Wrong Object Spawns After Merge](#6-wrong-object-spawns-after-merge)
7. [Object Has Wrong Size or Color](#7-object-has-wrong-size-or-color)
8. [Cannot Merge Final Tier Message](#8-cannot-merge-final-tier-message)
9. [base.Awake() Errors / NullReferenceException](#9-baseawake-errors--nullreferenceexception)
10. [Compiler Error: Does Not Implement Abstract Member](#10-compiler-error-does-not-implement-abstract-member-session-5)
11. [Game Over Triggers Too Quickly or Not At All](#11-game-over-triggers-too-quickly-or-not-at-all)
12. [Prefab Not Working After Adding Script](#12-prefab-not-working-after-adding-script)
13. [OnMerge Override Not Running](#13-onmerge-override-not-running-session-4)
14. [Multiple Objects Spawning on Merge](#14-multiple-objects-spawning-on-merge-double-merge-bug)

---
## 1. Objects Do Not Merge (Same Type)

### Symptom
Two objects of the same type (e.g., two TierZero objects) collide and stack on top of each other, but no merge happens. No error messages in the Console.

### Cause
The object prefab is missing one or more required components. The merge system requires:
- A script that inherits from MergeObject (e.g., TierZero, or a student-created class)
- A Rigidbody2D (required for OnCollisionEnter2D to fire)
- A CircleCollider2D (required for collision detection)

If the prefab has no MergeObject-derived script, GetComponent<MergeObject>() returns null in OnCollisionEnter2D and the merge check exits immediately.

### Fix
1. Select the object prefab in the Project window.
2. In the Inspector, verify it has ALL of the following components:
   - SpriteRenderer
   - Rigidbody2D (body type: Dynamic)
   - CircleCollider2D (NOT set as Trigger)
   - The correct derived script (e.g., TierZero, or your custom class)
3. If any component is missing, click Add Component and add it.
4. Make sure the CircleCollider2D **Is Trigger** checkbox is **unchecked**. The merge system uses OnCollisionEnter2D, not OnTriggerEnter2D.

---

## 2. Objects Do Not Merge (Wrong Tier Value)

### Symptom
Two objects that look the same collide but do not merge. Or, two different object types unexpectedly merge with each other.

### Cause
The tier value is set incorrectly in one or more derived classes. Merging only happens when both objects have the same tier value (the check is GetTier() == otherObject.GetTier()). If two objects of the same class have different tier values (e.g., one has tier = 3 and the other has tier = 4 due to a typo), they will not merge.

### Fix
1. Open the derived class script.
2. Verify the tier value matches the expected value for that class.
3. Make sure no two different classes share the same tier value.
4. Add a Debug.Log in Awake() to print the tier at startup if unsure:

   ```csharp
   Debug.Log(objectName + " tier = " + tier);
   ```

---

## 3. Objects Do Not Merge (Missing Collider or Rigidbody)

### Symptom
Objects overlap or pass through each other without any physics interaction. No collision events fire.

### Cause
Unity requires **both** colliding objects to have colliders, and **at least one** must have a Rigidbody2D, for OnCollisionEnter2D to fire. If either object is missing its CircleCollider2D or Rigidbody2D, collisions are not detected.

### Fix
1. Select each object prefab and confirm it has:
   - A CircleCollider2D (not set to Trigger)
   - A Rigidbody2D (body type: Dynamic)
2. If using a BoxCollider2D instead of CircleCollider2D, that will still work for collisions, but the shape may not match the round sprite.
3. Confirm neither collider has Is Trigger checked. Trigger colliders use OnTriggerEnter2D, which the merge system does not use.

---
## 4. Objects Fall Through the Floor

### Symptom
Objects drop and fall through the bottom of the container, disappearing off-screen.

### Cause A: Container walls are missing or misconfigured
The container is made of 3 manually placed GameObjects (LeftWall, RightWall, BottomWall) each with a BoxCollider2D. If any wall is missing from the scene or has no collider, objects will fall through.

### Fix A
1. Check the Hierarchy for LeftWall, RightWall, and BottomWall GameObjects.
2. If any are missing, create a new empty GameObject, name it appropriately, and add a BoxCollider2D.
3. Position the walls to form an open-top box (see SETUP_INSTRUCTIONS.md Step 3).

### Cause B: Object collider is set to Trigger
If the CircleCollider2D on the object prefab has Is Trigger checked, the object will not physically interact with the walls.

### Fix B
Select the object prefab, find the CircleCollider2D, and uncheck Is Trigger.

### Cause C: Layer collision matrix
If the objects and container walls are on layers that do not collide with each other, physics interactions will be ignored.

### Fix C
Go to Edit > Project Settings > Physics 2D and check the Layer Collision Matrix. Make sure the layers used by objects and walls are set to collide with each other.

---

## 5. New Object Type Does Not Appear in Game

### Symptom
A student wrote the code for a new derived class, but when two objects of the previous tier merge, nothing spawns. The Console shows a warning like:

   ```
   MergeObjectFactory: No prefab assigned for tier 3!
   ```

### Cause
The student created the script and possibly the prefab, but did not assign the prefab to the correct slot in the MergeObjectFactory Inspector.

### Fix
1. Confirm the object prefab exists in the `Assets/_Project/Prefabs/MergeObjects/` folder.
2. Select the MergeObjectFactory GameObject in the scene Hierarchy.
3. In the Inspector, find the Object Prefabs array.
4. Drag the object prefab into the correct slot (the slot index must match the tier value in the class).
5. If the array size is too small, increase it to accommodate the new tier.

---

## 6. Wrong Object Spawns After Merge

### Symptom
Two objects of the same tier merge, but the wrong type appears instead of the expected next tier.

### Cause
The prefabs are assigned to the wrong slots in the MergeObjectFactory array. The merge system calls CreateObjectAtPosition(nextTier, position) where nextTier = currentTier + 1. If a slot has the wrong prefab, the wrong object will spawn.

### Fix
1. Select the MergeObjectFactory GameObject.
2. Carefully verify every slot in the Object Prefabs array matches the correct tier.
3. The slot index must match the tier value set in the corresponding class's Awake() method.
4. A common mistake is dragging a prefab into the wrong slot, or having duplicate prefabs.

---
## 7. Object Has Wrong Size or Color

### Symptom
An object appears in the game but is the wrong color (e.g., white) or the wrong size (e.g., default scale of 1).

### Cause A: Awake() is empty or incomplete
The student created the class with the correct inheritance but forgot to set the objectSize and/or objectColor fields in Awake(). The base class defaults are objectSize = 1f and objectColor = Color.white.

### Fix A
Open the derived class script and verify all five fields are set in Awake():
- tier
- objectName
- pointValue
- objectSize
- objectColor

### Cause B: base.Awake() called before setting values
If the student calls base.Awake() at the top of their Awake() method instead of the bottom, the component references are cached before the values are set. While ApplyObjectProperties() runs in Start() (so size/color are usually fine), it is best practice to set fields first.

### Fix B
Move base.Awake() to the END of the derived Awake() method, after all field assignments.

### Cause C: Color values out of range
Unity Color constructor expects float values between 0.0f and 1.0f. If a student uses values like 255 (thinking of the 0-255 byte range), the color will be overbright white.

### Fix C
Remind students that Unity uses the 0.0-1.0 float range for colors, not 0-255. To convert: divide each value by 255. For example, RGB(200, 50, 50) becomes:

   ```csharp
   objectColor = new Color(200f / 255f, 50f / 255f, 50f / 255f);
   ```

---

## 8. Cannot Merge Final Tier Message

### Symptom
Two final-tier objects collide but do not merge. The Console prints: "Maximum tier reached! Cannot merge further."

### Cause
**This is expected behavior, not a bug.** The final tier in the merge chain overrides GetMergeResultTier() to return -1, which tells the merge system that no further merge is possible.

### Why This Exists
The GameManager.MergeObjects() method checks GetMergeResultTier() before spawning a new object. When it receives -1, it logs the message and skips the merge. This prevents the system from trying to create a nonexistent next tier.

### What to Tell Students
This message confirms that the GetMergeResultTier() override is working correctly. It is a sign of success, not failure. If a student does NOT see this message when two final-tier objects collide, it means they forgot to override GetMergeResultTier() in their final tier class.

### If Students Want Final-Tier Objects to Do Something on Collision
They could add special handling inside OnCollisionEnter2D or add a score bonus in the GameManager. Note that OnMerge() on the final tier will NOT be called during a same-tier collision because the GetMergeResultTier() check short-circuits before the OnMerge() calls.

---
## 9. base.Awake() Errors / NullReferenceException

### Symptom A: NullReferenceException at runtime
The game runs but crashes or shows errors like:

   ```
   NullReferenceException: Object reference not set to an instance of an object
   MergeObject.ApplyObjectProperties() (at Assets/_Project/Scripts/MergeObjects/MergeObject.cs:...)
   ```

### Cause A: Forgot to call base.Awake()
The student overrode Awake() and set their field values, but forgot to call base.Awake() at the end. Without base.Awake(), the component references (rb, spriteRenderer) are never cached and remain null.

### Fix A
Add base.Awake() as the last line inside the derived Awake() method:

   ```csharp
   protected override void Awake()
   {
       tier = 3;
       objectName = "MyObject";
       pointValue = 10;
       objectSize = 1.0f;
       objectColor = new Color(1.0f, 0.65f, 0.0f);

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
   error CS0534: 'MyClass' does not implement inherited abstract member
                 'MergeObject.InitializeMergeObjectProperties()'
   ```

### Cause
The MergeObject base class now has an abstract method (InitializeMergeObjectProperties()) that every derived class must implement. The student has not yet added the override in one or more derived classes.

### Fix
Add the InitializeMergeObjectProperties() override to the class that is showing the error. Move the field assignments from Awake() into this new method:

   ```csharp
   protected override void InitializeMergeObjectProperties()
   {
       tier = 3;
       objectName = "MyObject";
       pointValue = 10;
       objectSize = 1.0f;
       objectColor = new Color(1.0f, 0.65f, 0.0f);
   }
   ```

Once every derived class has this method, the Awake() override and base.Awake() call can be removed from the derived classes (since the base Awake() now calls InitializeMergeObjectProperties() automatically).

### Common Mistake
Students sometimes add the method but forget the override keyword. Without override, the method does not satisfy the abstract contract and the compiler error persists.

---
## 11. Game Over Triggers Too Quickly or Not At All

### Symptom A: Game over triggers almost immediately
The game declares game over within seconds of starting, even though no objects are near the top.

### Cause A: gameOverLineY is set too low
The GameManager.gameOverLineY value determines the Y position of the game over detection line. If this value is too low (e.g., 0 instead of 4.5), objects will be above the line as soon as they are dropped.

### Fix A
1. Select the GameManager GameObject in the scene.
2. In the Inspector, find the gameOverLineY field.
3. Set it to a value near the top of the container (default is 4.5).

### Symptom B: Game over never triggers
Objects stack far above the container and the game never ends.

### Cause B: gameOverLineY is set too high
If gameOverLineY is very high (e.g., 100), objects will never reach it.

### Fix B
Set gameOverLineY to approximately 4.5 (just above the container opening).

### Cause C: gameOverDelay is too long
The game over does not trigger instantly. An object must remain above the line for gameOverDelay seconds (default 2 seconds). If this value is very high, the game over takes a long time to trigger.

### Fix C
Set gameOverDelay to 2 seconds (default). A value of 1-3 seconds is reasonable.

---

## 12. Prefab Not Working After Adding Script

### Symptom
The student created a derived class and added it to a GameObject, but when that object is instantiated during a merge, it has no script behavior. It falls with physics but does not merge, has default white color, and default scale.

### Cause A: Script added to scene object but not saved to prefab
If the student adds the script to a GameObject in the scene but does not click "Apply" (or "Overrides > Apply All") to save changes back to the prefab, the prefab in the Project folder still lacks the script. When MergeObjectFactory instantiates the prefab, it creates a copy of the unmodified prefab.

### Fix A
1. Select the object GameObject in the Hierarchy (should show a blue name if it is a prefab instance).
2. In the Inspector, click the Overrides dropdown at the top and select Apply All.
3. Alternatively, drag the GameObject from the Hierarchy onto the existing prefab in the Project folder to replace it.

### Cause B: Wrong script attached
The student may have attached the wrong script (e.g., TierZero instead of their custom class) or attached the base MergeObject class directly.

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
4. The correct derived script

---
## 13. OnMerge Override Not Running (Session 4)

### Symptom
The student wrote a custom OnMerge() method in their derived class, but when that object merges, the custom Debug.Log message does not appear and the custom effects do not run. Instead, the default base class message appears.

### Cause A: Missing the override keyword
The student wrote:

   ```csharp
   public void OnMerge()  // WRONG: missing override
   {
       Debug.Log("Custom merge!");
   }
   ```

Without the override keyword, this method **hides** the base class version rather than **replacing** it. When the merge system calls OnMerge() through a MergeObject reference (which is how OnCollisionEnter2D works), the base class version runs because the method is not properly overridden.

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

### Cause C: Final-tier-specific issue
If the student added OnMerge() to their final tier class specifically, note that it will never be called during a same-tier collision at the final tier. The GetMergeResultTier() check returns -1, which causes OnCollisionEnter2D to exit before reaching the OnMerge() calls. See [Issue 8](#8-cannot-merge-final-tier-message) for details.

---

## 14. Multiple Objects Spawning on Merge (Double-Merge Bug)

### Symptom
When two objects merge, two (or more) copies of the next object appear instead of one. Sometimes objects also disappear unexpectedly.

### Cause A: hasMerged flag not working
The base class uses a private boolean hasMerged to prevent the same object from being claimed by multiple simultaneous collisions. If this flag is not being checked properly (e.g., if someone modified OnCollisionEnter2D), both objects in a collision might each independently trigger a merge, resulting in two merge events.

### Fix A
Verify that the OnCollisionEnter2D method in MergeObject.cs has not been modified. The critical lines are:

   ```csharp
   if (hasMerged || otherObject.HasMerged()) return;
   ```

and:

   ```csharp
   SetMerged();
   otherObject.SetMerged();
   ```

These lines must appear in this order: check first, then set. If a student accidentally deleted or reordered these lines, restore them from the original template.

### Cause B: Student added their own OnCollisionEnter2D
If a student added a second OnCollisionEnter2D method in their derived class, it may conflict with the base class version. In C#, a derived class method with the same name (without override) hides the base version.

### Fix B
Remove any OnCollisionEnter2D methods from derived classes. The merge detection logic should ONLY exist in the base MergeObject class. Derived classes customize behavior through OnMerge() overrides, not by duplicating collision code.

### Cause C: Multiple MergeObject scripts on one prefab
If a prefab accidentally has two MergeObject-derived scripts attached (e.g., both TierZero and TierOne on the same GameObject), each script independently runs OnCollisionEnter2D, causing duplicate merge processing.

### Fix C
Select the prefab and check the Inspector. Each object prefab should have exactly ONE script that inherits from MergeObject. Remove any duplicate or incorrect scripts.

---

## Quick Reference: Provided Tier Table

Use this table to verify the provided example values are correct.

| Tier | Class | objectName | pointValue | objectSize | objectColor (R, G, B) |
|------|-------|-----------|------------|-----------|----------------------|
| 0 | TierZero | TierZero | 1 | 0.5f | (0.85, 0.12, 0.15) |
| 1 | TierOne | TierOne | 3 | 0.65f | (0.95, 0.30, 0.35) |
| 2 | TierTwo | TierTwo | 6 | 0.8f | (0.55, 0.27, 0.68) |

Student-created classes will have tier values starting from 3 and beyond.

---

## Quick Reference: Required Prefab Components

Every object prefab must have these four components:

| Component | Settings |
|---|---|
| SpriteRenderer | Any sprite (default circle works); color is overridden by script |
| Rigidbody2D | Body Type: Dynamic; Gravity Scale: 1 |
| CircleCollider2D | Is Trigger: unchecked |
| [ClassName] script | The correct derived class (TierZero, TierOne, or student-created) |

---

## Quick Reference: MergeObjectFactory Array Slots

| Slot | Prefab |
|------|--------|
| 0 | TierZero |
| 1 | TierOne |
| 2 | TierTwo |
| 3+ | Student-created classes |

The array starts with 5 slots. Students expand it as they add more tiers.

Our Business is Fun
