# Contributing (team workflow)

## Ground rules
- `main` should always build and run.
- Do not commit Unity-generated folders: `Library/`, `Temp/`, `Obj/`, `Build/`.
- Use feature branches and merge via PR when possible.

## Branch naming
- `feature/<thing>` — new feature/mechanic/level
- `fix/<thing>` — bug fix
- `chore/<thing>` — refactor, cleanup, tooling
- `docs/<thing>` — documentation only

## Suggested branch examples for this project
- `feature/level-5`
- `feature/level-6`
- `feature/dialogue-expansion`
- `fix/input-handling`
- `fix/background-rendering`

## Pull Requests
- Keep PRs small and focused.
- Include a short test note:
  - Scene tested
  - Repro steps (if bug fix)
  - Any known issues
