# Branching workflow (GitHub Desktop friendly)

This matches the workflow described in the module guide:
- Avoid pushing directly to **main**
- Create a new branch per feature/task
- Push the branch
- Merge into main once tested

## 1) Create & publish a feature branch
**GitHub Desktop**
1. Open the repository.
2. At the top bar, click **Current branch** → **New branch**.
3. Name it using one of these patterns:
   - `feature/<short-name>` (new mechanic/level/system)
   - `fix/<short-name>` (bug fix)
   - `docs/<short-name>` (documentation only)
4. Click **Create branch**.
5. Click **Publish branch** (pushes it to GitHub).

## 2) Commit often (small commits)
- Keep commit messages short and specific:
  - ✅ `Add Level 7 secret portal`
  - ✅ `Fix respawn duplication`
  - ✅ `Tweak platform timings in Level 5`

## 3) Merge back into main (when stable)
Option A — **Preferred (Pull Request)**
1. Push your branch.
2. On GitHub.com, open a Pull Request from your branch → `main`.
3. Review changes, resolve conflicts if any, then **Merge**.

Option B — **Local merge in GitHub Desktop**
1. Switch to `main`.
2. **Branch → Merge into current branch…**
3. Choose your feature branch.
4. Push `main`.

## 4) Rules to avoid conflicts (Unity-specific)
- Do not work on the **same scene/prefab** in two branches at the same time.
- Keep “big binary assets” on one branch at a time.
- When possible, use prefabs and additive scenes to reduce merge pain.

## 5) If you use Git LFS (recommended for large assets)
Install Git LFS and then uncomment the LFS lines in `.gitattributes`.
