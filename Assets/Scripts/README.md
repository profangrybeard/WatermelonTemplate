# Watermelon Merge Template - Teaching Guide

## GAME 220: Core Programming - Inheritance and Polymorphism

This template is designed as an **MVP with scaffolding** - students complete TODO sections to learn inheritance while building a working merge game.

---

## How This Template Works

### The MVP Approach

1. **Session 1:** Everything works with 3 fruit types (Cherry, Strawberry, Grape merge correctly)
2. **Sessions 2-3:** Students create derived fruit classes following the Cherry pattern
3. **Session 4:** Students override OnMerge() for custom effects
4. **Session 5:** Stretch goal - convert to abstract class

### What Students Write vs What's Provided

| Concept | Provided (Complete) | Students Write |
|---------|---------------------|----------------|
| Fruit base class | `Fruit.cs` with all fields, virtual methods, merge detection | - |
| Reference fruits | `Cherry.cs`, `Strawberry.cs`, `Grape.cs` | - |
| Session 2 fruits | TODO scaffolds | `Orange.cs`, `Dekopon.cs`, `Apple.cs` |
| Session 3 fruits | TODO scaffolds | `Pear.cs` through `Watermelon.cs` |
| Merge detection | `OnCollisionEnter2D()` in Fruit.cs | - |
| Merge effects | Default `OnMerge()` in Fruit.cs | Override `OnMerge()` per fruit (Session 4) |
| Drop controller | `DropController.cs` | - |
| Game manager | `GameManager.cs` with `List<Fruit>` | - |
| Factory | `FruitFactory.cs` with `CreateFruit()` | - |
| Scoring | `ScoreManager.cs` | - |
| Container | `ContainerSetup.cs` | - |

---

## Session Breakdown

### Session 1: Explore + Understand Base Class
**Status:** Complete - no student work required

**What Works:**
- 3 fruit types drop, fall with gravity, stack in container
- Two Cherries merge into Strawberry, two Strawberries merge into Grape
- Score increases on merge
- Game over triggers when fruits stack too high

**Teaching Points:**
- What does `: Fruit` mean in `public class Cherry : Fruit`?
- What is `protected` and why not `private` or `public`?
- What does `virtual` mean on `GetTier()`?
- What does `base.Awake()` do and why is it needed?
- In DropController.cs, `currentFruit` is typed as `Fruit` — why not `Cherry`?
- In GameManager.cs, `List<Fruit>` holds Cherry, Strawberry, and Grape — how?

**Demo Ideas:**
- Change Cherry's fruitSize in code, show the fruit changes
- Remove base.Awake() from Cherry, show the NullReferenceException
- Open FruitFactory Inspector, show the prefab array slots

---

### Session 2: First Derived Classes
**TODO Locations:** `Orange.cs`, `Dekopon.cs`, `Apple.cs`

**Learning Goal:** Write a derived class following the Cherry pattern

**The Pattern:**
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

**Teaching Points:**
- `: Fruit` means "inherits from Fruit"
- `override` replaces the parent's version
- `base.Awake()` calls the parent's version
- Same pattern for every fruit — only values change

**Verification:**
- 6 fruit types appear when dropping
- Orange, Dekopon, Apple have correct sizes and colors
- Merging works through the new tiers

---

### Session 3: Remaining Fruits + Polymorphism
**TODO Locations:** `Pear.cs`, `Peach.cs`, `Pineapple.cs`, `Melon.cs`, `Watermelon.cs`

**Learning Goal:** Complete the merge chain, understand polymorphism

**Special Case — Watermelon:**
```csharp
public override int GetMergeResultTier()
{
    return -1;  // No further merge possible
}
```

**Polymorphism Teaching Script:**
- Open GameManager.cs, find `List<Fruit> activeFruits`
- "This list holds Cherry, Grape, Peach — ANY derived type"
- "When we call `fruit.GetTier()` in the foreach loop, which version runs?"
- Answer: "The derived version. Cherry returns 0, Grape returns 2, Peach returns 7"

**Verification:**
- Full merge chain: Cherry → Strawberry → ... → Watermelon
- Two Watermelons do NOT merge

---

### Session 4: Custom OnMerge() Overrides
**Learning Goal:** Override a virtual method with creative custom behavior

**Example:**
```csharp
// In Cherry.cs:
public override void OnMerge()
{
    Debug.Log("Pop! Two cherries merged!");
}
```

**Teaching Points:**
- Same merge code in Fruit.cs calls OnMerge() on every fruit
- Each fruit can have DIFFERENT effects — that's polymorphism
- The merge system never checks "is this a Cherry?" — it just calls OnMerge()

---

### Session 5: Abstract Refactor (Stretch)
**Learning Goal:** Understand abstract vs virtual

**Changes to Fruit.cs:**
```csharp
// Change class declaration:
public abstract class Fruit : MonoBehaviour

// Change select methods:
public abstract int GetTier();        // was: public virtual int GetTier() { return tier; }
public abstract int GetPointValue();  // was: public virtual int GetPointValue() { return pointValue; }
```

**Teaching Points:**
- `virtual` = CAN override (optional)
- `abstract` = MUST override (required, compiler enforces it)
- Cannot instantiate abstract class directly

---

### Sessions 6-8: Polish, Build, Present

---

## Key Vocabulary

| Term | Meaning |
|------|---------|
| Base class | A class that other classes inherit from (`Fruit`) |
| Derived class | A class that inherits from another (`Cherry : Fruit`) |
| `protected` | Accessible by this class AND its children |
| `virtual` | Method that CAN be overridden by derived classes |
| `override` | Replaces the parent's version of a method |
| `abstract` | Method that MUST be overridden (no default body) |
| Polymorphism | Using a base reference to work with any derived type |
| `base.Method()` | Calls the parent class version of a method |

---

## Student Implementation Checklist

### Session 2 Checklist
- [ ] Orange.cs compiles with correct values
- [ ] Dekopon.cs compiles with correct values
- [ ] Apple.cs compiles with correct values
- [ ] Created 3 prefabs, assigned to FruitFactory slots 3-5
- [ ] 6 fruit types appear in game

### Session 3 Checklist
- [ ] Pear through Melon complete with correct values
- [ ] Watermelon overrides GetMergeResultTier() → returns -1
- [ ] Created 5 prefabs, assigned to FruitFactory slots 6-10
- [ ] Full merge chain works
- [ ] Two Watermelons do NOT merge

### Session 4 Checklist
- [ ] At least 3 fruits have custom OnMerge() overrides
- [ ] Each override produces different behavior
- [ ] Merge effects trigger correctly during gameplay

---

## Troubleshooting

### "New fruit doesn't appear"
- Check prefab has the correct derived script (not just Fruit)
- Check FruitFactory Inspector — is the prefab in the right slot?
- Verify base.Awake() is called in the derived Awake()

### "Fruits don't merge"
- Both fruits need Rigidbody2D and CircleCollider2D
- Colliders must NOT be set as Trigger
- Check tier values match between the two fruits

### "NullReferenceException on fruit"
- Usually means base.Awake() was not called
- Check derived class has `base.Awake();` at the end of Awake()

### "Wrong size or color"
- Verify fruitSize and fruitColor values in derived Awake()
- Make sure values are set BEFORE base.Awake()

---

**Happy Teaching!**
