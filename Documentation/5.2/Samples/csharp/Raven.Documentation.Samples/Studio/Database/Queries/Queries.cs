/*
 * #region spatial.circle
from Employees
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.circle(20, 47.623473, -122.3060097, 'miles')
    )
#endregion
#region spatial.wkt
from Employees
where spatial.within(
    spatial.point(Address.Location.Latitude, Address.Location.Longitude),
    spatial.wkt('CIRCLE(-122.3060097 47.623473 d=20)')
    )
#endregion
*/
