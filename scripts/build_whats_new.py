#!/usr/bin/env python3
"""build_whats_new.py – regenerate RavenDB *What's New* pages

What the script does
--------------------
1. **Downloads** changelog entries for one or more RavenDB branches via the
   public Documentation API.
2. **Sorts** the entries strictly by their ``buildDate`` field (newest → oldest).
3. **Writes** them to ``whats-new.mdx`` files with front-matter already in place.
4. **Escapes** raw angle brackets outside code (inline/fenced), preserving only
   `<hr />` (and `<hr>` / `<hr/>`) as real HTML; everything else is escaped.
   Existing `&lt;` / `&gt;` aren’t double-escaped.
   Also logs any tag-like snippets that were escaped.

File locations
--------------
* The branch that the API reports as **latest** is considered *primary* and
  written to:

    ``<docs_root>/docs/whats-new.mdx``

* All other branches are written to:

    ``<docs_root>/versioned_docs/version-<branch>/whats-new.mdx``

Environment variable
--------------------
Set API endpoint in ``WHATS_NEW_URL`` before running.

Examples
--------
    # Rebuild for the currently latest branch only
    python build_whats_new.py 6.2

    # Rebuild for multiple branches
    python build_whats_new.py 6.2 6.0 5.4
"""

from __future__ import annotations

import re
import os
import sys
from collections import Counter
from datetime import datetime
from pathlib import Path
from typing import List, Dict, Any

import requests

# ============================================================================
# Configuration & paths
# ============================================================================

API_BASE_URL: str | None = os.environ.get("WHATS_NEW_URL")

if not API_BASE_URL:
    raise EnvironmentError("Environment variable 'WHATS_NEW_URL' is not set.")

SCRIPT_DIR = Path(__file__).resolve().parent
PROJECT_ROOT = SCRIPT_DIR.parent  # «../» relative to /scripts

# Docusaurus front-matter block that prefixes every generated MDX file
FRONT_MATTER = (
    "---\n"
    'title: "What\'s New"\n'
    "breadcrumbs: false\n"
    "pagination_next: null\n"
    "pagination_prev: null\n"
    "---\n\n"
)

DEFAULT_WHATS_NEW_CONTENT = """
                            This version is under development and hasn't been released yet.
                            """

# Date format used by the API – helper for ``datetime.strptime``
API_DATE_FMT = "%m/%d/%Y"

# ============================================================================
# REST helpers
# ============================================================================

def get_api_page(branch: str, page: int = 1) -> Dict[str, Any]:
    """Return a single paginated payload from the Documentation API."""
    response = requests.get(
        API_BASE_URL,
        headers={"Accept": "application/json"},
        params={"version": branch, "page": page},
        timeout=20,
    )

    if response.status_code == 404:
        sys.exit(f"Branch '{branch}' not found – check availableVersionsAndKeys in the API.")

    response.raise_for_status()
    return response.json()

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
# Escaping helpers (whitelist only <hr>, log tag-like escapes)
# ============================================================================

# fenced code blocks (``` or ~~~), with optional info string
_FENCE_RE = re.compile(r"(^|\n)(?P<fence>```|~~~)[^\n]*\n.*?\n(?P=fence)(?=\n|$)", re.DOTALL)
# inline code spans
_INLINE_CODE_RE = re.compile(r"`[^`]*`")
# tag-like matcher; allows attributes, self-closing, etc.
_HTML_TAG_RE = re.compile(r"</?\s*([A-Za-z][A-Za-z0-9:-]*)\b(?:\s+[^<>]*?)?/?>")
# '###Server' -> '### Server'
_HEADING_SPACE_RE = re.compile(r"(?m)^(#{1,6})(?!\s|#)")
# whitelist: keep only <hr>, <hr/>, <hr /> (case-insensitive)
_WHITELIST_TAGS = {"hr", "code"}

# per-run log of escaped tag-like snippets
_ESCAPED_TAG_EVENTS: list[str] = []

def _log_tag_escape(snippet: str) -> None:
    # keep the literal snippet for reporting
    _ESCAPED_TAG_EVENTS.append(snippet)

def _escape_angles(text: str) -> str:
    return text.replace("<", "&lt;").replace(">", "&gt;")

def _escape_preserving_hr_only(text: str) -> str:
    """Escape < and > in plain text, but keep only `<hr>` variants as HTML.
       Any other tag-like snippet (e.g., <T>, <div>, <Foo>) is escaped & logged.
    """
    out, last = [], 0
    for match in _HTML_TAG_RE.finditer(text):
        # escape plain text before the tag-like match
        out.append(_escape_angles(text[last:match.start()]))

        tag_full = match.group(0) # matched groups from regex
        tag_name = match.group(1).lower() if match.group(1) else "" # to check if it isn't whitelisted e.g. <hr>

        if tag_name in _WHITELIST_TAGS:
            out.append(tag_full)  # keep <hr> / <hr/> / <hr />
        else:
            _log_tag_escape(tag_full)
            out.append(_escape_angles(tag_full))  # escape non-whitelisted tag-like text

        last = match.end()

    out.append(_escape_angles(text[last:]))
    return "".join(out)

def _escape_outside_inline_code(text: str) -> str:
    """Within non-fenced areas, escape outside inline code spans."""
    out, last = [], 0
    for match in _INLINE_CODE_RE.finditer(text):
        # fix headings in the plain-text slice, then escape angles (keeping <hr>)
        chunk = text[last:match.start()]
        chunk = _HEADING_SPACE_RE.sub(r"\1 ", chunk)
        out.append(_escape_preserving_hr_only(chunk))
        out.append(match.group(0))  # keep inline code as-is
        last = match.end()
    # tail
    chunk = text[last:]
    chunk = _HEADING_SPACE_RE.sub(r"\1 ", chunk)
    out.append(_escape_preserving_hr_only(chunk))
    return "".join(out)

def escape_angle_brackets(markdown: str) -> str:
    """Escape < and > everywhere except inside fenced/inline code; keep only <hr>."""
    # Protect existing entities so we don't double-escape them
    LT, GT = "\x00LT\x00", "\x00GT\x00"
    markdown = markdown.replace("&lt;", LT).replace("&gt;", GT)

    out, last = [], 0
    for match in _FENCE_RE.finditer(markdown):
        out.append(_escape_outside_inline_code(markdown[last:match.start()]))  # non-fenced
        out.append(match.group(0))                                             # keep fenced code intact
        last = match.end()
    out.append(_escape_outside_inline_code(markdown[last:]))

    result = "".join(out)
    return result.replace(LT, "&lt;").replace(GT, "&gt;")

# ============================================================================
# Conversion helpers
# ============================================================================

def mdx_heading(entry: Dict[str, Any]) -> str:
    """Create a level-2 MDX heading from an API entry."""
    date_str = datetime.strptime(entry["buildDate"], API_DATE_FMT).strftime("%Y/%m/%d")
    return f"## {entry['version']} - {date_str}\n\n"

def mdx_block(entry: Dict[str, Any]) -> str:
    """Full MDX chunk for a single changelog entry (heading + body)."""
    safe_body = escape_angle_brackets(entry["changelogMarkdown"])
    return mdx_heading(entry) + safe_body + "\n\n"

# ============================================================================
# Filesystem helpers
# ============================================================================

def output_path_for(branch: str, is_primary: bool) -> Path:
    """Return where the *whats-new.mdx* for *branch* should live."""
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

def write_default_whats_new_file(destination: Path)-> None:
    destination.parent.mkdir(parents=True, exist_ok=True)

    destination.write_text(FRONT_MATTER + DEFAULT_WHATS_NEW_CONTENT, encoding="utf-8")

# ============================================================================
# Command-line interface
# ============================================================================

def main() -> None:
    if len(sys.argv) < 2:
        script_name = Path(sys.argv[0]).name
        sys.exit(f"Usage: python {script_name} <BRANCH> [<BRANCH> ...]")

    requested_branches = sys.argv[1:]

    if len(requested_branches) < 1:
        sys.exit("What's new has to be generated for at least one documentation version.")

    # Based on assumption that most recent version given in script parameters matches current version in Docusaurus config
    primary_branch = sorted(requested_branches)[-1]

    for branch in requested_branches:
        # reset log for this branch
        _ESCAPED_TAG_EVENTS.clear()

        is_primary = branch == primary_branch
        changelog_entries = fetch_branch_entries(branch)
        target_file = output_path_for(branch, is_primary)

        if len(changelog_entries) == 0 or changelog_entries[0].get("version").startswith(branch) == False:
            write_default_whats_new_file(target_file)
        else:
            write_whats_new_file(target_file, changelog_entries)

        print(f"Wrote {target_file.relative_to(PROJECT_ROOT)}")

        if _ESCAPED_TAG_EVENTS:
            counts = Counter(_ESCAPED_TAG_EVENTS)
            # print a concise per-branch summary for safe escapes
            summary = ", ".join(f"{tag}×{n}" for tag, n in counts.most_common())
            print(f"Escaped non-whitelisted tag-like snippets: {summary}")

    print("Finished.")

if __name__ == "__main__":
    main()
