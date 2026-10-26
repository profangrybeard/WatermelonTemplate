/*
 * GAME 220: Merge Template -- DropController (PRE-BUILT, no student changes needed)
 * Mouse aims, click drops, short cooldown, then a new random low-tier object appears.
 * Polymorphism: currentObject is typed MergeObject but holds whichever tier was spawned.
 * Concepts: Docs/INHERITANCE_CONCEPTS.md -- Polymorphism, example 1
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class DropController : MonoBehaviour
{
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

    [Header("References")]
    [Tooltip("Drag the MergeObjectFactory component from the scene")]
    public MergeObjectFactory objectFactory;

    [Tooltip("Drag the GameManager component from the scene")]
    public GameManager gameManager;

    // An InputAction is a named verb bound to a control. The code asks the action, never the mouse,
    // so rebinding in the Inspector (e.g. to a gamepad) needs no code change.
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

    private MergeObject currentObject;   // [POLY] one variable for every tier, not one per type

    private float cooldownTimer = 0f;
    private bool canDrop = true;
    private bool isGameOver = false;
    private float aimX = 0f;


    // ---------- Unity lifecycle ----------

    // An action that is never enabled does nothing, silently. OnEnable (not Start)
    // so input comes back if the object is disabled and re-enabled.
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


    // ---------- Input ----------

    void HandleInput()
    {
        Vector2 pointerScreenPos = aimAction.ReadValue<Vector2>();   // screen pixels
        Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerScreenPos);
        aimX = Mathf.Clamp(pointerWorldPos.x, minX, maxX);
    }

    bool ShouldDrop()
    {
        return dropAction.WasPressedThisFrame();
    }


    // ---------- Object management ----------

    // New random object floats (kinematic) at the aim point until dropped
    void SpawnNextObject()
    {
        if (objectFactory == null) return;

        int availableMaxTier = Mathf.Min(maxDropTier, objectFactory.GetMaxTier());
        if (availableMaxTier < 0) return;

        int randomTier = Random.Range(0, availableMaxTier + 1);

        currentObject = objectFactory.CreateObject(randomTier);   // [POLY]

        if (currentObject != null)
        {
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

        currentObject = null;
        canDrop = false;
        cooldownTimer = dropCooldown;
    }


    // ---------- Game state ----------

    // Called by GameManager: remove the aiming object and stop dropping
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
