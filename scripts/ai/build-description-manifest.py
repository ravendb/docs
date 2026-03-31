"""Build a manifest of all .mdx pages needing meta descriptions."""
import os, re, json, glob

BASE = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
manifest = []


def extract_frontmatter(content):
    m = re.match(r"^---\n(.*?)\n---", content, re.DOTALL)
    if not m:
        return {}, content
    fm_text = m.group(1)
    fm = {}
    for line in fm_text.split("\n"):
        kv = re.match(r"^(\w[\w_]*)\s*:\s*(.+)", line)
        if kv:
            key = kv.group(1)
            val = kv.group(2).strip().strip('"').strip("'")
            fm[key] = val
    body = content[m.end() :].strip()
    return fm, body


def get_section_path(filepath):
    rel = os.path.relpath(filepath, BASE).replace(os.sep, "/")
    parts = rel.split("/")
    if parts[0] in ("docs", "cloud"):
        section_parts = parts[1:-1]
    else:
        section_parts = parts[:-1]
    return (
        " > ".join(p.replace("-", " ").title() for p in section_parts)
        if section_parts
        else ""
    )


def get_content_excerpt(body):
    lines = body.split("\n")
    prose_lines = []
    for line in lines:
        stripped = line.strip()
        if stripped.startswith("import "):
            continue
        if stripped.startswith("<LanguageSwitcher") or stripped.startswith(
            "<LanguageContent"
        ):
            continue
        if stripped.startswith("</LanguageContent"):
            continue
        if not stripped:
            continue
        prose_lines.append(line)
        if len("\n".join(prose_lines)) > 1500:
            break
    return "\n".join(prose_lines)[:1500]


def find_csharp_partial(filepath):
    content_dir = os.path.join(os.path.dirname(filepath), "content")
    if not os.path.isdir(content_dir):
        return None
    partials = glob.glob(os.path.join(content_dir, "_*-csharp.mdx"))
    if partials:
        try:
            with open(partials[0], "r", encoding="utf-8") as f:
                return f.read()[:1500]
        except Exception:
            pass
    return None


# Find all target files
patterns = [
    os.path.join(BASE, "docs", "**", "*.mdx"),
    os.path.join(BASE, "cloud", "**", "*.mdx"),
]

all_files = []
for pattern in patterns:
    all_files.extend(glob.glob(pattern, recursive=True))

skipped = {"partials": 0, "has_desc": 0, "home": 0, "category": 0}

for filepath in sorted(all_files):
    fp = filepath.replace(os.sep, "/")
    filename = os.path.basename(filepath)

    # Skip partials (content/_*.mdx and _category_.json style)
    if "/content/_" in fp or filename.startswith("_"):
        skipped["partials"] += 1
        continue

    if filename == "home.mdx":
        skipped["home"] += 1
        continue

    try:
        with open(filepath, "r", encoding="utf-8") as f:
            content = f.read()
    except Exception:
        continue

    fm, body = extract_frontmatter(content)

    if "description" in fm:
        skipped["has_desc"] += 1
        continue

    title = fm.get("title", filename.replace(".mdx", "").replace("-", " ").title())
    section_path = get_section_path(filepath)
    excerpt = get_content_excerpt(body)

    has_langs = "supported_languages" in fm
    csharp_partial = None
    if has_langs or "<LanguageSwitcher" in body:
        csharp_partial = find_csharp_partial(filepath)

    rel_path = os.path.relpath(filepath, BASE).replace(os.sep, "/")
    parts = rel_path.split("/")
    top_section = parts[0] + "/" + parts[1] if len(parts) > 1 else parts[0]

    entry = {
        "file": rel_path,
        "title": title,
        "section": section_path,
        "top_section": top_section,
        "excerpt": excerpt,
    }
    if csharp_partial:
        entry["csharp_partial"] = csharp_partial

    manifest.append(entry)

# Stats
sections = {}
for entry in manifest:
    ts = entry["top_section"]
    sections.setdefault(ts, []).append(entry)

print(f"Total target files: {len(manifest)}")
for k, v in skipped.items():
    print(f"  Skipped ({k}): {v}")
print()
print("By section:")
for s in sorted(sections.keys()):
    print(f"  {s}: {len(sections[s])} files")

output_path = os.path.join(BASE, "scripts", "description-manifest.json")
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(manifest, f, indent=2, ensure_ascii=False)
print(f"\nManifest written to {output_path}")
