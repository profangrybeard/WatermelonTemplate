/*
 * GAME 220: Watermelon Merge Template
 * Sessions 1-5: Game Manager
 *
 * TEACHING FOCUS:
 * - POLYMORPHIC COLLECTION: List<Fruit> holds Cherry, Grape, Orange -- ANY derived type
 * - POLYMORPHIC PARAMETERS: MergeFruits(Fruit a, Fruit b) accepts any two fruit types
 * - VIRTUAL METHOD DISPATCH: Calling methods on Fruit references runs the derived version
 * - foreach LOOPS over polymorphic collections
 *
 * This script manages the overall game state:
 * - Tracks all active fruits in a List<Fruit> (polymorphic collection)
 * - Executes merges (destroy two fruits, spawn the next tier)
 * - Detects game over condition
 * - Provides polymorphic query methods for inspecting active fruits
 *
 * STUDENT TASKS:
 * - Session 1: Read and understand how List<Fruit> works with different fruit types
 * - Session 3: Examine the polymorphic query methods during the polymorphism deep dive
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ============================================
    // INSPECTOR REFERENCES
    // ============================================

    [Header("References")]
    [Tooltip("Drag the FruitFactory component from the scene")]
    public FruitFactory fruitFactory;

    [Tooltip("Drag the DropController component from the scene")]
    public DropController dropController;

    [Tooltip("Drag the ScoreManager component from the scene")]
    public ScoreManager scoreManager;

    [Header("Game Over Settings")]
    [Tooltip("Y position above which fruits trigger game over")]
    public float gameOverLineY = 4.5f;

    [Tooltip("Seconds a fruit must be above the line before game over triggers")]
    public float gameOverDelay = 2f;


    // ============================================
    // POLYMORPHIC COLLECTION
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHIC COLLECTION (List<Fruit>)
    //
    // List<Fruit> can hold Cherry, Strawberry, Grape, Orange, Watermelon --
    // ANY class that inherits from Fruit.
    //
    // When we loop through this list and call methods like GetTier(),
    // GetPointValue(), or GetFruitName(), the CORRECT DERIVED VERSION
    // runs for each fruit. A Cherry returns tier 0, a Grape returns tier 2,
    // an Orange returns tier 3, etc.
    //
    // We never need to check "is this a Cherry? is this a Grape?"
    // We just call the method, and polymorphism handles the rest.
    //
    // This is one of the most powerful concepts in object-oriented programming:
    // write code that works with the BASE type, and it automatically works
    // with ALL derived types -- even ones that don't exist yet!
    // =====================================================================
    private List<Fruit> activeFruits = new List<Fruit>();


    // ============================================
    // PRIVATE STATE
    // ============================================

    private bool isGameOver = false;
    private float gameOverTimer = 0f;


    // ============================================
    // UNITY LIFECYCLE
    // ============================================

    void Update()
    {
        if (isGameOver)
        {
            // Press R to restart after game over
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        CheckGameOverCondition();
    }


    // ============================================
    // FRUIT TRACKING (Polymorphic Collection Management)
    // ============================================

    /// <summary>
    /// Adds a fruit to the active fruits list.
    /// The parameter is typed as Fruit (base class), so this method
    /// accepts Cherry, Grape, Orange -- ANY derived type.
    /// </summary>
    public void RegisterFruit(Fruit fruit)
    {
        if (fruit != null && !activeFruits.Contains(fruit))
        {
            activeFruits.Add(fruit);
        }
    }

    /// <summary>
    /// Removes a fruit from the active fruits list.
    /// </summary>
    public void UnregisterFruit(Fruit fruit)
    {
        if (fruit != null)
        {
            activeFruits.Remove(fruit);
        }
    }


    // ============================================
    // MERGE EXECUTION
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHIC PARAMETERS
    //
    // MergeFruits takes two Fruit parameters. The actual objects could be
    // any combination: two Cherries, two Grapes, two Oranges, etc.
    //
    // Every method call on fruitA and fruitB uses polymorphism:
    //   fruitA.GetMergeResultTier()  -- calls the DERIVED version
    //   fruitA.GetPointValue()       -- calls the DERIVED version
    //   fruitB.GetPointValue()       -- calls the DERIVED version
    //
    // This ONE method handles ALL possible merge combinations.
    // We never write "if fruitA is Cherry" or "if fruitB is Grape".
    // Polymorphism makes that unnecessary.
    // =====================================================================

    /// <summary>
    /// Executes a merge between two fruits: destroys both and spawns the next tier.
    /// Called by Fruit.OnCollisionEnter2D() when two same-tier fruits collide.
    /// </summary>
    public void MergeFruits(Fruit fruitA, Fruit fruitB)
    {
        if (fruitA == null || fruitB == null) return;

        // [POLY] GetMergeResultTier() calls the derived version.
        // Most fruits return tier + 1. Watermelon returns -1.
        int nextTier = fruitA.GetMergeResultTier();
        if (nextTier == -1)
        {
            Debug.Log("Maximum tier reached! Two Watermelons cannot merge.");
            return;
        }

        // Calculate the spawn position (midpoint between the two fruits)
        Vector3 mergePosition = (fruitA.transform.position + fruitB.transform.position) / 2f;

        // [POLY] GetPointValue() calls the derived version for each fruit.
        // Cherry returns 1, Grape returns 6, Orange returns 10, etc.
        if (scoreManager != null)
        {
            int points = fruitA.GetPointValue() + fruitB.GetPointValue();
            scoreManager.AddScore(points);
        }

        // Remove both fruits from tracking and destroy them
        UnregisterFruit(fruitA);
        UnregisterFruit(fruitB);
        Destroy(fruitA.gameObject);
        Destroy(fruitB.gameObject);

        // Create the merged fruit (next tier) at the midpoint
        if (fruitFactory != null && fruitFactory.HasPrefabForTier(nextTier))
        {
            Fruit newFruit = fruitFactory.CreateFruitAtPosition(nextTier, mergePosition);
            if (newFruit != null)
            {
                // New fruit starts with full physics (falls naturally)
                newFruit.SetKinematic(false);
                newFruit.SetPhysicsEnabled(true);
                RegisterFruit(newFruit);

                // Update highest fruit display
                if (scoreManager != null)
                {
                    scoreManager.UpdateHighestFruit(newFruit.GetFruitName());
                }
            }
        }
        else
        {
            Debug.LogWarning($"FruitFactory: No prefab for tier {nextTier}. " +
                             $"Create the class and prefab, then assign it to slot {nextTier}!");
        }
    }


    // ============================================
    // GAME OVER DETECTION
    // ============================================

    /// <summary>
    /// Checks if any fruit has been above the game over line for too long.
    /// Uses a foreach loop over the polymorphic List<Fruit>.
    /// </summary>
    void CheckGameOverCondition()
    {
        bool fruitAboveLine = false;

        // TEACHING: foreach loop over a polymorphic collection.
        // Each 'fruit' in the loop could be Cherry, Grape, Orange, etc.
        // We don't need to know which type -- we just check the position.
        foreach (Fruit fruit in activeFruits)
        {
            if (fruit != null && fruit.transform.position.y > gameOverLineY)
            {
                // Only count fruits that have settled (not currently being dropped)
                Rigidbody2D fruitRb = fruit.GetComponent<Rigidbody2D>();
                if (fruitRb != null && fruitRb.bodyType == RigidbodyType2D.Dynamic)
                {
                    fruitAboveLine = true;
                    break;
                }
            }
        }

        if (fruitAboveLine)
        {
            gameOverTimer += Time.deltaTime;
            if (gameOverTimer >= gameOverDelay)
            {
                GameOver();
            }
        }
        else
        {
            gameOverTimer = 0f;
        }
    }

    /// <summary>
    /// Triggers the game over state.
    /// </summary>
    void GameOver()
    {
        isGameOver = true;

        if (dropController != null)
        {
            dropController.OnGameOver();
        }

        if (scoreManager != null)
        {
            scoreManager.ShowGameOver();
        }

        Debug.Log("Game Over! Press R to restart.");
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // ============================================
    // POLYMORPHIC QUERY METHODS
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHISM IN FOREACH LOOPS
    //
    // These methods demonstrate polymorphism in its most common form:
    // iterating a collection of base-class references and calling virtual
    // methods that dispatch to the correct derived version.
    //
    // In each loop, 'fruit' is typed as Fruit, but the actual objects
    // are Cherry, Grape, Orange, etc. When we call fruit.GetTier() or
    // fruit.GetPointValue(), the DERIVED version runs automatically.
    //
    // Session 3: Walk through these methods with the instructor to see
    // polymorphism in action with a running game.
    // =====================================================================

    /// <summary>
    /// Counts how many active fruits have the specified tier.
    /// Demonstrates polymorphism: fruit.GetTier() returns the derived value.
    /// </summary>
    public int CountFruitsOfTier(int targetTier)
    {
        int count = 0;

        foreach (Fruit fruit in activeFruits)
        {
            if (fruit != null && fruit.GetTier() == targetTier)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Sums the point values of all active fruits.
    /// Demonstrates polymorphism: fruit.GetPointValue() returns each fruit's specific value.
    /// </summary>
    public int GetTotalFruitPoints()
    {
        int total = 0;

        foreach (Fruit fruit in activeFruits)
        {
            if (fruit != null)
            {
                total += fruit.GetPointValue();
            }
        }

        return total;
    }

    /// <summary>
    /// Returns the highest tier among all active fruits.
    /// </summary>
    public int GetHighestTier()
    {
        int highest = -1;

        foreach (Fruit fruit in activeFruits)
        {
            if (fruit != null && fruit.GetTier() > highest)
            {
                highest = fruit.GetTier();
            }
        }

        return highest;
    }

    /// <summary>
    /// Returns the name of the highest-tier active fruit.
    /// Demonstrates polymorphism: fruit.GetFruitName() returns the derived name.
    /// </summary>
    public string GetHighestFruitName()
    {
        int highestTier = -1;
        string highestName = "None";

        foreach (Fruit fruit in activeFruits)
        {
            if (fruit != null && fruit.GetTier() > highestTier)
            {
                highestTier = fruit.GetTier();
                highestName = fruit.GetFruitName();
            }
        }

        return highestName;
    }

    /// <summary>
    /// Returns the total number of active fruits in the container.
    /// </summary>
    public int GetActiveFruitCount()
    {
        return activeFruits.Count;
    }


    // ============================================
    // EDITOR VISUALIZATION
    // ============================================

    void OnDrawGizmos()
    {
        // Draw the game over line in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(-5f, gameOverLineY, 0f),
            new Vector3(5f, gameOverLineY, 0f)
        );
    }
}
