/*
 * TierZero -- a DERIVED CLASS, and your reference: study this first.
 * Every merge object you write follows this exact pattern.
 *
 * TierZero only sets its own values. Merging, physics, size and color all come
 * from MergeObject.cs, because TierZero inherits them.
 *
 * Read alongside: Docs/INHERITANCE_CONCEPTS.md (Derived Class, override, base.Method())
 */

using UnityEngine;

// The colon means "TierZero inherits from MergeObject": TierZero IS a MergeObject.
public class TierZero : MergeObject
{
    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES
    // ─────────────────────────────────────────────────────────────────────────
    // This override EXTENDS MergeObject's Awake(): set this tier's values first,
    // then call base.Awake() so the parent's setup still runs.

    protected override void Awake()
    {
        // These fields are declared in MergeObject.cs, not here. Why can TierZero set them?
        tier = 0;                                     // first in the merge chain
        objectName = "TierZero";
        pointValue = 1;
        objectSize = 0.5f;                            // the smallest
        objectColor = new Color(0.85f, 0.12f, 0.15f); // red

        // Comment this line out and press Play. What looks different about TierZero, and why?
        base.Awake();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // SESSION 4 - Uncomment to give TierZero its own merge effect
    // ─────────────────────────────────────────────────────────────────────────
    // This one REPLACES the parent's OnMerge() instead of extending it.
    // How would you change it to run the parent's message too?

    // public override void OnMerge()
    // {
    //     Debug.Log("Pop! Two TierZeros merged!");
    // }
}
