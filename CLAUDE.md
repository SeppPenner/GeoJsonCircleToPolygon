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
  one static class each. Both are wrappers of exactly one line over the generic core, they exist
  because they are the published API and because a caller should not have to name a math strategy.
- `CircleToPolygonHelper.cs`: the `internal` generic core, `GetPolygonFromCircle<T, TMath>` validates
  its arguments and walks the edges. The private helpers `GetRadians`, `GetDegrees` and
  `GetOffsetCoordinates` do one thing each, keep new logic in that shape. This is the only place the
  calculation exists.
- `ICircleMath.cs` plus `DoubleCircleMath.cs` and `DecimalCircleMath.cs`: the trigonometry the core
  needs, once mapped to `System.Math` and once to `DecimalMath`, all three `internal`.
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

- The library multi-targets `net10.0;net11.0`, so building needs the .NET 11 SDK. The test project
  builds for `net11.0` only, so the `net10.0` assets are compiled but never executed by a test run.
  Keep the test project on the newest framework the library targets. `net9.0` was dropped because it
  is out of support, `net8.0` after version 1.0.7.0. No `RuntimeIdentifiers`, nothing in the code is
  platform specific.
- All build properties live directly in the two `.csproj` files and are duplicated there. There is
  **no** `Directory.Build.props` in this repository, unlike the sibling repository
  `CSharpLanguageManager`.
- .NET 7 is the technical floor. The generic core needs `INumber<T>` and `static abstract` interface
  members, both of which arrived with .NET 7 and C# 11. A target below that would mean bringing the
  two copies of the algorithm back.
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

- **The calculation exists once, the entry points twice.** Up to version 1.0.6.0 the algorithm was
  copied into both public helpers, which is how the same swapped `ArgumentException` argument ended
  up in the repository ten times. Since 1.0.7.0 both helpers forward to the generic
  `CircleToPolygonHelper`. The two test classes still mirror each other and carry their own expected
  results, and `GeoJsonTestsDecimal` compares both helpers against each other. The `decimal` variant
  is not just more precise, it is much slower, because `DecimalMath` evaluates series where the
  `double` variant calls a single intrinsic.
- **`ICircleMath<T>` exists because `decimal` has no trigonometry.** `INumber<T>` only brings the
  arithmetic, and `decimal` implements neither `ITrigonometricFunctions<T>` nor
  `IFloatingPointConstants<T>`, so `Sin`, `Cos`, `Asin`, `Atan2` and `Pi` come in through an
  interface with `static abstract` members. `DoubleCircleMath` and `DecimalCircleMath` are
  `readonly struct`s and not static classes, because a static class cannot be a type argument.
- **The order of the arithmetic in the core is load bearing.** Both test classes compare against
  exact expected values down to the last digit, so reordering a multiplication changes the result and
  turns them red. `T.CreateChecked(direction * 2)` keeps that multiplication in `int` and
  `T.CreateChecked(-i)` keeps the negation there too, which is what the two former copies did through
  their implicit conversions. Do not "simplify" those.
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

**Every new version is released completely.** Changelog, README, nuget.org, GitHub Packages and a
GitHub release belong to every version, none of these steps is optional and none of them waits for
a separate request.

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.7.0 (2026-08-12)** : Short description.`
3. Put the same version and description into `PackageReleaseNotes` in
   `src/GeoJsonCircleToPolygon/GeoJsonCircleToPolygon.csproj`, in the single line format
   `Version 1.0.7.0 (2026-08-12): Short description.`. It is the only place in the build files that
   names a version, and it is duplicated from the changelog by hand.
4. Update `README.md` for every version. The `## Available for` list has to match the
   `TargetFrameworks` of the library, the usage sample and the parameter list have to match the
   public API, and nothing there may describe a state the package has left behind.
5. Commit that.
6. Tag the commit with the plain version number, no `v` prefix (`1.0.7`, `1.0.6`, ...). The existing
   tags are lightweight tags, create new ones the same way.
7. Push the commits and the tag.
8. Only then build the package, and build it while the tag is the newest commit. GitVersion takes
   the version from the tag plus the distance to it, so a build before the tag or after another
   commit produces a prerelease version like `1.0.8-3+Branch.master.Sha...`, and that version would
   end up on nuget.org. `git describe --tags` has to answer with the plain tag.
9. Publish to nuget.org with `BuildAndPushPackage.bat`, started from the repository root, its first
   line is `cd .\src`. It deletes every `bin` and `obj` below `src`, builds `-c Release` and pushes
   the `.nupkg` and the `.snupkg` to nuget.org with `%NUGET_API_KEY%` and to the NuGet source named
   `github` with `%GITHUB_API_KEY%`. The four push lines carry a leading `@`, because `ECHO` is on
   there and cmd would otherwise write each of them to the console after the variable expansion,
   both API keys included. Do not remove those prefixes. The script ends with `PAUSE`, so it wants
   a console. Call it with its full path: the Claude Code shell
   sets `NoDefaultCurrentDirectoryInExePath=1`, so cmd does not find a bare
   `BuildAndPushPackage.bat` in the current directory. Because it always rebuilds, it is only
   correct as long as no commit sits on top of the tag. If one does, push the package that was built
   from the tagged state instead, by its file name and not with the wildcard, because
   `GeneratePackageOnBuild` leaves a `.nupkg` of every intermediate build in `bin`:
   `dotnet nuget push HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon.1.0.7.nupkg -s "nuget.org" --skip-duplicate -k %NUGET_API_KEY%`
   in `src/GeoJsonCircleToPolygon/bin/Release`. That command takes the matching `.snupkg` with it.
   A push to nuget.org cannot be taken back, the version is burned even after unlisting. nuget.org
   needs a few minutes to validate a new version before it is listed.
10. Publish the same `.nupkg` to GitHub Packages,
    `https://nuget.pkg.github.com/SeppPenner/index.json`. The script only reaches it if a NuGet
    source named `github` is registered on the machine, `dotnet nuget list source` shows that. On
    2026-11-24 only nuget.org was registered here, so the two `github` lines of the script fail and
    the push has to be done by hand in `src/GeoJsonCircleToPolygon/bin/Release`:
    `dotnet nuget push HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon.1.0.7.nupkg -s https://nuget.pkg.github.com/SeppPenner/index.json --skip-duplicate --api-key <token>`.
    The API key is a GitHub token with the scope `write:packages`, for example from
    `gh auth token`. If `gh auth status` lists no `write:packages`, the user has to run
    `gh auth refresh -h github.com -s write:packages,read:packages`, it needs a browser login.
    GitHub Packages takes no symbol packages, push the `.nupkg` only.
11. Create the GitHub release:
    `gh release create 1.0.7 -R SeppPenner/GeoJsonCircleToPolygon --verify-tag --latest --title "Version 1.0.7.0 (2026-08-12)" --notes-file <file>`.
    The notes are the changelog sentence plus the line
    `NuGet: https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.GeoJsonCircleToPolygon/1.0.7`.
    Write credits as `` `@name` `` in backticks, a bare `@name` notifies that person again.

The version in the `Changelog.md` and in `PackageReleaseNotes` has four parts (`1.0.7.0`), the tag
has three (`1.0.7`).

nuget.org and GitHub Packages hold every tagged version from 1.0.0 to 1.0.8, and every tag has a
GitHub release. The missing versions on GitHub Packages and the releases were added on 2026-11-24.

## Git

- Include the full GitHub pull request or issue URL in the body of each related commit on a `Refs:`
  line. Use the [GitHub issue skill](.claude/skills/github-issue/SKILL.md) for issue work and closeout.
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
