#region article_viewmodels_2
// Theoretical LINQ code to perform a JOIN against a relational database.
public RecipeViewModel GetRecipeDetails(string recipeId)
{
    var recipeViewModel = from recipe in dbContext.Recipes
                          where recipe.Id == 42
                          join chef in dbContext.Chefs on chef.Id equals recipe.ChefId
                          let ingredients = dbContext.Ingredients.Where(i => i.RecipeId == recipe.Id)
                          let comments = dbContext.Comments.Where(c => c.RecipeId == recipe.Id)
                          let categories = dbContext.Categories.Where(c => c.RecipeId == recipeId)
                          select new RecipeViewModel
                          {
                              RecipeId = recipe.Id
                              Name = recipe.Name,
                              PictureUrl = recipe.PictureUrl,
                              Categories = categories.Select(c => c.Name).ToList(),
                              ChefEmail = chef.Email,
                              ChefName = chef.Name,
                              Ingredients = ingredients.ToList(),
                              Comments = comments.ToList()
                          };
    return recipeViewModel;  
}
#endregion