# Build the Game Scene

**GAME 220 — Merge Remix · Inheritance & Polymorphism**

You have the scripts. You do not have the game yet. In this session you will assemble the
playable scene: a container, three merge objects, and the controller that drops them.

**Time:** about 30 minutes. Work in order — later parts depend on earlier ones.

When you are finished you will have a working merge game. Everything you build for the rest of
the term hangs off it.

---

## What is already in the scene

Open `Assets/_Project/Scenes/GameScene.unity`. Three things are waiting for you:

| Object | What it is | Leave it alone? |
|---|---|---|
| **Main Camera** | Orthographic, size 6, at `(0, 0, -10)` | Yes |
| **Global Light 2D** | Lights every sprite in the scene | Yes — see below |
| **Managers** | Carries `GameManager` + `MergeObjectFactory` | Yes, but you will fill in its Inspector slots in Part 6 |

> **Do not delete the Global Light 2D, and do not add a second GameManager.**
>
> This project renders with URP's 2D renderer, and every sprite you create gets the
> `Sprite-Lit-Default` material. A lit sprite in a scene with no light draws **black**. The
> light is already there so you never see that — but if your objects suddenly go black, this
> is why.
>
> A second GameManager causes a subtler problem: `MergeObject` finds the manager with
> `FindAnyObjectByType<GameManager>()`, which may pick the wrong one. Your score would then
> climb on a manager nothing else is watching.

---

## Part 1 — Build the container

Three walls, each an empty GameObject with a **Box Collider 2D**. No scripts.

| Name | Position | Box Collider 2D → Size |
|---|---|---|
| `LeftWall` | `(-3.25, 0, 0)` | `(0.5, 10)` |
| `RightWall` | `(3.25, 0, 0)` | `(0.5, 10)` |
| `BottomWall` | `(0, -5.25, 0)` | `(7, 0.5)` |

For each: **right-click in the Hierarchy → Create Empty**, rename it, set the position, then
**Add Component → Box Collider 2D** and set the size.

Optionally add a **Sprite Renderer** with a square sprite and a dark grey colour so you can see
the walls. This is cosmetic — the collider does the work.

**Why these numbers:** the walls are 0.5 thick and centred at ±3.25, so their inner faces sit
at **±3.0**. That gives a container **6 units wide**. The DropController clamps your aim to
±2.5, which keeps objects clear of the walls as they fall.

> ### Checkpoint
> In the Scene view you should see three green collider outlines forming an open-topped box.
> Nothing renders in Game view yet — that is correct.

---

## Part 2 — Make the circle sprite

Merge objects need a round sprite. **Aim for one that is about 1 world unit across.**

1. In the Project window: **Assets → Create → 2D → Sprites → Circle**
2. Save it into `Assets/_Project/Sprites/` (create the folder if it does not exist)

**Why 1 unit matters more than it sounds.** Each derived class sets an `objectSize`
(`0.5`, `0.65`, `0.8`) which is applied as the object's scale. Your sprite's own size is the
base that all of those multiply.

| Base sprite | Objects end up | Result |
|---|---|---|
| **~1 unit** | 0.5 – 0.8 units | About 7 across the container. **Right.** |
| ~0.3 units | 0.15 – 0.24 units | Tiny specks; stacking takes forever |
| ~2.5 units | 1.25 – 2.0 units | Two or three fill the box; the game ends immediately |

> **Do not use the built-in `Knob` sprite.** It imports at roughly 0.3 units — the middle row
> of that table. If you draw your own instead, match Pixels Per Unit to the image width
> (128 px at 128 PPU = exactly 1 unit).

> ### Checkpoint
> Drag the sprite into the scene at scale 1 and compare it to your container. It should be
> roughly **one sixth** of the container's width. Delete it again once you have looked.

---

## Part 3 — Build the TierZero prefab

This is the pattern you will repeat for every merge object you ever make. Learn it here.

**The order of these steps matters.** Do not skip ahead.

1. **Create Empty**, rename to `TierZero`, position `(0, 0, 0)`
2. **Add Component → Sprite Renderer** → assign your circle sprite
3. **Add Component → Rigidbody 2D**
   - Body Type: `Dynamic`
   - Gravity Scale: `1` · Mass: `1`
   - Linear Drag: `0.5` · Angular Drag: `0.5`
   - Collision Detection: `Continuous`
4. **Add Component → Circle Collider 2D**
   - **Do not type a radius.** Unity fits it to the sprite automatically. Leave the number it
     computes.
   - **Is Trigger: UNCHECKED**
5. **Add Component → TierZero** (the script)
6. Drag the object from the Hierarchy into `Assets/_Project/Prefabs/MergeObjects/`
7. **Delete it from the Hierarchy.** The prefab is saved; the scene copy is not needed.

### Why the order matters

**Sprite before collider.** Unity auto-fits a new Circle Collider 2D to the sprite already on
the object. Add the collider first and it has nothing to measure, so you get a radius that has
nothing to do with what you see. A mismatched collider makes objects overlap before merging,
or float apart with a visible gap — which looks exactly like a broken merge system, but isn't.

**Is Trigger must stay unchecked.** Merging is detected in `OnCollisionEnter2D`, which triggers
do not produce. Objects also need solid colliders to physically stack on each other.

**Continuous collision detection.** These objects are small and fall fast. On the default
setting they can pass straight through a wall between two frames.

### Two things that look wrong but are not

- **The prefab is white in the editor.** `TierZero` sets its colour at runtime, in `Start()`.
  It turns red when you press Play.
- **Nothing warns you about missing components.** `MergeObject` has no `[RequireComponent]`.
  Forget the Rigidbody 2D and the object simply never falls — silently, with no error. If an
  object hangs in mid-air, check that first.

> ### Checkpoint
> `TierZero.prefab` sits in `Assets/_Project/Prefabs/MergeObjects/`. Select it and confirm the
> green collider gizmo **hugs the circle** rather than sitting inside or outside it.

---

## Part 4 — TierOne and TierTwo

Now the short version. For each of `TierOne` and `TierTwo`:

1. Select `TierZero.prefab` → **Ctrl+D** to duplicate
2. Rename the copy (`TierOne`, then `TierTwo`)
3. **Double-click** it to open the prefab
4. **Remove** the `TierZero` script component
5. **Add Component** → the matching script (`TierOne` / `TierTwo`)
6. Click the **back arrow** to exit

### Stop and notice what you did *not* do

You did not change the size. You did not change the colour. You did not change the point value.
You swapped **one script** and everything else followed.

| | TierZero | TierOne | TierTwo |
|---|---|---|---|
| Tier | 0 | 1 | 2 |
| Points | 1 | 3 | 6 |
| Size | 0.5 | 0.65 | 0.8 |
| Colour | red | blue | green |

Every one of those values lives in the derived class, not in the prefab. All three prefabs are
otherwise identical because all three classes inherit the same components and the same
behaviour from `MergeObject`.

**That is the whole point of the unit.** Hold on to this feeling when you write your own class
in the next session — you will do step 4 above and nothing else.

> ### Checkpoint
> Three prefabs in `Assets/_Project/Prefabs/MergeObjects/`, each with exactly **one**
> MergeObject-derived script on it. Two scripts on one prefab will merge things twice.

---

## Part 5 — Create the DropController

1. **Create Empty**, rename to `DropController`, position `(0, 0, 0)`
2. **Add Component → DropController**
3. Confirm the values: Drop Cooldown `0.5`, Drop Line Y `4`, Min X `-2.5`, Max X `2.5`,
   Max Drop Tier `2`

### Check the Input section before you move on

Expand **Input** in the Inspector. Two actions should already be filled in:

| Action | Type | Binding |
|---|---|---|
| Aim | Value (Vector2) | `<Mouse>/position` |
| Drop | Button | `<Mouse>/leftButton` |

Those defaults come from the script, so they appear the moment you add the component.

> **If either binding list is empty, stop and fix it now.** An action with no binding listens
> to nothing, and an action that is never enabled fails **silently** — no error, no warning,
> input just does nothing. See `TROUBLESHOOTING.md` issue 16.

---

## Part 6 — Wire everything together

### On `Managers` → MergeObjectFactory

| Slot | Prefab |
|---|---|
| Element 0 | TierZero |
| Element 1 | TierOne |
| Element 2 | TierTwo |
| Elements 3–4 | leave empty |

**The slot index *is* the tier number.** The factory looks up prefabs by indexing this array
with a tier. Put TierOne in slot 2 and the merge chain breaks in a way that is very hard to
see. This array is the merge chain.

### On `Managers` → GameManager

- Object Factory → drag the **Managers** object
- Drop Controller → drag the **DropController** object

### On `DropController` → DropController

- Object Factory → drag the **Managers** object
- Game Manager → drag the **Managers** object

Then **save the scene** (Ctrl+S).

---

## Part 7 — Play it

Press **Play** and work down this list.

- [ ] Console prints `Merge Template r10`
- [ ] A **red** circle floats at the top and follows your mouse
- [ ] Left-click drops it — it falls, lands, and stays inside the container
- [ ] Objects land **on** the floor, not through it
- [ ] Two TierZeros touching merge into one **blue** TierOne, console logs `+2 points`
- [ ] Two TierOnes merge into a **green** TierTwo, console logs `+6 points`
- [ ] Stacking above the line triggers `Game Over` after about 2 seconds
- [ ] Pressing **R** restarts the game

### What happens at the top of the chain

Merge two TierTwos and something odd happens: **both objects disappear, you get +12 points, and
nothing takes their place.** The Console says:

```
MergeObjectFactory: No prefab for tier 3. Create the class and prefab, then assign it to slot 3!
```

This is not a bug. `TierTwo` is simply the last class that exists. It still reports "the next
tier is 3" — there is just nothing there yet.

**That warning is your next assignment.** Creating tier 3 is what Part 4's pattern was
preparing you for.

---

## If something is wrong

| What you see | Almost certainly |
|---|---|
| Objects are **black circles** | The Global Light 2D was deleted. Re-add it: **Hierarchy → right-click → Light → Global Light 2D**. Making `objectColor` brighter will not help |
| Objects are **white**, not coloured | You are looking at the prefab in the editor. Colour is applied at runtime — press Play |
| Object **hangs in mid-air**, never falls | No Rigidbody 2D on the prefab. Nothing warns you about this |
| Objects **fall through the floor** | BottomWall position or collider size is wrong, or the object's collider is set to Is Trigger |
| Objects **overlap** or **float apart** before merging | Collider does not match the sprite. Remove the Circle Collider 2D, confirm the sprite is assigned, re-add it, and leave the auto-fitted radius |
| Nothing drops, no errors at all | Input. Check the Aim/Drop bindings, then **Window → Analysis → Input Debugger** — if the actions are not listed while playing, they never enabled |
| `error CS0103: The name 'Input' does not exist` | Working as intended. This project uses the new Input System only. Copy the `InputAction` pattern from `DropController.cs`. Do **not** switch Active Input Handling back to *Both* |
| Objects merge but the **wrong type** appears | Factory slots are out of order. Slot index must equal tier number |
| Two objects spawn on one merge | Two MergeObject-derived scripts on the same prefab |

Fuller explanations for all of these are in `Docs/TROUBLESHOOTING.md`.

---

## What you just built

A merge game whose `GameManager` holds one `List<MergeObject>` containing three different
object types at once, and never once checks which is which.

You wired three prefabs into a factory that returns them all as the same base type. You will
add a fourth next session, and **you will not have to change GameManager, DropController, or
MergeObjectFactory to do it.**

That is what inheritance and polymorphism buy you. The rest of the term is spent proving it.

---

**Our Business is Fun**
