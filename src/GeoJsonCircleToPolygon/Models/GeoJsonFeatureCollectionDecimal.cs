// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonFeatureCollectionDecimal.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJson feature collection for decimals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Models;

/// <summary>
/// A GeoJson feature collection for decimals.
/// </summary>
public sealed record class GeoJsonFeatureCollectionDecimal : GeoJsonFeatureCollection<decimal>
{
    /// <inheritdoc cref="object"/>
    public override string ToString()
    {
        return base.ToString();
    }
}
