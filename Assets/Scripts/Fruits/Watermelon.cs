/*
 * GAME 220: Watermelon Merge Template
 * Session 3: Watermelon (STUDENT TODO -- SPECIAL CASE)
 *
 * TEACHING FOCUS:
 * - Same derived class pattern as all other fruits
 * - PLUS: Overriding a SECOND virtual method (GetMergeResultTier)
 * - Understanding when to return a special value
 *
 * Watermelon is the FINAL fruit in the merge chain.
 * It cannot merge into anything higher, so GetMergeResultTier() returns -1.
 *
 * STUDENT TASKS:
 * - Session 3: Complete Awake() AND override GetMergeResultTier()
 * - Session 4: Optionally add an OnMerge() override for custom effects
 */

using UnityEngine;

public class Watermelon : Fruit
{
    // ===========================================
    // TODO: SESSION 3 - Set Watermelon's properties
    //
    // Override Awake() and set the following values:
    //   tier = 10
    //   fruitName = "Watermelon"
    //   pointValue = 66
    //   fruitSize = 2.5f
    //   fruitColor = new Color(0.20f, 0.75f, 0.20f)  // Dark green
    //
    // Then call base.Awake() to run the parent's setup code.
    //
    // BONUS: Watermelon is the FINAL fruit in the merge chain.
    // Two Watermelons should NOT merge into anything.
    // Override GetMergeResultTier() to return -1.
    //
    //   public override int GetMergeResultTier()
    //   {
    //       return -1;  // No further merge possible
    //   }
    //
    // This is the ONLY fruit that needs to override GetMergeResultTier().
    // All other fruits use the default (tier + 1) from the base class.
    // ===========================================


}
