/*
 * GAME 220: Merge Template -- TierZero (REFERENCE: study this first)
 * Copy this pattern for every class you make: override Awake(), set values, call base.Awake().
 * Concepts: Docs/INHERITANCE_CONCEPTS.md -- derived class, override, base.Method()
 */

using UnityEngine;

public class TierZero : MergeObject   // inherits every field and method in MergeObject.cs
{
    // override: replaces MergeObject's virtual Awake()
    protected override void Awake()
    {
        tier = 0;                                     // first in the merge chain
        objectName = "TierZero";
        pointValue = 1;
        objectSize = 0.5f;                            // smallest
        objectColor = new Color(0.85f, 0.12f, 0.15f); // red

        base.Awake();  // REQUIRED: runs MergeObject's setup (Rigidbody2D + SpriteRenderer refs)
    }

    // SESSION 4: uncomment for a custom merge effect
    // public override void OnMerge()
    // {
    //     Debug.Log("Pop! Two TierZeros merged!");
    // }
}
