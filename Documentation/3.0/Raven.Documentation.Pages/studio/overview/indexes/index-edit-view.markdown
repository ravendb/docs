# Indexes : Index Edit View

Whenever you want to add new or edit existing index you will be redirected to `Index Edit View`. This view helps you shape the index definition by providing you code-completion, formatters, C# code generator and more.

## Action Bar

Following actions are available on bar:

- `Save` - saves index on server (if index existed and definition changed, then previous indexing data will be lost),
- `Add` - you can add another [mapping]() or [reducing]() function, define field or spatial field and set max index outputs,
- `Priority` - ability to change index priority. More [here](),
- `Format` - perform code formatting for mapping and reducing functions,
- `Query` - _edit only_ - redirects to [Query View](),
- `Terms` - _edit only_ - redirects to `Terms View` where you an view current index terms,
- `Copy` - opens dialog, where you can copy index definition
- `Generate C#` - creates index definition class in C#,
- `Refresh` - fetches index definition from server,
- `Delete` - removes index

![Figure 1. Studio. Index Edit View. Action Bar.](images/index-edit-view-action-bar.png)  

