/*
 * GAME 220: Merge Template -- TierTwo (REFERENCE #3)
 * Same structure as TierZero.cs; only the values change.
 */

using UnityEngine;

public class TierTwo : MergeObject
{
    protected override void Awake()
    {
        tier = 2;
        objectName = "TierTwo";
        pointValue = 6;
        objectSize = 0.8f;
        objectColor = new Color(0.20f, 0.75f, 0.30f); // green

        base.Awake();
    }
}
