// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleCircleMath.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The trigonometry of the circle calculation for doubles, taken from System.Math.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// The trigonometry of the circle calculation for <see cref="double"/>s, taken from <see cref="Math"/>. It is a
/// struct because a static class cannot be used as a type argument.
/// </summary>
internal readonly struct DoubleCircleMath : ICircleMath<double>
{
    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static double Pi => Math.PI;

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static double Sin(double value)
    {
        return Math.Sin(value);
    }

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static double Cos(double value)
    {
        return Math.Cos(value);
    }

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static double Asin(double value)
    {
        return Math.Asin(value);
    }

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static double Atan2(double y, double x)
    {
        return Math.Atan2(y, x);
    }
}
