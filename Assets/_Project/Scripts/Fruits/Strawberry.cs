/*
 * GAME 220: Watermelon Merge Template
 * Session 1: Strawberry (COMPLETE REFERENCE #2)
 *
 * TEACHING FOCUS:
 * - Second example of a derived class following Cherry's pattern
 * - Same structure, different values
 * - Reinforces the override + base.Awake() pattern
 *
 * WHAT'S DIFFERENT FROM CHERRY:
 * - tier = 1 (second in the merge chain)
 * - Slightly larger (0.65 vs 0.50)
 * - Worth more points (3 vs 1)
 * - Pink color instead of red
 *
 * STUDENT TASKS:
 * - Session 1: Compare this with Cherry.cs -- notice the pattern
 * - Session 2: Use Cherry and Strawberry as references for your own fruits
 */

using UnityEngine;

// TEACHING: Same pattern as Cherry. Notice how the STRUCTURE is identical --
// only the VALUES change. This is the pattern you'll follow for every fruit.
public class Strawberry : Fruit
{
    protected override void Awake()
    {
        // Strawberry is the second fruit in the merge chain
        tier = 1;
        fruitName = "Strawberry";

        // Worth more points than Cherry (fruits get more valuable as tier increases)
        pointValue = 3;

        // Slightly larger than Cherry
        fruitSize = 0.65f;

        // Pink-red color
        fruitColor = new Color(0.95f, 0.30f, 0.35f);

        // Always call base.Awake() to cache component references
        base.Awake();
    }
}
