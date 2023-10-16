// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonFeatureCollectionDouble.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJson feature collection for doubles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Models;

/// <summary>
/// A GeoJson feature collection for doubles.
/// </summary>
public sealed record class GeoJsonFeatureCollectionDouble : GeoJsonFeatureCollection<double>
{
    /// <inheritdoc cref="object"/>
    public override string ToString()
    {
        return base.ToString();
    }
}
