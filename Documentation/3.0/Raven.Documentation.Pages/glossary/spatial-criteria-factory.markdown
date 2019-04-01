# Glossary: SpatialCriteriaFactory

### Methods

| Signature | Description |
| ---------- | ----------- |
| **SpatialCriteria RelatesToShape(object shape, SpatialRelation relation)** | Matches elements based on shape and relation |
| **SpatialCriteria Intersects(object shape)** | Matches elements which intersects to given shape |
| **SpatialCriteria Contains(object shape)** | Matches elements which contains given shape |
| **SpatialCriteria Disjoint(object shape)** | Matches elements which disjoints given shape |
| **SpatialCriteria Within(object shape)** | Matches elements within given shape |
| **SpatialCriteria WithinRadiusOf(double radius, double x, double y)** | [Obsolete("Order of parameters in this method is inconsistent with the rest of the API (x = longitude, y = latitude). Please use 'WithinRadius'.")]   Matches elements within given radius with center point in (x,y) |
| **SpatialCriteria WithinRadius(double radius, double latitude, double longitude)** | Matches elements within given radius with center point in (latitude, longitude) |

<hr />

# SpatialCriteria

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Relation** | SpatialRelation | Spatial relation |
| **Shape** | object | Shape |
