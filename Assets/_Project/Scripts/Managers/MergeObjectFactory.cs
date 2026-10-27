/*
 * MergeObjectFactory -- builds merge objects. Give it a tier number, get back an object.
 *
 * It holds one prefab per tier in an array. The array index IS the tier, so there's no
 * if/else or switch: adding a tier means adding a class, a prefab, and filling a slot.
 *
 * Read alongside: Docs/INHERITANCE_CONCEPTS.md (Polymorphism, example 4)
 * Session 2+: drag each new prefab into its slot. Expand the array for more than 5 tiers.
 */

using UnityEngine;

public class MergeObjectFactory : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Fill these slots in the Inspector
    // ─────────────────────────────────────────────────────────────────────────
    // ARRAYS: a fixed number of slots, each found by its index.
    //   objectPrefabs[0] = TierZero, [1] = TierOne, [2] = TierTwo, [3] and up = yours
    //   objectPrefabs.Length = how many slots there are (empty ones included)
    // Compare GameManager's List, which grows and shrinks while the game runs.

    // Why an array here but a List in GameManager?
    [Header("Object Prefabs (One Per Tier)")]
    [Tooltip("Drag each merge object prefab into the correct slot. Slot 0 = TierZero, Slot 1 = TierOne, etc.")]
    public GameObject[] objectPrefabs = new GameObject[5];

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────
    // CreateObject's return type is MergeObject, the BASE class. What comes back is really
    // a TierZero, TierOne, or one of yours, and the caller never needs to know which. [POLY]
    // It returns null when it can't build one. Every caller has to check for that.

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

        // Why does GetComponent<MergeObject>() find a TierZero script?
        MergeObject mergeObject = obj.GetComponent<MergeObject>(); // [POLY]

        if (mergeObject == null)
        {
            Debug.LogWarning($"MergeObjectFactory: Prefab for tier {tier} is missing a MergeObject-derived script! " +
                             $"Make sure the prefab has a script that inherits from MergeObject.");
            Destroy(obj);
            return null;
        }

        return mergeObject;
    }

    // A small method that reuses a bigger one: build it, then move it.
    public MergeObject CreateObjectAtPosition(int tier, Vector3 position)
    {
        MergeObject mergeObject = CreateObject(tier);

        if (mergeObject != null)
        {
            mergeObject.transform.position = position;
        }

        return mergeObject;
    }

    // Highest tier with a prefab in its slot, or -1 if every slot is empty.
    public int GetMaxTier()
    {
        int maxTier = -1;

        // Why a for loop here, when GameManager uses foreach?
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i] != null)
            {
                maxTier = i;
            }
        }

        return maxTier;
    }

    // Returns true or false. A bool return is a yes/no answer.
    public bool HasPrefabForTier(int tier)
    {
        if (tier < 0 || tier >= objectPrefabs.Length) return false;
        return objectPrefabs[tier] != null;
    }
}
