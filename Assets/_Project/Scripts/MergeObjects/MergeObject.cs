/*
 * GAME 220: Merge Template
 * Sessions 1-5: MergeObject Base Class
 *
 * TEACHING FOCUS:
 * - INHERITANCE: This is the BASE CLASS that all merge objects inherit from
 * - PROTECTED FIELDS: Fields marked 'protected' are accessible by this class AND its children
 * - VIRTUAL METHODS: Methods marked 'virtual' CAN be overridden by derived classes
 * - POLYMORPHISM: Code that references MergeObject can work with ANY derived type
 *
 * This script defines the shared behavior for ALL merge object types in the game.
 * TierZero, TierOne, TierTwo, and every object students create INHERITS from this class.
 * That means they automatically get all the fields, methods, and logic defined here.
 *
 * WHAT THIS BASE CLASS PROVIDES:
 * - Protected fields for tier, size, color, points, and name
 * - Virtual methods that derived classes can override to customize behavior
 * - Merge detection logic (OnCollisionEnter2D) that works for ALL derived types
 * - Physics helpers for the drop controller
 * - Automatic visual setup (size, color) from derived class values
 *
 * STUDENT TASKS:
 * - Session 1: Read and understand this base class
 * - Session 4: Override OnMerge() in your classes for custom effects
 * - Session 5 (STRETCH): Convert this class to abstract
 */

using UnityEngine;

// =====================================================================
// TEACHING: BASE CLASS
//
// A base class is a blueprint that other classes can INHERIT from.
// 'MergeObject' defines what ALL merge objects have in common: a tier,
// a size, a color, points, and the ability to merge.
//
// Derived classes (TierZero, TierOne, TierTwo, and your own creations)
// inherit everything here and customize it by overriding Awake() to set
// their own values.
//
// The colon syntax below means "MergeObject inherits from MonoBehaviour":
//   public class MergeObject : MonoBehaviour
//
// When TierZero says "public class TierZero : MergeObject", it means:
//   "TierZero inherits everything from MergeObject (which inherits from MonoBehaviour)"
// =====================================================================
public class MergeObject : MonoBehaviour
{
    // ============================================
    // PROTECTED FIELDS (Set by Derived Classes)
    // ============================================

    // =====================================================================
    // TEACHING: PROTECTED ACCESS MODIFIER
    //
    // 'protected' means: this class AND any class that inherits from it
    // can read and write these fields. Classes that do NOT inherit from
    // MergeObject cannot access them.
    //
    // Compare with:
    //   private   = ONLY this class can access (too restrictive for children)
    //   public    = ANY class can access (too open, no protection)
    //   protected = this class + children (just right for inheritance!)
    //
    // Each derived class sets these values in its own Awake() method.
    // For example, TierZero sets tier = 0, objectSize = 0.5f, etc.
    // =====================================================================

    protected int tier = 0;                      // Position in the merge chain (0 = first, highest = last)
    protected string objectName = "MergeObject"; // Display name for UI and debug messages
    protected int pointValue = 0;                // Score points awarded when this object is part of a merge
    protected float objectSize = 1f;             // Transform.localScale multiplier (bigger number = bigger object)
    protected Color objectColor = Color.white;   // SpriteRenderer color tint

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
    // Awake() is virtual so that derived classes (TierZero, TierOne, etc.)
    // can override it to set their own tier, size, color, and points.
    // The full startup flow is:
    //   1. Derived Awake() sets field values (tier, objectSize, etc.)
    //   2. Derived Awake() calls base.Awake() to cache component references
    //   3. Start() calls ApplyObjectProperties() to apply values to Unity components
    //
    // Compare with 'abstract' (Session 5 stretch): an abstract method
    // has NO default -- derived classes MUST override it.
    // =====================================================================

    /// <summary>
    /// Caches component references. Derived classes override this to set
    /// their specific values (tier, size, color, etc.) and then call base.Awake().
    /// </summary>
    protected virtual void Awake()
    {
        // Cache component references so we don't call GetComponent every frame
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Applies the object's visual properties after Awake has run.
    /// </summary>
    protected virtual void Start()
    {
        // Apply the values set by the derived class to the actual Unity components
        ApplyObjectProperties();
    }


    // ============================================
    // PROPERTY APPLICATION
    // ============================================

    /// <summary>
    /// Applies objectSize and objectColor to the Unity components.
    /// Called automatically in Start(). Not virtual -- derived classes
    /// should NOT override this. Instead, set the protected fields
    /// in Awake() and let this method do the rest.
    /// </summary>
    protected void ApplyObjectProperties()
    {
        // Apply scale (objectSize controls how big the object appears)
        transform.localScale = Vector3.one * objectSize;

        // Apply color to the sprite renderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = objectColor;
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
    // When code calls obj.GetTier() on a MergeObject reference, the DERIVED
    // version runs if the actual object is TierZero, TierOne, etc.
    // This is called "virtual method dispatch" or "dynamic dispatch."
    //
    // Example:
    //   MergeObject obj = someTierZero;  // Typed as MergeObject, actual object is TierZero
    //   obj.GetTier();                   // Calls TierZero's version -> returns 0
    //
    //   MergeObject obj = someTierTwo;   // Typed as MergeObject, actual object is TierTwo
    //   obj.GetTier();                   // Calls TierTwo's version -> returns 2
    // =====================================================================

    /// <summary>
    /// Returns this object's tier (position in the merge chain).
    /// </summary>
    public virtual int GetTier()
    {
        return tier;
    }

    /// <summary>
    /// Returns the score points this object is worth when merged.
    /// </summary>
    public virtual int GetPointValue()
    {
        return pointValue;
    }

    /// <summary>
    /// Returns the display name of this object (e.g., "TierZero", "TierTwo").
    /// </summary>
    public virtual string GetObjectName()
    {
        return objectName;
    }

    /// <summary>
    /// Returns the tier that results from merging two of this type.
    /// Default: tier + 1 (next in the chain).
    /// Override this to return -1 if this is the final tier (no further merge possible).
    /// </summary>
    public virtual int GetMergeResultTier()
    {
        return tier + 1;
    }

    // =====================================================================
    // TEACHING: OnMerge() -- THE SESSION 4 OVERRIDE TARGET
    //
    // This virtual method is called on BOTH objects right before they merge.
    // The default implementation just logs a message.
    //
    // In Session 4, you will OVERRIDE this method in your derived classes
    // to add custom effects: color flashes, sounds, camera shakes, particles,
    // funny messages -- anything you want!
    //
    // Because OnMerge() is virtual, the merge detection code (below) can call
    // OnMerge() on any MergeObject reference, and the DERIVED version will run.
    // This is polymorphism: same method call, different behavior per type.
    //
    // Example override in a derived class:
    //   public override void OnMerge()
    //   {
    //       Debug.Log("Two of mine are merging! Pop!");
    //       // Add your custom effects here
    //   }
    // =====================================================================

    /// <summary>
    /// Called on both objects when they merge. Override in derived classes
    /// to add custom merge effects (Session 4).
    /// </summary>
    public virtual void OnMerge()
    {
        Debug.Log($"{objectName} is merging!");
    }


    // ============================================
    // MERGE STATE
    // ============================================

    /// <summary>
    /// Returns true if this object has already been claimed for a merge this frame.
    /// Prevents the same object from being merged twice simultaneously.
    /// </summary>
    public bool HasMerged()
    {
        return hasMerged;
    }

    /// <summary>
    /// Marks this object as claimed for a merge. Called during collision detection.
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
    // because objects need real physics to stack on top of each other.
    //
    // CRITICAL INSIGHT: This merge logic is written ONCE in the base class,
    // but it works for ALL derived types. TierZero, TierTwo, and every
    // class you create -- they ALL inherit this exact code.
    // That's the power of inheritance.
    //
    // POLYMORPHISM BREAKDOWN (every line marked with [POLY] uses it):
    //   GetComponent<MergeObject>() [POLY] Finds ANY derived type
    //   GetTier()                   [POLY] Returns the DERIVED class's tier value
    //   otherObject.GetTier()       [POLY] Returns the OTHER object's derived tier
    //   GetMergeResultTier()        [POLY] Returns tier+1, or -1 for the final tier
    //   OnMerge()                   [POLY] Calls the DERIVED class's custom effects
    //   otherObject.OnMerge()       [POLY] Calls the OTHER object's custom effects
    // =====================================================================
    private void OnCollisionEnter2D(Collision2D other)
    {
        // [POLY] GetComponent<MergeObject>() finds ANY derived type through the base class.
        // If the other object has a TierZero script, this still finds it because
        // TierZero inherits from MergeObject. Same for TierOne, TierTwo, and your classes.
        MergeObject otherObject = other.gameObject.GetComponent<MergeObject>();

        // Safety check: did the other object actually have a MergeObject component?
        if (otherObject == null) return;

        // Prevent double-merging: if either object is already part of a merge, skip
        if (hasMerged || otherObject.HasMerged()) return;

        // [POLY] GetTier() calls the DERIVED version for each object.
        // If 'this' is a TierZero, GetTier() returns 0.
        // If 'otherObject' is also a TierZero, otherObject.GetTier() also returns 0.
        // Same tier = they can merge!
        if (GetTier() == otherObject.GetTier())
        {
            // [POLY] GetMergeResultTier() returns tier + 1 for most objects.
            // The final tier overrides it to return -1 (cannot merge further).
            if (GetMergeResultTier() != -1)
            {
                // Mark both objects as merged to prevent other collisions
                // from trying to merge them again this frame
                SetMerged();
                otherObject.SetMerged();

                // [POLY] OnMerge() calls the DERIVED version for each object.
                // Each class can have different merge effects! (Session 4)
                OnMerge();
                otherObject.OnMerge();

                // Delegate to GameManager to handle the actual merge:
                // destroy both objects and spawn the next tier
                GameManager gm = FindFirstObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.MergeObjects(this, otherObject);
                }
            }
        }
    }


    // ============================================
    // PHYSICS HELPERS (Used by DropController)
    // ============================================

    /// <summary>
    /// Enables or disables physics simulation on this object.
    /// When disabled, the object freezes in place (useful while aiming).
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
    /// Kinematic = object stays where you put it (aiming mode).
    /// Dynamic = object falls with gravity and collides (dropped mode).
    /// </summary>
    public void SetKinematic(bool isKinematic)
    {
        if (rb != null)
        {
            rb.bodyType = isKinematic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;

            // When switching to kinematic, zero out velocity so the object
            // doesn't carry momentum from previous physics interactions
            if (isKinematic)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}
