//--------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class that provides the test data all test classes use.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Test;

/// <summary>
/// A class that provides the test data all test classes use. The values are the ones of the Readme example, which
/// is what makes the hardcoded expected results of the two test classes comparable.
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// The radius in meters of the example.
    /// </summary>
    public const int RadiusInMeters = 200000;

    /// <summary>
    /// The number of edges of the example, which is also the default of both helpers.
    /// </summary>
    public const int NumberOfEdges = 32;

    /// <summary>
    /// The earth radius both helpers use if the caller passes none.
    /// </summary>
    public const int EarthRadius = 6378137;

    /// <summary>
    /// Gets the center position of the example as doubles, longitude first.
    /// </summary>
    /// <returns>An <see cref="Array"/> of <see cref="double"/>s as center position.</returns>
    public static double[] GetCenterDouble()
    {
        return [173.283966, -41.270634];
    }

    /// <summary>
    /// Gets the center position of the example as decimals, longitude first.
    /// </summary>
    /// <returns>An <see cref="Array"/> of <see cref="decimal"/>s as center position.</returns>
    public static decimal[] GetCenterDecimal()
    {
        return [173.283966m, -41.270634m];
    }

    /// <summary>
    /// Gets the distance in meters between two positions on a sphere of <see cref="EarthRadius"/>. This is the
    /// inverse of what the helpers calculate, so a point of a polygon must come back at the radius it was created
    /// with.
    /// </summary>
    /// <param name="first">The first position, longitude first.</param>
    /// <param name="second">The second position, longitude first.</param>
    /// <returns>The distance in meters.</returns>
    public static double GetDistanceInMeters(double[] first, double[] second)
    {
        var latitude1 = GetRadians(first[1]);
        var latitude2 = GetRadians(second[1]);
        var deltaLatitude = latitude2 - latitude1;
        var deltaLongitude = GetRadians(second[0] - first[0]);
        var haversine = (Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2))
            + (Math.Cos(latitude1) * Math.Cos(latitude2) * Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2));
        return 2 * EarthRadius * Math.Asin(Math.Sqrt(haversine));
    }

    /// <summary>
    /// Gets the radians value from the angle in degrees.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    /// <returns>The radians value.</returns>
    private static double GetRadians(double angleInDegrees)
    {
        return (angleInDegrees * Math.PI) / 180;
    }
}
