# WinThumbsPreloader V2
### Fast and Advanced Thumbnail Preloader for Windows Explorer

Preload Windows Explorer thumbnails faster and more conveniently with WinThumbsPreloader V2.  
This project is designed to help generate thumbnails ahead of time so large folders can feel smoother to browse.

![WinThumbsPreloader Demo](https://raw.githubusercontent.com/Mfarooq360/WinThumbsPreloader/master/demo.gif)

> **Note:** The demo above is from the [original](https://github.com/bruhov/WinThumbsPreloader) single-threaded WinThumbsPreloader project.

---

## Overview

WinThumbsPreloader V2 is a Windows utility for pre-generating Explorer thumbnails, with support for:

- Multi-threaded generation
- Recursive directory scanning
- Silent command-line usage

It is useful for when browsing folders with large numbers of images, videos, or other thumbnail-supported files in Windows Explorer.

---

## Downloads

### WinThumbsPreloader V2
- **Project Page:** [WinThumbsPreloader-V2](https://github.com/Mfarooq360/WinThumbsPreloader-V2)
- **Standalone EXE:** [WinThumbsPreloader.exe](https://github.com/Mfarooq360/WinThumbsPreloader-V2/releases/download/v2.0.0-beta7/WinThumbsPreloader.exe)
- **Installer:** [WinThumbsPreloader-2.0.0-setup.exe](https://github.com/Mfarooq360/WinThumbsPreloader-V2/releases/download/v2.0.0-beta7/WinThumbsPreloader-2.0.0-setup.exe)

### WinThumbsPreloader V1 (Multi-threaded Fork)
- **Project Page:** [WinThumbsPreloader](https://github.com/Mfarooq360/WinThumbsPreloader)
- **Standalone EXE:** [WinThumbsPreloader.exe](https://github.com/Mfarooq360/WinThumbsPreloader/releases/download/v1.2.1/WinThumbsPreloader.exe)
- **Installer:** [WinThumbsPreloader-1.2.1-setup.exe](https://github.com/Mfarooq360/WinThumbsPreloader/releases/download/v1.2.1/WinThumbsPreloader-1.2.1-setup.exe)

---

## Optional Explorer Extensions

For better thumbnail and preview support with certain file types:

- **Fast SVG Explorer Extension**  
  Recommended over PowerToys' SVG implementation for significantly better performance.  
  [Download SVG Explorer Extension](https://github.com/tibold/svg-explorer-extension/releases)

- **HEIF / HEIC Support for Windows**  
  [Download HEIF Image Extensions](https://www.microsoft.com/en-us/p/heif-image-extensions/9pmmsr1cgpwg)

---

## Command-Line Usage

### CLI Options

- `-s` — Silent mode
- `-r` — Recursive directory search
- `-m` — Multi-threaded generation

### Example

```bash
WinThumbsPreloader.exe -m -r "C:\Users\YourName\Pictures"
