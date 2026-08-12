// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircleToPolygonHelper.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The one conversion of circles to polygons for GeoJSON, for every numeric type.
//   Forked and adapted to C# from https://github.com/gabzim/circle-to-polygon.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// The one conversion of circles to polygons for GeoJSON, for every numeric type.
/// Forked and adapted to C# from https://github.com/gabzim/circle-to-polygon.
/// The public entry points are <see cref="CircleToPolygonHelperDouble"/> and
/// <see cref="CircleToPolygonHelperDecimal"/>, this class holds the calculation both of them use. The order of the
/// operations is the one the two former copies had, because the expected results of the tests are exact.
/// </summary>
internal static class CircleToPolygonHelper
{
    /// <summary>
    /// Gets the polygon from a given circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the positions.</typeparam>
    /// <typeparam name="TMath">The trigonometry to use for <typeparamref name="T"/>.</typeparam>
    /// <param name="center">The center position.</param>
    /// <param name="radiusInMeters">The radius in meters.</param>
    /// <param name="numberOfEdges">The number of edges.</param>
    /// <param name="bearing">The bearing.</param>
    /// <param name="direction">The direction.</param>
    /// <param name="earthRadius">The earth radius.</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="Array"/> of <typeparamref name="T"/>s as positions list.</returns>
    /// <exception cref="ArgumentException">Thrown if any argument is invalid.</exception>
    internal static List<T[]> GetPolygonFromCircle<T, TMath>(
        T[] center,
        T radiusInMeters,
        int numberOfEdges,
        int bearing,
        int direction,
        int earthRadius)
        where T : INumber<T>
        where TMath : ICircleMath<T>
    {
        if (numberOfEdges < 3)
        {
            throw new ArgumentException("The number of edges must be at least 3.", nameof(numberOfEdges));
        }

        if (radiusInMeters <= T.Zero)
        {
            throw new ArgumentException("The radius must be positive.", nameof(radiusInMeters));
        }

        if (center.Length != 2)
        {
            throw new ArgumentException("The center coordinates must have exactly two elements.", nameof(center));
        }

        var longitude = center[0];
        var latitude = center[1];

        if (longitude > T.CreateChecked(180) || longitude < T.CreateChecked(-180))
        {
            throw new ArgumentException("The longitude must be between -180 and 180.", nameof(center));
        }

        if (latitude > T.CreateChecked(90) || latitude < T.CreateChecked(-90))
        {
            throw new ArgumentException("The latitude must be between -90 and 90.", nameof(center));
        }

        var start = GetRadians<T, TMath>(T.CreateChecked(bearing));
        var coordinates = new List<T[]>();

        for (var i = 0; i < numberOfEdges; ++i)
        {
            var offset = GetOffsetCoordinates<T, TMath>(center, radiusInMeters, T.CreateChecked(earthRadius), start + (T.CreateChecked(direction * 2) * TMath.Pi * T.CreateChecked(-i)) / T.CreateChecked(numberOfEdges));
            coordinates.Add(offset);
        }

        coordinates.Add(coordinates[0]);
        return coordinates;
    }

    /// <summary>
    /// Gets the radians value from the angle in degrees.
    /// </summary>
    /// <typeparam name="T">The numeric type of the angle.</typeparam>
    /// <typeparam name="TMath">The trigonometry to use for <typeparamref name="T"/>.</typeparam>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    /// <returns>The radians value.</returns>
    private static T GetRadians<T, TMath>(T angleInDegrees)
        where T : INumber<T>
        where TMath : ICircleMath<T>
    {
        return (angleInDegrees * TMath.Pi) / T.CreateChecked(180);
    }

    /// <summary>
    /// Gets the degrees value from the angle in radians.
    /// </summary>
    /// <typeparam name="T">The numeric type of the angle.</typeparam>
    /// <typeparam name="TMath">The trigonometry to use for <typeparamref name="T"/>.</typeparam>
    /// <param name="angleInRadians">The angle in radians.</param>
    /// <returns>The degrees value.</returns>
    private static T GetDegrees<T, TMath>(T angleInRadians)
        where T : INumber<T>
        where TMath : ICircleMath<T>
    {
        return (angleInRadians * T.CreateChecked(180)) / TMath.Pi;
    }

    /// <summary>
    /// Gets the offset coordinates.
    /// </summary>
    /// <typeparam name="T">The numeric type of the positions.</typeparam>
    /// <typeparam name="TMath">The trigonometry to use for <typeparamref name="T"/>.</typeparam>
    /// <param name="center">The center position.</param>
    /// <param name="distance">The distance.</param>
    /// <param name="earthRadius">The earth radius.</param>
    /// <param name="bearing">The bearing.</param>
    /// <returns>The offset coordinates.</returns>
    private static T[] GetOffsetCoordinates<T, TMath>(T[] center, T distance, T earthRadius, T bearing)
        where T : INumber<T>
        where TMath : ICircleMath<T>
    {
        var latitude1 = GetRadians<T, TMath>(center[1]);
        var longitude1 = GetRadians<T, TMath>(center[0]);
        var dByR = distance / earthRadius;
        var latitude = TMath.Asin(TMath.Sin(latitude1) * TMath.Cos(dByR) + TMath.Cos(latitude1) * TMath.Sin(dByR) * TMath.Cos(bearing));
        var longitude = longitude1 + TMath.Atan2(TMath.Sin(bearing) * TMath.Sin(dByR) * TMath.Cos(latitude1), TMath.Cos(dByR) - TMath.Sin(latitude1) * TMath.Sin(latitude));
        return [GetDegrees<T, TMath>(longitude), GetDegrees<T, TMath>(latitude)];
    }
}
