/*
 * GAME 220: Merge Template
 * Session 1: Drop Controller (PRE-BUILT)
 *
 * TEACHING FOCUS:
 * - POLYMORPHIC VARIABLE: 'currentObject' is typed as MergeObject (base class)
 *   but holds TierZero, TierOne, TierTwo, etc. (derived types)
 *
 * This script handles the player's ability to aim and drop merge objects:
 * - Move mouse left/right to aim, click to drop
 * - Input uses the Input System package (InputAction), not the legacy Input class
 * - Cooldown between drops prevents spam
 * - Creates random low-tier objects via MergeObjectFactory
 *
 * STUDENT TASKS:
 * - This file is fully pre-built. Students do NOT need to modify it.
 * - Students should notice that 'currentObject' is typed as MergeObject, not TierZero.
 *   This is polymorphism: the variable holds ANY derived type.
 */

using UnityEngine;
using UnityEngine.InputSystem;

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

    // ============================================
    // REFERENCES
    // ============================================

    [Header("References")]
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the GameManager component from the scene")]
    public GameManager gameManager;


    // ============================================
    // INPUT ACTIONS
    // ============================================

    // TEACHING: INPUT ACTIONS
    // An InputAction is a named verb ("Aim", "Drop") bound to a physical control.
    // The code below never mentions a mouse -- it asks the ACTION for its value.
    // Rebind the action in the Inspector and the same code drives a gamepad instead.
    //
    // These fields are [SerializeField] private: visible and editable in the
    // Inspector, but not reachable from other scripts. That is the encapsulation
    // idea from Session 1 applied to a real field.
    //
    // The default bindings are set here in code, so a freshly added DropController
    // component works immediately without clicking anything in the Inspector.

    [Header("Input")]
    [Tooltip("Value action (Vector2) that reads the pointer position on screen")]
    [SerializeField]
    private InputAction aimAction =
        new InputAction("Aim", InputActionType.Value, "<Mouse>/position",
                        expectedControlType: "Vector2");

    [Tooltip("Button action that drops the current object")]
    [SerializeField]
    private InputAction dropAction =
        new InputAction("Drop", InputActionType.Button, "<Mouse>/leftButton");


    // ============================================
    // PRIVATE STATE
    // ============================================

    // TEACHING: POLYMORPHIC VARIABLE
    // 'currentObject' is typed as MergeObject, but holds ANY derived type.
    // We don't need separate variables per type:
    //   TierZero currentTierZero;      // NO! Don't do this.
    //   TierOne currentTierOne;        // NO! Don't do this.
    // ONE base-class variable handles all types:
    //   MergeObject currentObject;     // YES! Holds any derived type.
    // When we call methods on it, the derived version runs. That's polymorphism.
    private MergeObject currentObject;

    private float cooldownTimer = 0f;
    private bool canDrop = true;
    private bool isGameOver = false;
    private float aimX = 0f;


    // ============================================
    // UNITY LIFECYCLE
    // ============================================

    // TEACHING: AN ACTION THAT IS NEVER ENABLED FAILS SILENTLY.
    // No exception, no Console warning -- input simply does nothing.
    // Use OnEnable/OnDisable, NOT Start: Start runs once ever, so an object that
    // is disabled and re-enabled would come back deaf.

    void OnEnable()
    {
        aimAction.Enable();
        dropAction.Enable();
    }

    void OnDisable()
    {
        aimAction.Disable();
        dropAction.Disable();
    }

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
    /// Reads the aim action to set the aim position.
    /// </summary>
    void HandleInput()
    {
        // ReadValue<Vector2>() returns the pointer position in SCREEN pixels.
        Vector2 pointerScreenPos = aimAction.ReadValue<Vector2>();
        Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerScreenPos);
        aimX = Mathf.Clamp(pointerWorldPos.x, minX, maxX);
    }

    /// <summary>
    /// Returns true if the player wants to drop the current object.
    /// </summary>
    bool ShouldDrop()
    {
        // WasPressedThisFrame() is the InputAction equivalent of GetMouseButtonDown.
        return dropAction.WasPressedThisFrame();
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
}
