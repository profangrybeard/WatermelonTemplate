/*
 * GAME 220: Watermelon Merge Template
 * Session 2: Apple (STUDENT TODO)
 *
 * TEACHING FOCUS:
 * - Practice creating a derived class following Cherry's pattern
 * - Same structure as Cherry, Strawberry, and Grape -- different values
 * - Apple is the first fruit that can ONLY be created by merging (tiers 5+ cannot be dropped)
 *
 * STUDENT TASKS:
 * - Session 2: Complete the Awake() method following Cherry's pattern
 * - Session 4: Optionally add an OnMerge() override for custom effects
 */

using UnityEngine;

// TEACHING: Apple inherits from Fruit, just like Cherry, Strawberry, and Grape.
// The pattern is always the same: override Awake(), set values, call base.Awake().
public class Apple : Fruit
{
    // ===========================================
    // TODO: SESSION 2 - Set Apple's properties
    //
    // Override Awake() and set the following values:
    //   tier = 5
    //   fruitName = "Apple"
    //   pointValue = 21
    //   fruitSize = 1.35f
    //   fruitColor = new Color(0.85f, 0.15f, 0.20f)  // Dark red
    //
    // Then call base.Awake() to run the parent's setup code.
    //
    // PATTERN:
    //   protected override void Awake()
    //   {
    //       // Set your values here
    //       base.Awake();
    //   }
    //
    // Hint: Look at Cherry.cs for the complete pattern to follow.
    // ===========================================


}
