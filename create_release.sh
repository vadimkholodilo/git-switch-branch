#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<EOF
Usage: $0 <major|minor|patch> [--push]

Creates a new annotated git tag with format V<major>.<minor>.<patch> by
reading the latest existing tag that matches Vx.x.x and incrementing the
requested component. If --push is provided, the new tag will be pushed to
origin.
EOF
}

if [ "$#" -lt 1 ]; then
  usage
  exit 2
fi

BUMP="$1"
PUSH=false
if [ "${2:-}" = "--push" ] || [ "${3:-}" = "--push" ]; then
  PUSH=true
fi

# ensure we're in a git repo
if ! git rev-parse --git-dir >/dev/null 2>&1; then
  echo "Not a git repository (or no .git directory found)." >&2
  exit 3
fi

# get latest tag matching Vx.x.x using git tag
LATEST_TAG=$(git tag --list 'V*.*.*' --sort=-v:refname | head -n1 || true)

if [ -z "$LATEST_TAG" ]; then
  echo "No existing Vx.x.x tags found. Starting from V0.0.0"
  MAJOR=0
  MINOR=0
  PATCH=0
else
  # strip leading V
  VER=${LATEST_TAG#V}
  IFS='.' read -r MAJOR MINOR PATCH <<<"$VER"
  MAJOR=${MAJOR:-0}
  MINOR=${MINOR:-0}
  PATCH=${PATCH:-0}
fi

case "$BUMP" in
  major)
    MAJOR=$((MAJOR + 1))
    MINOR=0
    PATCH=0
    ;;
  minor)
    MINOR=$((MINOR + 1))
    PATCH=0
    ;;
  patch)
    PATCH=$((PATCH + 1))
    ;;
  *)
    echo "Invalid bump type: $BUMP" >&2
    usage
    exit 2
    ;;
esac

NEW_TAG="V${MAJOR}.${MINOR}.${PATCH}"

echo "Creating tag $NEW_TAG (previous: ${LATEST_TAG:-none})"

git tag -a "$NEW_TAG" -m "Release $NEW_TAG"

echo "Tag $NEW_TAG created locally."
if [ "$PUSH" = true ]; then
  echo "Pushing tag $NEW_TAG to origin..."
  git push origin "$NEW_TAG"
  echo "Pushed $NEW_TAG"
else
  echo "Tag not pushed. To push: git push origin $NEW_TAG or run with --push"
fi
