GeoJsonCircleToPolygon
====================================

GeoJsonCircleToPolygon  is a library to generate GeoJSON representations of circles to polygon and a fork of https://github.com/gabzim/circle-to-polygon ported to C#. Special thanks to Gabriel Zimmermann, Johannes Jarbratt and Jan Žák for the original work.

[![Build status](https://ci.appveyor.com/api/projects/status/g97r2ruunhxqphtu?svg=true)](https://ci.appveyor.com/project/SeppPenner/geojsoncircletopolygon)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/GeoJsonCircleToPolygon.svg)](https://github.com/SeppPenner/GeoJsonCircleToPolygon/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/GeoJsonCircleToPolygon.svg)](https://github.com/SeppPenner/GeoJsonCircleToPolygon/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/GeoJsonCircleToPolygon.svg)](https://github.com/SeppPenner/GeoJsonCircleToPolygon/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/GeoJsonCircleToPolygon/master/License.txt)
[![Nuget](https://img.shields.io/badge/GeoJsonCircleToPolygon-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/GeoJsonCircleToPolygon/badge.svg)](https://snyk.io/test/github/SeppPenner/GeoJsonCircleToPolygon)
[![Gitter](https://badges.gitter.im/GeoJsonCircleToPolygon/community.svg)](https://gitter.im/GeoJsonCircleToPolygon/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Blogger](https://img.shields.io/badge/Follow_me_on-blogger-orange)](https://franzhuber23.blogspot.de/)
[![Patreon](https://img.shields.io/badge/Patreon-F96854?logo=patreon&logoColor=white)](https://patreon.com/SeppPennerOpenSourceDevelopment)
[![PayPal](https://img.shields.io/badge/PayPal-00457C?logo=paypal&logoColor=white)](https://paypal.me/th070795)

## Available for
* Net 6.0
* Net 7.0
* Net 8.0

## Net Core and Net Framework latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet

## Basic usage
```csharp
public void Test()
{
    // For decimals:
    var coordinatesDecimal = new decimal[] { 173.283966m, -41.270634m };
    var resultDecimal = CircleToPolygonHelperDecimal.GetPolygonFromCircle(coordinatesDecimal, 200000, 32);
    // For usage in the GeoJSON format.
    var featureCollectionDecimal = new GeoJsonFeatureCollectionDecimal();
    featureCollectionDecimal.AddCoordinates(resultDecimal);
    var geoJsonDecimal = featureCollectionDecimal.ToString();
    // -----------------------------------------------------------
    // For doubles:
    var coordinatesDouble = new double[] { 173.283966m, -41.270634m };
    var resultDouble = CircleToPolygonHelperDouble.GetPolygonFromCircle(coordinatesDouble, 200000, 32);
    // For usage in the GeoJSON format.
    var featureCollectionDouble = new GeoJsonFeatureCollectionDouble();
    featureCollectionDouble.AddCoordinates(resultDouble);
    var geoJsonDouble = featureCollectionDouble.ToString();
}
```

## Parameters
* `center`: The center coordinates: Array of length 2 or 3 (Required)
    * First element: Longitude (Required)
    * Second element: Latitude (Required)
    * Third element: Altitude (Ignored)
* `radiusInMeters`: Radius in meters (Required)
* `numberOfEdges`: The number of edges (Defaults to 32)
* `bearing`: How many degrees the circle should be rotated which is most noticeable for "circles" with few edges (Defaults to 0)
* `direction`: The direction of the circle (1 means right-hand-rule, -1 is opposite) (Defaults to 1)
* `earthRadius`: The earth radius (Defaults to 6378137)

## Disclaimers
- A circle whose edge cross longitude edges (-180 or 180) or a latitude edge (-90 or 90) will contain coordinate points that are outside the standardized coordinates (eg: [182, 23]). This is because there are two ways to represent a line going from [179, x] to [181, y]. One way is simply writing it as [[179, x], [182, y]] while the other is to write it as a multi-polygon. There is a plan to support multi-polygons but it has not yet been implemented.

## NuGet
The project can be found on [nuget](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/).

## Install

```bash
dotnet add package HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon
```

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/GeoJsonCircleToPolygon/blob/master/Changelog.md).
