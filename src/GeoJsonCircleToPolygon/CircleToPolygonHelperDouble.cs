// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircleToPolygonHelperDouble.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to convert circles to polygons for GeoJSON (For double).
//   Forked and adapted to C# from https://github.com/gabzim/circle-to-polygon.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// A class to convert circles to polygons for GeoJSON (For double). The calculation itself lives in
/// <see cref="CircleToPolygonHelper"/>, which the decimal helper uses as well.
/// </summary>
public static class CircleToPolygonHelperDouble
{
    /// <summary>
    /// Gets the polygon from a given circle.
    /// </summary>
    /// <param name="center">The center position.</param>
    /// <param name="radiusInMeters">The radius in meters.</param>
    /// <param name="numberOfEdges">The number of edges.</param>
    /// <param name="bearing">The bearing.</param>
    /// <param name="direction">The direction.</param>
    /// <param name="earthRadius">The earth radius.</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Array"/> of <see cref="double"/>s as positions list.</returns>
    /// <exception cref="ArgumentException">Thrown if any argument is invalid.</exception>
    public static List<double[]> GetPolygonFromCircle(
        double[] center,
        double radiusInMeters,
        int numberOfEdges = 32,
        int bearing = 0,
        int direction = 1,
        int earthRadius = 6378137)
    {
        return CircleToPolygonHelper.GetPolygonFromCircle<double, DoubleCircleMath>(center, radiusInMeters, numberOfEdges, bearing, direction, earthRadius);
    }
}
