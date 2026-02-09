/*
 * GAME 220: Merge Template
 * Sessions 1-5: Game Manager
 *
 * TEACHING FOCUS:
 * - POLYMORPHIC COLLECTION: List<MergeObject> holds TierZero, TierOne, TierTwo -- ANY derived type
 * - POLYMORPHIC PARAMETERS: MergeObjects(MergeObject a, MergeObject b) accepts any two derived types
 * - VIRTUAL METHOD DISPATCH: Calling methods on MergeObject references runs the derived version
 * - foreach LOOPS over polymorphic collections
 *
 * This script manages the overall game state:
 * - Tracks all active objects in a List<MergeObject> (polymorphic collection)
 * - Executes merges (destroy two objects, spawn the next tier)
 * - Detects game over condition
 * - Provides polymorphic query methods for inspecting active objects
 *
 * STUDENT TASKS:
 * - Session 1: Read and understand how List<MergeObject> works with different derived types
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
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the DropController component from the scene")]
    public DropController dropController;

    [Tooltip("Drag the ScoreManager component from the scene")]
    public ScoreManager scoreManager;

    [Header("Game Over Settings")]
    [Tooltip("Y position above which objects trigger game over")]
    public float gameOverLineY = 4.5f;

    [Tooltip("Seconds an object must be above the line before game over triggers")]
    public float gameOverDelay = 2f;


    // ============================================
    // POLYMORPHIC COLLECTION
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHIC COLLECTION (List<MergeObject>)
    //
    // List<MergeObject> can hold TierZero, TierOne, TierTwo --
    // ANY class that inherits from MergeObject.
    //
    // When we loop through this list and call methods like GetTier(),
    // GetPointValue(), or GetObjectName(), the CORRECT DERIVED VERSION
    // runs for each object. A TierZero returns tier 0, a TierTwo returns tier 2,
    // a TierThree returns tier 3, etc.
    //
    // We never need to check "is this a TierZero? is this a TierTwo?"
    // We just call the method, and polymorphism handles the rest.
    //
    // This is one of the most powerful concepts in object-oriented programming:
    // write code that works with the BASE type, and it automatically works
    // with ALL derived types -- even ones that don't exist yet!
    // =====================================================================
    private List<MergeObject> activeObjects = new List<MergeObject>();


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

    // =====================================================================
    // TEACHING: POLYMORPHIC PARAMETERS
    //
    // MergeObjects takes two MergeObject parameters. The actual objects could be
    // any combination: two TierZero, two TierTwo, two TierThree, etc.
    //
    // Every method call on objA and objB uses polymorphism:
    //   objA.GetMergeResultTier()  -- calls the DERIVED version
    //   objA.GetPointValue()       -- calls the DERIVED version
    //   objB.GetPointValue()       -- calls the DERIVED version
    //
    // This ONE method handles ALL possible merge combinations.
    // We never write "if objA is TierZero" or "if objB is TierTwo".
    // Polymorphism makes that unnecessary.
    // =====================================================================

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
        // TierZero returns 1, TierTwo returns 6, TierThree returns 10, etc.
        if (scoreManager != null)
        {
            int points = objA.GetPointValue() + objB.GetPointValue();
            scoreManager.AddScore(points);
        }

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

                // Update highest object display only if this is a new best
                if (scoreManager != null && newObj.GetTier() >= GetHighestTier())
                {
                    scoreManager.UpdateHighestObject(newObj.GetObjectName());
                }
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
    // In each loop, 'obj' is typed as MergeObject, but the actual objects
    // are your classes (TierZero, TierOne, TierTwo, etc.). When we call
    // obj.GetTier() or obj.GetPointValue(), the DERIVED version runs
    // automatically.
    //
    // Session 3: Walk through these methods with the instructor to see
    // polymorphism in action with a running game.
    // =====================================================================

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
