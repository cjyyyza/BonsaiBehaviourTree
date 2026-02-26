# AGENTS.md

## Cursor Cloud specific instructions

### Project Overview

This is **Bonsai Behaviour Tree** — a Unity C# plugin providing a behaviour tree engine and visual editor for the Unity game engine. It is NOT a standalone application; it runs inside Unity Editor/Runtime.

### Directory Structure

| Directory | Purpose |
|-----------|---------|
| `Core/` | Core behaviour tree engine (BehaviourTree, BehaviourNode, BehaviourIterator, Blackboard, etc.) |
| `Standard/` | Standard library of built-in nodes (Composites, Decorators, Tasks, ConditionalAborts) |
| `Editor/` | Unity Editor extension (visual tree designer — cannot compile outside Unity) |
| `Utility/` | Shared utility classes (Timer, FixedSizeStack, UpdateList, etc.) |
| `Tests/` | NUnit tests (require Unity runtime to execute) |

### Build & Lint (outside Unity)

The `.csproj` / `.sln` files added to this repo use `Unity3D.UnityEngine` NuGet stub packages to enable compilation verification without Unity installed.

- **Build**: `dotnet build Bonsai.sln` — builds Core, Standard, and Tests (0 warnings, 0 errors)
- **Lint**: `dotnet format Bonsai.sln --verify-no-changes` — checks formatting against `.editorconfig`
- **Test discovery**: `dotnet test Tests/Bonsai.Tests.csproj` — discovers all 14 NUnit tests

### Key Caveats

1. **Tests cannot execute outside Unity.** All 14 tests are discovered by NUnit but fail at runtime with `SecurityException` because `ScriptableObject.CreateInstance<T>()` is a native Unity method (ECall) not available in the NuGet stub. This is expected. Full test execution requires Unity's Test Runner.

2. **Editor project cannot compile outside Unity.** The `Editor/` directory code requires the full `UnityEditor` assembly (EditorWindow, GenericMenu, PropertyDrawer, etc.) which is not available as a NuGet stub. The `Editor/Bonsai.Editor.csproj` exists but is excluded from `Bonsai.sln`. Compilation of Editor code can only be verified inside Unity.

3. **Code style uses 2-space indentation and UTF-8 with BOM.** The `.editorconfig` is configured to match these conventions.

4. **.NET SDK 8.0 is required** for the `dotnet build` / `dotnet format` / `dotnet test` commands.
