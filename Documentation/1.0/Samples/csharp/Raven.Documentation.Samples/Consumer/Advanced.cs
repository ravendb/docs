using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RavenCodeSamples.Consumer
{
    public class Advanced : CodeSampleBase
    {
        public void CustomSerialization()
        {
            using (var store = NewDocumentStore())
            {
                #region custom_serialization4

                store.Conventions.JsonContractResolver = new DefaultContractResolver()
                                                             {
                                                                 DefaultMembersSearchFlags =
                                                                     BindingFlags.Public | BindingFlags.Instance
                                                             };

                #endregion
            }
        }

        #region custom_serialization1

        public class Carrot
        {
            public string Id { get; set; }

            public decimal Length { get; set; }

            [JsonIgnore]
            public decimal LengthInInch
            {
                get
                {
                    /* some calculations */
                    return Length;
                }

                set
                {
                    //...
                }
            }
        }

        #endregion

        public interface IVegetable
        {
        }

        #region custom_serialization2

        public class Recipe
        {
            public string Id { get; set; }

            [JsonProperty(PropertyName = "dishes")]
            public IList<IVegetable> SideDishes { get; set; }
        }

        #endregion

        #region custom_serialization3

        [JsonObject(IsReference = true)]
        public class Category
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public Category Parent { get; set; }
            public List<Category> Children { get; set; }

            public Category()
            {
                Children = new List<Category>();
            }

            public void Add(Category category)
            {
                category.Parent = this;
                Children.Add(category);
            }
        }

        #endregion
    }
}
