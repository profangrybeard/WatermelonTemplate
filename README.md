# Merge Template - Teaching Guide

## GAME 220: Core Programming - Inheritance and Polymorphism

This template teaches **inheritance** and **polymorphism** through a merge game (inspired by Suika Game). Students start with a fully playable 3-tier game and progressively build out their own merge object classes, learning OOP concepts hands-on as each class extends the `MergeObject` base class.

The merge system, physics, and game loop are **pre-built**. Students focus exclusively on creating derived classes, overriding virtual methods, and adding custom behavior -- the core skills of object-oriented programming.

---

## The MVP Approach

The game **works out of the box** with 3 merge objects (TierZero, TierOne, TierTwo). Students open the project, press Play, and immediately see dropping, stacking, and merging in action. From Session 2 onward, students create their own merge object classes from scratch to extend the merge chain. Each new class they complete is immediately playable -- they never wait for "enough code" before testing.

This follows the same philosophy as the SlitherTemplate: **give students a working game on day one, then let them make it their own.**

---

## What Students Write vs What's Provided

| Component | Status | Details |
|---|---|---|
| `MergeObject.cs` (base class) | **Provided** | All shared fields, virtual methods, merge detection, physics helpers |
| `TierZero.cs` | **Provided** | Complete derived class -- primary reference example |
| `TierOne.cs` | **Provided** | Complete derived class -- second reference example |
| `TierTwo.cs` | **Provided** | Complete derived class -- third reference example |
| Student classes | **Student-Created** | Sessions 2-3 -- students create their own derived classes from scratch |
| `GameManager.cs` | **Provided** | Merge execution, object tracking, game over detection |
| `MergeObjectFactory.cs` | **Provided** | Prefab array, polymorphic `CreateObject()` method |
| `DropController.cs` | **Provided** | Mouse input, aiming, dropping with cooldown |

---

## Session Breakdown

### Session 1: Explore the Template
**Status:** Game runs with TierZero, TierOne, and TierTwo already working.

**What Works:**
- Drop objects with mouse click
- Aim with mouse movement
- Two same-tier objects merge into the next tier (TierZero + TierZero = TierOne)
- Score logged to Console on each merge (Debug.Log)
- Game over triggers when objects stack above the line

**Teaching Points:**
- Play the game first -- understand the mechanics before reading code
- Open `MergeObject.cs` and read through the base class with its teaching comments
- Open `TierZero.cs` and trace the inheritance: `TierZero : MergeObject`
- Identify the pattern: override `Awake()`, set values, call `base.Awake()`
- Compare `TierZero.cs`, `TierOne.cs`, and `TierTwo.cs` -- same structure, different values
- Discuss: What does `protected` mean? Why not `public` or `private`?
- Discuss: What does `virtual` mean? What does `override` mean?

**Instructor Notes:**
- Walk through `MergeObject.cs` top to bottom, pausing at each TEACHING comment block
- Draw the inheritance diagram on the board: `MonoBehaviour -> MergeObject -> TierZero/TierOne/TierTwo`
- Let students open `GameManager.cs` and find `List<MergeObject>` -- explain that one list holds ALL object types
- End with: "Next session you will create your own derived classes following the TierZero pattern"

---

### Session 2: First Derived Classes (Student-Built)
**Status:** Students create their own derived classes from scratch.

**What Works After This Session:**
- Merge chain extends with student-created tiers
- Students have written their first derived classes

**Teaching Points:**
- Live-code a new derived class together as a class, following `TierZero.cs` line by line
- Students create additional classes independently (or in pairs)
- Create prefabs for each new object (see Docs/MERGE_REFERENCE.md for prefab setup steps)
- Assign prefabs to `MergeObjectFactory` inspector slots
- Test after EACH class -- do not batch all before testing
- Discuss: Why does `base.Awake()` need to be called? What happens if you forget it?
- Discuss: Why can `MergeObjectFactory.CreateObject()` return any object type as a `MergeObject` reference?

**Common Student Mistakes:**
- Forgetting `base.Awake()` -- object appears but has no physics or rendering
- Using `private` instead of `protected override` on `Awake()`
- Wrong tier number -- causes merge chain to break (merges skip a tier or loop)
- Forgetting to create the prefab or assign it to the correct `MergeObjectFactory` slot

---

### Session 3: More Classes + Polymorphism Deep Dive
**Status:** Students continue creating derived classes and explore polymorphism.

**What Works After This Session:**
- Extended merge chain with multiple student-created tiers
- Students encounter their first second-method override (`GetMergeResultTier()` on the final tier)

**Teaching Points:**
- Students should be faster now -- the pattern is muscle memory after several classes
- The final tier is the interesting one: it overrides **two** methods (`Awake()` and `GetMergeResultTier()`)
- Walk through `GameManager.MergeObjects()` and trace how polymorphism dispatches the correct method
- Walk through `GameManager.CountObjectsOfTier()` -- same loop, works for all types
- Demonstrate: add a `Debug.Log()` to one class's `Awake()` and show it runs instead of the base version
- Discuss: "What if we had 100 types? Would we need to change `GameManager` at all?" (No.)
- Discuss: How does `GetComponent<MergeObject>()` find a `TierZero` script? (Because TierZero IS-A MergeObject)

**Final Tier Special Case:**
- The final tier overrides `GetMergeResultTier()` to return `-1` (no further merge)
- This is the only class that needs this override -- all others use the default `tier + 1`
- Good discussion topic: "When should you override a virtual method vs. just use the default?"

---

### Session 4: OnMerge() Overrides -- Custom Behavior
**Status:** Students override `OnMerge()` in their classes to add custom effects.

**What Works After This Session:**
- Each object can have unique merge behavior (sounds, messages, visual effects)
- Students see polymorphism in action: same `OnMerge()` call, different behavior per type

**Teaching Points:**
- The base `OnMerge()` just logs a message -- students override it to do something interesting
- Start by uncommenting the `OnMerge()` example in `TierZero.cs` (it is in the comments)
- Show how `MergeObject.OnCollisionEnter2D()` calls `OnMerge()` on both objects -- the derived version runs
- This is **polymorphism in action**: the merge code does not know or care which object type it is calling
- Students choose what their `OnMerge()` does -- this is the first truly creative coding task
- Encourage variety: one class logs a joke, another changes the background color, another shakes the camera

**Example OnMerge() Overrides to Suggest:**
```csharp
// Simple: custom debug message
public override void OnMerge()
{
    Debug.Log("Two objects merged!");
}

// Intermediate: change the background color
public override void OnMerge()
{
    Camera.main.backgroundColor = objectColor;
}

// Advanced: spawn particles or play a sound
public override void OnMerge()
{
    Debug.Log(objectName + " merged for " + pointValue + " points!");
    // Students can add AudioSource.PlayClipAtPoint() or Instantiate() a particle prefab
}
```

---

### Session 5: Abstract Refactor (Stretch Goal)
**Status:** Advanced students convert `MergeObject` from a concrete class with `virtual` methods to an `abstract` class.

**What Works After This Session:**
- `MergeObject` cannot be instantiated directly (enforced by the compiler)
- Derived classes are REQUIRED to override certain methods (compiler error if they forget)
- Conceptual understanding of abstract vs. virtual

**Teaching Points:**
- Change `public class MergeObject` to `public abstract class MergeObject`
- Add a new abstract method: `protected abstract void InitializeMergeObjectProperties();`
- Update `Awake()` to call `InitializeMergeObjectProperties()` before caching components
- Derived classes replace their `Awake()` override with an `InitializeMergeObjectProperties()` override (no more `base.Awake()` needed)
- Show the compiler errors that appear when a derived class does NOT implement `InitializeMergeObjectProperties()`
- Discuss: When do you want `virtual` (optional override) vs. `abstract` (required override)?
- Discuss: Why would a game designer want to prevent someone from creating a generic "MergeObject" with no data?

**Instructor Notes:**
- This is a stretch goal -- not all students will reach it
- The game works perfectly fine without this refactor
- The value is conceptual: understanding the abstract/virtual spectrum
- Students who finish early in Session 4 can attempt this

---

### Session 6: Polish and Features
**Status:** Core gameplay is complete. Students add polish and student-driven features.

**What Works After This Session:**
- Students have chosen and begun implementing at least one feature from the feature menu
- Game has personalized touches (custom colors, sounds, UI additions)

**Teaching Points:**
- Review the Student-Driven Feature Ideas menu (below)
- Students pick features based on their comfort level
- Encourage students to plan before coding: "What scripts do you need to modify? What new scripts?"
- Code review: pair students up to review each other's `OnMerge()` implementations from Session 4
- Discuss: How does the architecture (base class + derived classes) make it easy to add new features?

---

### Session 7: Build and Test
**Status:** Students prepare their game for presentation.

**What Works After This Session:**
- Game is feature-complete and tested
- Built as a standalone executable (File > Build Settings > Build)
- Known bugs are documented or fixed

**Teaching Points:**
- Build the game: File > Build Settings > select platform > Build
- Test the build outside of Unity -- does everything work?
- Peer testing: students play each other's builds and give feedback
- Bug fixing: address any issues found during testing
- Final polish pass: adjust colors, sizes, speeds, UI text

---

### Session 8: Presentations
**Status:** Students present their finished games and explain the code.

**Teaching Points:**
- Each student (or pair) presents their game to the class
- Require students to explain at least one OOP concept using their own code:
  - "This is my TierThree class. It inherits from MergeObject, which means..."
  - "When two TierThree objects collide, OnMerge() runs MY version because of polymorphism..."
  - "I overrode GetMergeResultTier() in my final tier to return -1 because..."
- Discuss as a class: What would change if we wanted to add another tier? (Almost nothing!)
- Reflect: How is inheritance different from just copying and pasting code into every class?

---

## Key Vocabulary

| Term | Meaning |
|---|---|
| **Base class** | A class that other classes inherit from. `MergeObject` is the base class for all merge object types. It defines the shared fields and methods that every object has. |
| **Derived class** | A class that inherits from a base class. `TierZero`, `TierOne`, `TierTwo`, etc. are derived classes of `MergeObject`. They get everything `MergeObject` has and can customize it. |
| **`protected`** | An access modifier meaning "this class AND its children can access this." Used for fields like `tier`, `objectSize`, and `objectColor` that derived classes need to set. More restrictive than `public`, less restrictive than `private`. |
| **`virtual`** | A keyword on a method meaning "derived classes are ALLOWED to override this, but they don't have to." The base class provides a default implementation. Example: `MergeObject.OnMerge()` logs a message by default. |
| **`override`** | A keyword on a method meaning "I am replacing the base class version with my own version." Used in derived classes to customize behavior. Example: `TierZero` overrides `Awake()` to set `tier = 0`. |
| **`abstract`** | A keyword meaning "derived classes MUST override this -- there is no default." An abstract class cannot be instantiated directly. Session 5 stretch goal. |
| **Polymorphism** | "Many forms." When code written for the base type (`MergeObject`) automatically works with any derived type (`TierZero`, `TierOne`, `TierTwo`). A `List<MergeObject>` can hold all object types, and calling `GetTier()` on each one returns the correct value. |
| **`base.Method()`** | Calls the parent class's version of a method from within an override. `base.Awake()` in `TierZero` runs `MergeObject.Awake()` to cache component references. Forgetting this call is the most common student bug. |

---

## Student-Driven Feature Ideas

Students choose from this menu based on interest and skill level. These are suggestions, not requirements.

### Beginner Features
- **Custom object colors** -- Change the RGBA values in your classes to your own palette
- **Score UI** -- Add a Canvas with Text to display the score on screen instead of just the Console
- **Adjusted drop speed** -- Modify `dropCooldown` in the DropController Inspector
- **Container size tweaks** -- Adjust wall positions and collider sizes in the scene
- **Custom debug messages** -- Add personality to each object's `OnMerge()` log messages

### Intermediate Features
- **Background color change on merge** -- `Camera.main.backgroundColor = objectColor;` in `OnMerge()`
- **Merge counter** -- Track and display total number of merges performed
- **Combo system** -- Award bonus points for merges that happen within a short time window
- **Next object preview** -- Show the player what object they will drop next
- **Drop trajectory line** -- Draw a line downward from the current object to show where it will land
- **Object count HUD** -- Display how many of each type are currently in the container

### Advanced Features
- **Screen shake on big merges** -- Move the camera briefly when high-tier objects merge
- **Particle effects** -- Instantiate a particle system prefab in `OnMerge()`
- **Sound effects** -- Play a clip using `AudioSource.PlayClipAtPoint()` in `OnMerge()`
- **Merge chain animation** -- Brief scale pulse on the newly created object after a merge
- **High score persistence** -- Save and load the best score using `PlayerPrefs`
- **Evolution tracker** -- UI panel that shows which tiers the player has created this game
- **Custom sprites** -- Replace the circle sprite with actual themed artwork

---

## Student Implementation Checklist

### Session 1 Checklist
- [ ] Opened the project in Unity and pressed Play
- [ ] Dropped objects and observed merging behavior
- [ ] Read `MergeObject.cs` top to bottom (all teaching comments)
- [ ] Read `TierZero.cs` and identified: class declaration, override, field assignments, `base.Awake()`
- [ ] Compared `TierZero.cs`, `TierOne.cs`, and `TierTwo.cs` -- identified the shared pattern
- [ ] Can answer: What does `protected` mean? What does `virtual` mean?

### Session 2 Checklist
- [ ] Created a new derived class from scratch -- override `Awake()`, set all 5 values, call `base.Awake()`
- [ ] Created a prefab (GameObject with SpriteRenderer, Rigidbody2D, CircleCollider2D, your script)
- [ ] Assigned prefab to the correct `MergeObjectFactory` slot
- [ ] Tested: merging works with the new class
- [ ] Created additional classes -- same pattern, different values
- [ ] Created prefabs and assigned to `MergeObjectFactory` slots
- [ ] Tested: merge chain extends correctly
- [ ] Can answer: Why do we call `base.Awake()`? What happens if we forget?

### Session 3 Checklist
- [ ] Created additional derived classes, each with prefab and factory slot
- [ ] Final tier overrides `GetMergeResultTier()` to return `-1`
- [ ] Tested: Two final-tier objects collide but do NOT merge (expected behavior)
- [ ] Full merge chain works from first to last tier
- [ ] Can answer: How does one `List<MergeObject>` hold TierZero, TierOne, and TierTwo at the same time?

### Session 4 Checklist
- [ ] Added `OnMerge()` override to at least 3 classes
- [ ] Each override does something different (demonstrates polymorphism)
- [ ] Tested: merge two of each overridden type and confirmed custom behavior runs
- [ ] Can answer: Why does the collision code call `OnMerge()` without knowing the object type?
- [ ] Can explain polymorphism using your own code as an example

### Session 5 Checklist (Stretch)
- [ ] Changed `MergeObject` class declaration to `public abstract class MergeObject`
- [ ] Added `protected abstract void InitializeMergeObjectProperties();` to `MergeObject.cs`
- [ ] Updated `Awake()` to call `InitializeMergeObjectProperties()` before caching components
- [ ] Replaced `Awake()` overrides in all derived classes with `InitializeMergeObjectProperties()` overrides
- [ ] Confirmed all derived classes compile (no missing overrides)
- [ ] Can answer: What is the difference between `virtual` and `abstract`?

### Sessions 6-8 Checklist
- [ ] Chose and implemented at least one feature from the feature menu
- [ ] Game builds and runs as a standalone executable
- [ ] Tested build outside of Unity
- [ ] Prepared a brief explanation of one OOP concept using your own code
- [ ] Presented game to the class

---

## Assessment Ideas

### Formative (During Sessions)
- **Code walkthrough:** Ask individual students to explain what `base.Awake()` does while you watch
- **Predict-before-play:** Before testing a new class, ask "What will happen when two of these collide?"
- **Spot the bug:** Show a derived class with a missing `base.Awake()` call -- can students identify the problem?
- **Vocabulary check:** Point to a line of code and ask "Which OOP concept is this demonstrating?"
- **Peer review:** Students read a partner's `OnMerge()` override and explain what it does

### Summative (End of Unit)
- **Completed project:** All student-created classes compile, all prefabs assigned, full merge chain works
- **Code reading quiz:** Given an unfamiliar derived class, students identify the base class, overridden methods, and explain why `base.Awake()` is called
- **Short answer:** "Explain how `GameManager.MergeObjects()` can handle any two object types without checking which type they are." (Answer: polymorphism through virtual method dispatch)
- **Extension task:** "Create a new derived class that inherits from `MergeObject`. It should have a unique tier, point value, and override `OnMerge()` to log a victory message. What else would you need to change?" (Answer: create the prefab, assign to a new slot in MergeObjectFactory, update the previous final tier's `GetMergeResultTier()` to return the new tier instead of -1)
- **Presentation rubric:** Game runs, student can explain inheritance and polymorphism using their own code, at least one custom feature implemented

Our Business is Fun
