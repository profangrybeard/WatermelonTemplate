/*
 * GAME 220: Merge Template -- MergeObjectFactory
 * Holds one prefab per tier and creates objects by tier number.
 * Polymorphism: CreateObject() returns MergeObject, but the object is TierZero, TierOne, etc.
 * Concepts: Docs/INHERITANCE_CONCEPTS.md -- Polymorphism, example 4
 *
 * Session 2+: drag each new prefab into its slot. Expand the array for more than 5 tiers.
 */

using UnityEngine;

public class MergeObjectFactory : MonoBehaviour
{
    // Index = tier: [0] TierZero, [1] TierOne, [2] TierTwo, [3+] yours
    [Header("Object Prefabs (One Per Tier)")]
    [Tooltip("Drag each merge object prefab into the correct slot. Slot 0 = TierZero, Slot 1 = TierOne, etc.")]
    public GameObject[] objectPrefabs = new GameObject[5];


    // ---------- Factory methods ----------

    // Returns null if the tier is out of range or its slot is empty
    public MergeObject CreateObject(int tier)
    {
        if (tier < 0 || tier >= objectPrefabs.Length)
        {
            Debug.LogWarning($"MergeObjectFactory: Invalid tier {tier}! Valid range is 0-{objectPrefabs.Length - 1}. " +
                             $"Need more tiers? Expand the objectPrefabs array in the Inspector.");
            return null;
        }

        if (objectPrefabs[tier] == null)
        {
            Debug.LogWarning($"MergeObjectFactory: No prefab assigned for tier {tier}! " +
                             $"Create a MergeObject-derived class and prefab, then drag it into slot {tier} in the Inspector.");
            return null;
        }

        GameObject obj = Instantiate(objectPrefabs[tier]);

        MergeObject mergeObject = obj.GetComponent<MergeObject>(); // [POLY] finds any derived type

        if (mergeObject == null)
        {
            Debug.LogWarning($"MergeObjectFactory: Prefab for tier {tier} is missing a MergeObject-derived script! " +
                             $"Make sure the prefab has a script that inherits from MergeObject.");
            Destroy(obj);
            return null;
        }

        return mergeObject;
    }

    public MergeObject CreateObjectAtPosition(int tier, Vector3 position)
    {
        MergeObject mergeObject = CreateObject(tier);

        if (mergeObject != null)
        {
            mergeObject.transform.position = position;
        }

        return mergeObject;
    }


    // ---------- Helpers ----------

    // Highest tier with a prefab assigned (-1 if none)
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

    public bool HasPrefabForTier(int tier)
    {
        if (tier < 0 || tier >= objectPrefabs.Length) return false;
        return objectPrefabs[tier] != null;
    }
}
