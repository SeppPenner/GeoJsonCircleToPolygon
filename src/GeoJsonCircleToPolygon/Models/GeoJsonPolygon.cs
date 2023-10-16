// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonPolygon.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJson polygon.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Models;

/// <summary>
/// A GeoJson polygon.
/// </summary>
/// <typeparam name="T">The type parameter.</typeparam>
public sealed record class GeoJsonPolygon<T> where T: struct
{
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "Polygon";

    /// <summary>
    /// Gets or sets the coordinates.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public List<List<T[]>> Coordinates { get; init; } = new();
}
