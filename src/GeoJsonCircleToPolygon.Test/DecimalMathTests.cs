//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalMathTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A test class to test the <see cref="DecimalMath" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Test;

/// <summary>
/// A test class to test the <see cref="DecimalMath"/> class. It is public API of this library, and the decimal
/// circle helper stands and falls with it.
/// </summary>
[TestClass]
public class DecimalMathTests
{
    /// <summary>
    /// The tolerance the series of <see cref="DecimalMath"/> are compared to <see cref="Math"/> with. It is far
    /// above the precision of a decimal and far below the precision of a double.
    /// </summary>
    private const double Tolerance = 1e-12;

    /// <summary>
    /// Checks whether the sinus and the cosinus match the ones of <see cref="Math"/>.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    [TestMethod]
    [DataRow(0.0)]
    [DataRow(30.0)]
    [DataRow(90.0)]
    [DataRow(180.0)]
    [DataRow(270.0)]
    [DataRow(-41.270634)]
    public void TheSinusAndTheCosinusMatchTheOnesOfMath(double angleInDegrees)
    {
        var angleInRadians = (angleInDegrees * Math.PI) / 180;
        var angleInRadiansDecimal = (decimal)angleInDegrees * DecimalMath.Pi / 180;

        Assert.AreEqual(Math.Sin(angleInRadians), (double)DecimalMath.Sin(angleInRadiansDecimal), Tolerance, "The sinus differs.");
        Assert.AreEqual(Math.Cos(angleInRadians), (double)DecimalMath.Cos(angleInRadiansDecimal), Tolerance, "The cosinus differs.");
    }

    /// <summary>
    /// Checks whether the arcus sinus matches the one of <see cref="Math"/>.
    /// </summary>
    /// <param name="value">The value to get the arcus sinus of.</param>
    [TestMethod]
    [DataRow(0.0)]
    [DataRow(0.5)]
    [DataRow(-0.5)]
    [DataRow(1.0)]
    [DataRow(-0.66)]
    public void TheArcusSinusMatchesTheOneOfMath(double value)
    {
        Assert.AreEqual(Math.Asin(value), (double)DecimalMath.Asin((decimal)value), Tolerance);
    }

    /// <summary>
    /// Checks whether the arcus tangens of two values matches the one of <see cref="Math"/>, in every quadrant and
    /// on both axes.
    /// </summary>
    /// <param name="y">The y value.</param>
    /// <param name="x">The x value.</param>
    [TestMethod]
    [DataRow(1.0, 1.0)]
    [DataRow(1.0, -1.0)]
    [DataRow(-1.0, -1.0)]
    [DataRow(-1.0, 1.0)]
    [DataRow(1.0, 0.0)]
    [DataRow(-1.0, 0.0)]
    [DataRow(0.5, 2.0)]
    public void TheArcusTangensOfTwoValuesMatchesTheOneOfMath(double y, double x)
    {
        Assert.AreEqual(Math.Atan2(y, x), (double)DecimalMath.Atan2((decimal)y, (decimal)x), Tolerance);
    }

    /// <summary>
    /// Checks whether the square root matches the one of <see cref="Math"/>. The default epsilon of zero is only
    /// used for a value whose root is exact, the iteration stops at bit equality otherwise.
    /// </summary>
    [TestMethod]
    public void TheSquareRootMatchesTheOneOfMath()
    {
        Assert.AreEqual(2m, DecimalMath.Sqrt(4m), "The root of an exact square must be exact.");
        Assert.AreEqual(Math.Sqrt(2), (double)DecimalMath.Sqrt(2m, DecimalMath.Epsilon), Tolerance);
        Assert.AreEqual(Math.Sqrt(0.25), (double)DecimalMath.Sqrt(0.25m, DecimalMath.Epsilon), Tolerance);
        Assert.AreEqual(0m, DecimalMath.Sqrt(0m));
    }

    /// <summary>
    /// Checks whether the constants of <see cref="DecimalMath"/> match the ones of <see cref="Math"/> as far as a
    /// double can tell.
    /// </summary>
    [TestMethod]
    public void TheConstantsMatchTheOnesOfMath()
    {
        // The values go through a variable because comparing two constants is what MSTEST0025 complains about.
        var pi = (double)DecimalMath.Pi;
        var e = (double)DecimalMath.E;

        Assert.AreEqual(Math.PI, pi, Tolerance);
        Assert.AreEqual(Math.E, e, Tolerance);
    }

    /// <summary>
    /// Checks whether the integer power returns the same as the power of <see cref="Math"/>, negative exponents
    /// included.
    /// </summary>
    [TestMethod]
    public void TheIntegerPowerMatchesTheOneOfMath()
    {
        Assert.AreEqual(1m, DecimalMath.PowerN(7m, 0));
        Assert.AreEqual(1024m, DecimalMath.PowerN(2m, 10));
        Assert.AreEqual(0.25m, DecimalMath.PowerN(2m, -2));
    }

    /// <summary>
    /// Checks whether an arcus sinus outside of the defined range is reported instead of returning a wrong value.
    /// </summary>
    /// <param name="value">The value to get the arcus sinus of.</param>
    [TestMethod]
    [DataRow(1.1)]
    [DataRow(-1.1)]
    public void AnArcusSinusOutOfRangeThrowsAnArgumentException(double value)
    {
        Assert.ThrowsExactly<ArgumentException>(() => DecimalMath.Asin((decimal)value));
    }

    /// <summary>
    /// Checks whether the square root of a negative number is reported.
    /// </summary>
    [TestMethod]
    public void TheSquareRootOfANegativeNumberThrowsAnOverflowException()
    {
        Assert.ThrowsExactly<OverflowException>(() => DecimalMath.Sqrt(-1m));
    }

    /// <summary>
    /// Checks whether the logarithm of zero and of a negative number is reported.
    /// </summary>
    /// <param name="value">The value to get the logarithm of.</param>
    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-1.0)]
    public void TheLogarithmOfANonPositiveNumberThrowsAnArgumentException(double value)
    {
        Assert.ThrowsExactly<ArgumentException>(() => DecimalMath.Log((decimal)value));
    }
}
