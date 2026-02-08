/*
 * GAME 220: Watermelon Merge Template
 * Session 1: Drop Controller (PRE-BUILT)
 *
 * TEACHING FOCUS:
 * - POLYMORPHIC VARIABLE: 'currentFruit' is typed as Fruit (base class)
 *   but holds Cherry, Grape, Orange, etc. (derived types)
 * - Input handling with both mouse and keyboard
 * - State management (aiming vs cooldown)
 *
 * This script handles the player's ability to aim and drop fruits:
 * - Mouse: Move cursor left/right to aim, click to drop
 * - Keyboard: A/D or Arrow Keys to aim, Space to drop
 * - Cooldown between drops prevents spam
 * - Creates random low-tier fruits via FruitFactory
 *
 * STUDENT TASKS:
 * - This file is fully pre-built. Students do NOT need to modify it.
 * - Students should notice that 'currentFruit' is typed as Fruit, not Cherry.
 *   This is polymorphism: the variable holds ANY derived type.
 */

using UnityEngine;

public class DropController : MonoBehaviour
{
    // ============================================
    // DROP SETTINGS
    // ============================================

    [Header("Drop Settings")]
    [Tooltip("Seconds to wait between drops")]
    public float dropCooldown = 0.5f;

    [Tooltip("Y position of the drop/aim line")]
    public float dropLineY = 4f;

    [Tooltip("Left boundary for aiming (inside container)")]
    public float minX = -2.5f;

    [Tooltip("Right boundary for aiming (inside container)")]
    public float maxX = 2.5f;

    [Tooltip("Highest tier that can be randomly dropped (0=Cherry, 1=Strawberry, 2=Grape)")]
    public int maxDropTier = 2;

    [Header("Aim Settings")]
    [Tooltip("How fast the aim moves with keyboard input")]
    public float keyboardAimSpeed = 5f;

    // ============================================
    // REFERENCES
    // ============================================

    [Header("References")]
    [Tooltip("Drag the FruitFactory component from the scene")]
    public FruitFactory fruitFactory;

    [Tooltip("Drag the GameManager component from the scene")]
    public GameManager gameManager;


    // ============================================
    // PRIVATE STATE
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHIC VARIABLE
    //
    // 'currentFruit' is typed as Fruit (the base class), but the actual
    // object stored in it is Cherry, Grape, Orange, or whatever fruit
    // the FruitFactory created.
    //
    // We don't need separate variables for each fruit type:
    //   Cherry currentCherry;      // NO! Don't do this.
    //   Grape currentGrape;        // NO! Don't do this.
    //   Orange currentOrange;      // NO! Don't do this.
    //
    // Instead, ONE variable typed as the base class handles ALL types:
    //   Fruit currentFruit;        // YES! Holds any derived type.
    //
    // When we call currentFruit.GetFruitName(), currentFruit.GetTier(), etc.,
    // the correct derived version runs. That's polymorphism.
    // =====================================================================
    private Fruit currentFruit;

    private float cooldownTimer = 0f;
    private bool canDrop = true;
    private bool isGameOver = false;
    private float aimX = 0f;


    // ============================================
    // UNITY LIFECYCLE
    // ============================================

    void Start()
    {
        aimX = 0f; // Start aiming at center
        SpawnNextFruit();
    }

    void Update()
    {
        if (isGameOver) return;

        // Handle cooldown between drops
        if (!canDrop)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canDrop = true;
                SpawnNextFruit();
            }
            return;
        }

        // Handle aiming and dropping
        HandleInput();
        PositionCurrentFruit();

        if (ShouldDrop())
        {
            DropFruit();
        }
    }


    // ============================================
    // INPUT HANDLING
    // ============================================

    /// <summary>
    /// Reads mouse and keyboard input to set the aim position.
    /// Mouse movement sets the aim position directly.
    /// Keyboard nudges the aim position incrementally.
    /// </summary>
    void HandleInput()
    {
        // Keyboard input: A/D or Arrow Keys to nudge aim
        float keyboardInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            keyboardInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            keyboardInput = 1f;
        }

        if (keyboardInput != 0f)
        {
            // Keyboard: nudge aim left or right
            aimX += keyboardInput * keyboardAimSpeed * Time.deltaTime;
        }
        else if (Input.mousePresent)
        {
            // Mouse: set aim to cursor position (only when keyboard is not active)
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimX = mouseWorldPos.x;
        }

        // Clamp aim within container bounds
        aimX = Mathf.Clamp(aimX, minX, maxX);
    }

    /// <summary>
    /// Returns true if the player wants to drop the current fruit.
    /// Supports mouse click and spacebar.
    /// </summary>
    bool ShouldDrop()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }


    // ============================================
    // FRUIT MANAGEMENT
    // ============================================

    /// <summary>
    /// Creates a new random fruit for the player to aim and drop.
    /// The fruit starts in kinematic mode (no gravity) until dropped.
    /// </summary>
    void SpawnNextFruit()
    {
        if (fruitFactory == null) return;

        // Limit the drop tier to what's available in the factory
        int availableMaxTier = Mathf.Min(maxDropTier, fruitFactory.GetMaxTier());
        if (availableMaxTier < 0) return;

        // Pick a random tier from the droppable range
        int randomTier = Random.Range(0, availableMaxTier + 1);

        // TEACHING: CreateFruit returns a Fruit reference, but the actual object
        // is Cherry, Grape, etc. We store it in our Fruit-typed variable.
        currentFruit = fruitFactory.CreateFruit(randomTier);

        if (currentFruit != null)
        {
            // Set the fruit to kinematic so it floats at the aim position
            // (no gravity until the player drops it)
            currentFruit.SetKinematic(true);
            currentFruit.SetPhysicsEnabled(true);

            // Position at the current aim point
            PositionCurrentFruit();
        }
    }

    /// <summary>
    /// Moves the current fruit to follow the aim position.
    /// </summary>
    void PositionCurrentFruit()
    {
        if (currentFruit != null)
        {
            currentFruit.transform.position = new Vector3(aimX, dropLineY, 0f);
        }
    }

    /// <summary>
    /// Drops the current fruit by enabling physics and starting the cooldown.
    /// </summary>
    void DropFruit()
    {
        if (currentFruit == null) return;

        // Enable full physics so the fruit falls with gravity
        currentFruit.SetKinematic(false);

        // Register with GameManager for tracking
        if (gameManager != null)
        {
            gameManager.RegisterFruit(currentFruit);
        }

        // Clear the reference and start cooldown
        currentFruit = null;
        canDrop = false;
        cooldownTimer = dropCooldown;
    }


    // ============================================
    // GAME STATE
    // ============================================

    /// <summary>
    /// Called by GameManager when the game is over.
    /// Destroys the current aiming fruit and stops further drops.
    /// </summary>
    public void OnGameOver()
    {
        isGameOver = true;

        if (currentFruit != null)
        {
            Destroy(currentFruit.gameObject);
            currentFruit = null;
        }
    }


    // ============================================
    // EDITOR VISUALIZATION
    // ============================================

    void OnDrawGizmos()
    {
        // Draw the drop line
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f); // Semi-transparent yellow
        Gizmos.DrawLine(
            new Vector3(minX, dropLineY, 0f),
            new Vector3(maxX, dropLineY, 0f)
        );

        // Draw the current aim position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(aimX, dropLineY, 0f), 0.2f);

        // Draw the aim boundaries
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawLine(
            new Vector3(minX, dropLineY - 0.5f, 0f),
            new Vector3(minX, dropLineY + 0.5f, 0f)
        );
        Gizmos.DrawLine(
            new Vector3(maxX, dropLineY - 0.5f, 0f),
            new Vector3(maxX, dropLineY + 0.5f, 0f)
        );
    }
}
