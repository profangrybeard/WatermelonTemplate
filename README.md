# Watermelon Merge Template - Teaching Guide

## GAME 220: Core Programming - Inheritance and Polymorphism

This template teaches **inheritance** and **polymorphism** through a Watermelon Merge game (inspired by Suika Game). Students start with a fully playable 3-fruit game and progressively build out the remaining 8 fruit classes, learning OOP concepts hands-on as each fruit extends the `Fruit` base class.

The merge system, physics, UI, and game loop are **pre-built**. Students focus exclusively on creating derived classes, overriding virtual methods, and adding custom behavior -- the core skills of object-oriented programming.

---

## The MVP Approach

The game **works out of the box** with 3 fruits (Cherry, Strawberry, Grape). Students open the project, press Play, and immediately see dropping, stacking, and merging in action. From Session 2 onward, students fill in TODO-marked fruit classes to extend the merge chain. Each new fruit they complete is immediately playable -- they never wait for "enough code" before testing.

This follows the same philosophy as the SlitherTemplate: **give students a working game on day one, then let them make it their own.**

---

## What Students Write vs What's Provided

| Component | Status | Details |
|---|---|---|
| `Fruit.cs` (base class) | **Provided** | All shared fields, virtual methods, merge detection, physics helpers |
| `Cherry.cs` | **Provided** | Complete derived class -- primary reference example |
| `Strawberry.cs` | **Provided** | Complete derived class -- second reference example |
| `Grape.cs` | **Provided** | Complete derived class -- third reference example |
| `Orange.cs` | **Student TODO** | Session 2 -- first student-built derived class (tier 3) |
| `Dekopon.cs` | **Student TODO** | Session 2 -- second student-built derived class (tier 4) |
| `Apple.cs` | **Student TODO** | Session 2 -- third student-built derived class (tier 5) |
| `Pear.cs` | **Student TODO** | Session 3 -- tier 6 |
| `Peach.cs` | **Student TODO** | Session 3 -- tier 7 |
| `Pineapple.cs` | **Student TODO** | Session 3 -- tier 8 |
| `Melon.cs` | **Student TODO** | Session 3 -- tier 9 |
| `Watermelon.cs` | **Student TODO** | Session 3 -- tier 10, special `GetMergeResultTier()` override |
| `GameManager.cs` | **Provided** | Merge execution, fruit tracking, game over detection |
| `FruitFactory.cs` | **Provided** | Prefab array, polymorphic `CreateFruit()` method |
| `DropController.cs` | **Provided** | Input handling, aiming, dropping with cooldown |
| `ScoreManager.cs` | **Provided** | Score display, game over UI |
| `ContainerSetup.cs` | **Provided** | Procedural wall generation |

---

## Session Breakdown

### Session 1: Explore the Template
**Status:** Game runs with Cherry, Strawberry, and Grape already working.

**What Works:**
- Drop fruits with mouse click or spacebar
- Aim with mouse movement or A/D keys
- Two same-tier fruits merge into the next tier (Cherry + Cherry = Strawberry)
- Score increases on each merge
- Game over triggers when fruits stack above the line

**Teaching Points:**
- Play the game first -- understand the mechanics before reading code
- Open `Fruit.cs` and read through the base class with its teaching comments
- Open `Cherry.cs` and trace the inheritance: `Cherry : Fruit`
- Identify the pattern: override `Awake()`, set values, call `base.Awake()`
- Compare `Cherry.cs`, `Strawberry.cs`, and `Grape.cs` -- same structure, different values
- Open `Orange.cs` and read the TODO comment -- this is what you build next session
- Discuss: What does `protected` mean? Why not `public` or `private`?
- Discuss: What does `virtual` mean? What does `override` mean?

**Instructor Notes:**
- Walk through `Fruit.cs` top to bottom, pausing at each TEACHING comment block
- Draw the inheritance diagram on the board: `MonoBehaviour -> Fruit -> Cherry/Strawberry/Grape`
- Let students open `GameManager.cs` and find `List<Fruit>` -- explain that one list holds ALL fruit types
- End with: "Next session you will create Orange, Dekopon, and Apple following Cherry's pattern"

---

### Session 2: First Derived Classes (Student-Built)
**Status:** Students complete `Orange.cs`, `Dekopon.cs`, and `Apple.cs`.

**What Works After This Session:**
- Merge chain extends through tier 5 (Apple)
- 6 total fruit types dropping and merging
- Students have written their first derived classes

**Teaching Points:**
- Live-code `Orange.cs` together as a class, following `Cherry.cs` line by line
- Students complete `Dekopon.cs` and `Apple.cs` independently (or in pairs)
- Create prefabs for each new fruit (see Docs/FRUIT_REFERENCE.md for prefab setup steps)
- Assign prefabs to `FruitFactory` inspector slots 3, 4, and 5
- Test after EACH fruit -- do not batch all three before testing
- Discuss: Why does `base.Awake()` need to be called? What happens if you forget it?
- Discuss: Why can `FruitFactory.CreateFruit()` return any fruit type as a `Fruit` reference?

**Common Student Mistakes:**
- Forgetting `base.Awake()` -- fruit appears but has no physics or rendering
- Using `private` instead of `protected override` on `Awake()`
- Wrong tier number -- causes merge chain to break (merges skip a fruit or loop)
- Forgetting to create the prefab or assign it to the correct `FruitFactory` slot

---

### Session 3: Remaining Fruits + Polymorphism Deep Dive
**Status:** Students complete `Pear.cs`, `Peach.cs`, `Pineapple.cs`, `Melon.cs`, and `Watermelon.cs`.

**What Works After This Session:**
- Full 11-fruit merge chain is complete
- Watermelon is reachable through successive merges
- Students encounter their first second-method override (`GetMergeResultTier()` on Watermelon)

**Teaching Points:**
- Students should be faster now -- the pattern is muscle memory by fruit 4 or 5
- `Watermelon.cs` is the interesting one: it overrides **two** methods (`Awake()` and `GetMergeResultTier()`)
- Walk through `GameManager.MergeFruits()` and trace how polymorphism dispatches the correct method
- Walk through `GameManager.CountFruitsOfTier()` -- same loop, works for all 11 types
- Demonstrate: add a `Debug.Log()` to one fruit's `Awake()` and show it runs instead of the base version
- Discuss: "What if we had 100 fruit types? Would we need to change `GameManager` at all?" (No.)
- Discuss: How does `GetComponent<Fruit>()` find a `Cherry` script? (Because Cherry IS-A Fruit)

**Watermelon Special Case:**
- Watermelon overrides `GetMergeResultTier()` to return `-1` (no further merge)
- This is the only fruit that needs this override -- all others use the default `tier + 1`
- Good discussion topic: "When should you override a virtual method vs. just use the default?"

---

### Session 4: OnMerge() Overrides -- Custom Behavior
**Status:** Students override `OnMerge()` in their fruit classes to add custom effects.

**What Works After This Session:**
- Each fruit can have unique merge behavior (sounds, messages, visual effects)
- Students see polymorphism in action: same `OnMerge()` call, different behavior per type

**Teaching Points:**
- The base `OnMerge()` just logs a message -- students override it to do something interesting
- Start by uncommenting the `OnMerge()` example in `Cherry.cs` (it is in the comments)
- Show how `Fruit.OnCollisionEnter2D()` calls `OnMerge()` on both fruits -- the derived version runs
- This is **polymorphism in action**: the merge code does not know or care which fruit type it is calling
- Students choose what their `OnMerge()` does -- this is the first truly creative coding task
- Encourage variety: one fruit logs a joke, another changes the background color, another shakes the camera

**Example OnMerge() Overrides to Suggest:**
```csharp
// Simple: custom debug message
public override void OnMerge()
{
    Debug.Log("Citrus explosion! Two oranges merged!");
}

// Intermediate: change the background color
public override void OnMerge()
{
    Camera.main.backgroundColor = fruitColor;
}

// Advanced: spawn particles or play a sound
public override void OnMerge()
{
    Debug.Log(fruitName + " merged for " + pointValue + " points!");
    // Students can add AudioSource.PlayClipAtPoint() or Instantiate() a particle prefab
}
```

---

### Session 5: Abstract Refactor (Stretch Goal)
**Status:** Advanced students convert `Fruit` from a concrete class with `virtual` methods to an `abstract` class.

**What Works After This Session:**
- `Fruit` cannot be instantiated directly (enforced by the compiler)
- Derived classes are REQUIRED to override certain methods (compiler error if they forget)
- Conceptual understanding of abstract vs. virtual

**Teaching Points:**
- Change `public class Fruit` to `public abstract class Fruit`
- Add a new abstract method: `protected abstract void InitializeFruitProperties();`
- Update `Awake()` to call `InitializeFruitProperties()` before caching components
- Derived classes replace their `Awake()` override with an `InitializeFruitProperties()` override (no more `base.Awake()` needed)
- Show the compiler errors that appear when a derived class does NOT implement `InitializeFruitProperties()`
- Discuss: When do you want `virtual` (optional override) vs. `abstract` (required override)?
- Discuss: Why would a game designer want to prevent someone from creating a generic "Fruit" with no data?

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
  - "This is my Orange class. It inherits from Fruit, which means..."
  - "When two Oranges collide, OnMerge() runs MY version because of polymorphism..."
  - "I overrode GetMergeResultTier() in Watermelon to return -1 because..."
- Discuss as a class: What would change if we wanted to add a 12th fruit? (Almost nothing!)
- Reflect: How is inheritance different from just copying and pasting code into every class?

---

## Key Vocabulary

| Term | Meaning |
|---|---|
| **Base class** | A class that other classes inherit from. `Fruit` is the base class for all fruit types. It defines the shared fields and methods that every fruit has. |
| **Derived class** | A class that inherits from a base class. `Cherry`, `Grape`, `Orange`, etc. are derived classes of `Fruit`. They get everything `Fruit` has and can customize it. |
| **`protected`** | An access modifier meaning "this class AND its children can access this." Used for fields like `tier`, `fruitSize`, and `fruitColor` that derived classes need to set. More restrictive than `public`, less restrictive than `private`. |
| **`virtual`** | A keyword on a method meaning "derived classes are ALLOWED to override this, but they don't have to." The base class provides a default implementation. Example: `Fruit.OnMerge()` logs a message by default. |
| **`override`** | A keyword on a method meaning "I am replacing the base class version with my own version." Used in derived classes to customize behavior. Example: `Cherry` overrides `Awake()` to set `tier = 0`. |
| **`abstract`** | A keyword meaning "derived classes MUST override this -- there is no default." An abstract class cannot be instantiated directly. Session 5 stretch goal. |
| **Polymorphism** | "Many forms." When code written for the base type (`Fruit`) automatically works with any derived type (`Cherry`, `Grape`, `Orange`). A `List<Fruit>` can hold all 11 fruit types, and calling `GetTier()` on each one returns the correct value. |
| **`base.Method()`** | Calls the parent class's version of a method from within an override. `base.Awake()` in `Cherry` runs `Fruit.Awake()` to cache component references. Forgetting this call is the most common student bug. |

---

## Student-Driven Feature Ideas

Students choose from this menu based on interest and skill level. These are suggestions, not requirements.

### Beginner Features
- **Custom fruit colors** -- Change the RGBA values in your fruit classes to your own palette
- **Score display improvements** -- Add "Highest Fruit" text, change font size, reposition UI
- **Adjusted drop speed** -- Modify `dropCooldown` in the DropController Inspector
- **Container size tweaks** -- Change container `width` and `height` in the Inspector
- **Custom debug messages** -- Add personality to each fruit's `OnMerge()` log messages

### Intermediate Features
- **Background color change on merge** -- `Camera.main.backgroundColor = fruitColor;` in `OnMerge()`
- **Merge counter** -- Track and display total number of merges performed
- **Combo system** -- Award bonus points for merges that happen within a short time window
- **Next fruit preview** -- Show the player what fruit they will drop next
- **Drop trajectory line** -- Draw a line downward from the current fruit to show where it will land
- **Fruit count HUD** -- Display how many of each fruit type are currently in the container

### Advanced Features
- **Screen shake on big merges** -- Move the camera briefly when high-tier fruits merge
- **Particle effects** -- Instantiate a particle system prefab in `OnMerge()`
- **Sound effects** -- Play a clip using `AudioSource.PlayClipAtPoint()` in `OnMerge()`
- **Merge chain animation** -- Brief scale pulse on the newly created fruit after a merge
- **High score persistence** -- Save and load the best score using `PlayerPrefs`
- **Evolution tracker** -- UI panel that shows which fruits the player has created this game
- **Custom fruit sprites** -- Replace the circle sprite with actual fruit artwork

---

## Student Implementation Checklist

### Session 1 Checklist
- [ ] Opened the project in Unity and pressed Play
- [ ] Dropped fruits and observed merging behavior
- [ ] Read `Fruit.cs` top to bottom (all teaching comments)
- [ ] Read `Cherry.cs` and identified: class declaration, override, field assignments, `base.Awake()`
- [ ] Compared `Cherry.cs`, `Strawberry.cs`, and `Grape.cs` -- identified the shared pattern
- [ ] Opened `Orange.cs` and read the TODO instructions
- [ ] Can answer: What does `protected` mean? What does `virtual` mean?

### Session 2 Checklist
- [ ] Completed `Orange.cs` -- override `Awake()`, set all 5 values, call `base.Awake()`
- [ ] Created Orange prefab (GameObject with SpriteRenderer, Rigidbody2D, CircleCollider2D, Orange script)
- [ ] Assigned Orange prefab to `FruitFactory` slot 3
- [ ] Tested: Grape + Grape = Orange (merges correctly)
- [ ] Completed `Dekopon.cs` -- same pattern, different values
- [ ] Created Dekopon prefab and assigned to `FruitFactory` slot 4
- [ ] Tested: Orange + Orange = Dekopon
- [ ] Completed `Apple.cs` -- same pattern, different values
- [ ] Created Apple prefab and assigned to `FruitFactory` slot 5
- [ ] Tested: Dekopon + Dekopon = Apple
- [ ] Can answer: Why do we call `base.Awake()`? What happens if we forget?

### Session 3 Checklist
- [ ] Completed `Pear.cs` (tier 6), created prefab, assigned to slot 6, tested
- [ ] Completed `Peach.cs` (tier 7), created prefab, assigned to slot 7, tested
- [ ] Completed `Pineapple.cs` (tier 8), created prefab, assigned to slot 8, tested
- [ ] Completed `Melon.cs` (tier 9), created prefab, assigned to slot 9, tested
- [ ] Completed `Watermelon.cs` (tier 10), created prefab, assigned to slot 10, tested
- [ ] Watermelon overrides `GetMergeResultTier()` to return `-1`
- [ ] Tested: Two Watermelons collide but do NOT merge (expected behavior)
- [ ] Full merge chain works: Cherry through Watermelon
- [ ] Can answer: How does one `List<Fruit>` hold Cherry, Grape, and Orange at the same time?

### Session 4 Checklist
- [ ] Added `OnMerge()` override to at least 3 fruit classes
- [ ] Each override does something different (demonstrates polymorphism)
- [ ] Tested: merge two of each overridden fruit and confirmed custom behavior runs
- [ ] Can answer: Why does the collision code call `OnMerge()` without knowing the fruit type?
- [ ] Can explain polymorphism using your own code as an example

### Session 5 Checklist (Stretch)
- [ ] Changed `Fruit` class declaration to `public abstract class Fruit`
- [ ] Added `protected abstract void InitializeFruitProperties();` to `Fruit.cs`
- [ ] Updated `Awake()` to call `InitializeFruitProperties()` before caching components
- [ ] Replaced `Awake()` overrides in all derived classes with `InitializeFruitProperties()` overrides
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
- **Predict-before-play:** Before testing a new fruit, ask "What will happen when two of these collide?"
- **Spot the bug:** Show a fruit class with a missing `base.Awake()` call -- can students identify the problem?
- **Vocabulary check:** Point to a line of code and ask "Which OOP concept is this demonstrating?"
- **Peer review:** Students read a partner's `OnMerge()` override and explain what it does

### Summative (End of Unit)
- **Completed project:** All 11 fruit classes compile, all prefabs assigned, full merge chain works
- **Code reading quiz:** Given an unfamiliar derived class, students identify the base class, overridden methods, and explain why `base.Awake()` is called
- **Short answer:** "Explain how `GameManager.MergeFruits()` can handle any two fruit types without checking which type they are." (Answer: polymorphism through virtual method dispatch)
- **Extension task:** "Create a 12th fruit class called `GoldenWatermelon` that inherits from `Fruit`. It should have tier 11, be worth 100 points, and override `OnMerge()` to log a victory message. What else would you need to change?" (Answer: update Watermelon's `GetMergeResultTier()` to return 11 instead of -1, create the prefab, assign to a new slot in FruitFactory)
- **Presentation rubric:** Game runs, student can explain inheritance and polymorphism using their own code, at least one custom feature implemented
