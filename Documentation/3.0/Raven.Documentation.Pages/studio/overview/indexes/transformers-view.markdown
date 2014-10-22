# Indexes : Transformers View

This view allows listing all the transformers that are available for the current database. It also allows adding, editing, and deleting already existing transformers. 

## Action Bar

Action Bar contains the following buttons:

- `New Transformer` - redirects to [Transformer Edit View]() where you can add a new transformer,
- `Collapse` - collapses all transformers,
- `Paste` - quickly creates transformer by pasting [TransformerDefinition](),
- `Delete` - deletes **ALL** transformers from database

![Figure 1. Studio. Transformers View. Action Bar.](images/transformers_view-action_bar-1.png)  

## Transformer List

On the list transformers are grouped by collections on which they work. Each transformer is represented by the block where:

- clicking on the name of the transformer or `Edit` button will take you to [Transformer Edit View](),
- clicking the `Copy` button will make a window with transformer's definition pop up; copied definition may be used for quick creation of a new transformer, using `Paste` option from the `Action Bar`,
- clicking `Delete` button opens a new dialog and a request to confirm operation appears; if accepted, the transformer will be deleted from the server

![Figure 2. Studio. Transformers View. Transformer List.](images/transformers_view-transformer_list-2.png)