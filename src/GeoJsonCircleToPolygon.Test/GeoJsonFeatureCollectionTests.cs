//--------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonFeatureCollectionTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A test class to test the GeoJSON structures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Test;

/// <summary>
/// A test class to test the GeoJSON structures.
/// </summary>
[TestClass]
public class GeoJsonFeatureCollectionTests
{
    /// <summary>
    /// Checks whether the serialized collection carries the member names and the type values the GeoJSON format
    /// asks for, including the empty properties object.
    /// </summary>
    [TestMethod]
    public void TheSerializedCollectionIsValidGeoJson()
    {
        var coordinates = CircleToPolygonHelperDouble.GetPolygonFromCircle(TestDataProvider.GetCenterDouble(), TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);
        var featureCollection = new GeoJsonFeatureCollectionDouble();
        featureCollection.AddCoordinates(coordinates);

        using var document = JsonDocument.Parse(featureCollection.ToString());
        var root = document.RootElement;

        Assert.AreEqual("FeatureCollection", root.GetProperty("type").GetString());
        Assert.AreEqual(1, root.GetProperty("features").GetArrayLength());

        var feature = root.GetProperty("features")[0];
        Assert.AreEqual("Feature", feature.GetProperty("type").GetString());
        Assert.AreEqual(JsonValueKind.Object, feature.GetProperty("properties").ValueKind);

        var geometry = feature.GetProperty("geometry");
        Assert.AreEqual("Polygon", geometry.GetProperty("type").GetString());

        var rings = geometry.GetProperty("coordinates");
        Assert.AreEqual(1, rings.GetArrayLength());
        Assert.AreEqual(TestDataProvider.NumberOfEdges + 1, rings[0].GetArrayLength());
        Assert.AreEqual(2, rings[0][0].GetArrayLength());
    }

    /// <summary>
    /// Checks whether a second call of AddCoordinates adds another ring to the polygon of the first feature instead
    /// of a second feature. In GeoJSON that second ring is a hole, which is the documented behaviour of this type.
    /// </summary>
    [TestMethod]
    public void AddCoordinatesTwiceAddsARingToTheSameFeature()
    {
        var center = TestDataProvider.GetCenterDouble();
        var outerRing = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters, TestDataProvider.NumberOfEdges);
        var innerRing = CircleToPolygonHelperDouble.GetPolygonFromCircle(center, TestDataProvider.RadiusInMeters / 2, TestDataProvider.NumberOfEdges);
        var featureCollection = new GeoJsonFeatureCollectionDouble();

        featureCollection.AddCoordinates(outerRing);
        featureCollection.AddCoordinates(innerRing);

        Assert.AreEqual(1, featureCollection.Features.Count, "The second call must not create a second feature.");
        Assert.AreEqual(2, featureCollection.Features[0].Polygon.Coordinates.Count, "The second call must add a ring to the first polygon.");
    }

    /// <summary>
    /// Checks whether the two derived collections return JSON from ToString instead of the output a record generates
    /// on its own. Without the override that only calls the base implementation they would print
    /// "GeoJsonFeatureCollectionDouble { Type = FeatureCollection, ... }".
    /// </summary>
    [TestMethod]
    public void ToStringOfTheDerivedCollectionsReturnsJson()
    {
        var decimalCollection = new GeoJsonFeatureCollectionDecimal();
        var doubleCollection = new GeoJsonFeatureCollectionDouble();

        StringAssert.StartsWith(decimalCollection.ToString(), "{\"type\":\"FeatureCollection\"");
        StringAssert.StartsWith(doubleCollection.ToString(), "{\"type\":\"FeatureCollection\"");
    }

    /// <summary>
    /// Checks whether a collection nobody added coordinates to serializes an empty feature list.
    /// </summary>
    [TestMethod]
    public void AnEmptyCollectionSerializesAnEmptyFeatureList()
    {
        var featureCollection = new GeoJsonFeatureCollectionDouble();

        using var document = JsonDocument.Parse(featureCollection.ToString());

        Assert.AreEqual("FeatureCollection", document.RootElement.GetProperty("type").GetString());
        Assert.AreEqual(0, document.RootElement.GetProperty("features").GetArrayLength());
    }

    /// <summary>
    /// Checks whether the sample of the Readme compiles and returns GeoJSON for both numeric types. The Readme is
    /// the only user documentation there is, so a sample that does not compile is a bug of its own.
    /// </summary>
    [TestMethod]
    public void TheReadmeSampleReturnsGeoJsonForBothTypes()
    {
        // For decimals:
        var coordinatesDecimal = new decimal[] { 173.283966m, -41.270634m };
        var resultDecimal = CircleToPolygonHelperDecimal.GetPolygonFromCircle(coordinatesDecimal, 200000, 32);

        // For usage in the GeoJSON format.
        var featureCollectionDecimal = new GeoJsonFeatureCollectionDecimal();
        featureCollectionDecimal.AddCoordinates(resultDecimal);
        var geoJsonDecimal = featureCollectionDecimal.ToString();

        // For doubles:
        var coordinatesDouble = new double[] { 173.283966, -41.270634 };
        var resultDouble = CircleToPolygonHelperDouble.GetPolygonFromCircle(coordinatesDouble, 200000, 32);

        // For usage in the GeoJSON format.
        var featureCollectionDouble = new GeoJsonFeatureCollectionDouble();
        featureCollectionDouble.AddCoordinates(resultDouble);
        var geoJsonDouble = featureCollectionDouble.ToString();

        StringAssert.Contains(geoJsonDecimal, "\"type\":\"Polygon\"");
        StringAssert.Contains(geoJsonDouble, "\"type\":\"Polygon\"");
    }
}
