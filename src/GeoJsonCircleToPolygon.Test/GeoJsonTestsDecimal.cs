//--------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonTestsDecimal.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A test class to test the GeoJSON features (decimal).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Test;

/// <summary>
/// A test class to test the GeoJSON features (decimal).
/// </summary>
[TestClass]
public class GeoJsonTestsDecimal
{
    /// <summary>
    /// The expected result.
    /// </summary>
    private static readonly List<decimal[]> expectedResult = new()
    {
        new decimal[] { 173.283966m, -39.474003431760957129744997539m },
        new decimal[] { 172.82974266083432530195616004m, -39.507619453317972481439756816m },
        new decimal[] { 172.3916671758056518124186001m, -39.607271255365905859149756425m },
        new decimal[] { 171.98544458449058913538971803m, -39.769403407653453427930871159m },
        new decimal[] { 171.62589074038397723661499725m, -39.988201443168669599164945469m },
        new decimal[] { 171.32648028488368676213200767m, -40.255758887782202177592055739m },
        new decimal[] { 171.09888995216615415217871693m, -40.56231121046950378313280444m },
        new decimal[] { 170.95254312829123554369420866m, -40.896536249949876602907977413m },
        new decimal[] { 170.89416849173901250357196835m, -41.24591956982945068927760604m },
        new decimal[] { 170.92739416288480262647874785m, -41.597181119390938180548500775m },
        new decimal[] { 171.05240815857457566378065866m, -41.936756221454499768298030394m },
        new decimal[] { 171.26572430426509782565988677m, -42.251319123422792252599566407m },
        new decimal[] { 171.56009750883516451294051375m, -42.52833131449401444707792312m },
        new decimal[] { 171.92463048949189698516782764m, -42.756590192199282337995172484m },
        new decimal[] { 172.34510319598590650643271025m, -42.926747640181919981180171789m },
        new decimal[] { 172.80453558092948163510318833m, -43.031764221247446666267307843m },
        new decimal[] { 173.283966m, -43.067264568239042870255002442m },
        new decimal[] { 173.76339641907051836489681167m, -43.031764221247446666267307843m },
        new decimal[] { 174.22282880401409349356728975m, -42.926747640181919981180171789m },
        new decimal[] { 174.64330151050810301483217236m, -42.756590192199282337995172484m },
        new decimal[] { 175.00783449116483548705948625m, -42.52833131449401444707792312m },
        new decimal[] { 175.30220769573490217434011322m, -42.251319123422792252599566407m },
        new decimal[] { 175.51552384142542433621934133m, -41.936756221454499768298030394m },
        new decimal[] { 175.64053783711519737352125214m, -41.597181119390938180548500775m },
        new decimal[] { 175.67376350826098749642803165m, -41.24591956982945068927760604m },
        new decimal[] { 175.61538887170876445630579133m, -40.896536249949876602907977413m },
        new decimal[] { 175.46904204783384584782128306m, -40.56231121046950378313280444m },
        new decimal[] { 175.24145171511631323786799233m, -40.255758887782202177592055739m },
        new decimal[] { 174.94204125961602276338500275m, -39.988201443168669599164945469m },
        new decimal[] { 174.58248741550941086461028197m, -39.769403407653453427930871159m },
        new decimal[] { 174.1762648241943481875813999m, -39.607271255365905859149756425m },
        new decimal[] { 173.73818933916567469804383996m, -39.507619453317972481439756816m },
        new decimal[] { 173.283966m, -39.474003431760957129744997539m }
    };

    /// <summary>
    /// Tests the GeoJSON result from the example.
    /// </summary>
    [TestMethod]
    public void TestGeoJsonResultFromExample()
    {
        var coordinates = TestDataProvider.GetCenterDecimal();
        var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(coordinates, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

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
        var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(TestDataProvider.GetCenterDecimal(), TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

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
    public void TheNumberOfPositionsIsTheNumberOfEdgesPlusOne(int numberOfEdges)
    {
        var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(TestDataProvider.GetCenterDecimal(), TestDataProvider.RadiusInMeters, numberOfEdges);

        Assert.AreEqual(numberOfEdges + 1, result.Count);
    }

    /// <summary>
    /// Checks whether the number of edges defaults to 32, the value the Readme documents.
    /// </summary>
    [TestMethod]
    public void TheNumberOfEdgesDefaultsToThirtyTwo()
    {
        var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(TestDataProvider.GetCenterDecimal(), TestDataProvider.RadiusInMeters);

        Assert.AreEqual(TestDataProvider.NumberOfEdges + 1, result.Count);
    }

    /// <summary>
    /// Checks whether every position of the polygon really lies at the given radius around the center. The distance
    /// is measured in <see cref="double"/> because that is precise enough for a tolerance of a millimeter.
    /// </summary>
    [TestMethod]
    public void EveryPositionLiesAtTheGivenRadius()
    {
        var center = TestDataProvider.GetCenterDecimal();
        var centerAsDouble = TestDataProvider.GetCenterDouble();
        var expectedRadius = (double)TestDataProvider.RadiusInMeters;
        var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

        foreach (var position in result)
        {
            var distance = TestDataProvider.GetDistanceInMeters(centerAsDouble, [(double)position[0], (double)position[1]]);
            Assert.AreEqual(expectedRadius, distance, 0.001, $"The position {position[0]}|{position[1]} is not on the circle.");
        }
    }

    /// <summary>
    /// Checks whether the decimal helper and the double helper agree within the precision of a double. This is the
    /// test that would catch a change made to only one of the two copies of the algorithm.
    /// </summary>
    [TestMethod]
    public void TheDecimalResultMatchesTheDoubleResult()
    {
        var resultDecimal = CircleToPolygonHelperDecimal.GetPolygonFromCircle(TestDataProvider.GetCenterDecimal(), TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);
        var resultDouble = CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);

        Assert.AreEqual(resultDouble.Count, resultDecimal.Count);

        for (var i = 0; i < resultDouble.Count; i++)
        {
            Assert.AreEqual(resultDouble[i][0], (double)resultDecimal[i][0], 1e-9, "The longitudes of the two helpers differ.");
            Assert.AreEqual(resultDouble[i][1], (double)resultDecimal[i][1], 1e-9, "The latitudes of the two helpers differ.");
        }
    }

    /// <summary>
    /// Checks whether fewer than three edges are reported with the parameter name of the number of edges.
    /// </summary>
    [TestMethod]
    public void TooFewEdgesThrowAnArgumentException()
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDecimal.GetPolygonFromCircle(TestDataProvider.GetCenterDecimal(), TestDataProvider.RadiusInMeters, 2));

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
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDecimal.GetPolygonFromCircle(TestDataProvider.GetCenterDecimal(), radiusInMeters, TestDataProvider.NumberOfEdges));

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
        var oneElement = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDecimal.GetPolygonFromCircle([173.283966m], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));
        var threeElements = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDecimal.GetPolygonFromCircle([173.283966m, -41.270634m, 100m], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));

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
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDecimal.GetPolygonFromCircle([(decimal)longitude, -41.270634m], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));

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
        var exception = Assert.ThrowsExactly<ArgumentException>(() => CircleToPolygonHelperDecimal.GetPolygonFromCircle([173.283966m, (decimal)latitude], TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges));

        Assert.AreEqual("center", exception.ParamName);
        StringAssert.StartsWith(exception.Message, "The latitude must be between -90 and 90.");
    }
}
