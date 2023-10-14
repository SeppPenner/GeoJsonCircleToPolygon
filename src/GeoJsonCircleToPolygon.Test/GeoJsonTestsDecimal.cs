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
        var coordinates = new decimal[] { 173.283966m, -41.270634m };
        var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(coordinates, 200000, 32);
        var result2 = CircleToPolygonHelperDecimal.GetGeoJsonPolygonFromCircle(coordinates, 200000, 32);

        Assert.IsNotNull(result);
        var resultLines = result.Select(v => $"{v[0]}|{v[1]}").ToList();
        var expectedResultLines = expectedResult.Select(v => $"{v[0]}|{v[1]}").ToList();
        CollectionAssert.AreEqual(expectedResultLines, resultLines);
    }
}
