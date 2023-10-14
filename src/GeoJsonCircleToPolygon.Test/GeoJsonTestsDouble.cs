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
        var coordinates = new double[] { 173.283966, -41.270634 };
        var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(coordinates, 200000, 32);

        Assert.IsNotNull(result);
        var resultLines = result.Select(v => $"{v[0]}|{v[1]}").ToList();
        var expectedResultLines = expectedResult.Select(v => $"{v[0]}|{v[1]}").ToList();
        CollectionAssert.AreEqual(expectedResultLines, resultLines);
    }
}
