/*
 * DropController -- the player's hand. The mouse aims, a click drops, then after a short
 * cooldown a new random object appears. Pre-built: you don't need to change it.
 *
 * Look at `currentObject`: one variable, typed MergeObject, holds whichever tier was
 * spawned. That's polymorphism.
 *
 * Read alongside: Docs/INHERITANCE_CONCEPTS.md (Polymorphism, example 1)
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class DropController : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change how dropping feels
    // ─────────────────────────────────────────────────────────────────────────

    [Header("Drop Settings")]
    [Tooltip("Seconds to wait between drops")]
    public float dropCooldown = 0.5f;

    [Tooltip("Y position of the drop/aim line")]
    public float dropLineY = 4f;

    [Tooltip("Left boundary for aiming (inside container)")]
    public float minX = -2.5f;

    [Tooltip("Right boundary for aiming (inside container)")]
    public float maxX = 2.5f;

    // Raise this as you add tiers, or your new objects will never be dropped.
    [Tooltip("Highest tier that can be randomly dropped (0=TierZero, 1=TierOne, 2=TierTwo)")]
    public int maxDropTier = 2;

    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [Header("References")]
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the GameManager component from the scene")]
    public GameManager gameManager;

    // ─────────────────────────────────────────────────────────────────────────
    // INPUT - Input System actions (not the old Input class)
    // ─────────────────────────────────────────────────────────────────────────
    // An InputAction is a named verb ("Aim", "Drop") bound to a control. The code asks the
    // ACTION, never the mouse directly, so rebinding it in the Inspector needs no code change.
    // [SerializeField] private = editable in the Inspector, hidden from other scripts.

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

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - Tracked while playing
    // ─────────────────────────────────────────────────────────────────────────

    // Why type this as MergeObject instead of TierZero? [POLY]
    private MergeObject currentObject;

    private float cooldownTimer = 0f;
    private bool canDrop = true;
    private bool isGameOver = false;
    private float aimX = 0f;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    // An action that is never enabled does nothing, and shows no error.
    // Why enable in OnEnable() instead of Start()?
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
        aimX = 0f;
        SpawnNextObject();
    }

    void Update()
    {
        if (isGameOver) return;

        // Why `return` here instead of letting the rest of Update() run?
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

        HandleInput();
        PositionCurrentObject();

        if (ShouldDrop())
        {
            DropObject();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Input
    // ─────────────────────────────────────────────────────────────────────────

    void HandleInput()
    {
        Vector2 pointerScreenPos = aimAction.ReadValue<Vector2>();   // screen pixels
        Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerScreenPos);

        // What would happen without the Clamp?
        aimX = Mathf.Clamp(pointerWorldPos.x, minX, maxX);
    }

    // A one-line method that returns a bool. Why wrap it in a method at all?
    bool ShouldDrop()
    {
        return dropAction.WasPressedThisFrame();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - The object being aimed
    // ─────────────────────────────────────────────────────────────────────────

    void SpawnNextObject()
    {
        if (objectFactory == null) return;

        int availableMaxTier = Mathf.Min(maxDropTier, objectFactory.GetMaxTier());
        if (availableMaxTier < 0) return;

        // Random.Range with ints never returns the top number. Why the + 1?
        int randomTier = Random.Range(0, availableMaxTier + 1);

        currentObject = objectFactory.CreateObject(randomTier);   // [POLY]

        if (currentObject != null)
        {
            // Kinematic = floats at the aim point with no gravity until it's dropped
            currentObject.SetKinematic(true);
            currentObject.SetPhysicsEnabled(true);
            PositionCurrentObject();
        }
    }

    void PositionCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.position = new Vector3(aimX, dropLineY, 0f);
        }
    }

    void DropObject()
    {
        if (currentObject == null) return;

        currentObject.SetKinematic(false);   // gravity on

        if (gameManager != null)
        {
            gameManager.RegisterMergeObject(currentObject);
        }

        // Why set currentObject to null after dropping it?
        currentObject = null;
        canDrop = false;
        cooldownTimer = dropCooldown;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────

    // Called by GameManager: remove the object being aimed and stop dropping.
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
