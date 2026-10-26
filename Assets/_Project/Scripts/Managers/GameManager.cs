/*
 * GAME 220: Merge Template -- GameManager
 * Tracks dropped objects, executes merges, keeps score, detects game over.
 * Polymorphism: List<MergeObject> and MergeObjects(a, b) work with every derived type.
 * Concepts: Docs/INHERITANCE_CONCEPTS.md -- Polymorphism, examples 2 and 3
 *
 * Session 3: complete the two TODO query methods at the bottom.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    // Input pattern explained in DropController.cs
    [Header("Input")]
    [Tooltip("Button action that restarts the game after game over")]
    [SerializeField]
    private InputAction restartAction =
        new InputAction("Restart", InputActionType.Button, "<Keyboard>/r");

    // [POLY] One list holds TierZero, TierOne, TierTwo and any class you add
    private List<MergeObject> activeObjects = new List<MergeObject>();

    private bool isGameOver = false;
    private float gameOverTimer = 0f;
    private int score = 0;


    // ---------- Unity lifecycle ----------

    // An action that is never enabled does nothing, silently
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
            if (restartAction.WasPressedThisFrame())
            {
                RestartGame();
            }
            return;
        }

        CheckGameOverCondition();
    }


    // ---------- Object tracking ----------

    public void RegisterMergeObject(MergeObject obj)
    {
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


    // ---------- Merge execution ----------

    // Called by MergeObject.OnCollisionEnter2D. [POLY] one method handles every pair of types.
    public void MergeObjects(MergeObject objA, MergeObject objB)
    {
        if (objA == null || objB == null) return;

        int nextTier = objA.GetMergeResultTier();                   // [POLY]
        if (nextTier == -1)
        {
            Debug.Log("Maximum tier reached! Two max-tier objects cannot merge.");
            return;
        }

        Vector3 mergePosition = (objA.transform.position + objB.transform.position) / 2f;

        int points = objA.GetPointValue() + objB.GetPointValue();   // [POLY]
        score += points;
        Debug.Log($"Merge! +{points} points (Total: {score})");

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


    // ---------- Game over ----------

    void CheckGameOverCondition()
    {
        bool objectAboveLine = false;

        foreach (MergeObject obj in activeObjects)
        {
            if (obj != null && obj.transform.position.y > gameOverLineY)
            {
                // Only count dropped objects (Dynamic), not the one being aimed
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


    // ---------- Query methods (Session 3) ----------
    // Each obj is typed MergeObject; the derived GetTier()/GetPointValue() runs. [POLY]

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
