; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Hurricane"
#define MyAppVersion "0.3.4"
#define MyAppPublisher "Alkaline"
#define MyAppURL "http://www.hurricane.vincentgri.de"
#define MyAppExeName "Hurricane.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{337B211F-4DF1-4B0D-BC23-7510C1B636A4}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=..\LICENSE.txt
OutputDir=release
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes
UninstallDisplayName=Hurricane
UninstallDisplayIcon={app}\{#MyAppName}.exe

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "dependencies\dotNetFx45_Full_setup.exe"; DestDir: {tmp}; Flags: deleteafterinstall; AfterInstall: InstallFramework; Check: FrameworkIsNotInstalled
Source: "..\Hurricane\bin\Release\Hurricane.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\CSCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\Exceptionless.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\Exceptionless.Models.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\Exceptionless.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\MahApps.Metro.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\System.Windows.Interactivity.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\taglib-sharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\updateSystemDotNet.Controller.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\WPFFolderBrowser.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\WPFSoundVisualizationLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\Xceed.Wpf.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\ffmpeg.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\youtube-dl.exe"; DestDir: "{userappdata}\Hurricane"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\GongSolutions.Wpf.DragDrop.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\Hardcodet.Wpf.TaskbarNotification.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\AudioVisualisation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Hurricane\bin\Release\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "special_files\.IsInstalled"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
var CancelWithoutPrompt: boolean;

function InitializeSetup(): Boolean;
begin
  CancelWithoutPrompt := false;
  result := true;
end;

procedure CancelButtonClick(CurPageID: Integer; var Cancel, Confirm: Boolean);
begin
  if CurPageID=wpInstalling then
    Confirm := not CancelWithoutPrompt;
end;

function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full');
end;

procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing .NET framework...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
      if not Exec(ExpandConstant('{tmp}\dotNetFx45_Full_setup.exe'), '/q /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
  begin
    // you can interact with the user that the installation failed
    MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.',
      mbError, MB_OK);
    CancelWithoutPrompt := true;
    WizardForm.Close;
  end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

