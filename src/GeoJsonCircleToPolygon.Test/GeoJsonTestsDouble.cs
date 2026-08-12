//--------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonTestsDouble.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A test class to test the GeoJSON features (double).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Test;

/// <summary>
/// A test class to test the GeoJSON features (double).
/// </summary>
[TestClass]
public class GeoJsonTestsDouble
{
    /// <summary>
    /// The expected result.
    /// </summary>
    private static readonly List<double[]> expectedResult = new()
    {
        new double [] { 173.283966, -39.47400343176097 },
        new double [] { 172.8297426608343, -39.50761945331798 },
        new double [] { 172.39166717580562, -39.607271255365916 },
        new double [] { 171.98544458449058, -39.76940340765346 },
        new double [] { 171.62589074038397, -39.98820144316868 },
        new double [] { 171.3264802848837, -40.255758887782214 },
        new double [] { 171.09888995216616, -40.56231121046952 },
        new double [] { 170.9525431282912, -40.89653624994988 },
        new double [] { 170.894168491739, -41.24591956982946 },
        new double [] { 170.92739416288478, -41.597181119390946 },
        new double [] { 171.0524081585746, -41.9367562214545 },
        new double [] { 171.26572430426506, -42.251319123422796 },
        new double [] { 171.56009750883513, -42.528331314494025 },
        new double [] { 171.9246304894919, -42.75659019219929 },
        new double [] { 172.3451031959859, -42.92674764018193 },
        new double [] { 172.80453558092947, -43.03176422124745 },
        new double [] { 173.283966, -43.06726456823905 },
        new double [] { 173.76339641907052, -43.03176422124745 },
        new double [] { 174.22282880401409, -42.92674764018193 },
        new double [] { 174.64330151050808, -42.75659019219929 },
        new double [] { 175.00783449116483, -42.528331314494025 },
        new double [] { 175.3022076957349, -42.251319123422796 },
        new double [] { 175.51552384142542, -41.9367562214545 },
        new double [] { 175.6405378371152, -41.597181119390946 },
        new double [] { 175.673763508261, -41.24591956982946 },
        new double [] { 175.61538887170875, -40.89653624994988 },
        new double [] { 175.46904204783382, -40.56231121046952 },
        new double [] { 175.24145171511628, -40.255758887782214 },
        new double [] { 174.94204125961602, -39.98820144316868 },
        new double [] { 174.58248741550943, -39.76940340765346 },
        new double [] { 174.17626482419436, -39.607271255365916 },
        new double [] { 173.73818933916564, -39.50761945331798 },
        new double [] { 173.283966, -39.47400343176097 }
    };

    /// <summary>
    /// Tests the GeoJSON result from the example.
    /// </summary>
    [TestMethod]
    public void TestGeoJsonResultFromExample()
    {
        var coordinates = TestDataProvider.GetCenterDouble();
        var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(coordinates, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

        Assert.IsNotNull(result);
        var resultLines = result.Select(v => $"{v[0]}|{v[1]}").ToList();
        var expectedResultLines = expectedResult.Select(v => $"{v[0]}|{v[1]}").ToList();
        CollectionAssert.AreEqual(expectedResultLines, resultLines);
    }

    /// <summary>
    /// Checks whether the returned ring is closed, so that the last position repeats the first one.
    /// </summary>
    [TestMethod]
    public void ThePolygonIsClosed()
    {
        var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

        CollectionAssert.AreEqual(result[0], result[^1], "The ring does not end where it started.");
    }

    /// <summary>
    /// Checks whether the number of returned positions is the number of edges plus the repeated first position.
    /// </summary>
    /// <param name="numberOfEdges">The number of edges.</param>
    [TestMethod]
    [DataRow(3)]
    [DataRow(8)]
    [DataRow(32)]
    [DataRow(360)]
    public void TheNumberOfPositionsIsTheNumberOfEdgesPlusOne(int numberOfEdges)
    {
        var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), TestDataProvider.RadiusInMeters, numberOfEdges);

        Assert.AreEqual(numberOfEdges + 1, result.Count);
    }

    /// <summary>
    /// Checks whether the number of edges defaults to 32, the value the Readme documents.
    /// </summary>
    [TestMethod]
    public void TheNumberOfEdgesDefaultsToThirtyTwo()
    {
        var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), TestDataProvider.RadiusInMeters);

        Assert.AreEqual(TestDataProvider.NumberOfEdges + 1, result.Count);
    }

    /// <summary>
    /// Checks whether every position of the polygon really lies at the given radius around the center.
    /// </summary>
    [TestMethod]
    public void EveryPositionLiesAtTheGivenRadius()
    {
        var center = TestDataProvider.GetCenterDouble();
        var expectedRadius = (double)TestDataProvider.RadiusInMeters;
        var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

        foreach (var position in result)
        {
            var distance = TestDataProvider.GetDistanceInMeters(center, position);
            Assert.AreEqual(expectedRadius, distance, 0.001, $"The position {position[0]}|{position[1]} is not on the circle.");
        }
    }

    /// <summary>
    /// Checks whether the bearing rotates the polygon. Four edges and 90 degrees shift every position by one,
    /// because the four corners stay the same four corners.
    /// </summary>
    [TestMethod]
    public void TheBearingRotatesThePolygon()
    {
        var center = TestDataProvider.GetCenterDouble();
        var withoutBearing = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, 4);
        var withBearing = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, 4, 90);

        Assert.AreEqual(center[0], withoutBearing[0][0], 1e-9, "Without a bearing the first position must be due north of the center.");
        Assert.IsTrue(withoutBearing[0][1] > center[1], "Without a bearing the first position must be north of the center.");
        Assert.IsTrue(withBearing[0][0] > center[0], "With a bearing of 90 degrees the first position must be east of the center.");

        for (var i = 0; i < 3; i++)
        {
            Assert.AreEqual(withoutBearing[i][0], withBearing[i + 1][0], 1e-9, "The bearing did not shift the longitudes by one position.");
            Assert.AreEqual(withoutBearing[i][1], withBearing[i + 1][1], 1e-9, "The bearing did not shift the latitudes by one position.");
        }
    }

    /// <summary>
    /// Checks whether the direction reverses the way the ring runs around the center.
    /// </summary>
    [TestMethod]
    public void TheDirectionReversesTheRing()
    {
        var center = TestDataProvider.GetCenterDouble();
        var rightHanded = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);
        var leftHanded = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges, 0, -1);

        for (var i = 1; i < TestDataProvider.NumberOfEdges; i++)
        {
            Assert.AreEqual(rightHanded[TestDataProvider.NumberOfEdges - i][0], leftHanded[i][0], 1e-9, "The longitudes do not run the other way round.");
            Assert.AreEqual(rightHanded[TestDataProvider.NumberOfEdges - i][1], leftHanded[i][1], 1e-9, "The latitudes do not run the other way round.");
        }
    }

    /// <summary>
    /// Checks whether fewer than three edges are reported with the parameter name of the number of edges.
    /// </summary>
    [TestMethod]
    public void TooFewEdgesThrowAnArgumentException()
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), TestDataProvider.RadiusInMeters, 2));

        Assert.AreEqual("numberOfEdges", exception.ParamName);
        StringAssert.StartsWith(exception.Message, "The number of edges must be at least 3.");
    }

    /// <summary>
    /// Checks whether a radius of zero or less is reported with the parameter name of the radius.
    /// </summary>
    /// <param name="radiusInMeters">The radius in meters.</param>
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void ANonPositiveRadiusThrowsAnArgumentException(int radiusInMeters)
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), radiusInMeters, TestDataProvider.NumberOfEdges));

        Assert.AreEqual("radiusInMeters", exception.ParamName);
        StringAssert.StartsWith(exception.Message, "The radius must be positive.");
    }

    /// <summary>
    /// Checks whether a center position with anything but two elements is rejected. Three elements are rejected
    /// too, the altitude the original JavaScript library ignores is not accepted here.
    /// </summary>
    [TestMethod]
    public void ACenterWithoutExactlyTwoElementsThrowsAnArgumentException()
    {
        var oneElement = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDouble.GetPolygonFromCircle([173.283966], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));
        var threeElements = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDouble.GetPolygonFromCircle([173.283966, -41.270634, 100], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));

        Assert.AreEqual("center", oneElement.ParamName);
        Assert.AreEqual("center", threeElements.ParamName);
        StringAssert.StartsWith(oneElement.Message, "The center coordinates must have exactly two elements.");
        StringAssert.StartsWith(threeElements.Message, "The center coordinates must have exactly two elements.");
    }

    /// <summary>
    /// Checks whether a longitude outside of -180 to 180 is reported.
    /// </summary>
    /// <param name="longitude">The longitude.</param>
    [TestMethod]
    [DataRow(180.1)]
    [DataRow(-180.1)]
    public void ALongitudeOutOfRangeThrowsAnArgumentException(double longitude)
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDouble.GetPolygonFromCircle([longitude, -41.270634], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));

        Assert.AreEqual("center", exception.ParamName);
        StringAssert.StartsWith(exception.Message, "The longitude must be between -180 and 180.");
    }

    /// <summary>
    /// Checks whether a latitude outside of -90 to 90 is reported.
    /// </summary>
    /// <param name="latitude">The latitude.</param>
    [TestMethod]
    [DataRow(90.1)]
    [DataRow(-90.1)]
    public void ALatitudeOutOfRangeThrowsAnArgumentException(double latitude)
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDouble.GetPolygonFromCircle([173.283966, latitude], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));

        Assert.AreEqual("center", exception.ParamName);
        StringAssert.StartsWith(exception.Message, "The latitude must be between -90 and 90.");
    }
}
