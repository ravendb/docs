using Raven.Client.Documents;

namespace Raven.Documentation.Samples.AiIntegration;

#region class
public class SampleClass
{
    public string Id { get; set; }
    public string Title { get; set; }
    
    // Storing data in a RavenVector property for optimized storage and performance
    public RavenVector<float> EmbeddingRavenVector { get; set; }
    
    // Storing data in a regular array property
    public float[] EmbeddingVector { get; set; }
}
#endregion
