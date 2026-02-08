/*
 * GAME 220: Watermelon Merge Template
 * Session 1: Grape (COMPLETE REFERENCE #3)
 *
 * TEACHING FOCUS:
 * - Third example of the derived class pattern
 * - By now the pattern should be clear: override Awake(), set values, call base.Awake()
 *
 * STUDENT TASKS:
 * - Session 1: You've now seen THREE examples (Cherry, Strawberry, Grape)
 * - Session 2: Create Orange, Dekopon, and Apple following this same pattern
 */

using UnityEngine;

// TEACHING: Third derived class. Same pattern. Different values.
// If you understand Cherry, Strawberry, and Grape, you can write any fruit.
public class Grape : Fruit
{
    protected override void Awake()
    {
        tier = 2;
        fruitName = "Grape";
        pointValue = 6;
        fruitSize = 0.8f;
        fruitColor = new Color(0.55f, 0.27f, 0.68f); // Purple

        base.Awake();
    }
}
