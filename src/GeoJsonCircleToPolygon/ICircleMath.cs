// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICircleMath.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The trigonometry the circle calculation needs, for a numeric type that does not bring it along.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// The trigonometry the circle calculation needs. <see cref="INumber{TSelf}"/> only covers the arithmetic, and
/// <see cref="decimal"/> implements neither <see cref="ITrigonometricFunctions{TSelf}"/> nor
/// <see cref="IFloatingPointConstants{TSelf}"/>, so the functions come in through this interface instead.
/// </summary>
/// <typeparam name="T">The numeric type the functions work on.</typeparam>
internal interface ICircleMath<T> where T : INumber<T>
{
    /// <summary>
    /// Gets PI.
    /// </summary>
    static abstract T Pi { get; }

    /// <summary>
    /// Gets the sinus value of the given angle in radians.
    /// </summary>
    /// <param name="value">The angle in radians.</param>
    /// <returns>The sinus value.</returns>
    static abstract T Sin(T value);

    /// <summary>
    /// Gets the cosinus value of the given angle in radians.
    /// </summary>
    /// <param name="value">The angle in radians.</param>
    /// <returns>The cosinus value.</returns>
    static abstract T Cos(T value);

    /// <summary>
    /// Gets the arcus sinus value of the given value.
    /// </summary>
    /// <param name="value">The value to get the arcus sinus value from.</param>
    /// <returns>The arcus sinus value in radians.</returns>
    static abstract T Asin(T value);

    /// <summary>
    /// Gets the arcus tangens value of the quotient of the two given values.
    /// </summary>
    /// <param name="y">The y value.</param>
    /// <param name="x">The x value.</param>
    /// <returns>The arcus tangens value in radians.</returns>
    static abstract T Atan2(T y, T x);
}
