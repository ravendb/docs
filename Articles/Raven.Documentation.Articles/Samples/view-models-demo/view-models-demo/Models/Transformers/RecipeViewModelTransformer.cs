using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace view_models_demo.Models.Transformers
{
    #region article_viewmodels_4
    /// <summary>
    /// RavenDB Transformer that turns a Recipe into a RecipeViewModel.
    /// </summary>
    public class RecipeViewModelTransformer : AbstractTransformerCreationTask<Recipe>
    {
        public RecipeViewModelTransformer()
        {
            TransformResults = allRecipes => from recipe in allRecipes
                                             let chef = LoadDocument<Chef>(recipe.ChefId)
                                             let comments = LoadDocument<Comment>(recipe.CommentIds)
                                             select new RecipeViewModel
                                             {
                                                 RecipeId = recipe.Id,
                                                 Name = recipe.Name,
                                                 PictureUrl = recipe.PictureUrl,
                                                 Ingredients = recipe.Ingredients,
                                                 Categories = recipe.Categories,
                                                 ChefEmail = chef.Email,
                                                 ChefName = chef.Name,
                                                 Comments = comments
                                             };
        }
    }
    #endregion
}