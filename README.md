# git-switch-branch

A small CLI tool to quickly switch Git branches by typing a number or searching by name.

This tool helps when your team uses long branch names (for example including task IDs). Instead of copying/pasting long branch names, run the program and pick the branch to checkout. You can also provide a branch (or a fragment of it) on the command line to switch directly.

## Features

- Interactive selection: run without arguments to see numbered branch list, then type a number to switch.
- Direct switch: pass a full branch name to switch immediately.
- Smart search: pass part of a branch name to either:
  - Switch directly if there's exactly one match
  - Get a filtered list if multiple branches match
- Works on macOS, Linux and Windows (prebuilt, self-contained binaries are available in Releases).

## Quick examples

- Interactive mode (shows numbered list):

```bash
git-switch-branch
```

- Search by part of branch name:

```bash
git-switch-branch PROJECT-123
# If only one branch contains PROJECT-123: switches to it
# If multiple matches: shows numbered list of matching branches
```

## Installation

Download the latest pre-built binary for your platform from the Releases page and place the executable in a folder on your PATH (for example `/usr/local/bin` on macOS/Linux or a user-local `bin` folder on Windows).

## Building from source

Requires .NET 9.0 SDK:

```bash
# build and test
dotnet build && dotnet test
```

## Contributing

1. Fork and branch
2. Ensure tests pass (`dotnet test`)
3. Format code (`dotnet format`)
4. Open PR with clear description

## License

See `LICENSE` file.
