/*
 * GAME 220: Merge Template
 * Session 1: Score Manager (PRE-BUILT)
 *
 * TEACHING FOCUS:
 * - UI Text integration with UnityEngine.UI
 * - Public methods called by other scripts (GameManager, MergeObject)
 * - Separation of concerns: scoring logic is separate from game logic
 *
 * This script follows the same pattern as the SlitherTemplate's ScoreManager.
 * It manages the score display and game over text.
 *
 * STUDENT TASKS:
 * - This file is fully pre-built. Students do NOT need to modify it.
 * - Students may reference this when building their own UI features.
 */

using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // ============================================
    // UI REFERENCES
    // ============================================

    [Header("UI References")]
    [Tooltip("Drag the Score Text UI element from the Canvas")]
    public Text scoreText;

    [Tooltip("Drag the Game Over Text UI element from the Canvas")]
    public Text gameOverText;

    [Tooltip("(Optional) Drag a Text element to show the highest merge object achieved")]
    public Text highestObjectText;


    // ============================================
    // PRIVATE STATE
    // ============================================

    private int currentScore = 0;


    // ============================================
    // UNITY LIFECYCLE
    // ============================================

    void Start()
    {
        currentScore = 0;
        UpdateScoreDisplay();

        // Hide game over text at start
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        // Initialize highest object display
        if (highestObjectText != null)
        {
            highestObjectText.text = "Best: --";
        }
    }


    // ============================================
    // PUBLIC METHODS (Called by Other Scripts)
    // ============================================

    /// <summary>
    /// Adds points to the current score. Called by GameManager.MergeObjects().
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Updates the highest merge object display text.
    /// Called by GameManager when a new highest tier is achieved.
    /// </summary>
    public void UpdateHighestObject(string objectName)
    {
        if (highestObjectText != null)
        {
            highestObjectText.text = $"Best: {objectName}";
        }
    }

    /// <summary>
    /// Shows the game over message with the final score.
    /// Called by GameManager.GameOver().
    /// </summary>
    public void ShowGameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"Game Over!\nScore: {currentScore}\nPress R to Restart";
        }
    }

    /// <summary>
    /// Returns the current score. Useful for other scripts that need to read it.
    /// </summary>
    public int GetCurrentScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Resets the score to zero. Called when restarting the game.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }


    // ============================================
    // PRIVATE HELPERS
    // ============================================

    /// <summary>
    /// Updates the score text UI element.
    /// </summary>
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }
}
