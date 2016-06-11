using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using view_models_demo.Models;

namespace view_models_demo.Models.Indexes
{
    #region article_viewmodels_9
    /// <summary>
    /// RavenDB index that is run automatically whenever a Recipe changes. For every recipe, the index outputs a RecipeViewModel.
    /// </summary>
    public class RecipeViewModelIndex : AbstractIndexCreationTask<Recipe>
    {
        public RecipeViewModelIndex()
        {
            Map = allRecipes => from recipe in allRecipes
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

            StoreAllFields(Raven.Abstractions.Indexing.FieldStorage.Yes);
        }
    }
    #endregion
}