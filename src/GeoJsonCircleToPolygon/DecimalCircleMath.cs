// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalCircleMath.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The trigonometry of the circle calculation for decimals, taken from DecimalMath.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// The trigonometry of the circle calculation for <see cref="decimal"/>s, taken from <see cref="DecimalMath"/>. It is
/// a struct because a static class cannot be used as a type argument.
/// </summary>
internal readonly struct DecimalCircleMath : ICircleMath<decimal>
{
    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static decimal Pi => DecimalMath.Pi;

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static decimal Sin(decimal value)
    {
        return DecimalMath.Sin(value);
    }

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static decimal Cos(decimal value)
    {
        return DecimalMath.Cos(value);
    }

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static decimal Asin(decimal value)
    {
        return DecimalMath.Asin(value);
    }

    /// <inheritdoc cref="ICircleMath{T}"/>
    /// <seealso cref="ICircleMath{T}"/>
    public static decimal Atan2(decimal y, decimal x)
    {
        return DecimalMath.Atan2(y, x);
    }
}
