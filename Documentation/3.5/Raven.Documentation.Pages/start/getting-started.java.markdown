# Getting started

## Stable releases

Simply put ravendb dependency to your pom:

{CODE-BLOCK:xml}
&lt;dependency&gt;
  &lt;groupId&gt;net.ravendb&lt;/groupId&gt;
  &lt;artifactId&gt;ravendb-client&lt;/artifactId&gt;
  &lt;version&gt;3.2.1&lt;/version&gt;
&lt;/dependency&gt;
{CODE-BLOCK/}


## Create new maven project

Sample pom:

{CODE-BLOCK:xml}
&lt;project xmlns=&quot;http://maven.apache.org/POM/4.0.0&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xsi:schemaLocation=&quot;http://maven.apache.org/POM/4.0.0 http://maven.apache.org/xsd/maven-4.0.0.xsd&quot;&gt;
  &lt;modelVersion&gt;4.0.0&lt;/modelVersion&gt;
  &lt;groupId&gt;com.example&lt;/groupId&gt;
  &lt;artifactId&gt;ravendb-example&lt;/artifactId&gt;
  &lt;version&gt;0.0.1-SNAPSHOT&lt;/version&gt;

  &lt;dependencies&gt;
    &lt;dependency&gt;
      &lt;groupId&gt;net.ravendb&lt;/groupId&gt;
      &lt;artifactId&gt;ravendb-client&lt;/artifactId&gt;
      &lt;version&gt;3.2.0&lt;/version&gt;
    &lt;/dependency&gt;
  &lt;/dependencies&gt;

  &lt;build&gt;
	  &lt;plugins&gt;
		&lt;plugin&gt;
		  &lt;groupId&gt;com.mysema.maven&lt;/groupId&gt;
		  &lt;artifactId&gt;apt-maven-plugin&lt;/artifactId&gt;
		  &lt;version&gt;1.1.1&lt;/version&gt;
		  &lt;executions&gt;
			&lt;execution&gt;
			  &lt;goals&gt;
				&lt;goal&gt;process&lt;/goal&gt;
			  &lt;/goals&gt;
			  &lt;configuration&gt;
				&lt;outputDirectory&gt;target/generated-sources/java&lt;/outputDirectory&gt;
				&lt;processor&gt;net.ravendb.querydsl.RavenDBAnnotationProcessor&lt;/processor&gt;
				&lt;options&gt;
				  &lt;querydsl.entityAccessors&gt;true&lt;/querydsl.entityAccessors&gt;
				&lt;/options&gt;
			  &lt;/configuration&gt;
			&lt;/execution&gt;
		  &lt;/executions&gt;
		&lt;/plugin&gt;
	&lt;/plugins&gt;
  &lt;/build&gt;
&lt;/project&gt;
{CODE-BLOCK/}

Please note that plugins section contains `net.ravendb.querydsl.RavenDBAnnotationProcessor`. 
As Java does not have LINQ, all definitions of indexes/transformers must be created using strings.
Alternatively, you can use the Studio to create indexes/transformers, yet you can also use QueryDSL for strongly-typed querying. In order to use QueryDSL you have to mark your entities with `@QueryEntity` 
annotation and enable code generation in `pom.xml`. 

{CODE-BLOCK:java}
@QueryEntity
public class Category {
  private String id;
  private String name;
  private String description;
  public String getId() {
    return id;
  }
  public void setId(String id) {
    this.id = id;
  }
  public String getName() {
    return name;
  }
  public void setName(String name) {
    this.name = name;
  }
  public String getDescription() {
    return description;
  }
  public void setDescription(String description) {
    this.description = description;
  }
}
{CODE-BLOCK/}

After doing this you can use strongly-typed syntax. 

{CODE-BLOCK:java}
	QCategory c = QCategory.category;
    List<Category> candies = session
      .query(Category.class)
      .where(c.name.eq("Candy"))
      .toList();
{CODE-BLOCK/}


## Unstable releases

RavenDB Client Unstable is available as snapshot in daily-builds S3 maven repository.

{WARNING:Important}
Embedding repositories in pom is not a good practice, although it allows a quick start. Please consider moving repositories into `~/.m2/settings.xml`.
More information can be found [here](http://maven.apache.org/guides/mini/guide-multiple-repositories.html). 
{WARNING/}

{CODE-BLOCK:java}
  &lt;repositories&gt;
    &lt;repository&gt;
        &lt;id&gt;snapshots-repo&lt;/id&gt;
        &lt;url&gt;http://ravendb-maven.s3.amazonaws.com/snapshots/&lt;/url&gt;
        &lt;releases&gt;
           &lt;enabled&gt;false&lt;/enabled&gt;
        &lt;/releases&gt;
        &lt;snapshots&gt;
          &lt;enabled&gt;true&lt;/enabled&gt;
        &lt;/snapshots&gt;
     &lt;/repository&gt;
  &lt;/repositories&gt;
{CODE-BLOCK/}