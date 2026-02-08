# Inheritance Concepts Reference

**GAME 220: Core Programming -- Watermelon Merge Game**

This document maps inheritance vocabulary to the actual code in your Watermelon Merge project.
Every definition below includes the exact file and line where you can see the concept in action.

---

## The Fruit Hierarchy

```
                        MonoBehaviour
                             |
                          Fruit           <-- BASE CLASS (Fruit.cs)
                             |                Defines: tier, fruitName, pointValue,
                             |                fruitSize, fruitColor, merge detection,
                             |                physics helpers, virtual methods
                             |
     +-----------+-----------+-----------+-----------+-----------+
     |           |           |           |           |           |
   Cherry    Strawberry    Grape      Orange     Dekopon      Apple
   tier=0     tier=1      tier=2     tier=3      tier=4      tier=5
     |           |           |           |           |           |
   (small)   (small)     (medium)   (medium)    (medium)    (large)
     |           |           |           |           |           |
     +-----------+-----------+-----------+-----------+-----------+
                             |
     +-----------+-----------+-----------+-----------+-----------+
     |           |           |           |           |           |
    Pear      Peach     Pineapple     Melon    Watermelon       |
   tier=6     tier=7      tier=8     tier=9     tier=10         |
     |           |           |           |           |           |
   (large)   (large)     (huge)     (huge)    (biggest)         |
     |           |           |           |           |           |
     +-----------+-----------+-----------+-----------+-----------+

   MERGE CHAIN:  Cherry + Cherry --> Strawberry
                 Strawberry + Strawberry --> Grape
                 Grape + Grape --> Orange
                 ... and so on ...
                 Melon + Melon --> Watermelon
                 Watermelon + Watermelon --> NOTHING (max tier!)

   ALL 11 DERIVED CLASSES inherit the SAME fields and methods from Fruit.
   Each one ONLY overrides Awake() to set its own values.
   Watermelon ALSO overrides GetMergeResultTier() to return -1.
```

---

## Key Terms

---

### Base Class (Fruit.cs)

**Definition:** A base class is a class that other classes inherit from. It defines the
shared fields and methods that all derived classes receive automatically.

**In our game:** `Fruit` is the base class. It lives in `Assets/_Project/Scripts/Fruits/Fruit.cs`.
Every fruit in the game -- Cherry, Strawberry, Grape, all the way to Watermelon --
inherits from `Fruit`.

**What the base class provides:**

| Category               | What Fruit.cs Declares                                         |
|------------------------|----------------------------------------------------------------|
| Protected fields       | `tier`, `fruitName`, `pointValue`, `fruitSize`, `fruitColor`   |
| Component references   | `rb`, `circleCollider`, `spriteRenderer`                       |
| Virtual methods        | `Awake()`, `Start()`, `GetTier()`, `GetPointValue()`, etc.     |
| Merge detection        | `OnCollisionEnter2D()` -- works for ALL fruit types            |
| Physics helpers        | `SetPhysicsEnabled()`, `SetKinematic()`                        |

**Code snippet** (from `Fruit.cs`):

```csharp
public class Fruit : MonoBehaviour
{
    // Protected fields -- accessible by derived classes
    protected int tier = 0;
    protected string fruitName = "Fruit";
    protected int pointValue = 0;
    protected float fruitSize = 1f;
    protected Color fruitColor = Color.white;

    // Cached component references
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider;
    protected SpriteRenderer spriteRenderer;

    // Virtual method -- derived classes CAN override this
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
```

**Plain English:** Think of `Fruit` as a template. It says "every fruit has a tier, a name,
a point value, a size, and a color." It does not know *which* tier or *which* color -- that
is left to each specific fruit class to fill in.

---

### Derived Class (Cherry.cs, Grape.cs, etc.)

**Definition:** A derived class is a class that inherits from a base class. It receives all
the base class fields and methods, and can customize behavior by overriding virtual methods.

**The `:` syntax:** The colon after the class name means "inherits from."

```csharp
public class Cherry : Fruit      // Cherry inherits from Fruit
public class Grape : Fruit       // Grape inherits from Fruit
public class Watermelon : Fruit  // Watermelon inherits from Fruit
```

**What each derived class customizes:** Only the values. The structure is identical every time.

**Code snippet** (from `Cherry.cs`):

```csharp
public class Cherry : Fruit    // <-- the colon means "inherits from"
{
    protected override void Awake()
    {
        tier = 0;                                       // Cherry is first in the chain
        fruitName = "Cherry";                           // Display name
        pointValue = 1;                                 // Small fruit = small points
        fruitSize = 0.5f;                               // Smallest fruit
        fruitColor = new Color(0.85f, 0.12f, 0.15f);   // Bright red

        base.Awake();   // Run the parent Awake to cache components
    }
}
```

**What Cherry gets for free** (without writing a single extra line):

- `OnCollisionEnter2D()` merge detection
- `SetPhysicsEnabled()` and `SetKinematic()` physics helpers
- `GetTier()`, `GetPointValue()`, `GetFruitName()` getter methods
- `ApplyFruitProperties()` visual setup
- All component references (`rb`, `circleCollider`, `spriteRenderer`)

**Plain English:** Cherry says "I am a Fruit, but specifically I am tier 0, I am called
Cherry, I am worth 1 point, I am small, and I am red." Everything else -- how to merge, how
to fall, how to render -- it inherits from `Fruit` without doing any extra work.

---

### protected

**Definition:** The `protected` access modifier means a field or method is accessible by the
class that declares it AND by any class that inherits from it. Classes that do not inherit
from it cannot access protected members.

**Comparison:**

| Access Modifier | Who Can Access It                     | Use Case in Our Game                          |
|-----------------|---------------------------------------|-----------------------------------------------|
| `private`       | Only the declaring class              | `hasMerged` in Fruit.cs -- children do not need direct access |
| `protected`     | Declaring class + all derived classes | `tier`, `fruitName`, `fruitSize` -- children must set these    |
| `public`        | Any class in the project              | `GetTier()`, `SetPhysicsEnabled()` -- anyone can call these   |

**Code example** (from `Fruit.cs` and `Cherry.cs`):

```csharp
// In Fruit.cs -- declared as protected
protected int tier = 0;
protected string fruitName = "Fruit";

// In Cherry.cs -- accessible because Cherry inherits from Fruit
protected override void Awake()
{
    tier = 0;                  // OK! Cherry can access tier because it is protected
    fruitName = "Cherry";      // OK! Cherry can access fruitName because it is protected
    base.Awake();
}
```

**What would happen with `private` instead?**

```csharp
// If Fruit.cs declared:  private int tier = 0;
// Then Cherry.cs could NOT write:  tier = 0;
// Compiler error: "Fruit.tier is inaccessible due to its protection level"
```

**Plain English:** `protected` is the sweet spot for inheritance. It keeps data hidden from
unrelated classes (unlike `public`) but still lets child classes set values (unlike `private`).
It is the "family only" access level.

---

### virtual

**Definition:** The `virtual` keyword marks a method as overridable. A virtual method provides
a default implementation that derived classes *can* replace with their own version, but they
are not required to.

**Code example** (from `Fruit.cs`):

```csharp
// In Fruit.cs -- the virtual keyword allows derived classes to override
protected virtual void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    circleCollider = GetComponent<CircleCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}

public virtual void OnMerge()
{
    Debug.Log($"{fruitName} is merging!");
}

public virtual int GetMergeResultTier()
{
    return tier + 1;
}
```

**When to use `virtual`:**

- When you want a method to have a **default behavior** that works for most derived classes
- When some derived classes **might want** to customize it, but others are fine with the default
- Example: `GetMergeResultTier()` defaults to `tier + 1`, which is correct for 10 out of 11
  fruits. Only Watermelon needs to override it.

**Plain English:** `virtual` is like saying "here is the standard way to do this, but if you
need to do it differently, you are allowed to." Most fruits use the default `OnMerge()` that
just logs a message, but in Session 4 you can override it to add custom effects.

---

### override

**Definition:** The `override` keyword replaces a base class virtual method with a new
implementation in the derived class. The method signature (name, parameters, return type)
must match exactly.

**Code example** (from `Cherry.cs`):

```csharp
// In Fruit.cs (base class):
protected virtual void Awake()         // <-- virtual = CAN be overridden
{
    rb = GetComponent<Rigidbody2D>();
    // ...
}

// In Cherry.cs (derived class):
protected override void Awake()        // <-- override = REPLACING the base version
{
    tier = 0;
    fruitName = "Cherry";
    pointValue = 1;
    fruitSize = 0.5f;
    fruitColor = new Color(0.85f, 0.12f, 0.15f);

    base.Awake();                      // <-- still calls the base version (see next section)
}
```

**The pattern (used by every fruit class):**

1. Declare the method with `protected override void Awake()`
2. Set your specific values (`tier`, `fruitName`, `pointValue`, `fruitSize`, `fruitColor`)
3. Call `base.Awake()` at the end to run the parent component caching

**Another override example** (Watermelon overriding `GetMergeResultTier()`):

```csharp
// In Watermelon.cs:
public override int GetMergeResultTier()
{
    return -1;   // Watermelon is the final fruit -- no further merge possible
}
```

**Plain English:** `override` is like saying "I know the base class has a version of this
method, but I am replacing it with my own." The access modifier and method signature must
match the original -- you cannot change `protected` to `public` or add extra parameters.

---

### base.Method()

**Definition:** The `base` keyword refers to the parent (base) class. Calling `base.Method()`
runs the parent version of an overridden method from within the derived class override.

**Code example** (from `Cherry.cs`):

```csharp
protected override void Awake()
{
    // 1. First, set Cherry-specific values
    tier = 0;
    fruitName = "Cherry";
    pointValue = 1;
    fruitSize = 0.5f;
    fruitColor = new Color(0.85f, 0.12f, 0.15f);

    // 2. Then, call the parent Awake() to cache component references
    base.Awake();    // <-- runs Fruit.Awake(), which does:
                     //     rb = GetComponent<Rigidbody2D>();
                     //     circleCollider = GetComponent<CircleCollider2D>();
                     //     spriteRenderer = GetComponent<SpriteRenderer>();
}
```

**What happens if you forget `base.Awake()`?**

If you omit `base.Awake()`, the parent version never runs. That means `rb`,
`circleCollider`, and `spriteRenderer` will all be **null**. The fruit will:

- Not respond to physics (no Rigidbody2D reference)
- Not detect collisions for merging (no collider reference)
- Not display its color (no SpriteRenderer reference)
- Throw `NullReferenceException` errors at runtime

**The rule:** If you override a method that does important setup work, always call the `base`
version unless you have a specific reason not to. In our game, **always** call `base.Awake()`
at the end of your derived `Awake()`.

**Plain English:** `base.Awake()` means "also do what my parent does." Your override replaces
the parent method, so without `base.Awake()`, the parent code never runs. Think of it as
calling your parent to make sure they did their part of the setup.

---

### Polymorphism

**Definition:** Polymorphism means "many forms." In C#, it means a variable typed as a base
class can hold any derived class, and method calls on that variable automatically run the
correct derived version. You write code that works with the general type (`Fruit`), and it
automatically works with every specific type (`Cherry`, `Grape`, `Watermelon`, etc.).

**Plain English:** Polymorphism lets you treat all fruits the same way in your code, even
though each fruit behaves differently. You do not need to write "if this is a Cherry, do X;
if this is a Grape, do Y." You just call the method, and the right version runs.

**Three concrete examples from the codebase:**

#### Example 1: Polymorphic Variable -- `Fruit currentFruit` in DropController.cs

```csharp
// In DropController.cs:
private Fruit currentFruit;    // Typed as Fruit, but holds Cherry, Grape, etc.

void SpawnNextFruit()
{
    // FruitFactory returns a Fruit reference, but the actual object
    // is Cherry, Grape, Orange, or whatever was randomly chosen
    currentFruit = fruitFactory.CreateFruit(randomTier);

    // These method calls work regardless of which fruit type it actually is
    currentFruit.SetKinematic(true);
    currentFruit.SetPhysicsEnabled(true);
}
```

We never write `Cherry currentCherry` or `Grape currentGrape`. One variable, typed as
`Fruit`, handles every type. When the player drops the fruit, it does not matter whether it
is a Cherry or a Dekopon -- the same code works for all of them.

#### Example 2: Polymorphic Collection -- `List<Fruit> activeFruits` in GameManager.cs

```csharp
// In GameManager.cs:
private List<Fruit> activeFruits = new List<Fruit>();

// This list holds a MIX of Cherry, Grape, Orange, Watermelon, etc.
// all at the same time. We never need separate lists per type.

public int GetHighestTier()
{
    int highest = -1;

    foreach (Fruit fruit in activeFruits)    // Each fruit could be ANY derived type
    {
        if (fruit != null && fruit.GetTier() > highest)
        {
            highest = fruit.GetTier();   // Calls the derived version automatically
        }                                // Cherry returns 0, Grape returns 2, etc.
    }

    return highest;
}
```

The `foreach` loop iterates over a mix of different fruit types in a single list. Calling
`fruit.GetTier()` on each one returns the correct tier for that specific fruit, without
any type-checking `if` statements.

#### Example 3: Polymorphic Parameters -- `MergeFruits(Fruit a, Fruit b)` in GameManager.cs

```csharp
// In GameManager.cs:
public void MergeFruits(Fruit fruitA, Fruit fruitB)
{
    // fruitA and fruitB could be ANY fruit type -- two Cherries, two Grapes, etc.

    int nextTier = fruitA.GetMergeResultTier();   // Calls the derived version
    // Most fruits return tier + 1
    // Watermelon returns -1

    int points = fruitA.GetPointValue() + fruitB.GetPointValue();
    // Cherry returns 1, Grape returns 6, Orange returns 10, etc.

    fruitA.OnMerge();    // Calls the derived version (custom effects in Session 4)
    fruitB.OnMerge();    // Calls the derived version

    // Destroy both, spawn the next tier...
}
```

This ONE method handles every possible merge in the game. We never write
`MergeCherries(Cherry a, Cherry b)` or `MergeGrapes(Grape a, Grape b)`.
The parameters are typed as `Fruit`, so any two same-tier fruits work.

---

### abstract (Session 5 Stretch)

**Definition:** The `abstract` keyword marks a method (or class) as having **no default
implementation**. Derived classes **must** override an abstract method -- the compiler will
produce an error if they do not. An abstract method can only exist inside an abstract class.

**Comparison with `virtual`:**

| Feature            | `virtual`                                  | `abstract`                                  |
|--------------------|--------------------------------------------|---------------------------------------------|
| Has a body?        | Yes -- provides a default implementation   | No -- just a signature, no body             |
| Override required? | No -- derived classes *may* override       | Yes -- derived classes *must* override      |
| Class requirement  | Can exist in a regular class               | Must be in an `abstract class`              |
| Can instantiate?   | The class can be instantiated directly     | The abstract class cannot be instantiated   |

**What it would look like in our game** (Session 5 stretch goal):

```csharp
// Fruit.cs becomes an abstract class:
public abstract class Fruit : MonoBehaviour
{
    // Abstract method: NO body, NO default -- every fruit MUST implement this
    protected abstract void InitializeFruitProperties();

    protected virtual void Awake()
    {
        InitializeFruitProperties();  // Calls the derived version (guaranteed to exist)
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

// Cherry.cs MUST implement InitializeFruitProperties() -- compiler error if it does not:
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
}
```

**When to use `abstract`:**

- When there is **no sensible default** and every derived class **must** provide its own version
- When you want the **compiler to enforce** that derived classes implement the method
- When the base class should **never be instantiated** on its own (a generic "Fruit" with no
  specific values does not make sense in our game)

**Plain English:** `virtual` says "here is a default, but you can change it." `abstract` says
"I am not giving you a default -- you MUST write your own." It is a stronger contract.

---

## Side-by-Side Comparison

Below is `Fruit.cs` (base class) on the left and `Cherry.cs` (derived class) on the right,
with annotations showing how they connect.

```
  FRUIT.CS (Base Class)                         CHERRY.CS (Derived Class)
  ========================                      ============================

  public class Fruit : MonoBehaviour            public class Cherry : Fruit
                   ^                                              ^
                   |                                              |
         inherits from Unity                            inherits from Fruit
                                                (gets MonoBehaviour too, through Fruit)


  PROTECTED FIELDS (declared here)              SETTING INHERITED FIELDS (in Awake)
  --------------------------------              ------------------------------------
  protected int tier = 0;             <----     tier = 0;
  protected string fruitName = "Fruit";<----    fruitName = "Cherry";
  protected int pointValue = 0;       <----     pointValue = 1;
  protected float fruitSize = 1f;     <----     fruitSize = 0.5f;
  protected Color fruitColor;         <----     fruitColor = new Color(0.85f, 0.12f, 0.15f);


  VIRTUAL METHOD (can be overridden)            OVERRIDE (replaces base version)
  ----------------------------------            --------------------------------
  protected virtual void Awake()       <---->   protected override void Awake()
  {                                             {
      rb = GetComponent<...>();                     tier = 0;
      circleCollider = GetComponent<>();            fruitName = "Cherry";
      spriteRenderer = GetComponent<>();            pointValue = 1;
  }                                                 fruitSize = 0.5f;
       ^                                            fruitColor = new Color(...);
       |
       +-------------------------------------       base.Awake();  // calls the base ^
                                                }


  VIRTUAL METHOD (default behavior)             NOT OVERRIDDEN -- Cherry inherits as-is
  ---------------------------------             -------------------------------------------
  public virtual int GetTier()                  (Cherry does not override GetTier()
  {                                              so the base version runs, returning
      return tier;                               the value Cherry set: 0)
  }

  public virtual void OnMerge()                 (Cherry does not override OnMerge()
  {                                              so the base version runs, logging
      Debug.Log(...);                             "Cherry is merging!")
  }

  public virtual int GetMergeResultTier()       (Cherry does not override this either,
  {                                              so the base returns tier + 1 = 1,
      return tier + 1;                           meaning two Cherries merge into
  }                                              tier 1 = Strawberry)


  PRIVATE FIELD (hidden from children)          NO ACCESS
  ------------------------------------          ---------
  private bool hasMerged = false;               Cherry CANNOT access hasMerged directly.
                                                It uses the public HasMerged() and
                                                SetMerged() methods instead.


  NON-VIRTUAL METHODS (inherited as-is)         INHERITED -- Cherry gets these for free
  -------------------------------------         -----------------------------------------
  public void SetPhysicsEnabled(bool e)         Cherry can call SetPhysicsEnabled()
  public void SetKinematic(bool k)              Cherry can call SetKinematic()
  protected void ApplyFruitProperties()         Called by Start() automatically
  void OnCollisionEnter2D(Collision2D)          Merge detection works automatically
```

---

## Common Mistakes

| # | Mistake | Symptom | Fix |
|---|---------|---------|-----|
| 1 | **Forgetting `base.Awake()`** in your derived class | `NullReferenceException` at runtime. The fruit appears in the scene but has no physics, no collision detection, and no color. The console shows errors about null `Rigidbody2D` or `SpriteRenderer`. | Add `base.Awake();` as the **last line** of your derived `Awake()` method. This runs the parent code that caches `rb`, `circleCollider`, and `spriteRenderer`. |
| 2 | **Using `private` instead of `protected`** on fields in the base class | Compiler error in derived classes: `Fruit.tier is inaccessible due to its protection level`. Your derived class cannot set `tier`, `fruitName`, or other fields. | Change the field declaration in `Fruit.cs` from `private` to `protected`. Remember: `protected` = this class + children. |
| 3 | **Missing the `override` keyword** on `Awake()` in a derived class | The derived `Awake()` hides the base version instead of overriding it. You get the compiler warning: `Cherry.Awake() hides inherited member Fruit.Awake()`. The base `Awake()` may run instead of (or in addition to) your version, leading to unpredictable behavior. | Change `protected void Awake()` to `protected override void Awake()`. The `override` keyword is required to properly replace a `virtual` method. |
| 4 | **Forgetting to set all five fields** in the derived `Awake()` | The fruit uses the default values from `Fruit.cs` for any field you forgot. For example, if you forget `fruitSize`, the fruit uses `1f` instead of your intended size. If you forget `fruitName`, the UI shows "Fruit" instead of the actual name. | Set all five fields in every derived class: `tier`, `fruitName`, `pointValue`, `fruitSize`, `fruitColor`. Copy the pattern from `Cherry.cs` and change only the values. |
| 5 | **Changing the access modifier** when overriding | Compiler error: `Cherry.Awake(): cannot change access modifiers when overriding protected inherited member Fruit.Awake()`. For example, writing `public override void Awake()` when the base uses `protected`. | Match the base class access modifier exactly. If `Fruit.Awake()` is `protected virtual`, your override must be `protected override`. |
| 6 | **Writing `Fruit` instead of your specific class name** in the class declaration | The file creates a duplicate `Fruit` class instead of a new derived class. Compiler error: `The type Fruit already contains a definition for...` | Make sure your class declaration says `public class YourFruit : Fruit`, not `public class Fruit`. The name before the colon is *your* class; the name after the colon is the *parent*. |
| 7 | **Calling `base.Awake()` before setting field values** | No compiler error, but the fields may not be set in time for some base class logic. While this happens to work in the current codebase (because `ApplyFruitProperties()` runs in `Start()`), it is a bad habit. The convention in this project is to set values first, then call `base.Awake()`. | Move `base.Awake()` to the **last line** of your override, after all your field assignments. |
| 8 | **Forgetting the `: Fruit` in the class declaration** | Your class does not inherit from `Fruit`. It has none of the base class fields or methods. Compiler errors everywhere: `tier does not exist in the current context`, `base.Awake() is not valid`, etc. | Add `: Fruit` after your class name: `public class MyFruit : Fruit`. |
| 9 | **Overriding a non-virtual method** | Compiler error: `Cherry.ApplyFruitProperties(): cannot override inherited member Fruit.ApplyFruitProperties() because it is not marked virtual, abstract, or override`. | Only methods marked `virtual` or `abstract` in the base class can be overridden. In our game, `ApplyFruitProperties()` is intentionally non-virtual -- set the protected fields instead and let the base class apply them. |
| 10 | **Creating a fruit class without a corresponding prefab** | The fruit class compiles fine, but `FruitFactory` cannot spawn it. The console shows: `FruitFactory: No prefab for tier X. Create the class and prefab, then assign it to slot X!` | After creating your `.cs` file, create a Unity prefab with the script attached, a `Rigidbody2D`, a `CircleCollider2D`, and a `SpriteRenderer`. Then drag the prefab into the correct slot in the `FruitFactory` inspector. |
