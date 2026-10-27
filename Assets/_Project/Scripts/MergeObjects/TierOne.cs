/*
 * TierOne -- a second DERIVED CLASS. Put it side by side with TierZero.cs.
 * The structure is identical; only the values change. That's the pattern you'll repeat
 * for every merge object you write.
 */

using UnityEngine;

public class TierOne : MergeObject
{
    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES
    // ─────────────────────────────────────────────────────────────────────────

    // Same override as TierZero: set this tier's values, then extend with base.Awake().
    protected override void Awake()
    {
        tier = 1;
        objectName = "TierOne";

        // Two TierZeros (1 point each) make one TierOne. Why is TierOne worth 3, not 2?
        pointValue = 3;

        objectSize = 0.65f;
        objectColor = new Color(0.20f, 0.45f, 0.85f); // blue

        base.Awake();
    }
}
