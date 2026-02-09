/*
 * GAME 220: Merge Template
 * Session 1: Drop Controller (PRE-BUILT)
 *
 * TEACHING FOCUS:
 * - POLYMORPHIC VARIABLE: 'currentObject' is typed as MergeObject (base class)
 *   but holds TierZero, TierOne, TierTwo, etc. (derived types)
 * - Input handling with both mouse and keyboard
 * - State management (aiming vs cooldown)
 *
 * This script handles the player's ability to aim and drop merge objects:
 * - Mouse: Move cursor left/right to aim, click to drop
 * - Keyboard: A/D or Arrow Keys to aim, Space to drop
 * - Cooldown between drops prevents spam
 * - Creates random low-tier objects via MergeObjectFactory
 *
 * STUDENT TASKS:
 * - This file is fully pre-built. Students do NOT need to modify it.
 * - Students should notice that 'currentObject' is typed as MergeObject, not TierZero.
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

    [Tooltip("Highest tier that can be randomly dropped (0=TierZero, 1=TierOne, 2=TierTwo)")]
    public int maxDropTier = 2;

    [Header("Aim Settings")]
    [Tooltip("How fast the aim moves with keyboard input")]
    public float keyboardAimSpeed = 5f;

    // ============================================
    // REFERENCES
    // ============================================

    [Header("References")]
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the GameManager component from the scene")]
    public GameManager gameManager;


    // ============================================
    // PRIVATE STATE
    // ============================================

    // =====================================================================
    // TEACHING: POLYMORPHIC VARIABLE
    //
    // 'currentObject' is typed as MergeObject (the base class), but the actual
    // object stored in it is TierZero, TierOne, TierTwo, or whatever merge object
    // the MergeObjectFactory created.
    //
    // We don't need separate variables for each object type:
    //   TierZero currentTierZero;      // NO! Don't do this.
    //   TierOne currentTierOne;        // NO! Don't do this.
    //   TierTwo currentTierTwo;        // NO! Don't do this.
    //
    // Instead, ONE variable typed as the base class handles ALL types:
    //   MergeObject currentObject;     // YES! Holds any derived type.
    //
    // When we call currentObject.GetMergeObjectName(), currentObject.GetTier(), etc.,
    // the correct derived version runs. That's polymorphism.
    // =====================================================================
    private MergeObject currentObject;

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
        SpawnNextObject();
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
                SpawnNextObject();
            }
            return;
        }

        // Handle aiming and dropping
        HandleInput();
        PositionCurrentObject();

        if (ShouldDrop())
        {
            DropObject();
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
    /// Returns true if the player wants to drop the current object.
    /// Supports mouse click and spacebar.
    /// </summary>
    bool ShouldDrop()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }


    // ============================================
    // OBJECT MANAGEMENT
    // ============================================

    /// <summary>
    /// Creates a new random merge object for the player to aim and drop.
    /// The object starts in kinematic mode (no gravity) until dropped.
    /// </summary>
    void SpawnNextObject()
    {
        if (objectFactory == null) return;

        // Limit the drop tier to what's available in the factory
        int availableMaxTier = Mathf.Min(maxDropTier, objectFactory.GetMaxTier());
        if (availableMaxTier < 0) return;

        // Pick a random tier from the droppable range
        int randomTier = Random.Range(0, availableMaxTier + 1);

        // TEACHING: CreateObject returns a MergeObject reference, but the actual object
        // is TierZero, TierOne, etc. We store it in our MergeObject-typed variable.
        currentObject = objectFactory.CreateObject(randomTier);

        if (currentObject != null)
        {
            // Set the object to kinematic so it floats at the aim position
            // (no gravity until the player drops it)
            currentObject.SetKinematic(true);
            currentObject.SetPhysicsEnabled(true);

            // Position at the current aim point
            PositionCurrentObject();
        }
    }

    /// <summary>
    /// Moves the current object to follow the aim position.
    /// </summary>
    void PositionCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.position = new Vector3(aimX, dropLineY, 0f);
        }
    }

    /// <summary>
    /// Drops the current object by enabling physics and starting the cooldown.
    /// </summary>
    void DropObject()
    {
        if (currentObject == null) return;

        // Enable full physics so the object falls with gravity
        currentObject.SetKinematic(false);

        // Register with GameManager for tracking
        if (gameManager != null)
        {
            gameManager.RegisterMergeObject(currentObject);
        }

        // Clear the reference and start cooldown
        currentObject = null;
        canDrop = false;
        cooldownTimer = dropCooldown;
    }


    // ============================================
    // GAME STATE
    // ============================================

    /// <summary>
    /// Called by GameManager when the game is over.
    /// Destroys the current aiming object and stops further drops.
    /// </summary>
    public void OnGameOver()
    {
        isGameOver = true;

        if (currentObject != null)
        {
            Destroy(currentObject.gameObject);
            currentObject = null;
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
