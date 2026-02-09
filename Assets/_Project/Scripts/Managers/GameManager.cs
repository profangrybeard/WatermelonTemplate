/*
 * GAME 220: Merge Template
 * Sessions 1-5: Game Manager
 *
 * TEACHING FOCUS:
 * - POLYMORPHIC COLLECTION: List<MergeObject> holds ANY derived type
 * - POLYMORPHIC PARAMETERS: MergeObjects() accepts any two MergeObject types
 * - VIRTUAL METHOD DISPATCH: Calling methods on base references runs the derived version
 * - foreach LOOPS over polymorphic collections
 *
 * STUDENT TASKS:
 * - Session 1: Read and understand how List<MergeObject> works with different derived types
 * - Session 3: Complete the TODO query methods, examine polymorphism in foreach loops
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
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the DropController component from the scene")]
    public DropController dropController;

    [Header("Game Over Settings")]
    [Tooltip("Y position above which objects trigger game over")]
    public float gameOverLineY = 4.5f;

    [Tooltip("Seconds an object must be above the line before game over triggers")]
    public float gameOverDelay = 2f;


    // ============================================
    // POLYMORPHIC COLLECTION
    // ============================================

    // TEACHING: List<MergeObject> can hold TierZero, TierOne, TierTwo --
    // ANY class that inherits from MergeObject.
    //
    // When we loop through this list and call methods like GetTier() or
    // GetPointValue(), the CORRECT DERIVED VERSION runs for each object.
    // We never check "is this a TierZero?" -- polymorphism handles it.
    //
    // Write code for the BASE type, and it works with ALL derived types --
    // even ones that don't exist yet!
    private List<MergeObject> activeObjects = new List<MergeObject>();


    // ============================================
    // PRIVATE STATE
    // ============================================

    private bool isGameOver = false;
    private float gameOverTimer = 0f;
    private int score = 0;


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
    // OBJECT TRACKING (Polymorphic Collection Management)
    // ============================================

    /// <summary>
    /// Adds an object to the active objects list.
    /// The parameter is typed as MergeObject (base class), so this method
    /// accepts any derived type.
    /// </summary>
    public void RegisterMergeObject(MergeObject obj)
    {
        if (obj != null && !activeObjects.Contains(obj))
        {
            activeObjects.Add(obj);
        }
    }

    /// <summary>
    /// Removes an object from the active objects list.
    /// </summary>
    public void UnregisterMergeObject(MergeObject obj)
    {
        if (obj != null)
        {
            activeObjects.Remove(obj);
        }
    }


    // ============================================
    // MERGE EXECUTION
    // ============================================

    // TEACHING: POLYMORPHIC PARAMETERS
    // MergeObjects takes two MergeObject parameters. The actual objects could be
    // any combination: two TierZero, two TierTwo, two of your custom class, etc.
    // Every method call uses virtual dispatch to run the derived version.
    // This ONE method handles ALL merge combinations -- no type-checking needed.

    /// <summary>
    /// Executes a merge between two objects: destroys both and spawns the next tier.
    /// Called by MergeObject.OnCollisionEnter2D() when two same-tier objects collide.
    /// </summary>
    public void MergeObjects(MergeObject objA, MergeObject objB)
    {
        if (objA == null || objB == null) return;

        // [POLY] GetMergeResultTier() calls the derived version.
        // Most objects return tier + 1. The highest tier returns -1.
        int nextTier = objA.GetMergeResultTier();
        if (nextTier == -1)
        {
            Debug.Log("Maximum tier reached! Two max-tier objects cannot merge.");
            return;
        }

        // Calculate the spawn position (midpoint between the two objects)
        Vector3 mergePosition = (objA.transform.position + objB.transform.position) / 2f;

        // [POLY] GetPointValue() calls the derived version for each object.
        int points = objA.GetPointValue() + objB.GetPointValue();
        score += points;
        Debug.Log($"Merge! +{points} points (Total: {score})");

        // Remove both objects from tracking and destroy them
        UnregisterMergeObject(objA);
        UnregisterMergeObject(objB);
        Destroy(objA.gameObject);
        Destroy(objB.gameObject);

        // Create the merged object (next tier) at the midpoint
        if (objectFactory != null && objectFactory.HasPrefabForTier(nextTier))
        {
            MergeObject newObj = objectFactory.CreateObjectAtPosition(nextTier, mergePosition);
            if (newObj != null)
            {
                // New object starts with full physics (falls naturally)
                newObj.SetKinematic(false);
                newObj.SetPhysicsEnabled(true);
                RegisterMergeObject(newObj);
            }
        }
        else
        {
            Debug.LogWarning($"MergeObjectFactory: No prefab for tier {nextTier}. " +
                             $"Create the class and prefab, then assign it to slot {nextTier}!");
        }
    }


    // ============================================
    // GAME OVER DETECTION
    // ============================================

    /// <summary>
    /// Checks if any object has been above the game over line for too long.
    /// Uses a foreach loop over the polymorphic List<MergeObject>.
    /// </summary>
    void CheckGameOverCondition()
    {
        bool objectAboveLine = false;

        // TEACHING: foreach loop over a polymorphic collection.
        // Each 'obj' in the loop could be any derived type.
        // We don't need to know which type -- we just check the position.
        foreach (MergeObject obj in activeObjects)
        {
            if (obj != null && obj.transform.position.y > gameOverLineY)
            {
                // Only count objects that have settled (not currently being dropped)
                Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                if (objRb != null && objRb.bodyType == RigidbodyType2D.Dynamic)
                {
                    objectAboveLine = true;
                    break;
                }
            }
        }

        if (objectAboveLine)
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

        Debug.Log($"Game Over! Score: {score}. Press R to restart.");
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

    // TEACHING: POLYMORPHISM IN FOREACH LOOPS
    // Each 'obj' in the loop is typed as MergeObject, but the actual objects
    // are TierZero, TierOne, TierTwo, your custom classes, etc. When we call
    // obj.GetTier() or obj.GetPointValue(), the DERIVED version runs automatically.
    //
    // Session 3: Walk through these methods with the instructor.

    /// <summary>
    /// Counts how many active objects have the specified tier.
    /// Demonstrates polymorphism: obj.GetTier() returns the derived value.
    /// </summary>
    public int CountObjectsOfTier(int targetTier)
    {
        int count = 0;

        foreach (MergeObject obj in activeObjects)
        {
            if (obj != null && obj.GetTier() == targetTier)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Sums the point values of all active objects.
    /// Demonstrates polymorphism: obj.GetPointValue() returns each object's specific value.
    /// </summary>
    public int GetTotalObjectPoints()
    {
        // TODO: Use a foreach loop to iterate activeObjects and sum each object's GetPointValue()
        return 0;
    }

    /// <summary>
    /// Returns the highest tier among all active objects.
    /// </summary>
    public int GetHighestTier()
    {
        int highest = -1;

        foreach (MergeObject obj in activeObjects)
        {
            if (obj != null && obj.GetTier() > highest)
            {
                highest = obj.GetTier();
            }
        }

        return highest;
    }

    /// <summary>
    /// Returns the name of the highest-tier active object.
    /// Demonstrates polymorphism: obj.GetObjectName() returns the derived name.
    /// </summary>
    public string GetHighestObjectName()
    {
        // TODO: Use a foreach loop to find the object with the highest GetTier(), then return its GetObjectName()
        return "None";
    }

    /// <summary>
    /// Returns the total number of active objects in the container.
    /// </summary>
    public int GetActiveObjectCount()
    {
        return activeObjects.Count;
    }
}
