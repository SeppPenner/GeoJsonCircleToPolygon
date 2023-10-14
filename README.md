GeoJsonCircleToPolygon
====================================

GeoJsonCircleToPolygon is an to generate GeoJSON representations of circles to polygon.

[![Build status](https://ci.appveyor.com/api/projects/status/a3ptt999etgpfutv?svg=true)](https://ci.appveyor.com/project/SeppPenner/GeoJsonCircleToPolygon)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/GeoJsonCircleToPolygon.svg)](https://github.com/SeppPenner/GeoJsonCircleToPolygon/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/GeoJsonCircleToPolygon.svg)](https://github.com/SeppPenner/GeoJsonCircleToPolygon/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/GeoJsonCircleToPolygon.svg)](https://github.com/SeppPenner/GeoJsonCircleToPolygon/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/GeoJsonCircleToPolygon/master/License.txt)
[![Nuget](https://img.shields.io/badge/GeoJsonCircleToPolygon-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/GeoJsonCircleToPolygon/badge.svg)](https://snyk.io/test/github/SeppPenner/GeoJsonCircleToPolygon)
[![Gitter](https://badges.gitter.im/GeoJsonCircleToPolygon/community.svg)](https://gitter.im/GeoJsonCircleToPolygon/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## Available for
* Net 6.0
* Net 7.0

## Net Core and Net Framework latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet

## Basic usage
```csharp
public void Test()
{
    // For decimals:
    var coordinates = new decimal[] { 173.283966m, -41.270634m };
    var result = CircleToPolygonHelperDecimal.GetPolygonFromCircle(coordinates, 200000, 32);
    var result2 = CircleToPolygonHelperDecimal.GetGeoJsonPolygonFromCircle(coordinates, 200000, 32);
	// For doubles:
	var coordinates = new decimal[] { 173.283966m, -41.270634m };
    var result = CircleToPolygonHelperDouble.GetPolygonFromCircle(coordinates, 200000, 32);
    var result2 = CircleToPolygonHelperDouble.GetGeoJsonPolygonFromCircle(coordinates, 200000, 32);
}
```

The project can be found on [nuget](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/).

## Install

```bash
dotnet add package HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon
```

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/GeoJsonCircleToPolygon/blob/master/Changelog.md).