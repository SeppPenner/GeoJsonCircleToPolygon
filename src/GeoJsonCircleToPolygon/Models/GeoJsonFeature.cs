// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonFeature.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJson feature.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Models;

/// <summary>
/// A GeoJson feature.
/// </summary>
/// <typeparam name="T">The type parameter.</typeparam>
public sealed record class GeoJsonFeature<T> where T : struct
{
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "Feature";

    /// <summary>
    /// Gets or sets the properties.
    /// </summary>
    [JsonPropertyName("properties")]
    public GeoJsonProperties Properties { get; init; } = new();

    /// <summary>
    /// Gets or sets the polygon.
    /// </summary>
    [JsonPropertyName("geometry")]
    public GeoJsonPolygon<T> Polygon { get; init; } = new();
}
