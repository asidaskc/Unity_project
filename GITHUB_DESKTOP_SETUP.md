# GitHub setup steps (quick)

These steps mirror the tutorial you were given for GitHub Desktop.

## Create a repository (GitHub Desktop)
1. File → **New repository**
2. Choose a name (e.g., `my-unity-game`)
3. **.gitignore:** select **Unity**
4. Create repository
5. Click **Publish repository** (set it to Private if needed)

## Add your Unity project
**Best practice:** place the Unity project at the repository root.

If you already created the repo and your Unity project is inside a subfolder, move the
`.gitignore` into the Unity project folder so Unity-generated files are excluded there too.

## First commit
1. Back in GitHub Desktop, confirm you see changes.
2. Write a clear commit message, e.g. `Initial Unity project import`
3. **Commit to main**
4. **Push origin**

## Add collaborators (GitHub website)
Go to GitHub.com → your repo → **Settings** → **Collaborators** → invite by username/email.
