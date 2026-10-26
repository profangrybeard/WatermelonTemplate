/*
 * GAME 220: Merge Template -- TierOne (REFERENCE #2)
 * Same structure as TierZero.cs; only the values change.
 */

using UnityEngine;

public class TierOne : MergeObject
{
    protected override void Awake()
    {
        tier = 1;
        objectName = "TierOne";
        pointValue = 3;
        objectSize = 0.65f;
        objectColor = new Color(0.20f, 0.45f, 0.85f); // blue

        base.Awake();
    }
}
