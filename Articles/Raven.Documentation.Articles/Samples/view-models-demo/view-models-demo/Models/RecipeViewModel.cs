using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace view_models_demo.Models
{
    #region article_viewmodels_1
    /// <summary>
    /// View model for a Recipe. Contains everything we want to display on the Recipe details UI page.
    /// </summary>
    public class RecipeViewModel
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Categories { get; set; }
        public string ChefName { get; set; }
        public string ChefEmail { get; set; }
        public IList<Comment> Comments { get; set; }
    }
    #endregion
}