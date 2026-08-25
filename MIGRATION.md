# Migration: Unity 6.5 · URP · Input System

Companion doc for bringing this template in line with PeggleTemplate, which has already
been through this migration and tested in-editor.

Full playbook (both templates, side by side):
https://claude.ai/code/artifact/3480539e-c224-4a43-a2ff-c683f936ab6b

---

## Where this project stands

Read from disk, 25 Aug 2026.

| | |
|---|---|
| Unity | `6000.0.63f1` — needs upgrade |
| Active Input Handling | `2` (Both) — needs to become `1` (New only) |
| Render pipeline | Built-In (`m_CustomRenderPipeline: {fileID: 0}`) |
| Input System package | `1.16.0` |
| URP package | not installed |
| TMP `Examples & Extras` | absent ✅ — nothing blocking New-only |
| EventSystem | none — no Canvas in the scene yet |
| Legacy input call sites | 3 |
| Build Settings scene | correct ✅ (`Assets/_Project/Scenes/GameScene.unity`) |

**This project is the easier of the two.** SlitherTemplate carries TMP's sample folder,
which has 41 legacy `Input.` calls and blocks the New-only switch until it is deleted. That
folder is not here, so that whole step is skipped.

Scripts live under `Assets/_Project/Scripts/`, not `Assets/Scripts/`.

---

## Legacy input in this project

| File · line | Call | Becomes |
|---|---|---|
| `Assets/_Project/Scripts/Managers/GameManager.cs:80` | `Input.GetKeyDown(KeyCode.R)` | `restartAction.WasPressedThisFrame()` |
| `Assets/_Project/Scripts/Player/DropController.cs:122` | `Input.mousePosition` | `aimAction.ReadValue<Vector2>()` |
| `Assets/_Project/Scripts/Player/DropController.cs:131` | `Input.GetMouseButtonDown(0)` | `dropAction.WasPressedThisFrame()` |

`DropController` needs both an aim and a drop action — the same shape as Peggle's
`BallLauncher`, which reads the pointer every frame and fires on a button press. That
migration is done and tested, so it is worth reading
`PeggleTemplate/Assets/Scripts/BallLauncher.cs` side by side with this file.

`aimAction` is a **Value** action bound to `<Mouse>/position`. `dropAction` and
`restartAction` are **Button** actions bound to `<Mouse>/leftButton` and `<Keyboard>/r`.

Peggle had no restart action — that is the one pattern that is new here.

---

## The migration, in order

### 1. Close Unity first

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

### 2. Upgrade to Unity 6000.5.9f1

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

### 3. Decide on URP deliberately

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

### 4. Rewrite the input code

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

### 5. Switch Active Input Handling to New

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

## Status

The Peggle migration these steps describe was **tested in the editor** — it compiled, ran,
and played, hand-authored YAML included. This is a path that has been walked end to end.

Nothing about this project's actual gameplay has been reviewed — only versions, packages,
render pipeline, input call sites, scenes and build settings. The merge logic, tier
progression and physics tuning are all unexamined.
