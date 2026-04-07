# Workshop Additions (Thomas Was Alone Demo)

Unity version: **2022.3.45f1** (LTS)

This project was extended with workshop-friendly mechanics **and a fully playable Level4 showcase** (Bounce Pad + Timed Platforms + Checkpoint + Dialogue).

---

## What was added / updated

### 1) Better respawning + checkpoints (smoother deaths)
- **RespawnManager.cs** (updated)
  - Tracks the current player instance (prevents duplicate players after death).
  - Supports *smooth respawn* by re-enabling a disabled player when possible.
  - Adds **SetRespawnPosition(Vector3)** so checkpoints can move the respawn point.
  - Optional: **respawn quips** (loads a DialogueAsset from Resources and shows a quick one-liner on respawn).

- **Checkpoint.cs** (new)
  - Put on a trigger collider. When the Player enters, it updates the RespawnManager’s respawn point.

### 2) New obstacle building blocks
- **KillZone.cs**
  - Trigger collider that kills the player (pit / laser / offscreen).
  - If the player has **BreakIntoPiecesOnDeath**, it uses that effect automatically.

- **BouncePad.cs**
  - Put on a collider (not trigger) to bounce the player upward.

- **TimedPlatform.cs**
  - Toggles renderer + collider on a timer (appears/disappears).

### 3) Dialogue system (works immediately)
- **DialogueAsset.cs** (ScriptableObject with `lines`)
- **DialogueManager.cs**
  - Singleton + **auto-creates a simple TMP UI** at runtime if you did not assign references.
  - Supports full multi-line dialogue **and** quick “one-liner” popups that auto-hide.
- **DialogueTrigger.cs**
  - Trigger collider that starts dialogue.
  - Can reference a DialogueAsset **or** load from **Resources** using a path string.

Dialogue assets are included under:
- **Assets/WorkshopAdditions/Resources/Dialogue/**

---

## Level4 Showcase (already populated)

Open: **Assets/Scenes/Level4.unity**

What you’ll find near the start:
- **BouncePad_1** (launches you upward)
- **TimedPlatform_1..3** (appear/disappear on staggered timers)
- Dialogue triggers that fire at:
  - intro
  - bounce pad
  - timed platform area
  - checkpoint area
  - goal area
- Existing:
  - Checkpoint + KillZone pit

---

## Quick usage notes

### Adding dialogue to any level (fast)
1. Create an empty GameObject with **DialogueManager.cs** (optional — triggers can also auto-create it).
2. Create a trigger collider object and add **DialogueTrigger.cs**.
3. Set:
   - `dialogueResourceName` to something like `Dialogue/Goal_Goodbye`
   - (or assign a DialogueAsset directly)
4. Choose whether it is:
   - One-liner mode (auto-hide)
   - Full dialogue (press **E** / **Submit** to advance)

### Creating your own DialogueAsset
1. Right-click in Project window → **Create → Dialogue → Dialogue Asset**
2. Put it in `Assets/WorkshopAdditions/Resources/Dialogue/`
3. Name it (e.g. `MyNewDialogue.asset`)
4. Use it from triggers via `Dialogue/MyNewDialogue`

---

Happy building. Break things safely. :)
