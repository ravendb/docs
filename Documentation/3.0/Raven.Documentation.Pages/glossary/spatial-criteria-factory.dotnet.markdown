# Glossary : SpatialCriteriaFactory

### Methods

| Signature | Description |
| ---------- | ----------- |
| **SpatialCriteria RelatesToShape(object shape, SpatialRelation relation)** | Matches elements based on shape and relation |
| **SpatialCriteria Intersects(object shape)** | Matches elements which intersects to given shape |
| **SpatialCriteria Contains(object shape)** | Matches elements which contains given shape |
| **SpatialCriteria Disjoint(object shape)** | Matches elements which disjoints given shape |
| **SpatialCriteria Within(object shape)** | Matches elements within given shape |
| **SpatialCriteria WithinRadiusOf(double radius, double x, double y)** | Matches elements within given radius with center point in (x,y) |

<hr />

# SpatialCriteria

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Relation** | SpatialRelation | Spatial relation |
| **Shape** | object | Shape |
