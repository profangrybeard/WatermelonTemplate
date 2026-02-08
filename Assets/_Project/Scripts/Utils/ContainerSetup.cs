/*
 * GAME 220: Watermelon Merge Template
 * Session 1: Container Setup (PRE-BUILT)
 *
 * TEACHING FOCUS:
 * - BoxCollider2D for static walls
 * - Programmatic creation of game boundaries
 * - Inspector-tunable dimensions
 *
 * This script creates a box container with three walls (left, right, bottom)
 * and an open top for fruits to drop into. Walls are created automatically
 * when the scene starts, so you don't need to manually build them.
 *
 * STUDENT TASKS:
 * - This file is fully pre-built. Students do NOT need to modify it.
 * - Students can adjust container dimensions in the Inspector if desired.
 */

using UnityEngine;

public class ContainerSetup : MonoBehaviour
{
    // ============================================
    // CONTAINER DIMENSIONS
    // ============================================

    [Header("Container Dimensions")]
    [Tooltip("Internal width of the container")]
    public float width = 6f;

    [Tooltip("Internal height of the container")]
    public float height = 8f;

    [Tooltip("Thickness of wall colliders")]
    public float wallThickness = 0.5f;

    [Header("Visual Settings")]
    [Tooltip("Color of the wall sprites")]
    public Color wallColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    [Header("Physics Material (Optional)")]
    [Tooltip("Assign a PhysicsMaterial2D for custom friction/bounce on walls")]
    public PhysicsMaterial2D wallMaterial;


    // ============================================
    // UNITY LIFECYCLE
    // ============================================

    void Awake()
    {
        // Create three walls: left, right, and bottom
        // The top is open so fruits can be dropped in

        // Left wall
        CreateWall(
            "LeftWall",
            new Vector3(-(width / 2f) - (wallThickness / 2f), 0f, 0f),
            new Vector2(wallThickness, height)
        );

        // Right wall
        CreateWall(
            "RightWall",
            new Vector3((width / 2f) + (wallThickness / 2f), 0f, 0f),
            new Vector2(wallThickness, height)
        );

        // Bottom wall (extends under the side walls for a clean corner)
        CreateWall(
            "BottomWall",
            new Vector3(0f, -(height / 2f) - (wallThickness / 2f), 0f),
            new Vector2(width + (wallThickness * 2f), wallThickness)
        );
    }


    // ============================================
    // WALL CREATION
    // ============================================

    // Cached sprite shared by all walls (created once in the first CreateWall call)
    private Sprite wallSprite;

    /// <summary>
    /// Creates a single wall as a child GameObject with a BoxCollider2D and SpriteRenderer.
    /// </summary>
    void CreateWall(string wallName, Vector3 localPosition, Vector2 size)
    {
        // Create a new child GameObject for the wall
        GameObject wall = new GameObject(wallName);
        wall.transform.parent = transform;
        wall.transform.localPosition = localPosition;

        // Scale the wall to the desired size
        // The collider and sprite are both 1x1 in local space,
        // so localScale controls the actual dimensions of both.
        wall.transform.localScale = new Vector3(size.x, size.y, 1f);

        // Add a BoxCollider2D for physics (1x1 local, scaled by transform)
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();

        // Apply physics material if one is assigned
        if (wallMaterial != null)
        {
            collider.sharedMaterial = wallMaterial;
        }

        // Add a SpriteRenderer for visual representation
        SpriteRenderer renderer = wall.AddComponent<SpriteRenderer>();
        renderer.color = wallColor;

        // Create the shared sprite once, reuse for all walls
        if (wallSprite == null)
        {
            wallSprite = CreateSquareSprite();
        }
        renderer.sprite = wallSprite;

        // Set sorting order behind fruits
        renderer.sortingOrder = -1;
    }

    /// <summary>
    /// Creates a simple 1x1 white square sprite for wall rendering.
    /// </summary>
    Sprite CreateSquareSprite()
    {
        // Create a 4x4 white texture (small but sufficient for a solid color)
        Texture2D texture = new Texture2D(4, 4);
        Color[] pixels = new Color[16];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        // Create a sprite from the texture
        return Sprite.Create(
            texture,
            new Rect(0, 0, 4, 4),
            new Vector2(0.5f, 0.5f),
            4f // Pixels per unit -- 4 pixel texture / 4 ppu = 1 unit
        );
    }


    // ============================================
    // EDITOR VISUALIZATION
    // ============================================

    void OnDrawGizmos()
    {
        // Draw the container outline in the editor
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        // Left wall
        Gizmos.DrawWireCube(
            transform.position + new Vector3(-(width / 2f) - (wallThickness / 2f), 0f, 0f),
            new Vector3(wallThickness, height, 0.1f)
        );

        // Right wall
        Gizmos.DrawWireCube(
            transform.position + new Vector3((width / 2f) + (wallThickness / 2f), 0f, 0f),
            new Vector3(wallThickness, height, 0.1f)
        );

        // Bottom wall
        Gizmos.DrawWireCube(
            transform.position + new Vector3(0f, -(height / 2f) - (wallThickness / 2f), 0f),
            new Vector3(width + (wallThickness * 2f), wallThickness, 0.1f)
        );

        // Draw the open top boundary (dashed line suggestion)
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawLine(
            transform.position + new Vector3(-(width / 2f), height / 2f, 0f),
            transform.position + new Vector3(width / 2f, height / 2f, 0f)
        );
    }
}
