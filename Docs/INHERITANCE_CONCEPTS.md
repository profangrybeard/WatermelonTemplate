# Inheritance Concepts Reference

**GAME 220: Core Programming -- Merge Game**

This document maps inheritance vocabulary to the actual code in your Merge project.
Every definition below includes the exact file and line where you can see the concept in action.

---

## The MergeObject Hierarchy

```
                        MonoBehaviour
                             |
                       MergeObject        <-- BASE CLASS (MergeObject.cs)
                             |                Defines: tier, objectName, pointValue,
                             |                objectSize, objectColor, merge detection,
                             |                physics helpers, virtual methods
                             |
     +-----------+-----------+-----------+
     |           |           |           |
  TierZero    TierOne     TierTwo    (student-created classes)
   tier=0     tier=1      tier=2     tier=3, 4, ...

   MERGE CHAIN:  TierZero + TierZero --> TierOne
                 TierOne + TierOne --> TierTwo
                 TierTwo + TierTwo --> (next student-created tier)
                 ... and so on ...
                 Final tier + Final tier --> NOTHING (max tier!)

   ALL DERIVED CLASSES inherit the SAME fields and methods from MergeObject.
   Each one ONLY overrides Awake() to set its own values.
   The final tier ALSO overrides GetMergeResultTier() to return -1.
```

---

## Key Terms

---

### Base Class (MergeObject.cs)

**Definition:** A base class is a class that other classes inherit from. It defines the
shared fields and methods that all derived classes receive automatically.

**In our game:** `MergeObject` is the base class. It lives in `Assets/_Project/Scripts/MergeObjects/MergeObject.cs`.
Every object in the game -- TierZero, TierOne, TierTwo, and all student-created classes --
inherits from `MergeObject`.

**What the base class provides:**

| Category               | What MergeObject.cs Declares                                      |
|------------------------|-------------------------------------------------------------------|
| Protected fields       | `tier`, `objectName`, `pointValue`, `objectSize`, `objectColor`   |
| Component references   | `rb`, `circleCollider`, `spriteRenderer`                          |
| Virtual methods        | `Awake()`, `Start()`, `GetTier()`, `GetPointValue()`, etc.        |
| Merge detection        | `OnCollisionEnter2D()` -- works for ALL object types              |
| Physics helpers        | `SetPhysicsEnabled()`, `SetKinematic()`                           |

**Code snippet** (from `MergeObject.cs`):

```csharp
public class MergeObject : MonoBehaviour
{
    // Protected fields -- accessible by derived classes
    protected int tier = 0;
    protected string objectName = "MergeObject";
    protected int pointValue = 0;
    protected float objectSize = 1f;
    protected Color objectColor = Color.white;

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

**Plain English:** Think of `MergeObject` as a template. It says "every merge object has a tier, a name,
a point value, a size, and a color." It does not know *which* tier or *which* color -- that
is left to each specific derived class to fill in.

---

### Derived Class (TierZero.cs, TierOne.cs, etc.)

**Definition:** A derived class is a class that inherits from a base class. It receives all
the base class fields and methods, and can customize behavior by overriding virtual methods.

**The `:` syntax:** The colon after the class name means "inherits from."

```csharp
public class TierZero : MergeObject      // TierZero inherits from MergeObject
public class TierOne : MergeObject       // TierOne inherits from MergeObject
public class TierTwo : MergeObject       // TierTwo inherits from MergeObject
```

**What each derived class customizes:** Only the values. The structure is identical every time.

**Code snippet** (from `TierZero.cs`):

```csharp
public class TierZero : MergeObject    // <-- the colon means "inherits from"
{
    protected override void Awake()
    {
        tier = 0;                                       // TierZero is first in the chain
        objectName = "TierZero";                        // Display name
        pointValue = 1;                                 // Small object = small points
        objectSize = 0.5f;                              // Smallest object
        objectColor = new Color(0.85f, 0.12f, 0.15f);  // Bright red

        base.Awake();   // Run the parent Awake to cache components
    }
}
```

**What TierZero gets for free** (without writing a single extra line):

- `OnCollisionEnter2D()` merge detection
- `SetPhysicsEnabled()` and `SetKinematic()` physics helpers
- `GetTier()`, `GetPointValue()`, `GetObjectName()` getter methods
- `ApplyObjectProperties()` visual setup
- All component references (`rb`, `circleCollider`, `spriteRenderer`)

**Plain English:** TierZero says "I am a MergeObject, but specifically I am tier 0, I am called
TierZero, I am worth 1 point, I am small, and I am red." Everything else -- how to merge, how
to fall, how to render -- it inherits from `MergeObject` without doing any extra work.

---

### protected

**Definition:** The `protected` access modifier means a field or method is accessible by the
class that declares it AND by any class that inherits from it. Classes that do not inherit
from it cannot access protected members.

**Comparison:**

| Access Modifier | Who Can Access It                     | Use Case in Our Game                          |
|-----------------|---------------------------------------|-----------------------------------------------|
| `private`       | Only the declaring class              | `hasMerged` in MergeObject.cs -- children do not need direct access |
| `protected`     | Declaring class + all derived classes | `tier`, `objectName`, `objectSize` -- children must set these    |
| `public`        | Any class in the project              | `GetTier()`, `SetPhysicsEnabled()` -- anyone can call these   |

**Code example** (from `MergeObject.cs` and `TierZero.cs`):

```csharp
// In MergeObject.cs -- declared as protected
protected int tier = 0;
protected string objectName = "MergeObject";

// In TierZero.cs -- accessible because TierZero inherits from MergeObject
protected override void Awake()
{
    tier = 0;                  // OK! TierZero can access tier because it is protected
    objectName = "TierZero";   // OK! TierZero can access objectName because it is protected
    base.Awake();
}
```

**What would happen with `private` instead?**

```csharp
// If MergeObject.cs declared:  private int tier = 0;
// Then TierZero.cs could NOT write:  tier = 0;
// Compiler error: "MergeObject.tier is inaccessible due to its protection level"
```

**Plain English:** `protected` is the sweet spot for inheritance. It keeps data hidden from
unrelated classes (unlike `public`) but still lets child classes set values (unlike `private`).
It is the "family only" access level.

---

### virtual

**Definition:** The `virtual` keyword marks a method as overridable. A virtual method provides
a default implementation that derived classes *can* replace with their own version, but they
are not required to.

**Code example** (from `MergeObject.cs`):

```csharp
// In MergeObject.cs -- the virtual keyword allows derived classes to override
protected virtual void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    circleCollider = GetComponent<CircleCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}

public virtual void OnMerge()
{
    Debug.Log($"{objectName} is merging!");
}

public virtual int GetMergeResultTier()
{
    return tier + 1;
}
```

**When to use `virtual`:**

- When you want a method to have a **default behavior** that works for most derived classes
- When some derived classes **might want** to customize it, but others are fine with the default
- Example: `GetMergeResultTier()` defaults to `tier + 1`, which is correct for all classes
  except the final tier. Only the final tier needs to override it.

**Plain English:** `virtual` is like saying "here is the standard way to do this, but if you
need to do it differently, you are allowed to." Most classes use the default `OnMerge()` that
just logs a message, but in Session 4 you can override it to add custom effects.

---

### override

**Definition:** The `override` keyword replaces a base class virtual method with a new
implementation in the derived class. The method signature (name, parameters, return type)
must match exactly.

**Code example** (from `TierZero.cs`):

```csharp
// In MergeObject.cs (base class):
protected virtual void Awake()         // <-- virtual = CAN be overridden
{
    rb = GetComponent<Rigidbody2D>();
    // ...
}

// In TierZero.cs (derived class):
protected override void Awake()        // <-- override = REPLACING the base version
{
    tier = 0;
    objectName = "TierZero";
    pointValue = 1;
    objectSize = 0.5f;
    objectColor = new Color(0.85f, 0.12f, 0.15f);

    base.Awake();                      // <-- still calls the base version (see next section)
}
```

**The pattern (used by every derived class):**

1. Declare the method with `protected override void Awake()`
2. Set your specific values (`tier`, `objectName`, `pointValue`, `objectSize`, `objectColor`)
3. Call `base.Awake()` at the end to run the parent component caching

**Another override example** (final tier overriding `GetMergeResultTier()`):

```csharp
// In the final tier class:
public override int GetMergeResultTier()
{
    return -1;   // Final tier -- no further merge possible
}
```

**Plain English:** `override` is like saying "I know the base class has a version of this
method, but I am replacing it with my own." The access modifier and method signature must
match the original -- you cannot change `protected` to `public` or add extra parameters.

---

### base.Method()

**Definition:** The `base` keyword refers to the parent (base) class. Calling `base.Method()`
runs the parent version of an overridden method from within the derived class override.

**Code example** (from `TierZero.cs`):

```csharp
protected override void Awake()
{
    // 1. First, set TierZero-specific values
    tier = 0;
    objectName = "TierZero";
    pointValue = 1;
    objectSize = 0.5f;
    objectColor = new Color(0.85f, 0.12f, 0.15f);

    // 2. Then, call the parent Awake() to cache component references
    base.Awake();    // <-- runs MergeObject.Awake(), which does:
                     //     rb = GetComponent<Rigidbody2D>();
                     //     circleCollider = GetComponent<CircleCollider2D>();
                     //     spriteRenderer = GetComponent<SpriteRenderer>();
}
```

**What happens if you forget `base.Awake()`?**

If you omit `base.Awake()`, the parent version never runs. That means `rb`,
`circleCollider`, and `spriteRenderer` will all be **null**. The object will:

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
correct derived version. You write code that works with the general type (`MergeObject`), and it
automatically works with every specific type (`TierZero`, `TierOne`, `TierTwo`, etc.).

**Plain English:** Polymorphism lets you treat all merge objects the same way in your code, even
though each one behaves differently. You do not need to write "if this is a TierZero, do X;
if this is a TierOne, do Y." You just call the method, and the right version runs.

**Three concrete examples from the codebase:**

#### Example 1: Polymorphic Variable -- `MergeObject currentObject` in DropController.cs

```csharp
// In DropController.cs:
private MergeObject currentObject;    // Typed as MergeObject, but holds TierZero, TierOne, etc.

void SpawnNextObject()
{
    // MergeObjectFactory returns a MergeObject reference, but the actual object
    // is TierZero, TierOne, or whatever was randomly chosen
    currentObject = mergeObjectFactory.CreateObject(randomTier);

    // These method calls work regardless of which object type it actually is
    currentObject.SetKinematic(true);
    currentObject.SetPhysicsEnabled(true);
}
```

We never write `TierZero currentTierZero` or `TierOne currentTierOne`. One variable, typed as
`MergeObject`, handles every type. When the player drops the object, it does not matter whether it
is a TierZero or a TierTwo -- the same code works for all of them.

#### Example 2: Polymorphic Collection -- `List<MergeObject> activeObjects` in GameManager.cs

```csharp
// In GameManager.cs:
private List<MergeObject> activeObjects = new List<MergeObject>();

// This list holds a MIX of TierZero, TierOne, TierTwo, etc.
// all at the same time. We never need separate lists per type.

public int GetHighestTier()
{
    int highest = -1;

    foreach (MergeObject obj in activeObjects)    // Each object could be ANY derived type
    {
        if (obj != null && obj.GetTier() > highest)
        {
            highest = obj.GetTier();   // Calls the derived version automatically
        }                              // TierZero returns 0, TierTwo returns 2, etc.
    }

    return highest;
}
```

The `foreach` loop iterates over a mix of different object types in a single list. Calling
`obj.GetTier()` on each one returns the correct tier for that specific object, without
any type-checking `if` statements.

#### Example 3: Polymorphic Parameters -- `MergeObjects(MergeObject a, MergeObject b)` in GameManager.cs

```csharp
// In GameManager.cs:
public void MergeObjects(MergeObject objA, MergeObject objB)
{
    // objA and objB could be ANY object type -- two TierZeros, two TierOnes, etc.

    int nextTier = objA.GetMergeResultTier();   // Calls the derived version
    // Most objects return tier + 1
    // The final tier returns -1

    int points = objA.GetPointValue() + objB.GetPointValue();
    // TierZero returns 1, TierTwo returns 6, etc.

    objA.OnMerge();    // Calls the derived version (custom effects in Session 4)
    objB.OnMerge();    // Calls the derived version

    // Destroy both, spawn the next tier...
}
```

This ONE method handles every possible merge in the game. We never write
`MergeTierZeros(TierZero a, TierZero b)` or `MergeTierOnes(TierOne a, TierOne b)`.
The parameters are typed as `MergeObject`, so any two same-tier objects work.

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
// MergeObject.cs becomes an abstract class:
public abstract class MergeObject : MonoBehaviour
{
    // Abstract method: NO body, NO default -- every class MUST implement this
    protected abstract void InitializeMergeObjectProperties();

    protected virtual void Awake()
    {
        InitializeMergeObjectProperties();  // Calls the derived version (guaranteed to exist)
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

// TierZero.cs MUST implement InitializeMergeObjectProperties() -- compiler error if it does not:
public class TierZero : MergeObject
{
    protected override void InitializeMergeObjectProperties()
    {
        tier = 0;
        objectName = "TierZero";
        pointValue = 1;
        objectSize = 0.5f;
        objectColor = new Color(0.85f, 0.12f, 0.15f);
    }
}
```

**When to use `abstract`:**

- When there is **no sensible default** and every derived class **must** provide its own version
- When you want the **compiler to enforce** that derived classes implement the method
- When the base class should **never be instantiated** on its own (a generic "MergeObject" with no
  specific values does not make sense in our game)

**Plain English:** `virtual` says "here is a default, but you can change it." `abstract` says
"I am not giving you a default -- you MUST write your own." It is a stronger contract.

---

## Side-by-Side Comparison

Below is `MergeObject.cs` (base class) on the left and `TierZero.cs` (derived class) on the right,
with annotations showing how they connect.

```
  MERGEOBJECT.CS (Base Class)                   TIERZERO.CS (Derived Class)
  ========================                      ============================

  public class MergeObject : MonoBehaviour      public class TierZero : MergeObject
                   ^                                              ^
                   |                                              |
         inherits from Unity                            inherits from MergeObject
                                                (gets MonoBehaviour too, through MergeObject)


  PROTECTED FIELDS (declared here)              SETTING INHERITED FIELDS (in Awake)
  --------------------------------              ------------------------------------
  protected int tier = 0;             <----     tier = 0;
  protected string objectName = "MergeObject";<- objectName = "TierZero";
  protected int pointValue = 0;       <----     pointValue = 1;
  protected float objectSize = 1f;    <----     objectSize = 0.5f;
  protected Color objectColor;        <----     objectColor = new Color(0.85f, 0.12f, 0.15f);


  VIRTUAL METHOD (can be overridden)            OVERRIDE (replaces base version)
  ----------------------------------            --------------------------------
  protected virtual void Awake()       <---->   protected override void Awake()
  {                                             {
      rb = GetComponent<...>();                     tier = 0;
      circleCollider = GetComponent<>();            objectName = "TierZero";
      spriteRenderer = GetComponent<>();            pointValue = 1;
  }                                                 objectSize = 0.5f;
       ^                                            objectColor = new Color(...);
       |
       +-------------------------------------       base.Awake();  // calls the base ^
                                                }


  VIRTUAL METHOD (default behavior)             NOT OVERRIDDEN -- TierZero inherits as-is
  ---------------------------------             -------------------------------------------
  public virtual int GetTier()                  (TierZero does not override GetTier()
  {                                              so the base version runs, returning
      return tier;                               the value TierZero set: 0)
  }

  public virtual void OnMerge()                 (TierZero does not override OnMerge()
  {                                              so the base version runs, logging
      Debug.Log(...);                             "TierZero is merging!")
  }

  public virtual int GetMergeResultTier()       (TierZero does not override this either,
  {                                              so the base returns tier + 1 = 1,
      return tier + 1;                           meaning two TierZeros merge into
  }                                              tier 1 = TierOne)


  PRIVATE FIELD (hidden from children)          NO ACCESS
  ------------------------------------          ---------
  private bool hasMerged = false;               TierZero CANNOT access hasMerged directly.
                                                It uses the public HasMerged() and
                                                SetMerged() methods instead.


  NON-VIRTUAL METHODS (inherited as-is)         INHERITED -- TierZero gets these for free
  -------------------------------------         -----------------------------------------
  public void SetPhysicsEnabled(bool e)         TierZero can call SetPhysicsEnabled()
  public void SetKinematic(bool k)              TierZero can call SetKinematic()
  protected void ApplyObjectProperties()        Called by Start() automatically
  void OnCollisionEnter2D(Collision2D)          Merge detection works automatically
```

---

## Common Mistakes

| # | Mistake | Symptom | Fix |
|---|---------|---------|-----|
| 1 | **Forgetting `base.Awake()`** in your derived class | `NullReferenceException` at runtime. The object appears in the scene but has no physics, no collision detection, and no color. The console shows errors about null `Rigidbody2D` or `SpriteRenderer`. | Add `base.Awake();` as the **last line** of your derived `Awake()` method. This runs the parent code that caches `rb`, `circleCollider`, and `spriteRenderer`. |
| 2 | **Using `private` instead of `protected`** on fields in the base class | Compiler error in derived classes: `MergeObject.tier is inaccessible due to its protection level`. Your derived class cannot set `tier`, `objectName`, or other fields. | Change the field declaration in `MergeObject.cs` from `private` to `protected`. Remember: `protected` = this class + children. |
| 3 | **Missing the `override` keyword** on `Awake()` in a derived class | The derived `Awake()` hides the base version instead of overriding it. You get the compiler warning: `TierZero.Awake() hides inherited member MergeObject.Awake()`. The base `Awake()` may run instead of (or in addition to) your version, leading to unpredictable behavior. | Change `protected void Awake()` to `protected override void Awake()`. The `override` keyword is required to properly replace a `virtual` method. |
| 4 | **Forgetting to set all five fields** in the derived `Awake()` | The object uses the default values from `MergeObject.cs` for any field you forgot. For example, if you forget `objectSize`, the object uses `1f` instead of your intended size. If you forget `objectName`, the UI shows "MergeObject" instead of the actual name. | Set all five fields in every derived class: `tier`, `objectName`, `pointValue`, `objectSize`, `objectColor`. Copy the pattern from `TierZero.cs` and change only the values. |
| 5 | **Changing the access modifier** when overriding | Compiler error: `TierZero.Awake(): cannot change access modifiers when overriding protected inherited member MergeObject.Awake()`. For example, writing `public override void Awake()` when the base uses `protected`. | Match the base class access modifier exactly. If `MergeObject.Awake()` is `protected virtual`, your override must be `protected override`. |
| 6 | **Writing `MergeObject` instead of your specific class name** in the class declaration | The file creates a duplicate `MergeObject` class instead of a new derived class. Compiler error: `The type MergeObject already contains a definition for...` | Make sure your class declaration says `public class YourClass : MergeObject`, not `public class MergeObject`. The name before the colon is *your* class; the name after the colon is the *parent*. |
| 7 | **Calling `base.Awake()` before setting field values** | No compiler error, but the fields may not be set in time for some base class logic. While this happens to work in the current codebase (because `ApplyObjectProperties()` runs in `Start()`), it is a bad habit. The convention in this project is to set values first, then call `base.Awake()`. | Move `base.Awake()` to the **last line** of your override, after all your field assignments. |
| 8 | **Forgetting the `: MergeObject` in the class declaration** | Your class does not inherit from `MergeObject`. It has none of the base class fields or methods. Compiler errors everywhere: `tier does not exist in the current context`, `base.Awake() is not valid`, etc. | Add `: MergeObject` after your class name: `public class MyClass : MergeObject`. |
| 9 | **Overriding a non-virtual method** | Compiler error: `TierZero.ApplyObjectProperties(): cannot override inherited member MergeObject.ApplyObjectProperties() because it is not marked virtual, abstract, or override`. | Only methods marked `virtual` or `abstract` in the base class can be overridden. In our game, `ApplyObjectProperties()` is intentionally non-virtual -- set the protected fields instead and let the base class apply them. |
| 10 | **Creating a class without a corresponding prefab** | The class compiles fine, but `MergeObjectFactory` cannot spawn it. The console shows: `MergeObjectFactory: No prefab for tier X. Create the class and prefab, then assign it to slot X!` | After creating your `.cs` file, create a Unity prefab with the script attached, a `Rigidbody2D`, a `CircleCollider2D`, and a `SpriteRenderer`. Then drag the prefab into the correct slot in the `MergeObjectFactory` inspector. |

Our Business is Fun
