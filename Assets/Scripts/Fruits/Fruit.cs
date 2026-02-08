/*
 * GAME 220: Watermelon Merge Template
 * Sessions 1-6: Fruit Base Class
 *
 * TEACHING FOCUS:
 * - INHERITANCE: This is the BASE CLASS that all fruits inherit from
 * - PROTECTED FIELDS: Fields marked 'protected' are accessible by this class AND its children
 * - VIRTUAL METHODS: Methods marked 'virtual' CAN be overridden by derived classes
 * - POLYMORPHISM: Code that references Fruit can work with ANY derived fruit type
 *
 * This script defines the shared behavior for ALL fruit types in the game.
 * Cherry, Strawberry, Grape, Orange, and every other fruit INHERITS from this class.
 * That means they automatically get all the fields, methods, and logic defined here.
 *
 * WHAT THIS BASE CLASS PROVIDES:
 * - Protected fields for tier, size, color, points, and name
 * - Virtual methods that derived classes can override to customize behavior
 * - Merge detection logic (OnCollisionEnter2D) that works for ALL fruit types
 * - Physics helpers for the drop controller
 * - Automatic visual setup (size, color) from derived class values
 *
 * STUDENT TASKS:
 * - Session 1: Read and understand this base class
 * - Session 4: Override OnMerge() in your fruit classes for custom effects
 * - Session 5 (STRETCH): Convert this class to abstract
 */

using UnityEngine;

// =====================================================================
// TEACHING: BASE CLASS
//
// A base class is a blueprint that other classes can INHERIT from.
// 'Fruit' defines what ALL fruits have in common: a tier, a size,
// a color, points, and the ability to merge.
//
// Derived classes (Cherry, Grape, Orange, etc.) inherit everything here
// and customize it by overriding the Awake() method to set their own values.
//
// The colon syntax below means "Fruit inherits from MonoBehaviour":
//   public class Fruit : MonoBehaviour
//
// When Cherry says "public class Cherry : Fruit", it means:
//   "Cherry inherits everything from Fruit (which inherits from MonoBehaviour)"
// =====================================================================
public class Fruit : MonoBehaviour
{
    // ============================================
    // PROTECTED FIELDS (Set by Derived Classes)
    // ============================================

    // =====================================================================
    // TEACHING: PROTECTED ACCESS MODIFIER
    //
    // 'protected' means: this class AND any class that inherits from it
    // can read and write these fields. Classes that do NOT inherit from
    // Fruit cannot access them.
    //
    // Compare with:
    //   private   = ONLY this class can access (too restrictive for children)
    //   public    = ANY class can access (too open, no protection)
    //   protected = this class + children (just right for inheritance!)
    //
    // Each derived class (Cherry, Grape, etc.) sets these values in its
    // own Awake() method. For example, Cherry sets tier = 0, fruitSize = 0.5f, etc.
    // =====================================================================

    [Header("Fruit Identity (Set by Derived Class)")]
    protected int tier = 0;                      // Position in the merge chain (0=Cherry, 10=Watermelon)
    protected string fruitName = "Fruit";        // Display name for UI and debug messages

    [Header("Fruit Properties (Set by Derived Class)")]
    protected int pointValue = 0;                // Score points awarded when this fruit is part of a merge
    protected float fruitSize = 1f;              // Transform.localScale multiplier (bigger number = bigger fruit)
    protected Color fruitColor = Color.white;    // SpriteRenderer color tint

    // ============================================
    // COMPONENT REFERENCES (Cached in Awake)
    // ============================================

    // TEACHING: We cache component references in Awake() so we don't have to
    // call GetComponent<T>() every frame. This is a standard Unity optimization.
    // These are 'protected' so derived classes can access them if needed.
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider;
    protected SpriteRenderer spriteRenderer;

    // ============================================
    // PRIVATE STATE
    // ============================================

    // TEACHING: 'private' means ONLY this class can access hasMerged.
    // Derived classes don't need to touch this directly -- they use
    // the public HasMerged() and SetMerged() methods instead.
    private bool hasMerged = false;              // Prevents double-merge in the same frame


    // ============================================
    // UNITY LIFECYCLE METHODS
    // ============================================

    // =====================================================================
    // TEACHING: VIRTUAL METHODS
    //
    // A method marked 'virtual' provides a DEFAULT implementation that
    // derived classes CAN override. If a derived class doesn't override it,
    // this base version runs instead.
    //
    // Awake() is virtual so that derived classes (Cherry, Grape, etc.)
    // can override it to set their own tier, size, color, and points
    // BEFORE the base class caches component references.
    //
    // Compare with 'abstract' (Session 5 stretch): an abstract method
    // has NO default -- derived classes MUST override it.
    // =====================================================================

    /// <summary>
    /// Caches component references. Derived classes override this to set
    /// their specific values (tier, size, color, etc.) BEFORE calling base.Awake().
    /// </summary>
    protected virtual void Awake()
    {
        // Cache component references so we don't call GetComponent every frame
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Applies the fruit's visual properties after Awake has run.
    /// </summary>
    protected virtual void Start()
    {
        // Apply the values set by the derived class to the actual Unity components
        ApplyFruitProperties();
    }


    // ============================================
    // PROPERTY APPLICATION
    // ============================================

    /// <summary>
    /// Applies fruitSize and fruitColor to the Unity components.
    /// Called automatically in Start(). Not virtual -- derived classes
    /// should NOT override this. Instead, set the protected fields
    /// in Awake() and let this method do the rest.
    /// </summary>
    protected void ApplyFruitProperties()
    {
        // Apply scale (fruitSize controls how big the fruit appears)
        transform.localScale = Vector3.one * fruitSize;

        // Apply color to the sprite renderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = fruitColor;
        }
    }


    // ============================================
    // VIRTUAL METHODS (Override in Derived Classes)
    // ============================================

    // =====================================================================
    // TEACHING: VIRTUAL GETTERS
    //
    // These methods are marked 'virtual' so derived classes CAN override them.
    // By default, they return the protected field values set in Awake().
    //
    // POLYMORPHISM IN ACTION:
    // When code calls fruit.GetTier() on a Fruit reference, the DERIVED
    // version runs if the actual object is Cherry, Grape, etc.
    // This is called "virtual method dispatch" or "dynamic dispatch."
    //
    // Example:
    //   Fruit myFruit = someCherry;  // Typed as Fruit, actual object is Cherry
    //   myFruit.GetTier();           // Calls Cherry's version -> returns 0
    //
    //   Fruit myFruit = someGrape;   // Typed as Fruit, actual object is Grape
    //   myFruit.GetTier();           // Calls Grape's version -> returns 2
    // =====================================================================

    /// <summary>
    /// Returns this fruit's tier (position in the merge chain).
    /// Cherry = 0, Strawberry = 1, ... Watermelon = 10.
    /// </summary>
    public virtual int GetTier()
    {
        return tier;
    }

    /// <summary>
    /// Returns the score points this fruit is worth when merged.
    /// </summary>
    public virtual int GetPointValue()
    {
        return pointValue;
    }

    /// <summary>
    /// Returns the visual size multiplier for this fruit.
    /// </summary>
    public virtual float GetFruitSize()
    {
        return fruitSize;
    }

    /// <summary>
    /// Returns the display name of this fruit (e.g., "Cherry", "Watermelon").
    /// </summary>
    public virtual string GetFruitName()
    {
        return fruitName;
    }

    /// <summary>
    /// Returns the tier of the fruit that results from merging two of this type.
    /// Default: tier + 1 (next fruit in the chain).
    /// Watermelon overrides this to return -1 (no further merge possible).
    /// </summary>
    public virtual int GetMergeResultTier()
    {
        return tier + 1;
    }

    // =====================================================================
    // TEACHING: OnMerge() -- THE SESSION 4 OVERRIDE TARGET
    //
    // This virtual method is called on BOTH fruits right before they merge.
    // The default implementation just logs a message.
    //
    // In Session 4, you will OVERRIDE this method in your derived classes
    // to add custom effects: color flashes, sounds, camera shakes, particles,
    // funny messages -- anything you want!
    //
    // Because OnMerge() is virtual, the merge detection code (below) can call
    // OnMerge() on any Fruit reference, and the DERIVED version will run.
    // This is polymorphism: same method call, different behavior per type.
    //
    // Example override in a derived class:
    //   public override void OnMerge()
    //   {
    //       Debug.Log("Two cherries are merging! Pop!");
    //       // Add your custom effects here
    //   }
    // =====================================================================

    /// <summary>
    /// Called on both fruits when they merge. Override in derived classes
    /// to add custom merge effects (Session 4).
    /// </summary>
    public virtual void OnMerge()
    {
        Debug.Log($"{fruitName} is merging!");
    }


    // ============================================
    // MERGE STATE
    // ============================================

    /// <summary>
    /// Returns true if this fruit has already been claimed for a merge this frame.
    /// Prevents the same fruit from being merged twice simultaneously.
    /// </summary>
    public bool HasMerged()
    {
        return hasMerged;
    }

    /// <summary>
    /// Marks this fruit as claimed for a merge. Called during collision detection.
    /// </summary>
    public void SetMerged()
    {
        hasMerged = true;
    }


    // ============================================
    // COLLISION AND MERGE DETECTION (PRE-BUILT)
    // ============================================

    // =====================================================================
    // TEACHING: MERGE DETECTION -- POLYMORPHISM IN ACTION
    //
    // This method is called by Unity's physics system when two NON-TRIGGER
    // colliders touch. We use OnCollisionEnter2D (not OnTriggerEnter2D)
    // because fruits need real physics to stack on top of each other.
    //
    // CRITICAL INSIGHT: This merge logic is written ONCE in the base class,
    // but it works for ALL 11 fruit types. Cherry, Grape, Watermelon --
    // they ALL inherit this exact code. That's the power of inheritance.
    //
    // POLYMORPHISM BREAKDOWN (every line marked with [POLY] uses it):
    //   GetComponent<Fruit>()      [POLY] Finds Cherry, Grape, ANY derived type
    //   GetTier()                  [POLY] Returns the DERIVED class's tier value
    //   otherFruit.GetTier()       [POLY] Returns the OTHER fruit's derived tier
    //   GetMergeResultTier()       [POLY] Returns tier+1, or -1 for Watermelon
    //   OnMerge()                  [POLY] Calls the DERIVED class's custom effects
    //   otherFruit.OnMerge()       [POLY] Calls the OTHER fruit's custom effects
    // =====================================================================
    private void OnCollisionEnter2D(Collision2D other)
    {
        // [POLY] GetComponent<Fruit>() finds ANY derived type through the base class.
        // If the other object has a Cherry script, this still finds it because
        // Cherry inherits from Fruit. Same for Grape, Orange, Watermelon, etc.
        Fruit otherFruit = other.gameObject.GetComponent<Fruit>();

        // Safety check: did the other object actually have a Fruit component?
        if (otherFruit == null) return;

        // Prevent double-merging: if either fruit is already part of a merge, skip
        if (hasMerged || otherFruit.HasMerged()) return;

        // [POLY] GetTier() calls the DERIVED version for each fruit.
        // If 'this' is a Cherry, GetTier() returns 0.
        // If 'otherFruit' is also a Cherry, otherFruit.GetTier() also returns 0.
        // Same tier = they can merge!
        if (GetTier() == otherFruit.GetTier())
        {
            // [POLY] GetMergeResultTier() returns tier + 1 for most fruits.
            // Watermelon overrides it to return -1 (cannot merge further).
            if (GetMergeResultTier() != -1)
            {
                // Mark both fruits as merged to prevent other collisions
                // from trying to merge them again this frame
                SetMerged();
                otherFruit.SetMerged();

                // [POLY] OnMerge() calls the DERIVED version for each fruit.
                // If this is a Cherry, Cherry's OnMerge() runs.
                // If this is an Orange, Orange's OnMerge() runs.
                // Each fruit can have different merge effects! (Session 4)
                OnMerge();
                otherFruit.OnMerge();

                // Delegate to GameManager to handle the actual merge:
                // destroy both fruits and spawn the next tier
                GameManager gm = FindFirstObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.MergeFruits(this, otherFruit);
                }
            }
        }
    }


    // ============================================
    // PHYSICS HELPERS (Used by DropController)
    // ============================================

    /// <summary>
    /// Enables or disables physics simulation on this fruit.
    /// When disabled, the fruit freezes in place (useful while aiming).
    /// </summary>
    public void SetPhysicsEnabled(bool enabled)
    {
        if (rb != null)
        {
            rb.simulated = enabled;
        }
    }

    /// <summary>
    /// Toggles between Kinematic (no physics forces) and Dynamic (full physics).
    /// Kinematic = fruit stays where you put it (aiming mode).
    /// Dynamic = fruit falls with gravity and collides (dropped mode).
    /// </summary>
    public void SetKinematic(bool isKinematic)
    {
        if (rb != null)
        {
            rb.bodyType = isKinematic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;

            // When switching to kinematic, zero out velocity so the fruit
            // doesn't carry momentum from previous physics interactions
            if (isKinematic)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}
