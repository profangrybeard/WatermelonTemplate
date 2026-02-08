/*
 * GAME 220: Watermelon Merge Template
 * Sessions 1-5: Fruit Factory
 *
 * TEACHING FOCUS:
 * - POLYMORPHISM: CreateFruit() returns Fruit (base class) but the actual object
 *   is Cherry, Strawberry, Grape, or whatever derived type the prefab contains
 * - ARRAY OF PREFABS: Each array slot corresponds to a tier (0=Cherry, 1=Strawberry, etc.)
 * - FACTORY PATTERN: One method creates ANY fruit type based on tier number
 * - GetComponent<Fruit>(): Finds ANY derived script through the base class reference
 *
 * This script manages the creation of fruit objects. It holds an array of prefabs
 * (one per tier) and creates fruits on demand via CreateFruit(tier).
 *
 * STUDENT TASKS:
 * - Session 2: Assign Orange, Dekopon, Apple prefabs to slots 3-5 in the Inspector
 * - Session 3: Assign Pear through Watermelon prefabs to slots 6-10 in the Inspector
 */

using UnityEngine;

public class FruitFactory : MonoBehaviour
{
    // ============================================
    // FRUIT PREFAB ARRAY
    // ============================================

    // =====================================================================
    // TEACHING: ARRAY INDEXED BY TIER
    //
    // Each slot in this array holds the prefab for one fruit tier:
    //   fruitPrefabs[0] = Cherry prefab
    //   fruitPrefabs[1] = Strawberry prefab
    //   fruitPrefabs[2] = Grape prefab
    //   fruitPrefabs[3] = Orange prefab       (assign in Session 2)
    //   fruitPrefabs[4] = Dekopon prefab      (assign in Session 2)
    //   ...
    //   fruitPrefabs[10] = Watermelon prefab  (assign in Session 3)
    //
    // This is a simple but powerful design: to create a fruit of any tier,
    // just index into the array. No switch statement. No if/else chain.
    // The array IS the mapping from tier number to prefab.
    // =====================================================================

    [Header("Fruit Prefabs (One Per Tier)")]
    [Tooltip("Drag each fruit prefab into the correct slot. Slot 0 = Cherry, Slot 1 = Strawberry, etc.")]
    public GameObject[] fruitPrefabs = new GameObject[11];


    // ============================================
    // FACTORY METHODS
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHIC RETURN TYPE
    //
    // CreateFruit() returns 'Fruit' (the base class), but the actual object
    // it creates is Cherry, Grape, Orange, etc. (a derived class).
    //
    // The caller doesn't need to know which specific fruit type was created.
    // It just works with the Fruit reference, and polymorphism ensures the
    // correct derived methods run when called.
    //
    // Example:
    //   Fruit myFruit = fruitFactory.CreateFruit(0);  // Creates a Cherry
    //   myFruit.GetTier();   // Returns 0 (Cherry's version runs)
    //   myFruit.GetFruitName();  // Returns "Cherry" (Cherry's version runs)
    //
    //   Fruit myFruit = fruitFactory.CreateFruit(2);  // Creates a Grape
    //   myFruit.GetTier();   // Returns 2 (Grape's version runs)
    //   myFruit.GetFruitName();  // Returns "Grape" (Grape's version runs)
    //
    // Same variable type (Fruit), same method calls, different results.
    // That's polymorphism.
    // =====================================================================

    /// <summary>
    /// Creates a fruit of the specified tier and returns it as a Fruit reference.
    /// Returns null if the tier is invalid or the prefab slot is empty.
    /// </summary>
    public Fruit CreateFruit(int tier)
    {
        // Validate the tier is within range
        if (tier < 0 || tier >= fruitPrefabs.Length)
        {
            Debug.LogWarning($"FruitFactory: Invalid tier {tier}! Valid range is 0-{fruitPrefabs.Length - 1}.");
            return null;
        }

        // Check if a prefab has been assigned for this tier
        if (fruitPrefabs[tier] == null)
        {
            Debug.LogWarning($"FruitFactory: No prefab assigned for tier {tier}! " +
                             $"Create the fruit class and prefab, then drag it into slot {tier} in the Inspector.");
            return null;
        }

        // Create a copy of the prefab
        GameObject fruitObj = Instantiate(fruitPrefabs[tier]);

        // =====================================================================
        // TEACHING: GetComponent<Fruit>() WITH POLYMORPHISM
        //
        // The prefab has a specific script: Cherry, Grape, Orange, etc.
        // But GetComponent<Fruit>() FINDS IT because they all inherit from Fruit.
        //
        // The returned reference is typed as Fruit (base class), but the actual
        // object retains its derived type. When you call methods on it, the
        // derived version runs. This is polymorphism through GetComponent.
        // =====================================================================
        Fruit fruit = fruitObj.GetComponent<Fruit>();

        if (fruit == null)
        {
            Debug.LogWarning($"FruitFactory: Prefab for tier {tier} is missing a Fruit-derived script! " +
                             $"Make sure the prefab has a script that inherits from Fruit (e.g., Cherry, Grape, etc.).");
            Destroy(fruitObj);
            return null;
        }

        return fruit;
    }

    /// <summary>
    /// Creates a fruit of the specified tier at the given position.
    /// Convenience wrapper around CreateFruit(tier).
    /// </summary>
    public Fruit CreateFruitAtPosition(int tier, Vector3 position)
    {
        Fruit fruit = CreateFruit(tier);

        if (fruit != null)
        {
            fruit.transform.position = position;
        }

        return fruit;
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

        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i] != null)
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
        if (tier < 0 || tier >= fruitPrefabs.Length) return false;
        return fruitPrefabs[tier] != null;
    }
}
