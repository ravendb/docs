using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using view_models_demo.Models;
using view_models_demo.Models.Indexes;
using view_models_demo.Models.Transformers;

namespace view_models_demo.Controllers
{
    public class HomeController : Controller
    {
        public static IDocumentStore ravenStore = InitializeRaven();
        
        public ActionResult Index()
        {
            //using (var ravenSession = ravenStore.OpenSession())
            //{
            //    var chef = new Chef
            //    {
            //        Email = "borkbork@bork.com",
            //        Name = "Swei D. Sheff",
            //    };
            //    var comment1 = new Comment
            //    {
            //        Content = "This is good!",
            //        Name = "Hungry P. Erson"
            //    };
            //    var comment2 = new Comment
            //    {
            //        Content = "Yuck. Me no like salmon(ella)!",
            //        Name = "Deez A. Tisfied"
            //    };
            //    ravenSession.Store(chef);
            //    ravenSession.Store(comment1);
            //    ravenSession.Store(comment2);
            //    ravenSession.SaveChanges();
            //}

            return View();
        }

        public ActionResult Include()
        {            
            using (var ravenSession = ravenStore.OpenSession())
            {
                #region article_viewmodels_3
                // Query for the honey glazed salmon recipe 
                // and .Include the related Chef and Comment objects.
                var recipe = ravenSession.Query<Recipe>()
                    .Include(r => r.ChefId)
                    .Include(r => r.CommentIds)
                    .Where(r => r.Name == "Sweet honey glazed salmon")
                    .First();

                // No extra DB call here; it's already loaded into memory via the .Include calls.
                var chef = ravenSession.Load<Chef>(recipe.ChefId);

                // Ditto.
                var comments = ravenSession.Load<Comment>(recipe.CommentIds);

                var recipeViewModel = new RecipeViewModel
                {
                    RecipeId = recipe.Id,
                    Name = recipe.Name,
                    PictureUrl = recipe.PictureUrl,
                    ChefEmail = chef.Email,
                    ChefName = chef.Name,
                    Comments = comments.ToList(),
                    Ingredients = recipe.Ingredients,
                    Categories = recipe.Categories
                };
                #endregion
            }

            return View();
        }

        public ActionResult Transformer()
        {
            using (var ravenSession = ravenStore.OpenSession())
            {
                #region article_viewmodels_5
                // Query for the honey glazed salmon recipe 
                // and transform it into a RecipeViewModel.
                var recipeViewModel = ravenSession.Query<Recipe>()
                    .TransformWith<RecipeViewModelTransformer, RecipeViewModel>()
                    .Where(r => r.Name == "Sweet honey glazed salmon")
                    .First();
                #endregion

                #region article_viewmodels_6
                // Query for all recipes and transform them into RecipeViewModels.
                var allRecipeViewModels = ravenSession.Query<Recipe>()
                    .TransformWith<RecipeViewModelTransformer, RecipeViewModel>()
                    .ToList();
                #endregion
            }

            return View();
        }

        public ActionResult StoreViewModel()
        {
            using (var ravenSession = ravenStore.OpenSession())
            {
                #region article_viewmodels_7
                var recipeViewModel = new RecipeViewModel
                {
                    Name = "Sweet honey glazed salmon",
                    RecipeId = "recipes/1",
                    PictureUrl = "http://tastyrecipesyum.com/recipes/1/profile.jpg",
                    Categories = new List<string> { "salmon", "fish", "seafood", "healthy" },
                    ChefName = "Swei D. Sheff",
                    ChefEmail = "borkbork@bork.com",
                    Comments = new List<Comment> { new Comment { Name = "Dee Liteful", Content = "I really enjoyed this dish!" }  },
                    Ingredients = new List<Ingredient> { new Ingredient { Name = "salmon fillet", Amount = "5 ounce" } }
                };

                // Raven allows us to store complex objects, even whole view models.
                ravenSession.Store(recipeViewModel);
                #endregion
            }

            return View();
        }

        public ActionResult Changes()
        {
            using (var ravenSession = ravenStore.OpenSession())
            {
                #region article_viewmodels_8
                // Listen for changes to Recipes. 
                // When that happens, update the corresponding RecipeViewModel.
                ravenStore
                    .Changes()
                    .ForDocumentsOfType<Recipe>()
                    .Subscribe(docChange => this.UpdateViewModelFromRecipe(docChange.Id));
                #endregion
            }

            return View();
        }

        public ActionResult Indexes()
        {
            using (var ravenSession = ravenStore.OpenSession())
            {
                #region article_viewmodels_10
                // Load up the RecipeViewModels stored in our index.
                var recipeViewModel = ravenSession.Query<RecipeViewModel, RecipeViewModelIndex>()
                    .Where(r => r.Name == "Sweet honey glazed salmon")
                    .ProjectFromIndexFieldsInto<RecipeViewModel>()
                    .First();
                #endregion
            }

            return View();
        }

        private void UpdateViewModelFromRecipe(string recipeId)
        {
            // Omitted: Load the Recipe, then update the RecipeViewModel accordingly.
        }
        
        private static IDocumentStore InitializeRaven()
        {
            var store = new DocumentStore
            {
                Url = "http://mmm-ravendb-dev3818.cloudapp.net",
                DefaultDatabase = "ViewModels"
            };
            store.Initialize();
            new RecipeViewModelTransformer().Execute(store);
            new RecipeViewModelIndex().Execute(store);
            return store;
        }
    }
}