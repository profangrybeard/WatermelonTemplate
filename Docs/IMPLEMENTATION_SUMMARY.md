# Merge Template - Implementation Summary

**Project:** GAME 220 - Merge Unity Template
**Date Started:** February 7, 2026
**Status:** Scripts and documentation complete. Unity scene setup not yet done.
**Previous Template:** SlitherTemplate (Arrays and Loops) -- this is the follow-up project.

---

## Project Context

This is the **third teaching template** in a series for GAME 220 (Core Programming) at SCAD. The SlitherTemplate taught **arrays and loops** using a Slither.io clone. This Merge Template teaches **inheritance and polymorphism** using a merge game (Suika Game) clone.

Both templates follow the same pedagogical approach called **"MVP with Scaffolds"**: the game works out of the box and students create their own classes from scratch to learn concepts through repetition. The SlitherTemplate is the gold standard reference at `C:\SCAD\Projects\SlitherTemplate`.

### Teaching Philosophy (shared with SlitherTemplate)
- **"Last Known Good":** Every session has a working game state. Missing classes don't crash -- they just mean a shorter merge chain.
- **"Read, Repeat, Understand":** Code is commented like lecture notes. Students read working examples, then implement similar patterns.
- **Pattern Repetition:** The same concept (derived class creation) appears multiple times for muscle memory.
- **Inspector-Friendly:** All tunable values exposed with `[Header]` attributes and sensible defaults.
- **Separation of Concerns:** Each script has one clear responsibility.

---

## Project Statistics

- **Total C# Scripts:** 8 files (1 base class + 3 given derived + 4 system scripts)
- **Total Lines of Code:** ~1,600 lines (including extensive teaching comments)
- **Comment Density:** ~44% (matches SlitherTemplate)
- **Documentation Files:** 6 markdown files (5 in Docs/ folder + 1 inside Assets/_Project/Scripts/)
- **Unity Version:** 6000.0.63f1
- **Render Pipeline:** Built-in 2D
- **Packages:** Input System 1.16.0, UGUI 2.0.0 (same as SlitherTemplate)
- **Repository Location:** `C:\SCAD\Projects\MergeTemplate`
- **Git State:** main branch. Scripts and docs committed.

---

## Design Decisions Made with the Instructor

These decisions were made through multiple rounds of Q&A before implementation:

### Session Structure (8 sessions)
1. **Sessions 1+2 were merged** into a single Session 1 (explore + understand base class). The instructor felt two read-only sessions before writing code was too slow.
2. Students **write code starting Session 2**.
3. **Student-driven features start from Session 2 onward** -- not saved for the end. Students choose their own features (score combos, next preview, particles, sounds, etc.) and work on them alongside the structured lessons.
4. **Session 7 is polish/build, Session 8 is presentations.** So there are 6 instructional sessions.

### What's Pre-Built vs Student-Written
1. **Merge system is fully pre-built.** The instructor changed his mind during planning -- originally wanted student-built merge, then decided students should focus on class design and polymorphism, not collision code. `OnCollisionEnter2D` in `MergeObject.cs` is complete with heavy teaching comments.
2. **3 derived classes are given complete** (TierZero, TierOne, TierTwo). Originally was going to be 1 (TierZero only), but instructor chose to give 3 references so students have more examples before writing their own.
3. **Students create their own classes from scratch** starting in Session 2. No scaffolds or TODO files are provided -- students build new classes following the reference pattern.

### Inheritance Depth
1. **Start with 2-level hierarchy** (MergeObject base -> concrete derived classes). No intermediate abstract mid-tier classes.
2. **Abstract refactor in Session 5 as a stretch goal** -- convert `MergeObject` from `class` to `abstract class`, add `protected abstract void InitializeMergeObjectProperties()` called from `Awake()`. Only for students who finish everything else.
3. **Interfaces are not covered** -- can wait for another course.

### Must-Have Concepts
- Base class + derived classes
- Protected fields
- Virtual methods + override
- `base.Awake()` (calling parent methods)
- Polymorphism: `List<MergeObject>`, `MergeObject` parameters, `MergeObject` return types, `GetComponent<MergeObject>()`

### Nice-to-Have Concepts (stretch)
- Abstract classes/methods (Session 5)

### Other Decisions
- **Theme-agnostic design** -- class names use tier-based naming (TierZero, TierOne, etc.) so students can apply any theme
- **Properties hardcoded in each derived class** (not ScriptableObjects -- keeps focus on inheritance)
- **Both mouse and keyboard input pre-built** (not the teaching focus, keep frictionless)
- **Simple box container** (3 walls, open top -- not a rounded Suika-style container)
- **Merge approach is up to students** -- the pre-built system destroys both and spawns new, but students can modify
- **No next-object preview** -- instructor wants features beyond core merging to be student-driven choices
- **Enhanced documentation** -- more docs than SlitherTemplate, plus an INHERITANCE_CONCEPTS.md
- **5-slot prefab array** -- starts with 5 slots, students expand as they add more tiers

---

## Provided Tier Data

Three tiers are provided as complete reference examples. Students create additional tiers from scratch.

```
Tier  Class          Size   Points  Color                              Merge Into     Status
----  -----------    -----  ------  ---------------------------------  -----------    ---------
 0    TierZero       0.50   1       new Color(0.85f, 0.12f, 0.15f)    TierOne        GIVEN
 1    TierOne        0.65   3       new Color(0.95f, 0.30f, 0.35f)    TierTwo        GIVEN
 2    TierTwo        0.80   6       new Color(0.55f, 0.27f, 0.68f)    (student)      GIVEN
 3+   Student tiers  ...    ...     (student-chosen)                   ...            STUDENT-CREATED
```

---

## Complete File Structure

```
MergeTemplate/
├── Assets/
│   └── _Project/
│       ├── Animations/
│       ├── Audio/
│       │   ├── Music/
│       │   └── SFX/
│       ├── Fonts/
│       ├── Input/
│       │   └── InputSystem_Actions.inputactions
│       ├── Materials/
│       ├── Prefabs/
│       │   └── MergeObjects/             (created during setup -- see SETUP_INSTRUCTIONS.md)
│       ├── Scenes/
│       │   └── GameScene.unity           (needs manual setup)
│       ├── Scripts/
│       │   ├── MergeObjects/
│       │   │   ├── MergeObject.cs        Base class -- fully pre-built
│       │   │   ├── TierZero.cs           Given complete -- reference #1
│       │   │   ├── TierOne.cs            Given complete -- reference #2
│       │   │   └── TierTwo.cs            Given complete -- reference #3
│       │   ├── Player/
│       │   │   └── DropController.cs     Pre-built -- mouse + keyboard input
│       │   ├── Managers/
│       │   │   ├── GameManager.cs        Pre-built -- List<MergeObject>, merge execution
│       │   │   ├── MergeObjectFactory.cs Pre-built -- polymorphic factory
│       │   │   └── ScoreManager.cs       Pre-built -- UI score display
│       │   ├── Utils/
│       │   │   └── ContainerSetup.cs     Pre-built -- 3-wall box container
│       │   └── README.md                 Concise teaching guide inside Scripts/
│       └── Sprites/
├── Docs/
│   ├── SETUP_INSTRUCTIONS.md             Unity setup from scripts-only to playable
│   ├── INHERITANCE_CONCEPTS.md           OOP reference with game examples
│   ├── MERGE_REFERENCE.md               Tier data + prefab instructions
│   ├── TROUBLESHOOTING.md               Common student issues and fixes
│   └── IMPLEMENTATION_SUMMARY.md        (this file) Full project context
└── README.md                            Main teaching guide
```

---

## Class Architecture

### Inheritance Hierarchy
```
MonoBehaviour
  └── MergeObject (base class)
        ├── TierZero      (tier 0  -- GIVEN complete)
        ├── TierOne       (tier 1  -- GIVEN complete)
        ├── TierTwo       (tier 2  -- GIVEN complete)
        └── (student-created classes from tier 3 onward)
```

### MergeObject.cs -- The Base Class

The central teaching artifact. Fully pre-built including merge detection.

**Protected fields** (set by derived classes in their Awake):
```csharp
protected int tier = 0;
protected string objectName = "MergeObject";
protected int pointValue = 0;
protected float objectSize = 1f;
protected Color objectColor = Color.white;
protected Rigidbody2D rb;
protected CircleCollider2D circleCollider;
protected SpriteRenderer spriteRenderer;
private bool hasMerged = false;
```

**Virtual methods** (derived classes can override):
```csharp
public virtual int GetTier()                // returns tier
public virtual int GetPointValue()          // returns pointValue
public virtual string GetObjectName()       // returns objectName
public virtual int GetMergeResultTier()     // returns tier + 1 (final tier overrides to -1)
public virtual void OnMerge()               // default Debug.Log (Session 4: students override)
```

**Lifecycle methods:**
```csharp
protected virtual void Awake()              // caches GetComponent references
protected virtual void Start()              // calls ApplyObjectProperties()
protected void ApplyObjectProperties()      // applies objectSize/objectColor to Unity components
```

**Merge detection** (fully pre-built, NOT a student TODO):
```csharp
private void OnCollisionEnter2D(Collision2D other)
// Gets other MergeObject via GetComponent<MergeObject>(), checks same tier, checks not already merged,
// checks merge possible (not final tier), marks both merged, calls OnMerge() on both,
// delegates to GameManager.MergeObjects(this, otherObject)
// Heavy teaching comments highlight every polymorphic call with [POLY] markers
```

**Physics helpers:**
```csharp
public void SetPhysicsEnabled(bool enabled)   // sets rb.simulated
public void SetKinematic(bool isKinematic)     // toggles Kinematic/Dynamic, zeros velocity
```

**Key API note:** Uses `FindFirstObjectByType<GameManager>()` (Unity 6 API, not the deprecated `FindObjectOfType`).

### TierZero.cs -- Complete Reference

Heavily commented. Shows the entire derived class pattern:
```csharp
public class TierZero : MergeObject
{
    protected override void Awake()
    {
        tier = 0;
        objectName = "TierZero";
        pointValue = 1;
        objectSize = 0.5f;
        objectColor = new Color(0.85f, 0.12f, 0.15f);
        base.Awake();
    }
}
```

Includes a commented-out `OnMerge()` override example for Session 4.

### TierOne.cs and TierTwo.cs -- Given References #2 and #3

Same pattern as TierZero with progressively fewer comments (by the third example, the pattern is clear).

### DropController.cs -- Pre-Built

- `private MergeObject currentObject` -- **polymorphic variable** typed as base class
- Both mouse (cursor position) and keyboard (A/D, arrow keys) input
- Space bar or mouse click to drop
- `SpawnNextObject()` calls `mergeObjectFactory.CreateObject(Random.Range(0, maxDropTier + 1))`
- Object starts kinematic (no gravity while aiming), switches to dynamic on drop
- `maxDropTier` Inspector field controls which tiers can spawn (start at 2 for 3-tier game)
- OnDrawGizmos draws drop line and aim boundaries

### MergeObjectFactory.cs -- Pre-Built

- `public GameObject[] objectPrefabs = new GameObject[5]` -- tier-indexed array
- `public MergeObject CreateObject(int tier)` -- **polymorphic return type**: Instantiate + `GetComponent<MergeObject>()`
- `public MergeObject CreateObjectAtPosition(int tier, Vector3 position)` -- convenience wrapper
- `public int GetMaxTier()` -- iterates to find highest non-null slot
- `public bool HasPrefabForTier(int tier)` -- checks if slot is assigned
- Detailed warning messages when slots are empty guide students to create missing prefabs

### GameManager.cs -- Pre-Built

- `private List<MergeObject> activeObjects` -- **polymorphic collection**
- `public void MergeObjects(MergeObject objA, MergeObject objB)` -- **polymorphic parameters**: gets next tier via `objA.GetMergeResultTier()`, calculates midpoint, awards points via `GetPointValue()`, destroys both, spawns next tier via factory, registers new object
- `RegisterObject(MergeObject)` / `UnregisterObject(MergeObject)` -- list management
- Game over detection: foreach over activeObjects, checks Y position against `gameOverLineY`, accumulates timer, triggers after `gameOverDelay` seconds
- Press R to restart (SceneManager.LoadScene)
- **Polymorphic query methods** (teaching showcase for Session 3):
  - `CountObjectsOfTier(int targetTier)`
  - `GetTotalObjectPoints()`
  - `GetHighestTier()`
  - `GetHighestObjectName()`
  - `GetActiveObjectCount()`
- OnDrawGizmos draws game over line

### ScoreManager.cs -- Pre-Built

Same pattern as SlitherTemplate's ScoreManager. Uses `UnityEngine.UI.Text` (Legacy UI).
- `AddScore(int points)` / `GetCurrentScore()` / `ResetScore()`
- `UpdateHighestObject(string objectName)`
- `ShowGameOver()`

### ContainerSetup.cs -- Pre-Built, in Utils/ folder

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
| `MergeObject currentObject` | Polymorphic variable | DropController.cs |
| `List<MergeObject> activeObjects` | Polymorphic collection | GameManager.cs |
| `MergeObjects(MergeObject a, MergeObject b)` | Polymorphic parameters | GameManager.cs |
| `MergeObject CreateObject(int tier)` | Polymorphic return type | MergeObjectFactory.cs |
| `GetComponent<MergeObject>()` | Polymorphic component lookup | MergeObjectFactory.cs, MergeObject.cs |
| `foreach (MergeObject obj in activeObjects)` | Polymorphic iteration | GameManager.cs query methods |

---

## Session Plan

### Session 1: Explore + Understand Base Class
- **Student code work:** None -- read, play, annotate
- **What works out of the box:** 3 tier types drop, merge, score, game over
- **Teaching focus:** Read MergeObject.cs and TierZero.cs, identify the pattern
- **Key questions:** What does `: MergeObject` mean? What is `protected`? What does `virtual` do? What does `base.Awake()` do?
- **Students choose** which personal features to add going forward

### Session 2: First Student-Written Derived Classes
- **Student code work:** Create new derived classes from scratch following the TierZero pattern
- **Also:** Create prefabs in Unity, assign to MergeObjectFactory slots
- **Teaching focus:** Write a derived class following the TierZero pattern
- **Verification:** New tiers appear, merging works through new tiers
- **Student-driven features:** Begin implementing

### Session 3: More Classes + Polymorphism Deep Dive
- **Student code work:** Create additional tiers, including a final tier with GetMergeResultTier() override
- **Special case:** Final tier overrides `GetMergeResultTier()` to return -1
- **Teaching focus:** Polymorphism -- walk through `List<MergeObject>` query methods, ask "which version of `GetTier()` runs?"
- **Verification:** Full merge chain works, final-tier objects do NOT merge
- **Student-driven features:** Continue

### Session 4: Custom OnMerge() Overrides
- **Student code work:** Override `OnMerge()` in classes for custom effects
- **Teaching focus:** Virtual method dispatch -- same merge code, different effects per type
- **Examples:** Debug.Log messages, color flashes, camera shake calls, particle triggers
- **Key insight:** The merge system calls `OnMerge()` without knowing which type. Each class's override runs automatically.
- **Student-driven features:** Continue

### Session 5: Abstract Refactor (Stretch)
- **Student code work:** Convert `MergeObject` to `abstract class`, add `InitializeMergeObjectProperties()` abstract method, update all derived classes
- **Teaching focus:** Virtual (CAN override) vs abstract (MUST override)
- **Only for students** who have finished their merge chain and custom behaviors
- **Student-driven features:** Continue

### Sessions 6-8: Polish, Build, Present

---

## Teaching Comment Conventions

Matches SlitherTemplate exactly:

1. **File headers:** `/* GAME 220: Merge Template / Session X: Name / TEACHING FOCUS: bullets / STUDENT TASKS: bullets */`
2. **TEACHING: prefix:** `// TEACHING: 'protected' means this class and its children can access this`
3. **Boxed concept blocks:** `// =====================================================================` borders around major concept introductions (protected, virtual, polymorphism, etc.)
4. **Section dividers:** `// ============================================` with section names
5. **[POLY] markers:** Used in `OnCollisionEnter2D` to mark every line where polymorphism is in action
6. **`[Header()]` attributes:** On all Inspector-exposed field groups
7. **XML doc comments:** `/// <summary>` on all public methods
8. **OnDrawGizmos:** On every script with spatial data (Container, DropController, GameManager)

---

## What's Done vs What's Still Needed

### DONE:
- All 8 C# scripts written and in place
- All 6 documentation files written
- Professional directory structure with standard Unity folders
- All files committed to git
- Assets organized: Animations, Audio, Fonts, Input, Materials, Prefabs/MergeObjects, Scenes, Scripts, Sprites
- Documentation consolidated in Docs/ folder

### NOT YET DONE (requires Unity Editor):
- Scene setup (camera, GameObjects, component assignment, Inspector references)
- Prefab creation (TierZero, TierOne, TierTwo with SpriteRenderer + Rigidbody2D + CircleCollider2D + derived script)
- MergeObjectFactory prefab slot assignment (slots 0-2)
- Canvas/UI creation (ScoreText, GameOverText, HighestTierText)
- Physics tuning (gravity, iterations, drag, optional PhysicsMaterial2D)

See `Docs/SETUP_INSTRUCTIONS.md` for the complete step-by-step Unity setup guide.

---

## Code Metrics

### Lines by Script:

| Script | Lines | Purpose | Status |
|--------|-------|---------|--------|
| MergeObject.cs | ~388 | Base class | Pre-built |
| TierZero.cs | ~122 | Reference derived #1 | Given |
| TierOne.cs | ~45 | Reference derived #2 | Given |
| TierTwo.cs | ~30 | Reference derived #3 | Given |
| DropController.cs | ~294 | Input/drop mechanic | Pre-built |
| GameManager.cs | ~402 | Game state/merge | Pre-built |
| MergeObjectFactory.cs | ~173 | Prefab factory | Pre-built |
| ScoreManager.cs | ~138 | UI scoring | Pre-built |
| ContainerSetup.cs | ~173 | Wall container | Pre-built |

### Lines by Documentation:

| Document | Lines | Purpose |
|----------|-------|---------|
| README.md | ~335 | Main teaching guide (repo root) |
| Docs/SETUP_INSTRUCTIONS.md | ~363 | Unity setup from scripts-only |
| Docs/INHERITANCE_CONCEPTS.md | ~563 | OOP reference with game examples |
| Docs/MERGE_REFERENCE.md | ~165 | Tier data + prefab instructions |
| Docs/TROUBLESHOOTING.md | ~474 | Common issues and fixes |
| Assets/_Project/Scripts/README.md | ~224 | Concise in-code teaching guide |
| **TOTAL** | **~2,124** | |

---

## Comparison with SlitherTemplate

| Aspect | SlitherTemplate | MergeTemplate |
|--------|----------------|---------------|
| Teaching concept | Arrays and Loops | Inheritance and Polymorphism |
| Total scripts | 8 | 8 (base + 3 derived + 4 system) |
| Total code lines | 1,375 | ~1,600 |
| Comment density | ~44% | ~44% |
| Doc files | 4 | 6 |
| Given examples | N/A (TODOs are in system scripts) | 3 derived classes (TierZero, TierOne, TierTwo) |
| Student work | 6 for-loop blocks across 3 files | Create derived classes from scratch |
| Sessions | 6 instructional | 5-6 instructional (+ abstract stretch) |
| Folder convention | `AI/`, `Food/`, `Player/`, `Managers/`, `Utils/` | `MergeObjects/`, `Player/`, `Managers/`, `Utils/` |
| Root docs | `README.md`, `SETUP_INSTRUCTIONS.md`, `IMPLEMENTATION_SUMMARY.md`, `QUICKSTART_CHECKLIST.md` | `README.md` at root + 5 docs in `Docs/` folder |
| Scripts README | Yes (`Assets/_Project/Scripts/README.md`) | Yes (`Assets/_Project/Scripts/README.md`) |
| Unity version | 6000.0.63f1 | 6000.0.63f1 |

---

## Potential Future Work

These items were discussed but deferred or left as student-driven choices:

1. **Interfaces** (e.g., `IMergeable`) -- not covered in this template, could be a future project
2. **ScriptableObjects for object data** -- discussed as a possible Session 7 refactoring exercise but decided against (keeps focus on inheritance)
3. **Next-object preview UI** -- deliberately left as a student-driven feature option
4. **Rounded container** -- discussed but went with simple box for predictable physics
5. **Particle effects on merge** -- left for student creativity
6. **Sound effects** -- left for student creativity
7. **Multi-level inheritance** (e.g., `MergeObject -> LargeObject -> FinalTier`) -- discussed but kept flat hierarchy for clarity

---

## Key Files for Any Future Modifications

If you need to modify the template, these are the critical files:

1. **`Assets/_Project/Scripts/MergeObjects/MergeObject.cs`** -- The base class. Any change here affects all derived types. Touch with care.
2. **`Assets/_Project/Scripts/MergeObjects/TierZero.cs`** -- The reference example. If you change the derived class pattern, update TierZero first, then update TierOne and TierTwo to match.
3. **`Assets/_Project/Scripts/Managers/GameManager.cs`** -- The merge execution and polymorphic query methods. This is where `MergeObjects()` lives.
4. **`Assets/_Project/Scripts/Managers/MergeObjectFactory.cs`** -- The polymorphic factory. The prefab array indexing here must match tier values.
5. **`Docs/SETUP_INSTRUCTIONS.md`** -- Must stay accurate to the current script state. Update if you change Inspector fields or component requirements.

---

## Verification Checklist

After completing Unity setup (per Docs/SETUP_INSTRUCTIONS.md), verify:

- [ ] TierZero, TierOne, TierTwo prefabs created with correct components
- [ ] MergeObjectFactory slots 0-2 assigned
- [ ] DropController Max Drop Tier set to 2
- [ ] Three object types appear when dropping
- [ ] Objects fall with gravity and stack in container
- [ ] Two TierZero objects merge into TierOne
- [ ] Two TierOne objects merge into TierTwo
- [ ] Score increases on merge
- [ ] Game over triggers when objects stack above the line
- [ ] Press R restarts the game
- [ ] Creating a new derived class -> creating prefab -> assigning slot works
- [ ] OnMerge() override in TierZero triggers custom behavior on merge

Our Business is Fun
