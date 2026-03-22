# SDRSharp TETRAPOL plugin

This directory contains a working base for an SDR# / SDRSharp plugin that reuses
this repository's native TETRAPOL decoder.

## What is included

- `Tetrapol.SdrSharp.csproj`: Windows Forms plugin project targeting `.NET Framework 4.8`.
- `TetrapolPlugin.cs`: SDR# plugin entry point.
- `TetrapolPanel.cs`: in-application control panel.
- `SimpleGmskSlicer.cs`: lightweight real-time bit slicer for 8 ksym/s TETRAPOL channels.
- `TetrapolDecoder.cs` and `TetrapolNative.cs`: P/Invoke bridge to the native decoder in this repository.

## Native DLL build

On Windows, build a shared library named `tetrapol.dll` from the repository root:

```powershell
cmake -S . -B build -DBUILD_SHARED_LIBS=ON
cmake --build build --config Release
```

Copy `build\lib\Release\tetrapol.dll` next to `Tetrapol.SdrSharp.dll` in your SDR# installation.

## Managed plugin build

1. Define `SDRSHARP_ROOT` so the project can resolve `SDRSharp.Common.dll`.
2. Open `Tetrapol.SdrSharp.csproj` in Visual Studio.
3. Build the project in `Release|x86`.
4. Copy `Tetrapol.SdrSharp.dll` into the SDR# plugin directory.
5. Register the plugin in `Plugins.xml`.

Example entry:

```xml
<add key="Tetrapol" value="Tetrapol.SdrSharp.TetrapolPlugin, Tetrapol.SdrSharp" />
```

## Runtime notes

- Tune SDR# in narrow FM on a TETRAPOL downlink carrier.
- Prefer discriminator / flat audio if your SDR# build exposes it.
- The included `SimpleGmskSlicer` is intentionally simple so the plugin stays easy to adapt.
  If your local SDR# build already provides symbol-timed binary slicing or a better real-data
  processor interface, replace that class first.
- Set `SCR` to `-1` for automatic scrambling constant detection.
