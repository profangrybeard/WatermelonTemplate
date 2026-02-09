# Merge Template - Teaching Guide

## GAME 220: Core Programming - Inheritance and Polymorphism

This template is designed as an **MVP with examples** — students study 3 working reference classes, then create their own derived classes to learn inheritance while building a working merge game.

---

## How This Template Works

### The MVP Approach

1. **Session 1:** Everything works with 3 object types (TierZero, TierOne, TierTwo merge correctly)
2. **Session 2+:** Students create their OWN derived classes following the TierZero pattern
3. **Session 4:** Students override OnMerge() for custom effects
4. **Session 5:** Stretch goal — convert to abstract class

### What Students Write vs What's Provided

| Concept | Provided (Complete) | Students Create |
|---------|---------------------|-----------------|
| Base class | `MergeObject.cs` with all fields, virtual methods, merge detection | — |
| Reference examples | `TierZero.cs`, `TierOne.cs`, `TierTwo.cs` | — |
| Their own objects | — | New classes inheriting from MergeObject (theme is theirs to choose) |
| Merge detection | `OnCollisionEnter2D()` in MergeObject.cs | — |
| Merge effects | Default `OnMerge()` in MergeObject.cs | Override `OnMerge()` per class (Session 4) |
| Query methods | `CountObjectsOfTier()`, `GetHighestTier()` | `GetTotalObjectPoints()`, `GetHighestObjectName()` (TODO stubs) |
| Drop controller | `DropController.cs` | — |
| Game manager | `GameManager.cs` with `List<MergeObject>` | — |
| Factory | `MergeObjectFactory.cs` with `CreateObject()` | — |

---

## Session Breakdown

### Session 1: Explore + Understand Base Class
**Status:** Complete — no student work required

**What Works:**
- 3 object types drop, fall with gravity, stack in container
- Two TierZeros merge into TierOne, two TierOnes merge into TierTwo
- Score logged to Console on merge (Debug.Log)
- Game over triggers when objects stack too high

**Teaching Points:**
- What does `: MergeObject` mean in `public class TierZero : MergeObject`?
- What is `protected` and why not `private` or `public`?
- What does `virtual` mean on `GetTier()`?
- What does `base.Awake()` do and why is it needed?
- In DropController.cs, `currentObject` is typed as `MergeObject` — why not `TierZero`?
- In GameManager.cs, `List<MergeObject>` holds TierZero, TierOne, and TierTwo — how?

**Demo Ideas:**
- Change TierZero's objectSize in code, show the object changes
- Remove base.Awake() from TierZero, show the NullReferenceException
- Open MergeObjectFactory Inspector, show the prefab array slots

---

### Session 2: First Derived Classes
**Students create their own .cs files from scratch**

**Learning Goal:** Write a derived class following the TierZero pattern

**The Pattern:**
```csharp
public class YourClassName : MergeObject
{
    protected override void Awake()
    {
        tier = 3;
        objectName = "YourClassName";
        pointValue = 10;
        objectSize = 1.0f;
        objectColor = new Color(1.0f, 0.65f, 0.0f);
        base.Awake();
    }
}
```

**Teaching Points:**
- `: MergeObject` means "inherits from MergeObject"
- `override` replaces the parent's version
- `base.Awake()` calls the parent's version
- Same pattern for every class — only values change
- Students pick their own theme, names, colors, and sizes

**Verification:**
- New objects appear when dropping
- Objects have correct sizes and colors
- Merging works through the new tiers

---

### Session 3: Polymorphism Deep Dive
**Students may add more classes; focus shifts to understanding WHY it all works**

**The Final Tier — Override GetMergeResultTier():**
```csharp
public override int GetMergeResultTier()
{
    return -1;  // No further merge possible
}
```

**Polymorphism Teaching Script:**
- Open GameManager.cs, find `List<MergeObject> activeObjects`
- "This list holds TierZero, TierTwo, your classes — ANY derived type"
- "When we call `obj.GetTier()` in the foreach loop, which version runs?"
- Answer: "The derived version. Each class returns its own tier value"

**TODO Query Methods:**
- Students complete `GetTotalObjectPoints()` and `GetHighestObjectName()` in GameManager.cs
- They follow the pattern from the working `CountObjectsOfTier()` and `GetHighestTier()` methods

---

### Session 4: Custom OnMerge() Overrides
**Learning Goal:** Override a virtual method with creative custom behavior

**Example:**
```csharp
public override void OnMerge()
{
    Debug.Log("Pop! Two of mine merged!");
}
```

**Teaching Points:**
- Same merge code in MergeObject.cs calls OnMerge() on every object
- Each class can have DIFFERENT effects — that's polymorphism
- The merge system never checks "is this a TierZero?" — it just calls OnMerge()

---

### Session 5: Abstract Refactor (Stretch)
**Learning Goal:** Understand abstract vs virtual

**Changes to MergeObject.cs:**
```csharp
// Change class declaration:
public abstract class MergeObject : MonoBehaviour

// Add a new abstract method:
protected abstract void InitializeMergeObjectProperties();

// Update Awake() to call it:
protected virtual void Awake()
{
    InitializeMergeObjectProperties();  // Derived classes MUST implement this
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}
```

**Derived classes change from:**
```csharp
protected override void Awake()
{
    tier = 0;
    objectName = "TierZero";
    // ... set values ...
    base.Awake();
}
```

**To:**
```csharp
protected override void InitializeMergeObjectProperties()
{
    tier = 0;
    objectName = "TierZero";
    // ... set values ...
}
// No Awake() override needed. No base.Awake() call needed.
```

**Teaching Points:**
- `virtual` = CAN override (optional)
- `abstract` = MUST override (required, compiler enforces it)
- Cannot instantiate abstract class directly
- Forgetting to implement `InitializeMergeObjectProperties()` causes a compiler error

---

### Sessions 6-8: Polish, Build, Present

---

## Key Vocabulary

| Term | Meaning |
|------|---------|
| Base class | A class that other classes inherit from (`MergeObject`) |
| Derived class | A class that inherits from another (`TierZero : MergeObject`) |
| `protected` | Accessible by this class AND its children |
| `virtual` | Method that CAN be overridden by derived classes |
| `override` | Replaces the parent's version of a method |
| `abstract` | Method that MUST be overridden (no default body) |
| Polymorphism | Using a base reference to work with any derived type |
| `base.Method()` | Calls the parent class version of a method |

---

## Student Checklist

### Session 2 Checklist
- [ ] At least 1 new class created, compiles with correct values
- [ ] Created prefab with SpriteRenderer + Rigidbody2D + CircleCollider2D + your script
- [ ] Assigned prefab to correct MergeObjectFactory slot
- [ ] New object appears in game with correct size and color

### Session 3 Checklist
- [ ] Final tier class overrides GetMergeResultTier() → returns -1
- [ ] TODO query methods completed in GameManager.cs
- [ ] Full merge chain works
- [ ] Two final-tier objects do NOT merge

### Session 4 Checklist
- [ ] At least 3 classes have custom OnMerge() overrides
- [ ] Each override produces different behavior
- [ ] Merge effects trigger correctly during gameplay

---

## Troubleshooting

### "New object doesn't appear"
- Check prefab has the correct derived script (not just MergeObject)
- Check MergeObjectFactory Inspector — is the prefab in the right slot?
- Verify base.Awake() is called in the derived Awake()

### "Objects don't merge"
- Both objects need Rigidbody2D and CircleCollider2D
- Colliders must NOT be set as Trigger
- Check tier values match between the two objects

### "NullReferenceException on object"
- Usually means base.Awake() was not called
- Check derived class has `base.Awake();` at the end of Awake()

### "Wrong size or color"
- Verify objectSize and objectColor values in derived Awake()
- Make sure values are set before calling base.Awake()

---

**Our Business is Fun!**
