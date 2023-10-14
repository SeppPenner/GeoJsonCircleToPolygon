//--------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonCircleDouble.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJSON circle representation (For double).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon;

/// <summary>
/// A GeoJSON circle representation (For double).
/// </summary>
public sealed record class GeoJsonCircleDouble
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
    public List<List<double[]>> Coordinates { get; init; } = new();
}
