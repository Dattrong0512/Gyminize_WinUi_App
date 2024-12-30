@echo off
set PGUSER=neondb_owner
set PGPASSWORD=u9TmlZdkw4qo
set PGHOST=ep-gentle-wind-a1igy7r9-pooler.ap-southeast-1.aws.neon.tech
set PGDATABASE=gyminize_cloud
set PGPORT=5432

set BIN_FILE=%~dp0pg_bin_directory.txt

if not exist "%BIN_FILE%" (
    echo Error: File pg_bin_directory.txt not found in the script directory.
    pause
    exit /b
)

for /f "delims=" %%a in (%BIN_FILE%) do (
    set PGBIN=%%a
)

if "%PGBIN%"=="" (
    echo Error: No path found in pg_bin_directory.txt.
    pause
    exit /b
)

if not exist "%PGBIN%" (
    echo Error: PostgreSQL bin directory does not exist: %PGBIN%.
    pause
    exit /b
)

set BACKUP_DIR=%~dp0backup
if not exist "%BACKUP_DIR%" (
    mkdir "%BACKUP_DIR%"
)

for /f "tokens=1-3 delims=/ " %%a in ("%date%") do (
    set DD=%%a
    set MM=%%b
    set YYYY=%%c
)
set TIMESTAMP=%DD%-%MM%-%YYYY%_%time:~0,2%-%time:~3,2%-%time:~6,2%
set TIMESTAMP=%TIMESTAMP: =0%
set TIMESTAMP=%TIMESTAMP::=-%

set BACKUP_FILE=%BACKUP_DIR%\%PGDATABASE%_backup_%TIMESTAMP%.dump

echo Performing database backup...
"%PGBIN%\pg_dump.exe" -U %PGUSER% -h %PGHOST% -d %PGDATABASE% -p %PGPORT% -F c --verbose --create --clean --no-owner -f "%BACKUP_FILE%"

if %ERRORLEVEL% equ 0 (
    echo Backup successful! File saved at: %BACKUP_FILE%
) else (
    echo Backup failed. Please check the configuration.
)

set PGPASSWORD=

pause