# Project rules for Claude

## What this is

GeoJsonCircleToPolygon is a class library that turns a circle (a center coordinate plus a radius in
meters) into a polygon with a given number of edges, and wraps the resulting points into a GeoJSON
`FeatureCollection` if the caller wants that. It is a fork of
[circle-to-polygon](https://github.com/gabzim/circle-to-polygon) ported to C#. The library **is**
published as the NuGet package `HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon`:
`GeneratePackageOnBuild` is on, `BuildAndPushPackage.bat` pushes to nuget.org and to the GitHub
package feed.

One solution `src/GeoJsonCircleToPolygon.sln` with exactly two projects:

- `src/GeoJsonCircleToPolygon/GeoJsonCircleToPolygon.csproj`, a library, multi-targeted, the actual
  code.
- `src/GeoJsonCircleToPolygon.Test/GeoJsonCircleToPolygon.Test.csproj`, MSTest.

Layout inside `src/GeoJsonCircleToPolygon`:

- `CircleToPolygonHelperDouble.cs` and `CircleToPolygonHelperDecimal.cs`: the public entry points,
  one static class each, `GetPolygonFromCircle` validates its arguments and walks the edges. The
  private helpers `GetRadians`, `GetDegrees` and `GetOffsetCoordinates` do one thing each, keep new
  logic in that shape. Both files hold the same calculation, once for `double` and once for
  `decimal`.
- `DecimalMath.cs`: a stand-in for `System.Math` in `decimal`, because `Math` only knows `double`.
  Taylor series with a `MaximumIterations` cap of 100. Only `Asin`, `Atan2`, `Sin`, `Cos` and
  `Sqrt` are actually used by the helper, the rest of the class comes from the original source.
- `Models/GeoJsonFeatureCollection.cs`: the generic `GeoJsonFeatureCollection<T> where T : struct`
  with `AddCoordinates` and a `ToString` that returns the serialized JSON.
  `Models/GeoJsonFeatureCollectionDecimal.cs` and `Models/GeoJsonFeatureCollectionDouble.cs` are
  the sealed, non generic types the Readme sample uses. `Models/GeoJsonFeature.cs`,
  `Models/GeoJsonPolygon.cs` and `Models/GeoJsonProperties.cs` are the GeoJSON structure itself, all
  of them `record class` with `init` only properties and `JsonPropertyName` attributes.
- `GlobalUsings.cs`: all usings of the project.

Layout inside `src/GeoJsonCircleToPolygon.Test`:

- `GeoJsonTestsDouble.cs` and `GeoJsonTestsDecimal.cs`: the polygon of the Readme example against a
  hardcoded expected result of 33 coordinate pairs, the closed ring, the number of positions, the
  default number of edges, the distance of every position to the center, the bearing, the direction
  and one test per rejected argument. `GeoJsonTestsDecimal` additionally compares both helpers
  against each other, which is the test that catches a change made to only one of the two copies of
  the algorithm.
- `GeoJsonFeatureCollectionTests.cs`: the serialized GeoJSON structure, the second `AddCoordinates`
  call that adds a ring instead of a feature, the `ToString` of the two derived collections, the
  empty collection and the Readme sample compiled as a test.
- `DecimalMathTests.cs`: `Sin`, `Cos`, `Asin`, `Atan2`, `Sqrt`, `PowerN` and the constants against
  `System.Math`, plus the three exceptions.
- `TestDataProvider.cs`: the center, the radius and the number of edges all test classes use, the
  values of the Readme example, plus the haversine distance the radius checks need.
- `GlobalUsings.cs`: all usings of the test project.

Repository root: `README.md` (the only user documentation, with the usage sample, the parameter list
and the disclaimer about circles crossing the date line), `Changelog.md`, `Updating.md`,
`License.txt` (MIT), `Icon.png` (the package icon), `Globe.ico`, `BuildAndPushPackage.bat`,
`Delete-BIN-OBJ-Folders.bat`, `.gitattributes`, `.gitignore` and `src/.editorconfig`. There is no
`HowToUse.md` and no screenshots.

## Build

```powershell
dotnet build src/GeoJsonCircleToPolygon.sln
```

```powershell
dotnet test src/GeoJsonCircleToPolygon.sln
```

- The library multi-targets `net8.0;net10.0`, the test project builds for `net10.0` only, so the
  `net8.0` assets are compiled but never executed by a test run. `net9.0` was dropped because it is
  out of support, the sibling repository `CSharpLanguageManager` did the same. No
  `RuntimeIdentifiers`, nothing in the code is platform specific.
- All build properties live directly in the two `.csproj` files and are duplicated there. There is
  **no** `Directory.Build.props` in this repository, unlike the sibling repository
  `CSharpLanguageManager`.
- `TreatWarningsAsErrors` is enabled in both projects, so every warning breaks the build, NuGet
  warnings (`NU****`) from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- `GenerateDocumentationFile` is on in the library, so the `.xml` file ships inside the package and a
  missing XML comment breaks the build through `CS1591`.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.6-3` for the third
  commit after tag `1.0.5`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/GeoJsonCircleToPolygon.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest, in the single test project `src/GeoJsonCircleToPolygon.Test`, which follows the
  same package set as the sibling repository `AesCryptFromStreamExample`: `Microsoft.NET.Test.Sdk`,
  `MSTest.TestAdapter`, `MSTest.TestFramework`, `coverlet.collector` and `GitVersion.MsBuild`, plus
  `JunitXml.TestLogger` for the external build. `dotnet test` runs 65 tests, they need no network, no
  file system and no fixture outside the repository, and a run leaves the working tree untouched.
  Never claim a test run happened without running it.
- The MSTest analyzers are active and their diagnostics are errors like every other warning.
  `MSTEST0025` fires when both sides of an `Assert.AreEqual` are constants, which is why
  `DecimalMathTests` compares its constants through a local variable.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members and private constants included,
  no exceptions.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into the `GlobalUsings.cs` of the respective project, inside the
  existing `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig
  requires usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`),
  which global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas. The
  comment text in that block is German because Visual Studio generated it, leave it alone.
- Instance members are accessed with `this.` qualification (`dotnet_style_qualification_for_*` at
  severity `warning`). The two helper classes are static, so `this.` never appears there, the models
  use it.
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.
- Every file in the repository is UTF-8 without BOM and uses CRLF, the markdown files at the root
  included. Keep that when you rewrite a file.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **Everything exists twice, for `double` and for `decimal`.** `CircleToPolygonHelperDouble` and
  `CircleToPolygonHelperDecimal` are the same algorithm with a different numeric type, and
  `GeoJsonTestsDouble` and `GeoJsonTestsDecimal` mirror each other the same way. A change to the
  calculation has to be made in both files, and both test classes carry their own expected result.
  The `decimal` variant is not just more precise, it is much slower, because `DecimalMath` evaluates
  series where the `double` variant calls a single intrinsic.
- **The `ToString` overrides in the two derived collections are not redundant.** A `record`
  generates its own `ToString`, so `GeoJsonFeatureCollectionDecimal` and
  `GeoJsonFeatureCollectionDouble` would print the compiler generated record output instead of the
  JSON of the base class. The `override` that only calls `base.ToString()` is what keeps
  `ToString()` returning JSON. Deleting it breaks the Readme sample.
- **`AddCoordinates` adds a ring, not a feature.** The first call creates the single feature and its
  polygon. Every later call appends the coordinates to `Features[0].Polygon.Coordinates`, which in
  GeoJSON is another ring of the *same* polygon, so a second circle becomes a hole rather than a
  second shape. That is why the type never grows a second feature.
- **`GeoJsonProperties` is deliberately empty.** Every feature therefore serializes
  `"properties": {}`, which is what the GeoJSON spec asks for, an absent member would be invalid.
- **`center` must have exactly two elements.** Longitude first, latitude second, the order that
  GeoJSON uses and the opposite of what most map UIs show.
- **Circles crossing the date line leave the coordinate range.** A circle around a center near
  ±180 degrees longitude produces values such as `182`, because the alternative would be a
  multi polygon, which the original library never implemented either. Documented in the Readme,
  do not "fix" it with a modulo.
- **The tests compare formatted strings, not numbers.** Both test classes project every coordinate
  to `$"{v[0]}|{v[1]}"` before the `CollectionAssert`. That hides the last bits of a floating point
  difference and it makes a failure readable. Both sides are formatted the same way, so the current
  culture cancels out.
- **`DecimalMath` keeps the formatting of its origin.** Its XML comments are indented with extra
  spaces (`///     Analogy of Math.Abs.`) unlike every other file in the repository, and it throws
  plain `Exception` in two places inside `Power`. It is imported code, leave the shape alone.
- **`DecimalMath.Sqrt` defaults to an epsilon of zero**, so the Newton iteration runs until two
  successive values are bit for bit equal instead of stopping at a tolerance. That terminates for
  the values this library feeds it, but it is the reason the parameter exists at all.
- **`Globe.ico` is unreferenced.** No project sets `ApplicationIcon`, the library is not an
  executable. `Icon.png` is the one that matters, it is packed into the NuGet package.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
  `JunitXml.TestLogger` in the test project exists for that external build.
- **`Updating.md` and `BuildAndPushPackage.bat` describe the same release from two angles.**
  `Updating.md` is the manual route through Visual Studio, the batch file is the automated one and
  needs `NUGET_API_KEY` and `GITHUB_API_KEY` in the environment. Neither is wrong, they are just
  redundant.
- **The solution file carries an orphan project GUID.** `{2B608369-1C6C-4EAB-8534-299AD7366B40}`
  appears in `ProjectConfigurationPlatforms` without a matching `Project(...)` entry, a leftover of
  a removed project. Visual Studio ignores it.
- **`.gitattributes` sets `* text=auto`** and every rule of the Visual Studio template below it is
  commented out. Any binary file added later needs its own rule.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.6.0 (2026-08-12)** : Short description.`
3. Put the same version and description into `PackageReleaseNotes` in
   `src/GeoJsonCircleToPolygon/GeoJsonCircleToPolygon.csproj`.
4. Commit that.
5. Tag the commit with the plain version number, no `v` prefix (`1.0.5`, `1.0.4`, ...). The existing
   tags are lightweight tags, create new ones the same way.
6. Only then build the package. GitVersion reads the tag of the current commit, so building before
   tagging burns a prerelease version such as `1.0.6-3` into the assembly and into the `.nupkg`.
7. Push the commits and the tag, then push the package with `BuildAndPushPackage.bat`.

The version in the `Changelog.md` and in `PackageReleaseNotes` has four parts (`1.0.6.0`), the tag
has three (`1.0.6`).

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
