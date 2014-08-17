# Bundle: Scripted Index Results

When creating a database, if you want to use the scripted index results bundle you need to select it in the database creation window:  
![Tasks Fig 1](Images/studio_scripted_index_1.PNG)  

After pressing "Next" your database will be created so you will be able to configure the scripted index results. To do so, you need to enter database settings using the top navigation panel.
![Tasks Fig 2](Images/studio_scripted_index_2.PNG)  

On the left you will notice a 'Scripted Index' tab where you be able to configure scripts for each desired index.
![Tasks Fig 3](Images/studio_scripted_index_3.PNG)  

## Example

{INFO Below walktrough is a Studio equivalent of the example described in details [here](../../server/extending/bundles/scripted-index-results#example) /}

To attach our `Orders/ByCompany` index results within company itself we must add two scripts:

1. _IndexScript_ that will be applied to reduce result.     
2. _DeleteScript_ that will be applied when index entry will be deleted.   

![Tasks Fig 4](Images/studio_scripted_index_4.PNG) 

As a result of our script all our companies will have additional property `Orders` with `Total` and `Count` attached.

![Tasks Fig 5](Images/studio_scripted_index_5.PNG) 

{INFO You can read more about the Scripted Index Results Bundle in [here](../../server/extending/bundles/scripted-index-results). /}