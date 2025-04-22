# Configuration: Patching
---

{NOTE: }

* The following configuration options control the behavior of the **JavaScript engine** during patch operations.
* Learn more about patching in:
  *  [Single document patch operations](../../client-api/operations/patching/single-document)
  *  [Set-base patch operations](../../client-api/operations/patching/set-based)
  *  [Apply patching from the Studio](../../studio/database/documents/patch-view)

---

* In this article:
    * [Patching.AllowStringCompilation](../../server/configuration/patching-configuration#patching.allowstringcompilation)
    * [Patching.MaxStepsForScript](../../server/configuration/patching-configuration#patching.maxstepsforscript)
    * [Patching.StrictMode](../../server/configuration/patching-configuration#patching.strictmode)

{NOTE/}

---

{PANEL: Patching.AllowStringCompilation}

* Determines whether the JavaScript engine is allowed to compile code from strings at runtime,  
  using constructs such as `eval(...)` or `new Function(arg1, arg2, ..., functionBody)`.

* A `JavaScriptException` is thrown if this option is disabled and such a construct is used.

---

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: Patching.MaxStepsForScript}

Specifies the maximum number of execution steps a patch script can perform.  
A `Jint.Runtime.StatementsCountOverflowException` is thrown if the script exceeds this number.

- **Type**: `int`
- **Default**: `10_000`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: Patching.StrictMode}

Enables strict mode in the JavaScript engine used during patching.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide or per database

{PANEL/}
