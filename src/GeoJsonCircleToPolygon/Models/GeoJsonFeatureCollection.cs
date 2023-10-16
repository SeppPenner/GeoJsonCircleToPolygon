// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJsonFeatureCollection.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A GeoJson feature collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeoJsonCircleToPolygon.Models;

/// <summary>
/// A GeoJson feature collection.
/// </summary>
/// <typeparam name="T">The type parameter.</typeparam>
public record class GeoJsonFeatureCollection<T> where T: struct
{
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "FeatureCollection";

    /// <summary>
    /// Gets or sets the features.
    /// </summary>
    [JsonPropertyName("features")]
    public List<GeoJsonFeature<T>> Features { get; init; } = new();

    /// <summary>
    /// Adds a list of coordinates to the given structure.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    public void AddCoordinates(List<T[]> coordinates)
    {
        // If there is no feature yet, create one.
        if (this.Features.Count == 0)
        {
            var feature = new GeoJsonFeature<T>
            {
                Polygon = new GeoJsonPolygon<T>
                {
                    Coordinates = new List<List<T[]>>
                    {
                        coordinates
                    }
                }
            };

            this.Features.Add(feature);
            return;
        }

        // If there is a feature, just add the coordinates to the collection.
        var firstFeature = this.Features[0];
        firstFeature.Polygon.Coordinates.Add(coordinates);
    }

    /// <inheritdoc cref="object"/>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
