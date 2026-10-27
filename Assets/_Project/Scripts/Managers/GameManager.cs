/*
 * GameManager -- runs the game: tracks every dropped object, merges pairs, keeps score,
 * and ends the game when the pile gets too high.
 *
 * It never asks "is this a TierZero or a TierTwo?" It talks to every object as a
 * MergeObject, and each one answers with its own values. That's polymorphism.
 *
 * Read alongside: Docs/INHERITANCE_CONCEPTS.md (Polymorphism, examples 2 and 3)
 * Session 3: finish the two TODO methods at the bottom of this file.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [Header("References")]
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the DropController component from the scene")]
    public DropController dropController;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change the rules
    // ─────────────────────────────────────────────────────────────────────────

    [Header("Game Over Settings")]
    [Tooltip("Y position above which objects trigger game over")]
    public float gameOverLineY = 4.5f;

    [Tooltip("Seconds an object must be above the line before game over triggers")]
    public float gameOverDelay = 2f;

    // Same InputAction pattern as DropController.cs
    [Header("Input")]
    [Tooltip("Button action that restarts the game after game over")]
    [SerializeField]
    private InputAction restartAction =
        new InputAction("Restart", InputActionType.Button, "<Keyboard>/r");

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - Tracked while playing
    // ─────────────────────────────────────────────────────────────────────────
    // LISTS: a List grows and shrinks while the game runs. An array's size is fixed.
    //   activeObjects.Add(obj)       put one in
    //   activeObjects.Remove(obj)    take one out
    //   activeObjects.Count          how many are in it right now
    //   foreach (MergeObject obj in activeObjects) { ... }   visit each one in turn
    // It's a List<MergeObject>, so ONE list holds every tier at once. [POLY]

    // Why one List<MergeObject> instead of a separate list for each tier?
    private List<MergeObject> activeObjects = new List<MergeObject>();

    private bool isGameOver = false;
    private float gameOverTimer = 0f;
    private int score = 0;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    // An InputAction that is never enabled does nothing, and shows no error.
    void OnEnable()
    {
        restartAction.Enable();
    }

    void OnDisable()
    {
        restartAction.Disable();
    }

    void Start()
    {
        Debug.Log($"Merge Template {BuildInfo.Version}");
    }

    void Update()
    {
        if (isGameOver)
        {
            // Why does R only work after game over? What would change if this check moved
            // above the `if (isGameOver)` line?
            if (restartAction.WasPressedThisFrame())
            {
                RestartGame();
            }
            return;
        }

        CheckGameOverCondition();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────

    // Called by DropController when an object is dropped.
    public void RegisterMergeObject(MergeObject obj)
    {
        // Why check Contains() before adding?
        if (obj != null && !activeObjects.Contains(obj))
        {
            activeObjects.Add(obj);
        }
    }

    public void UnregisterMergeObject(MergeObject obj)
    {
        if (obj != null)
        {
            activeObjects.Remove(obj);
        }
    }

    // Called by MergeObject.OnCollisionEnter2D when two same-tier objects touch.
    // The parameters are typed MergeObject, so this ONE method handles every pair of tiers. [POLY]
    public void MergeObjects(MergeObject objA, MergeObject objB)
    {
        if (objA == null || objB == null) return;

        int nextTier = objA.GetMergeResultTier();                   // [POLY]
        if (nextTier == -1)
        {
            Debug.Log("Maximum tier reached! Two max-tier objects cannot merge.");
            return;
        }

        // Why spawn the new object at the midpoint and not at objA's position?
        Vector3 mergePosition = (objA.transform.position + objB.transform.position) / 2f;

        int points = objA.GetPointValue() + objB.GetPointValue();   // [POLY]
        score += points;
        Debug.Log($"Merge! +{points} points (Total: {score})");

        // Why remove them from the List before destroying them?
        UnregisterMergeObject(objA);
        UnregisterMergeObject(objB);
        Destroy(objA.gameObject);
        Destroy(objB.gameObject);

        if (objectFactory != null && objectFactory.HasPrefabForTier(nextTier))
        {
            MergeObject newObj = objectFactory.CreateObjectAtPosition(nextTier, mergePosition);
            if (newObj != null)
            {
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

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Game over
    // ─────────────────────────────────────────────────────────────────────────

    void CheckGameOverCondition()
    {
        bool objectAboveLine = false;

        foreach (MergeObject obj in activeObjects)
        {
            if (obj != null && obj.transform.position.y > gameOverLineY)
            {
                // Why skip objects that aren't Dynamic? Which object would that be?
                Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                if (objRb != null && objRb.bodyType == RigidbodyType2D.Dynamic)
                {
                    objectAboveLine = true;
                    break;
                }
            }
        }

        // Why wait gameOverDelay seconds instead of ending the game right away?
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

    void GameOver()
    {
        isGameOver = true;

        if (dropController != null)
        {
            dropController.OnGameOver();
        }

        Debug.Log($"Game Over! Score: {score}. Press R to restart.");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // QUERY METHODS - Session 3: loops over the List that RETURN an answer
    // ─────────────────────────────────────────────────────────────────────────
    // Each of these walks the List with foreach and hands one value back with `return`.
    // obj is typed MergeObject, but GetTier() / GetPointValue() run each object's own
    // version. [POLY]  Read the two finished ones first; the TODOs follow the same shape.

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

    public int GetTotalObjectPoints()
    {
        // TODO: Use a foreach loop to iterate activeObjects and sum each object's GetPointValue()
        return 0;
    }

    public int GetHighestTier()
    {
        // Why start at -1 instead of 0?
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

    public string GetHighestObjectName()
    {
        // TODO: Use a foreach loop to find the object with the highest GetTier(), then return its GetObjectName()
        return "None";
    }

    public int GetActiveObjectCount()
    {
        return activeObjects.Count;
    }
}
