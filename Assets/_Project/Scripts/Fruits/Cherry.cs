/*
 * GAME 220: Watermelon Merge Template
 * Session 1: Cherry (COMPLETE REFERENCE -- Study This First!)
 *
 * TEACHING FOCUS:
 * - DERIVED CLASS: Cherry INHERITS from Fruit using the : syntax
 * - OVERRIDE: Cherry provides its own values by overriding Awake()
 * - base.Awake(): Calling the parent's method to preserve shared setup
 *
 * WHAT CHERRY GETS FOR FREE FROM FRUIT (through inheritance):
 * - tier, fruitName, pointValue, fruitSize, fruitColor fields
 * - GetTier(), GetPointValue(), GetFruitName() methods
 * - GetMergeResultTier(), OnMerge() methods
 * - OnCollisionEnter2D() merge detection
 * - SetPhysicsEnabled(), SetKinematic() helpers
 * - ApplyFruitProperties() auto-applies values to Unity components
 *
 * WHAT CHERRY CUSTOMIZES:
 * - Overrides Awake() to set its specific values (tier 0, small size, red color)
 *
 * STUDENTS: This is your PRIMARY REFERENCE when creating other fruit classes.
 * Copy this pattern exactly, changing only the values.
 *
 * STUDENT TASKS:
 * - Session 1: READ this file, understand every line
 * - Session 2: Use this as your template for Orange, Dekopon, and Apple
 * - Session 4: Optionally add an OnMerge() override for custom effects
 */

using UnityEngine;

// =====================================================================
// TEACHING: DERIVED CLASS (Complete Example)
//
// The colon in "Cherry : Fruit" means "Cherry inherits from Fruit."
// Cherry automatically gets ALL the fields and methods defined in Fruit.cs.
// Cherry does NOT need to declare tier, fruitName, pointValue, etc. --
// those already exist because Fruit declared them as 'protected'.
//
// Cherry ONLY needs to:
// 1. Override Awake() to set its specific values
// 2. Call base.Awake() to run the parent's setup code
//
// That's it! Everything else (merge detection, physics, rendering)
// is inherited from Fruit and works automatically.
// =====================================================================
public class Cherry : Fruit
{
    // =====================================================================
    // TEACHING: OVERRIDE
    //
    // The 'override' keyword means "I am replacing the parent's version
    // of this method with my own version."
    //
    // Fruit.Awake() is marked 'virtual', which means derived classes
    // are ALLOWED to override it. Cherry takes advantage of this to
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
        // These fields (tier, fruitName, pointValue, fruitSize, fruitColor)
        // are declared in Fruit.cs as 'protected'. Because Cherry inherits
        // from Fruit, Cherry can access and set them directly.
        //
        // We set these values first, then call base.Awake() to cache
        // component references. Later, Start() calls ApplyFruitProperties()
        // which reads these values and applies them to the Unity components
        // (scale, color).
        // =====================================================================

        // Cherry is the first fruit in the merge chain (tier 0)
        tier = 0;

        // Display name for UI and debug messages
        fruitName = "Cherry";

        // Cherry is worth 1 point when merged (small fruit = small points)
        pointValue = 1;

        // Cherry is the smallest fruit (0.5x scale)
        fruitSize = 0.5f;

        // Bright red color
        fruitColor = new Color(0.85f, 0.12f, 0.15f);

        // =====================================================================
        // TEACHING: base.Awake()
        //
        // 'base' refers to the PARENT class (Fruit). Calling base.Awake()
        // runs Fruit's version of Awake(), which caches the component
        // references (Rigidbody2D, CircleCollider2D, SpriteRenderer).
        //
        // If you FORGET to call base.Awake(), the component references
        // will be null, and the fruit won't render or have physics!
        //
        // RULE: Always call base.Awake() at the end of your derived Awake().
        // =====================================================================
        base.Awake();
    }

    // =====================================================================
    // TEACHING: OnMerge() OVERRIDE (Session 4 Preview)
    //
    // Uncomment the method below to give Cherry custom merge effects.
    // The base class OnMerge() just logs a message. You can override it
    // to do anything: play sounds, spawn particles, change colors, etc.
    //
    // This is what you'll do in Session 4 for all your fruit classes!
    //
    // public override void OnMerge()
    // {
    //     Debug.Log("Pop! Two cherries merged!");
    //     // Add your custom cherry merge effects here
    // }
    // =====================================================================
}
