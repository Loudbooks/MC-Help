@echo off
set /p discord_username=Enter your Discord username: 

:check_username
if "%discord_username%"=="" (
    echo Discord username cannot be blank. Please try again.
    echo.
    set /p discord_username=Enter your Discord username: 
    goto check_username
)

:check_log_type
echo.
echo Confirm your log type:
echo 1) Latest Log
echo 2) Launcher Log
set /p log_type=Enter your choice: 

if "%log_type%"=="1" set log_name=latest
if "%log_type%"=="2" set log_name=launcher

if not "%log_name%"=="latest" if not "%log_name%"=="launcher" (
    echo.
    echo Invalid log type. Please try again.
    goto check_log_type
)

:check_launcher_type
echo.
echo Confirm your launcher:
echo 1) Official Launcher
echo 2) CurseForge Launcher
echo 3) Prism Launcher
echo 4) Modrinth Launcher
echo 5) Technic Launcher
set /p launcher_type=Enter your choice: 

if not "%launcher_type%"=="1" if not "%launcher_type%"=="2" if not "%launcher_type%"=="3" if not "%launcher_type%"=="4" if not "%launcher_type%"=="5" (
    echo.
    echo Invalid launcher type. Please try again.
    goto check_launcher_type
)

if "%launcher_type%"=="1" goto official
if "%launcher_type%"=="2" goto curseforge
if "%launcher_type%"=="3" goto prism
if "%launcher_type%"=="4" goto modrinth
if "%launcher_type%"=="5" goto technic


:official
setlocal enabledelayedexpansion
if %log_name%==latest (
    set selected_directory=%appdata%\.minecraft\logs\latest.log
    set log_name=Latest Log
) else (
    set selected_directory=%appdata%\.minecraft\launcher_log.txt
    set log_name=Launcher Log
)
goto upload_log


:curseforge
setlocal enabledelayedexpansion

for /d %%d in ("%homedrive%%homepath%\curseforge\minecraft\Instances\*") do (
    set /a index+=1
)

if %index%==1 (
    echo.
    echo No CurseForge installations found.
    pause
    goto check_log_type
)

echo.
echo Your CurseForge installations:
set index=1
for /d %%d in ("%homedrive%%homepath%\curseforge\minecraft\Instances\*") do (
    echo !index!^) %%~nxd
    set /a index+=1
)

echo.
set /p selected_index=Enter the index of the installation you are using: 
set /a index=1
for /d %%d in ("%homedrive%%homepath%\curseforge\minecraft\Instances\*") do (
    if !index!==%selected_index% (
        if %log_type%==launcher (
            set selected_directory=%homedrive%%homepath%\curseforge\minecraft\Instances\%%~nxd\logs\launcher_log.txt
            set log_name=CurseForge Launcher Log ^(%%~nxd^)
        ) else (
            set selected_directory=%homedrive%%homepath%\curseforge\minecraft\Instances\%%~nxd\logs\latest.log
            set log_name=CurseForge Latest Log ^(%%~nxd^)
        )
    )
    set /a index+=1
)

goto upload_log


:prism
setlocal enabledelayedexpansion

for /d %%d in ("%appdata%\PrismLauncher\instances\*") do (
    set /a index+=1
)

if %index%==1 (
    echo.
    echo No Prism installations found.
    pause
    goto check_log_type
)

echo.
echo Your Prism installations:
set index=1
for /d %%d in ("%appdata%\PrismLauncher\instances\*") do (
    echo !index!^) %%~nxd
    set /a index+=1
)

echo.
set /p selected_index=Enter the index of the installation you are using: 
set /a index=1

for /d %%d in ("%appdata%\PrismLauncher\instances\*") do (
    if !index!==%selected_index% (
        if %log_type%==launcher (
            set selected_directory=%appdata%\PrismLauncher\instances\%%~nxd\logs\launcher_log.txt
            set log_name=Prism Launcher Log ^(%%~nxd^)
        ) else (
            set selected_directory=%appdata%\PrismLauncher\instances\%%~nxd\logs\latest.log
            set log_name=Prism Latest Log ^(%%~nxd^)
        )
    )
    set /a index+=1
)

goto upload_log


:modrinth
setlocal enabledelayedexpansion

for /d %%d in ("%appdata%\com.modrinth.theseus\profiles\*") do (
    set /a index+=1
)

if %index%==1 (
    echo.
    echo No Modrinth installations found.
    pause
    goto check_log_type
)

echo.
echo Your Modrinth installations:
set index=1
for /d %%d in ("%appdata%\com.modrinth.theseus\profiles\*") do (
    echo !index!^) %%~nxd
    set /a index+=1
)

echo.
set /p selected_index=Enter the index of the installation you are using: 
set /a index=1
for /d %%d in ("%appdata%\com.modrinth.theseus\profiles\*") do (
    if !index!==%selected_index% (
        if %log_type%==launcher (
            set selected_directory=%appdata%\com.modrinth.theseus\profiles\%%~nxd\logs\launcher_log.txt
            set log_name=Modrinth Launcher Log ^(%%~nxd^)
        ) else (
            set selected_directory=%appdata%\com.modrinth.theseus\profiles\%%~nxd\logs\latest.log
            set log_name=Modrinth Latest Log ^(%%~nxd^)
        )
    )
    set /a index+=1
)

goto upload_log


:technic
setlocal enabledelayedexpansion

for /d %%d in ("%appdata%\.technic\modpacks\*") do (
    set /a index+=1
)

if %index%==1 (
    echo.
    echo No Technic installations found.
    pause
    goto check_log_type
)

echo.
echo Your Technic installations:
set index=1

for /d %%d in ("%appdata%\.technic\modpacks\*") do (
    echo !index!^) %%~nxd
    set /a index+=1
)

echo.
set /p selected_index=Enter the index of the installation you are using: 
set /a index=1

for /d %%d in ("%appdata%\.technic\modpacks\*") do (
    if !index!==%selected_index% (
        if %log_type%==launcher (
            set selected_directory=%appdata%\.technic\modpacks\%%~nxd\logs\launcher_log.txt
            set log_name=Technic Launcher Log ^(%%~nxd^)
        ) else (
            set selected_directory=%appdata%\.technic\modpacks\%%~nxd\logs\latest.log
            set log_name=Technic Latest Log ^(%%~nxd^)
        )
    )
    set /a index+=1
)

goto upload_log


:upload_log
if "%selected_directory%"=="" (
    echo.
    echo Log not found: %selected_directory%
    pause
    goto check_log_type
)

if not exist "%selected_directory%" (
    echo.
    echo Log not found: %selected_directory%
    pause
    goto check_log_type
)

echo.
echo Your paste link:
curl -H "title: %discord_username%'s %log_name%" -X POST --upload-file "%selected_directory%" https://pastebook.dev/api/upload
echo.
echo Log uploaded successfully.
pause
