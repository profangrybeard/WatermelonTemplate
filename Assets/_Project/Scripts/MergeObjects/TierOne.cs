/*
 * GAME 220: Merge Template
 * Session 1: TierOne (COMPLETE REFERENCE #2)
 *
 * TEACHING FOCUS:
 * - Second example of a derived class following TierZero's pattern
 * - Same structure, different values
 * - Reinforces the override + base.Awake() pattern
 *
 * WHAT'S DIFFERENT FROM TIERZERO:
 * - tier = 1 (second in the merge chain)
 * - Slightly larger (0.65 vs 0.50)
 * - Worth more points (3 vs 1)
 * - Different color
 *
 * STUDENT TASKS:
 * - Session 1: Compare this with TierZero.cs -- notice the pattern
 * - Session 2: Use TierZero and TierOne as references for your own classes
 */

using UnityEngine;

// TEACHING: Same pattern as TierZero. Notice how the STRUCTURE is identical --
// only the VALUES change. This is the pattern you'll follow for every class.
public class TierOne : MergeObject
{
    protected override void Awake()
    {
        // TierOne is the second object in the merge chain
        tier = 1;
        objectName = "TierOne";

        // Worth more points than TierZero (objects get more valuable as tier increases)
        pointValue = 3;

        // Slightly larger than TierZero
        objectSize = 0.65f;

        // Blue color
        objectColor = new Color(0.20f, 0.45f, 0.85f);

        // Always call base.Awake() to cache component references
        base.Awake();
    }
}
