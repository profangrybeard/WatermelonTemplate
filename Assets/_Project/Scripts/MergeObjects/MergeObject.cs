/*
 * MergeObject -- the BASE CLASS. Every merge object (TierZero, TierOne, TierTwo, and the
 * ones you write) inherits from it, so the code here is written once and used by all of them.
 *
 * HOW THE PROJECT FITS TOGETHER
 *   DropController      follows the mouse, drops an object on click
 *        | asks
 *   MergeObjectFactory  builds an object for a tier number (slot 0 = TierZero, 1 = TierOne...)
 *        | builds
 *   TierZero / TierOne / TierTwo / yours      each one IS a MergeObject
 *        | when two of the same tier touch, they tell
 *   GameManager         keeps the List of every object, adds score, spawns the next tier
 *
 * Read alongside: Docs/INHERITANCE_CONCEPTS.md
 * Session 1: read this file   Session 4: override OnMerge()   Session 5 (stretch): make it abstract
 */

using UnityEngine;

public class MergeObject : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // VALUES EACH TIER SETS - Derived classes fill these in their Awake()
    // ─────────────────────────────────────────────────────────────────────────
    // protected: this class AND every class that inherits from it can use these.
    //   private   = only MergeObject could touch them, so TierZero couldn't set its size
    //   public    = any script could change them, even ones that shouldn't

    protected int tier = 0;                      // Position in the merge chain (0 = first)
    protected string objectName = "MergeObject"; // Name shown in Console messages
    protected int pointValue = 0;                // Points scored when this object merges
    protected float objectSize = 1f;             // Scale multiplier (bigger = bigger)
    protected Color objectColor = Color.white;   // Sprite tint

    // ─────────────────────────────────────────────────────────────────────────
    // COMPONENTS - Found once in Awake(), used later
    // ─────────────────────────────────────────────────────────────────────────

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE
    // ─────────────────────────────────────────────────────────────────────────

    // Why is this one private when the values above are protected?
    private bool hasMerged = false;              // Stops one object merging twice in a frame

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────
    // virtual = "a derived class may override this."
    // An override can REPLACE the parent's version or EXTEND it. Every tier EXTENDS Awake():
    //
    //     protected override void Awake()
    //     {
    //         tier = 0;          // 1. the derived class does its own work
    //         base.Awake();      // 2. then the parent's version still runs
    //     }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        // Why apply the size and color here in Start() and not in Awake()?
        ApplyObjectProperties();
    }

    // Not virtual: derived classes set the fields; this method does the applying.
    protected void ApplyObjectProperties()
    {
        transform.localScale = Vector3.one * objectSize;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = objectColor;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────
    // RETURN VALUES: these methods hand a value BACK to whoever called them.
    //   The type before the name (int, string) is what comes back.
    //   `return` sends it back and ends the method.
    //       int t = someObject.GetTier();    // t now holds that object's tier
    //   A `void` method (like OnMerge) does something but hands nothing back.

    public virtual int GetTier()
    {
        return tier;
    }

    public virtual int GetPointValue()
    {
        return pointValue;
    }

    public virtual string GetObjectName()
    {
        return objectName;
    }

    // Default: the next tier up. A final tier overrides this and returns -1.
    public virtual int GetMergeResultTier()
    {
        // Why return -1 instead of just leaving this method out?
        return tier + 1;
    }

    // SESSION 4: override this in your classes for a custom merge effect.
    public virtual void OnMerge()
    {
        Debug.Log($"{objectName} is merging!");
    }

    public bool HasMerged()
    {
        return hasMerged;
    }

    public void SetMerged()
    {
        hasMerged = true;
    }

    // Used by DropController: false = frozen in place
    public void SetPhysicsEnabled(bool enabled)
    {
        if (rb != null)
        {
            rb.simulated = enabled;
        }
    }

    // Used by DropController: true = floats at the aim point, false = falls and collides
    public void SetKinematic(bool isKinematic)
    {
        if (rb != null)
        {
            rb.bodyType = isKinematic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;

            if (isKinematic)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // MERGE DETECTION - Written once here, runs for every tier
    // ─────────────────────────────────────────────────────────────────────────
    // POLYMORPHISM: `otherObject` is typed MergeObject, but it is really a TierZero, a
    // TierTwo, or one of yours. Each call marked [POLY] runs THAT object's own version.

    private void OnCollisionEnter2D(Collision2D other)
    {
        MergeObject otherObject = other.gameObject.GetComponent<MergeObject>(); // [POLY]
        if (otherObject == null) return;

        // Why check HasMerged() on BOTH objects?
        if (hasMerged || otherObject.HasMerged()) return;

        if (GetTier() == otherObject.GetTier())             // [POLY]
        {
            if (GetMergeResultTier() != -1)                 // [POLY]
            {
                SetMerged();
                otherObject.SetMerged();

                OnMerge();                                  // [POLY] each class's own effect
                otherObject.OnMerge();

                // Why hand the merge to GameManager instead of destroying both objects here?
                GameManager gm = FindAnyObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.MergeObjects(this, otherObject);
                }
            }
        }
    }
}
