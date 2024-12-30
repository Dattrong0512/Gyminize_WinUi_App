@echo off

set TASK_NAME=DailyBackupTask3AM
set TASK_TIME=03:00
set SCRIPT_PATH=%~dp0backup.bat

if not exist "%SCRIPT_PATH%" (
    echo backup.bat not found in %~dp0.
    pause
    exit /b
)

schtasks /create /tn %TASK_NAME% /tr "%SCRIPT_PATH%" /sc daily /st %TASK_TIME% /f
if %ERRORLEVEL% equ 0 (
    echo Task %TASK_NAME% has been created and scheduled successfully.
) else (
    echo Failed to create the task. Check permissions or task scheduler settings.
)

pause