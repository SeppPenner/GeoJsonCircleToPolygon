// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircleToPolygonHelperDecimal.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to convert circles to polygons for GeoJSON (For decimal).
//   Forked and adapted to C# from https://github.com/gabzim/circle-to-polygon.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// A class to convert circles to polygons for GeoJSON (For decimal).
/// Forked and adapted to C# from https://github.com/gabzim/circle-to-polygon.
/// </summary>
public static class CircleToPolygonHelperDecimal
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
    /// <returns>A <see cref="List{T}"/> of <see cref="Array"/> of <see cref="decimal"/>s as positions list.</returns>
    /// <exception cref="ArgumentException">Thrown if any argument is invalid.</exception>
    public static List<decimal[]> GetPolygonFromCircle(
        decimal[] center,
        decimal radiusInMeters,
        int numberOfEdges = 32,
        int bearing = 0,
        int direction = 1,
        int earthRadius = 6378137)
    {
        if (numberOfEdges < 3)
        {
            throw new ArgumentException(nameof(numberOfEdges), "The number of edges must be at least 3.");
        }

        if (radiusInMeters <= 0)
        {
            throw new ArgumentException(nameof(radiusInMeters), "The radius must be positive.");
        }

        if (center.Length != 2)
        {
            throw new ArgumentException(nameof(center), "The center coordinates must have exactly two elements.");
        }

        var longitude = center[0];
        var latitude = center[1];

        if (longitude > 180 || longitude < -180)
        {
            throw new ArgumentException(nameof(center), "The longitude must be between -180 and 180.");
        }

        if (latitude > 90 || latitude < -90)
        {
            throw new ArgumentException(nameof(center), "The latitude must be between -90 and 90.");
        }

        var start = GetRadians(bearing);
        var coordinates = new List<decimal[]>();

        for (var i = 0; i < numberOfEdges; ++i)
        {
            var offset = GetOffsetCoordinates(center, radiusInMeters, earthRadius, start + (direction * 2 * DecimalMath.Pi * -i) / numberOfEdges);
            coordinates.Add(offset);
        }

        coordinates.Add(coordinates[0]);
        return coordinates;
    }

    /// <summary>
    /// Gets the radians value from the angle in degrees.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    /// <returns>The radians value.</returns>
    private static decimal GetRadians(decimal angleInDegrees)
    {
        return (angleInDegrees * DecimalMath.Pi) / 180;
    }

    /// <summary>
    /// Gets the degrees value from the angle in radians.
    /// </summary>
    /// <param name="angleInRadians">The angle in radians.</param>
    /// <returns>The degrees value.</returns>
    private static decimal GetDegrees(decimal angleInRadians)
    {
        return (angleInRadians * 180) / DecimalMath.Pi;
    }

    /// <summary>
    /// Gets the offset coordinates.
    /// </summary>
    /// <param name="center">The center position.</param>
    /// <param name="distance">The distance.</param>
    /// <param name="earthRadius">The earth radius.</param>
    /// <param name="bearing">The bearing.</param>
    /// <returns>The offset coordinates.</returns>
    private static decimal[] GetOffsetCoordinates(decimal[] center, decimal distance, decimal earthRadius, decimal bearing)
    {
        var latitude1 = GetRadians(center[1]);
        var longitude1 = GetRadians(center[0]);
        var dByR = distance / earthRadius;
        var latitude = DecimalMath.Asin(DecimalMath.Sin(latitude1) * DecimalMath.Cos(dByR) + DecimalMath.Cos(latitude1) * DecimalMath.Sin(dByR) * DecimalMath.Cos(bearing));
        var longitude = longitude1 + DecimalMath.Atan2(DecimalMath.Sin(bearing) * DecimalMath.Sin(dByR) * DecimalMath.Cos(latitude1), DecimalMath.Cos(dByR) - DecimalMath.Sin(latitude1) * DecimalMath.Sin(latitude));
        return new decimal[] { GetDegrees(longitude), GetDegrees(latitude) };
    }
}
