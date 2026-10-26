/*
 * GAME 220: Merge Template -- MergeObject (BASE CLASS)
 * Every merge object (TierZero, TierOne, TierTwo, yours) inherits from this class.
 * Concepts: Docs/INHERITANCE_CONCEPTS.md -- protected, virtual, polymorphism, abstract
 *
 * Session 1: read it   Session 4: override OnMerge()   Session 5 (stretch): make it abstract
 */

using UnityEngine;

public class MergeObject : MonoBehaviour
{
    // ---------- Fields each derived class sets in its Awake() ----------
    // protected = this class + classes that inherit from it

    protected int tier = 0;                      // Position in the merge chain (0 = first)
    protected string objectName = "MergeObject"; // Name shown in debug messages
    protected int pointValue = 0;                // Points scored when this object merges
    protected float objectSize = 1f;             // localScale multiplier
    protected Color objectColor = Color.white;   // SpriteRenderer tint

    // ---------- Cached components ----------
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    private bool hasMerged = false;              // Stops one object merging twice in a frame


    // ---------- Unity lifecycle ----------

    // virtual: derived classes override this to set their values, then call base.Awake()
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        ApplyObjectProperties();
    }

    // Not virtual: set the fields in Awake() and this applies them
    protected void ApplyObjectProperties()
    {
        transform.localScale = Vector3.one * objectSize;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = objectColor;
        }
    }


    // ---------- Virtual getters (polymorphism: the derived class's values come back) ----------

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

    // Default: next tier up. Override and return -1 to make a final tier that can't merge.
    public virtual int GetMergeResultTier()
    {
        return tier + 1;
    }

    // SESSION 4: override this in your classes for custom merge effects
    public virtual void OnMerge()
    {
        Debug.Log($"{objectName} is merging!");
    }


    // ---------- Merge state ----------

    public bool HasMerged()
    {
        return hasMerged;
    }

    public void SetMerged()
    {
        hasMerged = true;
    }


    // ---------- Merge detection (written once, works for every derived type) ----------

    private void OnCollisionEnter2D(Collision2D other)
    {
        MergeObject otherObject = other.gameObject.GetComponent<MergeObject>(); // [POLY] finds any derived type
        if (otherObject == null) return;

        if (hasMerged || otherObject.HasMerged()) return;

        if (GetTier() == otherObject.GetTier())             // [POLY]
        {
            if (GetMergeResultTier() != -1)                 // [POLY] -1 = final tier
            {
                SetMerged();
                otherObject.SetMerged();

                OnMerge();                                  // [POLY] each class's own effect
                otherObject.OnMerge();

                GameManager gm = FindAnyObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.MergeObjects(this, otherObject);     // destroys both, spawns next tier
                }
            }
        }
    }


    // ---------- Physics helpers (used by DropController) ----------

    // false = frozen in place
    public void SetPhysicsEnabled(bool enabled)
    {
        if (rb != null)
        {
            rb.simulated = enabled;
        }
    }

    // true = floats at the aim point (no gravity); false = falls and collides
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
}
