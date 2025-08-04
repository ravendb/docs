#!/usr/bin/env python3
"""build_whats_new.py – regenerate RavenDB *What's New* pages

The script lives in the project's **/scripts** folder, therefore the Docusaurus
root is assumed to be its parent directory (``../``).

What the script does
--------------------
1. **Downloads** changelog entries for one or more RavenDB branches via the
   public Documentation API.
2. **Converts** each entry's HTML body to Markdown using *markdownify*.
3. **Sorts** the entries strictly by their ``buildDate`` field (newest → oldest).
4. **Writes** them to ``whats-new.mdx`` files with front‑matter already in place.

File locations
--------------
* The branch that the API reports as **latest** is considered *primary* and
  written to:

    ``<docs_root>/docs/whats-new.mdx``

* All other branches are written to:

    ``<docs_root>/versioned_docs/version-<branch>/whats-new.mdx``

Environment variable
--------------------
Set your RavenDB docs API key in ``RAVENDB_API_KEY`` before running.

Examples
--------
    # Rebuild for the currently latest branch only
    python build_whats_new.py 6.2

    # Rebuild for multiple branches
    python build_whats_new.py 6.2 6.0 5.4
"""

from __future__ import annotations

import os
import re
import sys
from datetime import datetime
from pathlib import Path
from typing import List, Dict, Any

import requests
from markdownify import markdownify as md

# ============================================================================
# Configuration & paths
# ============================================================================

API_BASE_URL: str | None = os.environ.get("API_WEB_RAVENDB_NET_HOST") + "/api/v1/documentation/whats-new"

if not API_BASE_URL:
    raise EnvironmentError("Environment variable 'API_WEB_RAVENDB_NET_HOST' is not set.")

SCRIPT_DIR = Path(__file__).resolve().parent
PROJECT_ROOT = SCRIPT_DIR.parent  # «../» relative to /scripts

# Docusaurus front‑matter block that prefixes every generated MDX file
FRONT_MATTER = (
    "---\n"
    'title: "What\'s New"\n'
    "breadcrumbs: false\n"
    "pagination_next: null\n"
    "pagination_prev: null\n"
    "hide_table_of_contents: true\n"
    "---\n\n"
)

# Regex that replaces markdownify's three‑dash <hr> (---) with MDX‑safe <hr/>
HR_MARKDOWN = re.compile(r"^-{3,}$", re.MULTILINE)

# Date format used by the API – helper for ``datetime.strptime``
API_DATE_FMT = "%m/%d/%Y"

# ============================================================================
# REST helpers
# ============================================================================

def get_api_page(branch: str, page: int = 1) -> Dict[str, Any]:
    """Return a single paginated payload from the Documentation API."""
    response = requests.get(
        API_BASE_URL,
        headers={
            "Accept": "application/json",
        },
        params={"version": branch, "page": page},
        timeout=20,
    )

    if response.status_code == 404:
        sys.exit(f"Branch '{branch}' not found – check availableVersionsAndKeys in the API.")

    response.raise_for_status()
    return response.json()


def api_latest_branch() -> str:
    """The newest branch announced by the API (e.g. "6.2")."""
    probe = get_api_page("6.0", 1)  # 6.0 endpoint is guaranteed to exist
    return sorted(probe["availableVersionsAndKeys"])[-1]


def fetch_branch_entries(branch: str) -> List[Dict[str, Any]]:
    """Download *all* changelog entries for a given branch.

    The API is paginated – this helper stitches the pages into one list.
    """
    first_page = get_api_page(branch, 1)
    entries: List[Dict[str, Any]] = first_page["entries"]

    for page_no in range(2, first_page["totalPages"] + 1):
        entries.extend(get_api_page(branch, page_no)["entries"])

    return entries

# ============================================================================
# Conversion helpers
# ============================================================================

def mdx_heading(entry: Dict[str, Any]) -> str:
    """Create a level‑2 MDX heading from an API entry."""
    date_str = datetime.strptime(entry["buildDate"], API_DATE_FMT).strftime("%Y/%m/%d")
    return f"## {entry['version']} - {date_str}\n\n"


def mdx_block(entry: Dict[str, Any]) -> str:
    """Full MDX chunk for a single changelog entry (heading + body)."""
    return mdx_heading(entry) + entry["changelogMarkdown"]

# ============================================================================
# Filesystem helpers
# ============================================================================

def output_path_for(branch: str, is_primary: bool) -> Path:
    """Return where the *whats‑new.mdx* for *branch* should live."""
    # We only need major.minor for the directory name – e.g. "6.2.1" → "6.2"
    major_minor = ".".join(branch.split(".")[:2])

    if is_primary:
        return PROJECT_ROOT / "docs" / "whats-new.mdx"

    return PROJECT_ROOT / "versioned_docs" / f"version-{major_minor}" / "whats-new.mdx"


def write_whats_new_file(destination: Path, entries: List[Dict[str, Any]]) -> None:
    """Write an MDX file sorted by *buildDate* (newest first)."""
    destination.parent.mkdir(parents=True, exist_ok=True)

    entries.sort(
        key=lambda e: datetime.strptime(e["buildDate"], API_DATE_FMT),
        reverse=True,  # newest → oldest
    )

    body = "".join(mdx_block(e) for e in entries)
    destination.write_text(FRONT_MATTER + body, encoding="utf-8")

# ============================================================================
# Command‑line interface
# ============================================================================

def main() -> None:
    if len(sys.argv) < 2:
        script_name = Path(sys.argv[0]).name
        sys.exit(f"Usage: python {script_name} <BRANCH> [<BRANCH> ...]")

    primary_branch = api_latest_branch()
    requested_branches = sys.argv[1:]

    for branch in requested_branches:
        is_primary = branch == primary_branch
        changelog_entries = fetch_branch_entries(branch)
        target_file = output_path_for(branch, is_primary)
        write_whats_new_file(target_file, changelog_entries)
        print(f"✅  Wrote {target_file.relative_to(PROJECT_ROOT)}")

    print("🏁  Finished.")


if __name__ == "__main__":
    main()