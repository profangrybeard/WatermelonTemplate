/*
 * GAME 220: Merge Template
 * Sessions 1-5: MergeObject Factory
 *
 * TEACHING FOCUS:
 * - POLYMORPHISM: CreateObject() returns MergeObject (base class) but the actual object
 *   is TierZero, TierOne, TierTwo, or whatever derived type the prefab contains
 * - ARRAY OF PREFABS: Each array slot corresponds to a tier (0=TierZero, 1=TierOne, etc.)
 * - FACTORY PATTERN: One method creates ANY object type based on tier number
 * - GetComponent<MergeObject>(): Finds ANY derived script through the base class reference
 *
 * This script manages the creation of merge objects. It holds an array of prefabs
 * (one per tier) and creates objects on demand via CreateObject(tier).
 *
 * STUDENT TASKS:
 * - Session 2+: Assign your new prefabs to slots in the Inspector
 * - Expand the array size if you create more than 5 tiers
 */

using UnityEngine;

public class MergeObjectFactory : MonoBehaviour
{
    // ============================================
    // OBJECT PREFAB ARRAY
    // ============================================

    // TEACHING: ARRAY INDEXED BY TIER
    //   objectPrefabs[0] = TierZero, [1] = TierOne, [2] = TierTwo, [3+] = yours
    // To create an object of any tier, just index into the array.
    // No switch statement. No if/else chain. The array IS the mapping.
    // Need more tiers? Expand the array in the Inspector and create more classes!

    [Header("Object Prefabs (One Per Tier)")]
    [Tooltip("Drag each merge object prefab into the correct slot. Slot 0 = TierZero, Slot 1 = TierOne, etc.")]
    public GameObject[] objectPrefabs = new GameObject[5];


    // ============================================
    // FACTORY METHODS
    // ============================================

    // TEACHING: POLYMORPHIC RETURN TYPE
    // CreateObject() returns MergeObject (base class), but the actual object
    // is TierZero, TierOne, etc. (derived class). The caller doesn't need to
    // know which type -- polymorphism ensures the correct methods run.
    //   MergeObject obj = factory.CreateObject(0);  // Creates a TierZero
    //   obj.GetTier();       // Returns 0 (TierZero's version runs)
    //   obj.GetObjectName(); // Returns "TierZero" (TierZero's version runs)

    /// <summary>
    /// Creates a merge object of the specified tier and returns it as a MergeObject reference.
    /// Returns null if the tier is invalid or the prefab slot is empty.
    /// </summary>
    public MergeObject CreateObject(int tier)
    {
        // Validate the tier is within range
        if (tier < 0 || tier >= objectPrefabs.Length)
        {
            Debug.LogWarning($"MergeObjectFactory: Invalid tier {tier}! Valid range is 0-{objectPrefabs.Length - 1}. " +
                             $"Need more tiers? Expand the objectPrefabs array in the Inspector.");
            return null;
        }

        // Check if a prefab has been assigned for this tier
        if (objectPrefabs[tier] == null)
        {
            Debug.LogWarning($"MergeObjectFactory: No prefab assigned for tier {tier}! " +
                             $"Create a MergeObject-derived class and prefab, then drag it into slot {tier} in the Inspector.");
            return null;
        }

        // Create a copy of the prefab
        GameObject obj = Instantiate(objectPrefabs[tier]);

        // TEACHING: GetComponent<MergeObject>() finds ANY derived type (TierZero,
        // TierOne, etc.) because they all inherit from MergeObject.
        // The returned reference is typed as MergeObject, but the actual object
        // retains its derived type. That's polymorphism through GetComponent.
        MergeObject mergeObject = obj.GetComponent<MergeObject>();

        if (mergeObject == null)
        {
            Debug.LogWarning($"MergeObjectFactory: Prefab for tier {tier} is missing a MergeObject-derived script! " +
                             $"Make sure the prefab has a script that inherits from MergeObject.");
            Destroy(obj);
            return null;
        }

        return mergeObject;
    }

    /// <summary>
    /// Creates a merge object of the specified tier at the given position.
    /// Convenience wrapper around CreateObject(tier).
    /// </summary>
    public MergeObject CreateObjectAtPosition(int tier, Vector3 position)
    {
        MergeObject mergeObject = CreateObject(tier);

        if (mergeObject != null)
        {
            mergeObject.transform.position = position;
        }

        return mergeObject;
    }


    // ============================================
    // HELPER METHODS
    // ============================================

    /// <summary>
    /// Returns the highest tier that has a prefab assigned.
    /// Useful for determining the current merge chain length.
    /// </summary>
    public int GetMaxTier()
    {
        int maxTier = -1;

        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i] != null)
            {
                maxTier = i;
            }
        }

        return maxTier;
    }

    /// <summary>
    /// Returns true if a prefab has been assigned for the given tier.
    /// Used by the merge system to check if the next tier exists before merging.
    /// </summary>
    public bool HasPrefabForTier(int tier)
    {
        if (tier < 0 || tier >= objectPrefabs.Length) return false;
        return objectPrefabs[tier] != null;
    }
}
