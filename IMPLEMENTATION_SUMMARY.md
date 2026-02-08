# Watermelon Merge Template - Implementation Summary

**Project:** GAME 220 - Watermelon Merge Unity Template
**Date Started:** February 7, 2026
**Status:** Scripts and documentation complete. Unity scene setup not yet done.
**Previous Template:** SlitherTemplate (Arrays and Loops) — this is the follow-up project.

---

## Project Context

This is the **third teaching template** in a series for GAME 220 (Core Programming) at SCAD. The SlitherTemplate taught **arrays and loops** using a Slither.io clone. This WatermelonTemplate teaches **inheritance and polymorphism** using a Watermelon Merge (Suika Game) clone.

Both templates follow the same pedagogical approach called **"MVP with Scaffolds"**: the game works out of the box and students fill in TODO sections to learn concepts through repetition. The SlitherTemplate is the gold standard reference at `C:\SCAD\Projects\SlitherTemplate`.

### Teaching Philosophy (shared with SlitherTemplate)
- **"Last Known Good":** Every session has a working game state. Missing TODOs don't crash — they just mean missing features.
- **"Read, Repeat, Understand":** Code is commented like lecture notes. Students read working examples, then implement similar patterns.
- **Pattern Repetition:** The same concept (derived class creation) appears 10+ times for muscle memory.
- **Inspector-Friendly:** All tunable values exposed with `[Header]` attributes and sensible defaults.
- **Separation of Concerns:** Each script has one clear responsibility.

---

## Project Statistics

- **Total C# Scripts:** 17 files (1 base class + 3 given derived + 8 scaffold derived + 5 system scripts)
- **Total Lines of Code:** 2,108 lines (including extensive teaching comments)
- **Comment Density:** ~44% (matches SlitherTemplate)
- **Documentation Files:** 7 markdown files (6 at repo root + 1 inside Assets/Scripts/)
- **Unity Version:** 6000.0.63f1
- **Render Pipeline:** Built-in 2D
- **Packages:** Input System 1.16.0, UGUI 2.0.0 (same as SlitherTemplate)
- **Repository Location:** `C:\SCAD\Projects\WatermelonTemplate`
- **Worktree Location:** `C:\Users\rinds\.claude-worktrees\WatermelonTemplate\optimistic-mayer`
- **Git State:** main branch, 2 commits (Initial + vanilla unity). All new files are untracked — nothing committed yet.

---

## Design Decisions Made with the Instructor

These decisions were made through multiple rounds of Q&A before implementation:

### Session Structure (8 sessions)
1. **Sessions 1+2 were merged** into a single Session 1 (explore + understand base class). The instructor felt two read-only sessions before writing code was too slow.
2. Students **write code starting Session 2**.
3. **Student-driven features start from Session 2 onward** — not saved for the end. Students choose their own features (score combos, next preview, particles, sounds, etc.) and work on them alongside the structured lessons.
4. **Session 7 is polish/build, Session 8 is presentations.** So there are 6 instructional sessions.

### What's Pre-Built vs Student-Written
1. **Merge system is fully pre-built.** The instructor changed his mind during planning — originally wanted student-built merge, then decided students should focus on class design and polymorphism, not collision code. `OnCollisionEnter2D` in `Fruit.cs` is complete with heavy teaching comments.
2. **3 fruit classes are given complete** (Cherry, Strawberry, Grape). Originally was going to be 1 (Cherry only), but instructor chose to give 3 references so students have more examples before writing their own.
3. **8 fruit classes are student TODO scaffolds** (Orange through Watermelon). All 8 are required.

### Inheritance Depth
1. **Start with 2-level hierarchy** (Fruit base → concrete derived classes). No intermediate abstract mid-tier classes.
2. **Abstract refactor in Session 5 as a stretch goal** — convert `Fruit` from `class` to `abstract class`, convert select virtual methods to abstract. Only for students who finish everything else.
3. **Interfaces are not covered** — can wait for another course.

### Must-Have Concepts
- Base class + derived classes
- Protected fields
- Virtual methods + override
- `base.Awake()` (calling parent methods)
- Polymorphism: `List<Fruit>`, `Fruit` parameters, `Fruit` return types, `GetComponent<Fruit>()`

### Nice-to-Have Concepts (stretch)
- Abstract classes/methods (Session 5)

### Other Decisions
- **11 total fruit types** (authentic Watermelon game merge chain)
- **Properties hardcoded in each derived class** (not ScriptableObjects — keeps focus on inheritance)
- **Both mouse and keyboard input pre-built** (not the teaching focus, keep frictionless)
- **Simple box container** (3 walls, open top — not a rounded Suika-style container)
- **Merge approach is up to students** — the pre-built system destroys both and spawns new, but students can modify
- **No next-fruit preview** — instructor wants features beyond core merging to be student-driven choices
- **Enhanced documentation** — more docs than SlitherTemplate, plus an INHERITANCE_CONCEPTS.md

---

## The 11-Fruit Merge Chain

Two identical-tier fruits merge into the next tier. Two Watermelons cannot merge (tier 10 is the ceiling). Only tiers 0-4 can be dropped; tiers 5-10 only come from merging.

```
Tier  Class          Size   Points  Color                              Merge Into     Status
----  -----------    -----  ------  ---------------------------------  -----------    ---------
 0    Cherry         0.50   1       new Color(0.85f, 0.12f, 0.15f)    Strawberry     GIVEN
 1    Strawberry     0.65   3       new Color(0.95f, 0.30f, 0.35f)    Grape          GIVEN
 2    Grape          0.80   6       new Color(0.55f, 0.27f, 0.68f)    Orange         GIVEN
 3    Orange         1.00   10      new Color(1.00f, 0.65f, 0.00f)    Dekopon        TODO (Session 2)
 4    Dekopon        1.15   15      new Color(1.00f, 0.80f, 0.20f)    Apple          TODO (Session 2)
 5    Apple          1.35   21      new Color(0.85f, 0.15f, 0.20f)    Pear           TODO (Session 2)
 6    Pear           1.50   28      new Color(0.75f, 0.85f, 0.20f)    Peach          TODO (Session 3)
 7    Peach          1.70   36      new Color(1.00f, 0.80f, 0.70f)    Pineapple      TODO (Session 3)
 8    Pineapple      1.90   45      new Color(0.95f, 0.85f, 0.15f)    Melon          TODO (Session 3)
 9    Melon          2.15   55      new Color(0.55f, 0.85f, 0.40f)    Watermelon     TODO (Session 3)
10    Watermelon     2.50   66      new Color(0.20f, 0.75f, 0.20f)    NONE (-1)      TODO (Session 3)
```

---

## Complete File Structure

```
WatermelonTemplate/
├── Assets/
│   ├── Scenes/
│   │   └── SampleScene.unity          (existing, needs manual setup)
│   ├── Prefabs/                        (needs to be created — see SETUP_INSTRUCTIONS.md)
│   ├── Scripts/
│   │   ├── Fruits/
│   │   │   ├── Fruit.cs               (388 lines) Base class — fully pre-built
│   │   │   ├── Cherry.cs              (122 lines) Given complete — reference #1
│   │   │   ├── Strawberry.cs          (45 lines)  Given complete — reference #2
│   │   │   ├── Grape.cs               (30 lines)  Given complete — reference #3
│   │   │   ├── Orange.cs              (43 lines)  TODO scaffold — Session 2
│   │   │   ├── Dekopon.cs             (44 lines)  TODO scaffold — Session 2
│   │   │   ├── Apple.cs               (44 lines)  TODO scaffold — Session 2
│   │   │   ├── Pear.cs                (41 lines)  TODO scaffold — Session 3
│   │   │   ├── Peach.cs               (41 lines)  TODO scaffold — Session 3
│   │   │   ├── Pineapple.cs           (41 lines)  TODO scaffold — Session 3
│   │   │   ├── Melon.cs               (41 lines)  TODO scaffold — Session 3
│   │   │   └── Watermelon.cs          (48 lines)  TODO scaffold + bonus override — Session 3
│   │   ├── Player/
│   │   │   └── DropController.cs      (294 lines) Pre-built — mouse + keyboard input
│   │   ├── Managers/
│   │   │   ├── GameManager.cs         (402 lines) Pre-built — List<Fruit>, merge execution
│   │   │   ├── FruitFactory.cs        (173 lines) Pre-built — polymorphic factory
│   │   │   └── ScoreManager.cs        (138 lines) Pre-built — UI score display
│   │   ├── Utils/
│   │   │   └── ContainerSetup.cs      (173 lines) Pre-built — 3-wall box container
│   │   └── README.md                  (224 lines) Concise teaching guide inside Scripts/
│   └── InputSystem_Actions.inputactions (existing from vanilla Unity)
├── README.md                           (348 lines) Main teaching guide
├── SETUP_INSTRUCTIONS.md               (349 lines) Unity setup from scripts-only to playable
├── INHERITANCE_CONCEPTS.md             (574 lines) OOP reference with game examples
├── SESSION_GUIDE.md                    (764 lines) Per-session walkthrough with solutions
├── FRUIT_REFERENCE.md                  (163 lines) All 11 fruit data + prefab instructions
├── TROUBLESHOOTING.md                  (500 lines) Common student issues and fixes
└── IMPLEMENTATION_SUMMARY.md           (this file) Full project context
```

---

## Class Architecture

### Inheritance Hierarchy
```
MonoBehaviour
  └── Fruit (base class)
        ├── Cherry        (tier 0  — GIVEN complete)
        ├── Strawberry    (tier 1  — GIVEN complete)
        ├── Grape         (tier 2  — GIVEN complete)
        ├── Orange        (tier 3  — TODO scaffold)
        ├── Dekopon       (tier 4  — TODO scaffold)
        ├── Apple         (tier 5  — TODO scaffold)
        ├── Pear          (tier 6  — TODO scaffold)
        ├── Peach         (tier 7  — TODO scaffold)
        ├── Pineapple     (tier 8  — TODO scaffold)
        ├── Melon         (tier 9  — TODO scaffold)
        └── Watermelon    (tier 10 — TODO scaffold + GetMergeResultTier override)
```

### Fruit.cs — The Base Class (388 lines)

The central teaching artifact. Fully pre-built including merge detection.

**Protected fields** (set by derived classes in their Awake):
```csharp
protected int tier = 0;
protected string fruitName = "Fruit";
protected int pointValue = 0;
protected float fruitSize = 1f;
protected Color fruitColor = Color.white;
protected Rigidbody2D rb;
protected CircleCollider2D circleCollider;
protected SpriteRenderer spriteRenderer;
private bool hasMerged = false;
```

**Virtual methods** (derived classes can override):
```csharp
public virtual int GetTier()                // returns tier
public virtual int GetPointValue()          // returns pointValue
public virtual float GetFruitSize()         // returns fruitSize
public virtual string GetFruitName()        // returns fruitName
public virtual int GetMergeResultTier()     // returns tier + 1 (Watermelon overrides → -1)
public virtual void OnMerge()               // default Debug.Log (Session 4: students override)
```

**Lifecycle methods:**
```csharp
protected virtual void Awake()              // caches GetComponent references
protected virtual void Start()              // calls ApplyFruitProperties()
protected void ApplyFruitProperties()       // applies fruitSize/fruitColor to Unity components
```

**Merge detection** (fully pre-built, NOT a student TODO):
```csharp
private void OnCollisionEnter2D(Collision2D other)
// Gets other Fruit via GetComponent<Fruit>(), checks same tier, checks not already merged,
// checks merge possible (not Watermelon), marks both merged, calls OnMerge() on both,
// delegates to GameManager.MergeFruits(this, otherFruit)
// Heavy teaching comments highlight every polymorphic call with [POLY] markers
```

**Physics helpers:**
```csharp
public void SetPhysicsEnabled(bool enabled)   // sets rb.simulated
public void SetKinematic(bool isKinematic)     // toggles Kinematic/Dynamic, zeros velocity
```

**Key API note:** Uses `FindFirstObjectByType<GameManager>()` (Unity 6 API, not the deprecated `FindObjectOfType`).

### Cherry.cs — Complete Reference (122 lines)

Heavily commented. Shows the entire derived class pattern:
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
}
```

Includes a commented-out `OnMerge()` override example for Session 4.

### Strawberry.cs (45 lines) and Grape.cs (30 lines) — Given References #2 and #3

Same pattern as Cherry with progressively fewer comments (by the third example, the pattern is clear).

### TODO Scaffold Pattern (Orange through Watermelon)

Each scaffold file contains:
- File header with session number and TEACHING FOCUS
- Class declaration inheriting from Fruit: `public class Orange : Fruit`
- Boxed TODO comment block with exact values to set and the `base.Awake()` pattern
- Hint pointing to Cherry.cs
- Empty class body

**Watermelon.cs is special** — it has a bonus TODO to also override `GetMergeResultTier()` to return -1.

### DropController.cs (294 lines) — Pre-Built

- `private Fruit currentFruit` — **polymorphic variable** typed as base class
- Both mouse (cursor position) and keyboard (A/D, arrow keys) input
- Space bar or mouse click to drop
- `SpawnNextFruit()` calls `fruitFactory.CreateFruit(Random.Range(0, maxDropTier + 1))`
- Fruit starts kinematic (no gravity while aiming), switches to dynamic on drop
- `maxDropTier` Inspector field controls which tiers can spawn (start at 2 for 3-fruit game)
- OnDrawGizmos draws drop line and aim boundaries

### FruitFactory.cs (173 lines) — Pre-Built

- `public GameObject[] fruitPrefabs = new GameObject[11]` — tier-indexed array
- `public Fruit CreateFruit(int tier)` — **polymorphic return type**: Instantiate + `GetComponent<Fruit>()`
- `public Fruit CreateFruitAtPosition(int tier, Vector3 position)` — convenience wrapper
- `public int GetMaxTier()` — iterates to find highest non-null slot
- `public bool HasPrefabForTier(int tier)` — checks if slot is assigned
- Detailed warning messages when slots are empty guide students to create missing prefabs

### GameManager.cs (402 lines) — Pre-Built

- `private List<Fruit> activeFruits` — **polymorphic collection**
- `public void MergeFruits(Fruit fruitA, Fruit fruitB)` — **polymorphic parameters**: gets next tier via `fruitA.GetMergeResultTier()`, calculates midpoint, awards points via `GetPointValue()`, destroys both, spawns next tier via factory, registers new fruit
- `RegisterFruit(Fruit)` / `UnregisterFruit(Fruit)` — list management
- Game over detection: foreach over activeFruits, checks Y position against `gameOverLineY`, accumulates timer, triggers after `gameOverDelay` seconds
- Press R to restart (SceneManager.LoadScene)
- **Polymorphic query methods** (teaching showcase for Session 3):
  - `CountFruitsOfTier(int targetTier)`
  - `GetTotalFruitPoints()`
  - `GetHighestTier()`
  - `GetHighestFruitName()`
  - `GetActiveFruitCount()`
- OnDrawGizmos draws game over line

### ScoreManager.cs (138 lines) — Pre-Built

Same pattern as SlitherTemplate's ScoreManager. Uses `UnityEngine.UI.Text` (Legacy UI).
- `AddScore(int points)` / `GetCurrentScore()` / `ResetScore()`
- `UpdateHighestFruit(string fruitName)`
- `ShowGameOver()`

### ContainerSetup.cs (173 lines) — Pre-Built, in Utils/ folder

- Creates 3 child GameObjects at runtime: LeftWall, RightWall, BottomWall
- Each wall gets BoxCollider2D + SpriteRenderer
- `CreateSquareSprite()` generates a 4x4 white pixel texture for wall rendering
- Inspector-tunable: width, height, wallThickness, wallColor
- Optional PhysicsMaterial2D slot for wall friction/bounce
- OnDrawGizmos draws container outline

---

## Where Polymorphism Appears (6 locations)

| Location | Type | Script |
|----------|------|--------|
| `Fruit currentFruit` | Polymorphic variable | DropController.cs |
| `List<Fruit> activeFruits` | Polymorphic collection | GameManager.cs |
| `MergeFruits(Fruit a, Fruit b)` | Polymorphic parameters | GameManager.cs |
| `Fruit CreateFruit(int tier)` | Polymorphic return type | FruitFactory.cs |
| `GetComponent<Fruit>()` | Polymorphic component lookup | FruitFactory.cs, Fruit.cs |
| `foreach (Fruit fruit in activeFruits)` | Polymorphic iteration | GameManager.cs query methods |

---

## Session Plan

### Session 1: Explore + Understand Base Class
- **Student code work:** None — read, play, annotate
- **What works out of the box:** 3 fruit types drop, merge, score, game over
- **Teaching focus:** Read Fruit.cs and Cherry.cs, identify the pattern
- **Key questions:** What does `: Fruit` mean? What is `protected`? What does `virtual` do? What does `base.Awake()` do?
- **Students choose** which personal features to add going forward

### Session 2: First Student-Written Derived Classes
- **Student code work:** Complete Orange.cs, Dekopon.cs, Apple.cs TODOs
- **Also:** Create prefabs in Unity, assign to FruitFactory slots 3-5
- **Teaching focus:** Write a derived class following the Cherry pattern
- **Verification:** 6 fruit types appear, merging works through new tiers
- **Student-driven features:** Begin implementing

### Session 3: Remaining Fruits + Polymorphism Deep Dive
- **Student code work:** Complete Pear.cs through Watermelon.cs (5 more)
- **Special case:** Watermelon overrides `GetMergeResultTier()` → returns -1
- **Teaching focus:** Polymorphism — walk through `List<Fruit>` query methods, ask "which version of `GetTier()` runs?"
- **Verification:** Full 11-fruit merge chain works, two Watermelons do NOT merge
- **Student-driven features:** Continue

### Session 4: Custom OnMerge() Overrides
- **Student code work:** Override `OnMerge()` in fruit classes for custom effects
- **Teaching focus:** Virtual method dispatch — same merge code, different effects per fruit
- **Examples:** Debug.Log messages, color flashes, camera shake calls, particle triggers
- **Key insight:** The merge system calls `OnMerge()` without knowing which fruit type. Each fruit's override runs automatically.
- **Student-driven features:** Continue

### Session 5: Abstract Refactor (Stretch)
- **Student code work:** Convert `Fruit` to `abstract class`, convert select virtual methods to abstract
- **Teaching focus:** Virtual (CAN override) vs abstract (MUST override)
- **Only for students** who have finished all 11 fruits and the merge system
- **Student-driven features:** Continue

### Sessions 6-8: Polish, Build, Present

---

## Teaching Comment Conventions

Matches SlitherTemplate exactly:

1. **File headers:** `/* GAME 220: Watermelon Merge Template / Session X: Name / TEACHING FOCUS: bullets / STUDENT TASKS: bullets */`
2. **TEACHING: prefix:** `// TEACHING: 'protected' means this class and its children can access this`
3. **Boxed concept blocks:** `// =====================================================================` borders around major concept introductions (protected, virtual, polymorphism, etc.)
4. **Section dividers:** `// ============================================` with section names
5. **TODO scaffolds:** Boxed with `// ===========================================`, include numbered steps, hints, pattern examples, and a reference to Cherry.cs
6. **[POLY] markers:** Used in `OnCollisionEnter2D` to mark every line where polymorphism is in action
7. **`[Header()]` attributes:** On all Inspector-exposed field groups
8. **XML doc comments:** `/// <summary>` on all public methods
9. **OnDrawGizmos:** On every script with spatial data (Container, DropController, GameManager)

---

## What's Done vs What's Still Needed

### DONE:
- All 17 C# scripts written and in place
- All 7 documentation files written
- Folder structure matches SlitherTemplate conventions
- Both repo and worktree are in sync

### NOT YET DONE (requires Unity Editor):
- Scene setup (camera, GameObjects, component assignment, Inspector references)
- Prefab creation (Cherry, Strawberry, Grape with SpriteRenderer + Rigidbody2D + CircleCollider2D + derived script)
- FruitFactory prefab slot assignment (slots 0-2)
- Canvas/UI creation (ScoreText, GameOverText, HighestFruitText)
- Physics tuning (gravity, iterations, drag, optional PhysicsMaterial2D)
- Git commit of all new files

See `SETUP_INSTRUCTIONS.md` for the complete step-by-step Unity setup guide.

### NOT YET DONE (optional):
- Materials folder (unlike SlitherTemplate, fruits set color via script so materials aren't strictly needed)
- Prefabs folder (needs to be created during setup)

---

## Code Metrics

### Lines by Script:

| Script | Lines | Purpose | Status |
|--------|-------|---------|--------|
| Fruit.cs | 388 | Base class | Pre-built |
| Cherry.cs | 122 | Reference derived #1 | Given |
| Strawberry.cs | 45 | Reference derived #2 | Given |
| Grape.cs | 30 | Reference derived #3 | Given |
| Orange.cs | 43 | Student TODO | Scaffold |
| Dekopon.cs | 44 | Student TODO | Scaffold |
| Apple.cs | 44 | Student TODO | Scaffold |
| Pear.cs | 41 | Student TODO | Scaffold |
| Peach.cs | 41 | Student TODO | Scaffold |
| Pineapple.cs | 41 | Student TODO | Scaffold |
| Melon.cs | 41 | Student TODO | Scaffold |
| Watermelon.cs | 48 | Student TODO + bonus | Scaffold |
| DropController.cs | 294 | Input/drop mechanic | Pre-built |
| GameManager.cs | 402 | Game state/merge | Pre-built |
| FruitFactory.cs | 173 | Prefab factory | Pre-built |
| ScoreManager.cs | 138 | UI scoring | Pre-built |
| ContainerSetup.cs | 173 | Wall container | Pre-built |
| **TOTAL** | **2,108** | | |

### Lines by Documentation:

| Document | Lines | Purpose |
|----------|-------|---------|
| README.md | 348 | Main teaching guide |
| SETUP_INSTRUCTIONS.md | 349 | Unity setup from scripts-only |
| INHERITANCE_CONCEPTS.md | 574 | OOP reference with game examples |
| SESSION_GUIDE.md | 764 | Per-session walkthrough with solutions |
| FRUIT_REFERENCE.md | 163 | All 11 fruit data table |
| TROUBLESHOOTING.md | 500 | Common issues and fixes |
| Assets/Scripts/README.md | 224 | Concise in-code teaching guide |
| **TOTAL** | **2,922** | |

---

## Comparison with SlitherTemplate

| Aspect | SlitherTemplate | WatermelonTemplate |
|--------|----------------|-------------------|
| Teaching concept | Arrays and Loops | Inheritance and Polymorphism |
| Total scripts | 8 | 17 (many are small scaffolds) |
| Total code lines | 1,375 | 2,108 |
| Comment density | ~44% | ~44% |
| Doc files | 4 | 7 |
| Given examples | N/A (TODOs are in system scripts) | 3 fruit classes (Cherry, Strawberry, Grape) |
| Student TODOs | 6 for-loop blocks across 3 files | 8 derived classes + OnMerge overrides |
| Sessions | 6 instructional | 5-6 instructional (+ abstract stretch) |
| Folder convention | `AI/`, `Food/`, `Player/`, `Managers/`, `Utils/` | `Fruits/`, `Player/`, `Managers/`, `Utils/` |
| Root docs | `README.md`, `SETUP_INSTRUCTIONS.md`, `IMPLEMENTATION_SUMMARY.md`, `QUICKSTART_CHECKLIST.md` | `README.md`, `SETUP_INSTRUCTIONS.md`, `IMPLEMENTATION_SUMMARY.md`, `INHERITANCE_CONCEPTS.md`, `SESSION_GUIDE.md`, `FRUIT_REFERENCE.md`, `TROUBLESHOOTING.md` |
| Scripts README | Yes (`Assets/Scripts/README.md`) | Yes (`Assets/Scripts/README.md`) |
| Unity version | 6000.0.63f1 | 6000.0.63f1 |

---

## Potential Future Work

These items were discussed but deferred or left as student-driven choices:

1. **Interfaces** (e.g., `IMergeable`) — not covered in this template, could be a future project
2. **ScriptableObjects for fruit data** — discussed as a possible Session 7 refactoring exercise but decided against (keeps focus on inheritance)
3. **Next-fruit preview UI** — deliberately left as a student-driven feature option
4. **Rounded container** — discussed but went with simple box for predictable physics
5. **Particle effects on merge** — left for student creativity
6. **Sound effects** — left for student creativity
7. **Multi-level inheritance** (e.g., `Fruit → LargeFruit → Watermelon`) — discussed but kept flat hierarchy for clarity

---

## Key Files for Any Future Modifications

If you need to modify the template, these are the critical files:

1. **`Assets/Scripts/Fruits/Fruit.cs`** — The base class. Any change here affects all 11 fruit types. Touch with care.
2. **`Assets/Scripts/Fruits/Cherry.cs`** — The reference example. If you change the derived class pattern, update Cherry first, then update all scaffolds to match.
3. **`Assets/Scripts/Managers/GameManager.cs`** — The merge execution and polymorphic query methods. This is where `MergeFruits()` lives.
4. **`Assets/Scripts/Managers/FruitFactory.cs`** — The polymorphic factory. The prefab array indexing here must match fruit tier values.
5. **`SETUP_INSTRUCTIONS.md`** — Must stay accurate to the current script state. Update if you change Inspector fields or component requirements.

---

## Verification Checklist

After completing Unity setup (per SETUP_INSTRUCTIONS.md), verify:

- [ ] Cherry, Strawberry, Grape prefabs created with correct components
- [ ] FruitFactory slots 0-2 assigned
- [ ] DropController Max Drop Tier set to 2
- [ ] Three fruit types appear when dropping
- [ ] Fruits fall with gravity and stack in container
- [ ] Two Cherries merge into Strawberry
- [ ] Two Strawberries merge into Grape
- [ ] Score increases on merge
- [ ] Game over triggers when fruits stack above the line
- [ ] Press R restarts the game
- [ ] Filling in Orange.cs TODO → creating prefab → assigning slot 3 works
- [ ] OnMerge() override in Cherry triggers custom behavior on merge
