// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonCircleDecimal.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJSON circle representation (For decimal).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// A GeoJSON circle representation (For decimal).
/// </summary>
public sealed record class GeoJsonCircleDecimal
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
    public List<List<decimal[]>> Coordinates { get; init; } = new();
}
