#define MyAppName "WinThumbsPreloader"
#define MyAppReleaseDirectory "..\WinThumbsPreloader\WinThumbsPreloader\bin\Release\net10.0-windows7.0\win-x64"
#define MyAppFilename MyAppName + ".exe"
#define MyAppFilepath MyAppReleaseDirectory + "\" + MyAppFilename
#define MyAppConfig  MyAppReleaseDirectory + "\" + MyAppName
#dim Version[4]
#expr GetVersionComponents(MyAppFilepath, Version[0], Version[1], Version[2], Version[3])
#define MyAppVersion Str(Version[0]) + "." + Str(Version[1]) + "." + Str(Version[2])
#define MyAppPublisher1 "Dmitry Bruhov"
#define MyAppPublisher2 "Mutahar Farooq"
#define MyAppId "CF49DD18-AA76-4E79-97C2-4FEAED1AED5F"

[Setup]
AppCopyright=Copyright (c) 2018 {#MyAppPublisher1}, Copyright (c) 2026 {#MyAppPublisher2}
AppId={#MyAppId}
AppMutex={#MyAppId}
AppName={#MyAppName}
AppPublisher={#MyAppPublisher1}, {#MyAppPublisher2}
AppPublisherURL=https://github.com/Mfarooq360/WinThumbsPreloader-V2
AppSupportURL=https://github.com/Mfarooq360/WinThumbsPreloader-V2/issues
AppUpdatesURL=https://github.com/Mfarooq360/WinThumbsPreloader-V2/releases
AppVerName={#MyAppName} {#MyAppVersion}
AppVersion={#MyAppVersion}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
ChangesEnvironment=yes
DefaultDirName={commonpf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DirExistsWarning=no
DisableReadyPage=yes
DisableProgramGroupPage=yes
LicenseFile=license.txt
MinVersion=10.0
OutputBaseFilename={#MyAppName}-{#MyAppVersion}-setup
OutputDir=Output
ShowLanguageDialog=no
UninstallDisplayIcon={app}\{#MyAppFilename}
UninstallDisplayName={#MyAppName}
VersionInfoTextVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
WizardImageFile=WizardImageFile.bmp
WizardImageStretch=no
WizardSmallImageFile=WizardSmallImageFile.bmp
SolidCompression=yes
Compression=lzma2/max

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: ru; MessagesFile: "compiler:Languages\Russian.isl"

[CustomMessages]
; Context menu items
en.PreloadThumbnails=Preload thumbnails
ru.PreloadThumbnails=Загрузить эскизы

en.PreloadThumbnailsRecursively=Preload thumbnails recursively
ru.PreloadThumbnailsRecursively=Загрузить эскизы включая подпапки

[Files]
; Core app files
Source: "{#MyAppFilepath}";               DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppConfig}.dll";             DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppConfig}.deps.json";       DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppConfig}.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppConfig}.dll.config";    DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppReleaseDirectory}\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppReleaseDirectory}\ru\*.resources.dll"; DestDir: "{app}\Languages\ru"; Flags: ignoreversion
Source: "{#MyAppReleaseDirectory}\Microsoft.Win32.TaskScheduler.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppFilename}"

[Run]
Filename: "{app}\{#MyAppFilename}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent shellexec

[Registry]
; ---- Directory context menu ----
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{#MyAppName}"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}"; ValueType: string; ValueName: "ExtendedSubCommandsKey"; ValueData: "Directory\shell\{#MyAppName}"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell"

Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell\Preload"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{cm:PreloadThumbnails}"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell\Preload"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell\Preload\command"; ValueType: string; ValueData: """{app}\{#MyAppFilename}"" -m ""%V"""

Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell\PreloadRecursively"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{cm:PreloadThumbnailsRecursively}"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell\PreloadRecursively"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Directory\shell\{#MyAppName}\Shell\PreloadRecursively\command"; ValueType: string; ValueData: """{app}\{#MyAppFilename}"" -m -r ""%V"""

; ---- Directory background context menu ----
Root: "HKCR"; Subkey: "Directory\Background\shell\{#MyAppName}"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: "Directory\Background\shell\{#MyAppName}"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{#MyAppName}"
Root: "HKCR"; Subkey: "Directory\Background\shell\{#MyAppName}"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Directory\Background\shell\{#MyAppName}"; ValueType: string; ValueName: "ExtendedSubCommandsKey"; ValueData: "Directory\Background\ContextMenus\Menu{#MyAppName}"

Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\Preload"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{cm:PreloadThumbnails}"
Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\Preload"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\Preload\command"; ValueType: string; ValueData: """{app}\{#MyAppFilename}"" -m ""%V"""

Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\PreloadRecursively"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{cm:PreloadThumbnailsRecursively}"
Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\PreloadRecursively"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\PreloadRecursively"; ValueType: string; ValueName: "HasLUAShield"; ValueData: ""
Root: "HKCR"; Subkey: "Directory\Background\ContextMenus\Menu{#MyAppName}\shell\PreloadRecursively\command"; ValueType: string; ValueData: """{app}\{#MyAppFilename}"" -m -r ""%V"""

; ---- Drive context menu ----
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{#MyAppName}"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}"; ValueType: string; ValueName: "ExtendedSubCommandsKey"; ValueData: "Drive\shell\{#MyAppName}"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell"

Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell\Preload"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{cm:PreloadThumbnails}"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell\Preload"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell\Preload\command"; ValueType: string; ValueData: """{app}\{#MyAppFilename}"" -m %V"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell\PreloadRecursively"; ValueType: string; ValueName: "MUIVerb"; ValueData: "{cm:PreloadThumbnailsRecursively}"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell\PreloadRecursively"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\{#MyAppFilename}"",0"
Root: "HKCR"; Subkey: "Drive\shell\{#MyAppName}\Shell\PreloadRecursively\command"; ValueType: string; ValueData: """{app}\{#MyAppFilename}"" -m -r %V"

Root: "HKLM"; Subkey: "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"; ValueType: expandsz; ValueName: "Path"; ValueData: "{olddata};{app}"; Check: not IsInPath(ExpandConstant('{app}'))

[Code]
function IsInPath(Dir: string): Boolean;
var
  S: string;
begin
  if RegQueryStringValue(HKLM,
     'SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
     'Path', S) then
  begin
    S := ';' + Uppercase(S) + ';';
    Result := Pos(';' + Uppercase(Dir) + ';', S) > 0;
  end
  else
    Result := False;
end;