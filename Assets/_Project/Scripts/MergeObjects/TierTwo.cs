/*
 * TierTwo -- the third DERIVED CLASS, and right now the last tier in the chain.
 * If you understand TierZero, TierOne and TierTwo, you can write any merge object.
 */

using UnityEngine;

public class TierTwo : MergeObject
{
    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES
    // ─────────────────────────────────────────────────────────────────────────

    protected override void Awake()
    {
        tier = 2;
        objectName = "TierTwo";
        pointValue = 6;
        objectSize = 0.8f;
        objectColor = new Color(0.20f, 0.75f, 0.30f); // green

        base.Awake();
    }

    // Two TierTwos currently vanish with a "No prefab for tier 3" warning.
    // TierTwo doesn't override GetMergeResultTier(), so it asks for tier 3. What are the two
    // ways to fix that: one adds a new class, the other adds a method here?
}
