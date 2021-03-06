﻿# Starlight Director

**Work In Progress**

[![AppVeyor](https://img.shields.io/appveyor/ci/hozuki/starlightdirector.svg)](https://ci.appveyor.com/project/hozuki/starlightdirector)
[![GitHub contributors](https://img.shields.io/github/contributors/hozuki/StarlightDirector.svg)](https://github.com/hozuki/StarlightDirector/graphs/contributors)
[![Libraries.io for GitHub](https://img.shields.io/librariesio/github/hozuki/StarlightDirector.svg)](https://github.com/hozuki/StarlightDirector)
[![Github All Releases](https://img.shields.io/github/downloads/hozuki/StarlightDirector/total.svg)](https://github.com/hozuki/StarlightDirector/releases)

**Downloads:**

- [Nightly Build](https://ci.appveyor.com/api/projects/hozuki/starlightdirector/artifacts/sd-latest.zip)
- [Releases](https://github.com/hozuki/StarlightDirector/releases)

## What is this?

The next generation of Starlight Director from [DereTore](https://github.com/OpenCGSS/DereTore). Starlight Director is the unofficial beatmap
editor for [Idolmaster Cinderella Girls Starlight Stage](http://cinderella.idolmaster.jp/sl-stage/). (Will it support Million Live Theater Days? TBD)

It is now in its beta phase. But still, thank you [@statmentreply](https://github.com/statementreply) and [@logchan](https://github.com/logchan)
for maintaing the old version.

## Requirements

- Windows 7 or later
- [.NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=42642)
- Direct3D 11 and Direct2D

##  Usage

Experience with rhythm games is suggested.

[使用说明（部分完成）](StarlightDirector.App/Resources/Docs/Help.md)

## Building

1. Clone from GitHub: `git clone https://github.com/hozuki/StarlightDirector.git`;
2. Install missing NuGet packages: `nuget restore StarlightDirector.sln` (or use NuGet Package Manager in Visual Studio);
3. Open `StarlightDirector.sln` in Visual Studio (Visual Studio 2017 or later is required for supporting C# 7 syntax);
4. Build the solution.

## Screenshot

![Screenshot 23 May 2017](res/images/early_preview.png)

## License

MIT License. Hey logchan and stat, please add your names into the license!
