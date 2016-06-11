using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace view_models_demo.Models
{
    public class Recipe
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Categories { get; set; }
        public string ChefId { get; set; }
        public List<string> CommentIds { get; set; }
    }
}