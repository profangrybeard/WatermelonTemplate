# Migration: Unity 6.5 · URP · Input System

Companion doc for bringing this template in line with PeggleTemplate, which has already
been through this migration and tested in-editor.

Full playbook (both templates, side by side):
https://claude.ai/code/artifact/3480539e-c224-4a43-a2ff-c683f936ab6b

---

## Status: COMPLETE

Migration finished and compile-verified 26 Aug 2026. The steps below are kept as the record
of what was done and why.

| | Before (25 Aug) | Now |
|---|---|---|
| Unity | `6000.0.63f1` | `6000.5.9f1` |
| Active Input Handling | `2` (Both) | `1` (New only) |
| Render pipeline | Built-In | URP 17.5.0, **2D renderer** (`Renderer2DData`) |
| Input System package | `1.16.0` | `1.20.0` |
| URP package | not installed | `17.5.0` |
| TMP `Examples & Extras` | absent | absent |
| EventSystem | none | none — still no Canvas |
| Legacy input call sites | 3 | **0** |
| Build Settings scene | correct | correct |

**Verification.** Unity 6.5 was run headless against the project and compiled
`Assembly-CSharp.dll` with **zero errors and zero warnings** under New-only input handling:

    Unity.exe -batchmode -quit -nographics -projectPath <project> -logFile compile.log

**Gameplay is NOT verified.** The scene has a camera and nothing else, so there is nothing to
press Play on. Merge, stack and settle behaviour under Unity 6.5's new 2D physics module is
still untested — see the warning in step 2. That test has to wait until
`Docs/SETUP_INSTRUCTIONS.md` has been worked through.

**This project was the easier of the two.** SlitherTemplate carries TMP's sample folder,
which has 41 legacy `Input.` calls and blocks the New-only switch until it is deleted. That
folder is not here, so that whole step was skipped.

Scripts live under `Assets/_Project/Scripts/`, not `Assets/Scripts/`.

---

## Legacy input in this project — all three migrated

| File | Was | Now |
|---|---|---|
| `Managers/GameManager.cs` | `Input.GetKeyDown(KeyCode.R)` | `restartAction.WasPressedThisFrame()` |
| `Player/DropController.cs` | `Input.mousePosition` | `aimAction.ReadValue<Vector2>()` |
| `Player/DropController.cs` | `Input.GetMouseButtonDown(0)` | `dropAction.WasPressedThisFrame()` |

`DropController` needs both an aim and a drop action — the same shape as Peggle's
`BallLauncher`, which reads the pointer every frame and fires on a button press. That
migration is done and tested, so it is worth reading
`PeggleTemplate/Assets/Scripts/BallLauncher.cs` side by side with this file.

`aimAction` is a **Value** action bound to `<Mouse>/position`. `dropAction` and
`restartAction` are **Button** actions bound to `<Mouse>/leftButton` and `<Keyboard>/r`.

Peggle had no restart action — that is the one pattern that is new here.

### One deviation from the Peggle pattern: bindings live in code, not YAML

Peggle's scene was already built, so its bindings could be hand-authored into the scene YAML
(step 4 below). **This project's scene is not built** — `SETUP_INSTRUCTIONS.md` has the
instructor create every GameObject from scratch, so there is no YAML to author into. A bare
`[SerializeField] private InputAction aimAction;` would arrive with an *empty binding list*
every time somebody adds the component, which is the silent-dead-input failure by default.

So the default bindings are set with a field initialiser instead:

    [SerializeField]
    private InputAction aimAction =
        new InputAction("Aim", InputActionType.Value, "<Mouse>/position",
                        expectedControlType: "Vector2");

Unity runs field initialisers when a component is first added, so the action shows up in the
Inspector already bound — and is still fully rebindable there. Prefer this shape in any
template whose scene is built by the reader.

---

## The migration, in order

### 1. Close Unity first ✅

Scenes, prefabs and `ProjectSettings/*.asset` are YAML. Editing them while the editor holds
them open means the editor wins on next save. Script and Markdown edits are safe with Unity
running — it just recompiles.

```bash
tasklist //FI "IMAGENAME eq Unity.exe" | grep Unity.exe
```

```bash
ls Temp/UnityLockfile
```

Empty output from the first and a missing lockfile from the second both mean it is safe.
Commit before starting, so every step below is revertible.

### 2. Upgrade to Unity 6000.5.9f1 ✅

Open in 6.5 and let it convert. All of this is expected in `git status`:

- Package version bumps
- `com.unity.modules.physicscore2d` added
- `com.unity.modules.vr` removed
- `com.unity.render-pipelines.universal` added *as a dependency of the 2D feature set*
- New files: `DefaultVolumeProfile.asset`, `UniversalRenderPipelineGlobalSettings.asset`,
  `PhysicsCoreProjectSettings2D.asset`, `ProjectAuditorSettings.asset`

Installing the URP package does **not** switch the renderer. Check what is actually active:

```bash
grep -n "m_CustomRenderPipeline" ProjectSettings/GraphicsSettings.asset
```

`{fileID: 0}` means Built-In. A real guid means URP is live.

> **Watch the 2D physics module.** This is a merge game built on stacking and settling, so
> it leans harder on 2D physics than either of the other templates. Unity 6.5 adds
> `physicscore2d` and a new `PhysicsCoreProjectSettings2D.asset`. Play a full session after
> the upgrade and confirm the fruit still stack, settle and merge the way they did — this
> is the project most likely to feel different afterwards.

### 3. Decide on URP deliberately ✅ — URP taken, 2D renderer

Two traps hit Peggle:

- **Unity's default URP asset is the 3D renderer** (`UniversalRendererData`). A 2D project
  wants `Renderer2DData`, or there is no 2D lighting path at all.
- **Built-In particle materials go magenta.** `Default-Particle` is fileID `10301`.
  `Sprites-Default` (fileID `10754`) still resolves fine under URP.

```bash
grep -rn "fileID: 10301" Assets --include="*.prefab" --include="*.unity"
```

```bash
grep -rn "fileID: 10754" Assets --include="*.prefab" --include="*.unity"
```

The first finds particle materials that will break. The second finds sprites, which are
fine. A merge game often has a burst effect on each merge — if that uses the built-in
particle material, it is exactly what goes magenta.

Staying on Built-In is a legitimate answer. Only take URP if you want 2D lights — and if
you do, use the 2D renderer.

**Outcome here:** URP was taken and the trap was avoided. `Assets/_Project/Render/` holds a
URP asset whose renderer is `Renderer2DData`, and `GraphicsSettings.asset` points at it
(guid `92d052426988a6a48abe47ed1106d2e5`). The magenta-material grep found nothing — the
scene has no particle systems yet, so there was nothing to break.

### 4. Rewrite the input code ✅

The pattern Peggle settled on: one serialized `InputAction` field per verb. Teaches actions
and bindings, stays readable, needs no `.inputactions` asset or `PlayerInput` component.

```csharp
using UnityEngine.InputSystem;

[SerializeField] private InputAction aimAction;      // Value  · Vector2
[SerializeField] private InputAction dropAction;     // Button

private void OnEnable()  { aimAction.Enable();  dropAction.Enable(); }
private void OnDisable() { aimAction.Disable(); dropAction.Disable(); }

Vector2 pointer = aimAction.ReadValue<Vector2>();
bool dropped    = dropAction.WasPressedThisFrame();
```

> **An action that is never enabled fails silently.** No exception, no console warning —
> input just does nothing. Forgetting `OnEnable()` is the most common way this migration
> looks broken.

Use `OnEnable`/`OnDisable`, not `Start`. `Start` runs once ever; an object disabled and
re-enabled would come back deaf.

Bindings live in the prefab or scene YAML. If hand-authoring rather than clicking through
the Inspector, the shape is:

```yaml
  dropAction:
    m_Name: Drop
    m_Type: 1
    m_ExpectedControlType:
    m_Id: <guid>
    m_Processors:
    m_Interactions:
    m_SingletonActionBindings:
    - m_Name:
      m_Id: <guid>
      m_Path: <Mouse>/leftButton
      m_Interactions:
      m_Processors:
      m_Groups:
      m_Action: Drop
      m_Flags: 0
    m_Flags: 0
    m_Priority: 0
```

`m_Type` is `0` for Value, `1` for Button, `2` for PassThrough. Add another `- m_Name:`
block under `m_SingletonActionBindings` for a second device — one action with two bindings
is what lets a single line of code serve both mouse and keyboard.

### 5. Switch Active Input Handling to New ✅

Verify nothing legacy survives **first**:

```bash
grep -rn "Input\.\|KeyCode\." Assets --include="*.cs" | grep -v "InputSystem\|InputAction"
```

That must return nothing. Then:

```bash
sed -i "s/^  activeInputHandler: 2$/  activeInputHandler: 1/" ProjectSettings/ProjectSettings.asset
```

The values are `0` Old, `1` New only, `2` Both.

New-only is the point: pasting `Input.GetKeyDown` from a tutorial now fails to compile
instead of quietly working.

### 6. EventSystem — nothing to fix yet

`GameScene.unity` has no Canvas and no EventSystem at all, so there is nothing to migrate
today. **The first time you add UI**, the EventSystem must use `InputSystemUIInputModule`,
not `StandaloneInputModule`, or the UI will not respond under New-only.

Unity adds the correct one automatically when the Input System package is active, but
verify:

```bash
grep -c "4f231c4fb786f3946a6b90b886c48677" Assets/_Project/Scenes/GameScene.unity
```

```bash
grep -c "01614664b831546d2ae94a42149d80ac" Assets/_Project/Scenes/GameScene.unity
```

The first is the old `StandaloneInputModule` and should stay `0`. The second is
`InputSystemUIInputModule` and should become `1` once UI exists.

---

## Audit checklist

Independent of the migration. Every item below was actually wrong in Peggle.

- [ ] **Debug values left in the scene.** Scene overrides beat script defaults silently.
      Read the serialized values in the scene, not the field initialisers in the `.cs`.
      Worth checking the merge tier thresholds and spawn weights especially.
- [ ] **Components on the instance instead of the prefab.** Peggle's bucket collider lived
      in the scene, so the prefab caught nothing when dragged into a fresh scene. This
      project instantiates fruit prefabs at runtime, so an instance-only component would
      break every spawned object. Look for non-empty `m_AddedComponents:` in scene YAML.
- [ ] **Orphaned property overrides from deleted script fields.** Peggle's scene carried 50
      overrides for a field the script no longer had — that is how the win condition became
      unreachable. Check the `MergeObject` tier prefabs.
- [ ] **Public methods nothing calls.** `grep` each public method name across `Assets/`.
      One hit means only the definition exists, and nothing ever calls it. This is how
      Peggle shipped a game that could not be won.
- [ ] **Docs describing files that do not exist.** Check `README.md` against reality after
      the migration.
- [ ] **Meta files committed**, new Markdown files included.
- [ ] **Git LFS installed before cloning**, not after.
- [ ] **Build Settings** already correct here — re-verify after the upgrade, since Unity
      sometimes rewrites `EditorBuildSettings.asset` during conversion.

---

## Reusable from PeggleTemplate

| File | Reuse |
|---|---|
| `Assets/Scripts/INPUT_SYSTEM.md` | Copy as-is; swap the action names |
| `Assets/Scripts/BallLauncher.cs` | Closest working model for `DropController` — aim + fire |
| `README.md` troubleshooting | Two entries: legacy `Input` not compiling, and silent dead input |
| `STUDENT_GUIDE.md` Week 1 | Input moved to Week 1; the rebind experiment works anywhere |
| `CLAUDE.md` tech stack | Record New-only **and why**, so nobody sets it back to Both |

---

## What the audit turned up

Run against this project on 26 Aug 2026, after the migration.

| Checklist item | Result |
|---|---|
| Debug values left in the scene | **N/A** — scene holds only a camera |
| Components on instance instead of prefab | **Clean** — no `m_AddedComponents:` in the scene |
| Orphaned property overrides | **Clean** — no prefabs exist yet |
| Public methods nothing calls | **5 found** — see below |
| Docs describing files that do not exist | **Several found and fixed** — see below |
| Meta files committed | **Clean** — every asset and Markdown file has one |
| Build Settings | **Correct** — survived the 6.5 conversion |

### Deprecated API caught by the compiler

`MergeObject.cs` used `FindFirstObjectByType<GameManager>()`, which Unity 6.5 deprecates
(CS0618 — it relies on instance-ID ordering). Changed to `FindAnyObjectByType<GameManager>()`.
Note that `IMPLEMENTATION_SUMMARY.md` had specifically praised the old call as "the Unity 6
API, not the deprecated FindObjectOfType" — a doc that was correct when written and went
stale under the upgrade. Exactly the failure mode the audit checklist is looking for.

### Five public methods with no callers

All five are `GameManager` query methods: `CountObjectsOfTier`, `GetTotalObjectPoints`,
`GetHighestTier`, `GetHighestObjectName`, `GetActiveObjectCount`.

**This is not the Peggle bug.** In Peggle a dead public method meant the game could not be
won. Here they are a deliberate Session 3 teaching exercise, and two of them are TODO stubs
students fill in. But the pedagogical smell is real: a student completes
`GetTotalObjectPoints()` and has **no way to see whether it works**, because nothing calls it
and there is no UI. Worth either calling them from a debug `Debug.Log`, or pointing the
Session 6 "Score UI" feature at them. `README.md` now says so.

### Docs that had drifted

- Unity version stated as `6000.0.63f1` in two docs
- Input System stated as `1.16.0`; render pipeline stated as "Built-in 2D"
- Repository path given as `MergeTemplate` (it is `WatermelonTemplate`)
- Script count 7 (actually 8 — `BuildInfo.cs`), line count and comment density both stale
- `README.md` claimed the game "works out of the box" without mentioning that
  `SETUP_INSTRUCTIONS.md` is a required 30-40 minute prerequisite
- No doc anywhere mentioned Input System usage, despite it being the thing most likely to
  block a student

`Assets/_Project/Input/InputSystem_Actions.inputactions` exists but is Unity's stock
first-person sample (Move / Look / Jump / Sprint). **No script references it** — but it is not
fully orphaned either: `EditorBuildSettings.asset` registers it as the project-wide input
actions asset (`com.unity.input.settings.actions`, guid `3590b91b...`). Nothing reads that, so
it is harmless. Deleting the file means clearing that reference under
**Edit > Project Settings > Input System Package** first, or a dangling guid is left behind.
Now documented as such rather than as "unused".

---

## Still open

1. **Gameplay is unverified.** Nothing has been play-tested, because the scene is not built.
   The 2D-physics warning in step 2 still stands: merge, stack and settle behaviour under
   Unity 6.5's `physicscore2d` module needs a full session of play once setup is done.
2. ~~**The scene is a partial setup.**~~ **Resolved 26 Aug.** The stray GameObject literally
   named `GameObject`, carrying a lone `GameManager`, has been renamed to **Managers** and given
   the missing `MergeObjectFactory` component — completing step 4 of `SETUP_INSTRUCTIONS.md`.
   Authored directly in the scene YAML with Unity closed, then verified by opening the scene
   headless: two roots (`Main Camera`, `Managers`), no missing scripts, factory reporting 5
   empty prefab slots. That removes the duplicate-GameManager trap, since the guide now tells
   the reader to verify the existing object rather than create a second one.

   Steps 2 and 4 of the setup guide are therefore pre-done. **Steps 3, 5, 6, 7-10 and 11
   (walls, sprite, three prefabs, DropController, wiring) are still the instructor's job.**
3. **EventSystem.** Still nothing to migrate, and still a trap the first time a Canvas is
   added. `README.md`'s Score UI feature entry now carries the warning.

---

## Status

The Peggle migration these steps describe was **tested in the editor** — it compiled, ran,
and played, hand-authored YAML included. This is a path that has been walked end to end.

This project now **compiles clean** under Unity 6.5 + URP 2D + New-only input, verified
headless. Its merge logic, tier progression and physics tuning remain unexamined at runtime,
for the reason in "Still open" above.
