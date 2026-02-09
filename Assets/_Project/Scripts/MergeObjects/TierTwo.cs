/*
 * GAME 220: Merge Template
 * Session 1: TierTwo (COMPLETE REFERENCE #3)
 *
 * TEACHING FOCUS:
 * - Third example of the derived class pattern
 * - By now the pattern should be clear: override Awake(), set values, call base.Awake()
 *
 * STUDENT TASKS:
 * - Session 1: You've now seen THREE examples (TierZero, TierOne, TierTwo)
 * - Session 2: Create your own merge objects following this same pattern
 */

using UnityEngine;

// TEACHING: Third derived class. Same pattern. Different values.
// If you understand TierZero, TierOne, and TierTwo, you can write any merge object.
public class TierTwo : MergeObject
{
    protected override void Awake()
    {
        tier = 2;
        objectName = "TierTwo";
        pointValue = 6;
        objectSize = 0.8f;
        objectColor = new Color(0.20f, 0.75f, 0.30f); // Green

        base.Awake();
    }
}
