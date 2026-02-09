/*
 * GAME 220: Merge Template
 * Session 1: TierZero (COMPLETE REFERENCE -- Study This First!)
 *
 * TEACHING FOCUS:
 * - DERIVED CLASS: TierZero INHERITS from MergeObject using the : syntax
 * - OVERRIDE: TierZero provides its own values by overriding Awake()
 * - base.Awake(): Calling the parent's method to preserve shared setup
 *
 * WHAT TIERZERO GETS FOR FREE FROM MERGEOBJECT (through inheritance):
 * - tier, objectName, pointValue, objectSize, objectColor fields
 * - GetTier(), GetPointValue(), GetObjectName() methods
 * - GetMergeResultTier(), OnMerge() methods
 * - OnCollisionEnter2D() merge detection
 * - SetPhysicsEnabled(), SetKinematic() helpers
 * - ApplyObjectProperties() auto-applies values to Unity components
 *
 * WHAT TIERZERO CUSTOMIZES:
 * - Overrides Awake() to set its specific values (tier 0, small size, red color)
 *
 * STUDENTS: This is your PRIMARY REFERENCE when creating your own classes.
 * Copy this pattern exactly, changing only the values.
 *
 * STUDENT TASKS:
 * - Session 1: READ this file, understand every line
 * - Session 2: Use this as your template for your own merge objects
 * - Session 4: Optionally add an OnMerge() override for custom effects
 */

using UnityEngine;

// =====================================================================
// TEACHING: DERIVED CLASS (Complete Example)
//
// The colon in "TierZero : MergeObject" means "TierZero inherits from MergeObject."
// TierZero automatically gets ALL the fields and methods defined in MergeObject.cs.
// TierZero does NOT need to declare tier, objectName, pointValue, etc. --
// those already exist because MergeObject declared them as 'protected'.
//
// TierZero ONLY needs to:
// 1. Override Awake() to set its specific values
// 2. Call base.Awake() to run the parent's setup code
//
// That's it! Everything else (merge detection, physics, rendering)
// is inherited from MergeObject and works automatically.
// =====================================================================
public class TierZero : MergeObject
{
    // =====================================================================
    // TEACHING: OVERRIDE
    //
    // The 'override' keyword means "I am replacing the parent's version
    // of this method with my own version."
    //
    // MergeObject.Awake() is marked 'virtual', which means derived classes
    // are ALLOWED to override it. TierZero takes advantage of this to
    // set its own specific values.
    //
    // 'protected' matches the access modifier of the parent's Awake().
    // You cannot change the access level when overriding.
    // =====================================================================
    protected override void Awake()
    {
        // =====================================================================
        // TEACHING: SETTING INHERITED FIELDS
        //
        // These fields (tier, objectName, pointValue, objectSize, objectColor)
        // are declared in MergeObject.cs as 'protected'. Because TierZero
        // inherits from MergeObject, TierZero can access and set them directly.
        //
        // We set these values first, then call base.Awake() to cache
        // component references. Later, Start() calls ApplyObjectProperties()
        // which reads these values and applies them to the Unity components
        // (scale, color).
        // =====================================================================

        // TierZero is the first object in the merge chain (tier 0)
        tier = 0;

        // Display name for UI and debug messages
        objectName = "TierZero";

        // TierZero is worth 1 point when merged (small object = small points)
        pointValue = 1;

        // TierZero is the smallest object (0.5x scale)
        objectSize = 0.5f;

        // Red color
        objectColor = new Color(0.85f, 0.12f, 0.15f);

        // =====================================================================
        // TEACHING: base.Awake()
        //
        // 'base' refers to the PARENT class (MergeObject). Calling base.Awake()
        // runs MergeObject's version of Awake(), which caches the component
        // references (Rigidbody2D, CircleCollider2D, SpriteRenderer).
        //
        // If you FORGET to call base.Awake(), the component references
        // will be null, and the object won't render or have physics!
        //
        // RULE: Always call base.Awake() at the end of your derived Awake().
        // =====================================================================
        base.Awake();
    }

    // =====================================================================
    // TEACHING: OnMerge() OVERRIDE (Session 4 Preview)
    //
    // Uncomment the method below to give TierZero custom merge effects.
    // The base class OnMerge() just logs a message. You can override it
    // to do anything: play sounds, spawn particles, change colors, etc.
    //
    // This is what you'll do in Session 4 for all your classes!
    //
    // public override void OnMerge()
    // {
    //     Debug.Log("Pop! Two TierZeros merged!");
    //     // Add your custom merge effects here
    // }
    // =====================================================================
}
