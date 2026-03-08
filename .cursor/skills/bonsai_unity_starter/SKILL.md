---
name: bonsai_unity_starter
description: Minimal starter skill for Cloud agents working in this Bonsai Unity behavior tree codebase. Use it before trying to run the app, open the editor, or test runtime/editor changes.
---

# Bonsai Unity starter skill

Use this skill whenever you need to run, debug, or test code in this repository.

## What this repo is

- This repository is a Unity/C# behavior tree library with a custom Unity editor, not a web app and not a service repo.
- Main code areas:
  - `Core/`: runtime tree engine and `BonsaiTreeComponent`.
  - `Standard/`: built-in composites, decorators, and tasks.
  - `Editor/`: Unity editor tooling, including the Bonsai designer window.
  - `Tests/`: NUnit EditMode tests.
  - `Utility/`: shared helper utilities.
- There is no `package.json`, `docker-compose.yml`, `Makefile`, backend server, or login flow in this repo.

## First thing to check: are you in a real Unity project root?

Do this before trying to launch Unity from `/workspace`.

- A full Unity project root normally contains `ProjectSettings/ProjectVersion.txt` and usually `Packages/manifest.json`.
- This repo does not contain those files, so `/workspace` is likely a package/plugin source folder, not a standalone Unity project.
- If Unity fails to open `/workspace`, do not keep guessing flags. Instead:
  - locate the actual host Unity project that imports this code, or
  - create/use a small host Unity project and place this repo inside it as the Bonsai code folder/package under test.

## Login, auth, and feature flags

### Login / auth

- There is no app login, API auth, or seeded account workflow in this repository.
- The only "login" concern is Unity itself:
  - the machine needs a usable Unity Editor installation;
  - batch/headless test runs may also require an activated/licensed Unity environment.
- Do not waste time looking for repo-specific credentials. None are defined here.

### Feature flags / defines

- There are no product feature flags in the codebase.
- The important build/test gate is `UNITY_INCLUDE_TESTS`.
- If EditMode tests are missing from Test Runner, check that test assemblies are included and that `UNITY_INCLUDE_TESTS` is enabled for the test environment.

## Codebase areas and how to work in each one

### 1) `Core/` and `Standard/`: runtime behavior tree logic

Use this area for:
- tree execution logic;
- node traversal;
- built-in composites/decorators/tasks;
- runtime component behavior.

Fastest validation workflow:
1. Prefer an EditMode NUnit test in `Tests/`.
2. Build trees entirely in memory with `Helper.CreateTree()` and `Helper.CreateNode<T>()`.
3. Execute with `Helper.StartBehaviourTree()`, `Helper.StepBehaviourTree()`, or `Helper.RunBehaviourTree()`.
4. Assert final status, traversal order, utility ordering, or node relationships.

Why this is the default path:
- tests do not need scenes, network calls, or external mock services;
- most runtime logic can be verified without entering Play Mode.

When to use a manual Unity run instead:
- when changing `BonsaiTreeComponent`;
- when validating actual Play Mode behavior on a `GameObject`;
- when checking interaction between runtime state and the visual editor.

### 2) `Editor/`: custom Unity editor tooling

Use this area for:
- Bonsai designer window behavior;
- asset opening/saving behavior;
- editor-only inspectors/preferences/visual workflows.

Primary manual workflow:
1. Open the host Unity project in Unity Editor.
2. Open `Window > Bonsai Designer`.
3. Create a tree asset from `Create > Bonsai > Behaviour Tree`.
4. Double-click the asset to confirm it opens in the designer.
5. Make the editor change under test.
6. If relevant, enter Play Mode and select a `GameObject` with `BonsaiTreeComponent` to verify the window switches into view mode for the running tree.

Use manual editor testing for `Editor/` changes even if code compiles cleanly. Most editor regressions are interaction-level issues.

### 3) `Tests/`: NUnit EditMode verification

Use this area for:
- targeted coverage for runtime logic changes;
- reproduction tests for bugs;
- quick confidence checks before manual editor testing.

Expected test style in this repo:
- NUnit `[Test]` methods;
- EditMode tests only;
- tree setup done in memory via helper methods;
- no dependence on an application login, database, or remote service.

Prefer small, focused tests near the behavior you changed instead of large scene-driven tests.

## How to start and test the codebase

## Quick start checklist

1. Confirm whether you have a real Unity project root.
2. If yes, open that project in Unity Editor.
3. If no, attach this repo to a host Unity project before trying to run anything.
4. For runtime logic changes, start with EditMode tests.
5. For editor UX changes, manually test in Unity Editor.
6. For `BonsaiTreeComponent` or play/view interactions, do a Play Mode check after tests pass.

### Starting the editor

Preferred:
- Open the host Unity project in the Unity Editor UI.

Typical CLI shape if Unity is available:
- `<UnityEditor> -projectPath <unity_project_root>`

Do not hardcode `/workspace` as `-projectPath` unless you have confirmed it is a real Unity project root.

### Running tests

Preferred interactive path:
- open Unity Test Runner;
- run EditMode tests only.

Typical headless CLI shape if Unity is available:
- `<UnityEditor> -batchmode -quit -projectPath <unity_project_root> -runTests -testPlatform EditMode -testResults <output.xml>`

Important notes:
- Run EditMode tests, not PlayMode tests, unless you intentionally add a Play Mode case.
- If no tests appear, check `UNITY_INCLUDE_TESTS` first.

## Practical testing workflows by code area

### Runtime logic change in `Core/` or `Standard/`

Use this workflow:
1. Add or update a focused EditMode test in `Tests/`.
2. Build the tree in memory with `Helper`.
3. Run EditMode tests.
4. If the change affects in-game component behavior, also verify once in Play Mode with a `GameObject` that uses `BonsaiTreeComponent`.

Good examples:
- cloning and parent/child structure checks;
- sequence/selector/pass/fail behavior;
- utility ordering and traversal history assertions.

### Editor change in `Editor/`

Use this workflow:
1. Open Unity Editor on the host project.
2. Open `Window > Bonsai Designer`.
3. Create or open a `BehaviourTree` asset.
4. Exercise the exact interaction you changed.
5. If the change touches Play Mode viewing, enter Play Mode and select the object with `BonsaiTreeComponent`.
6. Capture a screenshot or short video of the working behavior.

### `BonsaiTreeComponent` or runtime/editor integration change

Use this workflow:
1. Add a focused EditMode test if core execution behavior changed.
2. In Unity, create a `BehaviourTree` asset.
3. Add `BonsaiTreeComponent` to a `GameObject`.
4. Assign the tree asset to `TreeBlueprint`.
5. Enter Play Mode.
6. Confirm the tree starts and updates without null errors.
7. Select the object and verify the Bonsai window can view the running tree if your change touches that flow.

## Common pitfalls

- Unity will not launch correctly from `/workspace` if `/workspace` is only the package/plugin folder.
- Looking for npm, pnpm, yarn, Docker, or service credentials is wasted effort in this repo.
- Missing tests in Test Runner usually means the environment is not including tests, not that the test files are broken.
- Editor changes need interaction testing; passing compile checks alone is weak evidence.

## When you discover new runbook knowledge

Update this skill immediately when you learn something reusable, especially:
- the real host project layout used by this repo;
- the exact Unity version/path used in Cloud;
- reliable headless test commands that work in this environment;
- any recurring Test Runner fixes;
- any editor workflow needed to reproduce a bug quickly.

Keep updates practical:
- write the exact command or click path;
- say which code area it applies to;
- include the failure symptom it fixes;
- remove stale or speculative instructions.
