@echo off

set TASK_NAME=DailyBackupTask3AM

schtasks /delete /tn %TASK_NAME% /f
if %ERRORLEVEL% equ 0 (
    echo Task %TASK_NAME% has been disabled and removed successfully.
) else (
    echo Failed to delete the task. The task might not exist.
)
pause
exit /b
