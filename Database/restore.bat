@echo off
set PGUSER=neondb_owner
set PGPASSWORD=u9TmlZdkw4qo
set PGHOST=ep-gentle-wind-a1igy7r9-pooler.ap-southeast-1.aws.neon.tech
set PGPORT=5432
set PGDATABASE=gyminize_cloud

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

echo Searching for the most recent backup file in %BACKUP_DIR%...
set LATEST_BACKUP=
for /f "delims=" %%f in ('dir /b /od /a-d "%BACKUP_DIR%\*.dump"') do (
    set LATEST_BACKUP=%%f
)

if "%LATEST_BACKUP%"=="" (
    echo No backup files found in %BACKUP_DIR%.
    pause
    exit /b
)

set BACKUP_FILE=%BACKUP_DIR%\%LATEST_BACKUP%
echo Found the most recent backup file: %BACKUP_FILE%

echo Dropping and recreating database: %PGDATABASE%
"%PGBIN%\psql.exe" -U %PGUSER% -h %PGHOST% -p %PGPORT% -c "DROP DATABASE IF EXISTS %PGDATABASE;"
if %ERRORLEVEL% neq 0 (
    echo Warning: Could not drop the database. It might not exist.
)

"%PGBIN%\psql.exe" -U %PGUSER% -h %PGHOST% -p %PGPORT% -c "CREATE DATABASE %PGDATABASE;"
if %ERRORLEVEL% neq 0 (
    echo Error: Failed to create the database.
    pause
    exit /b
)

echo Restoring database from backup file: %BACKUP_FILE%
"%PGBIN%\pg_restore.exe" -U %PGUSER% -h %PGHOST% -p %PGPORT% -d %PGDATABASE% --verbose --clean --no-owner --no-privileges -F c "%BACKUP_FILE%"

if %ERRORLEVEL% lss 2 (
    echo Restore successful! Note: Warnings may have occurred.
) else (
    echo Restore failed. Please check the configuration and backup file.
)

set PGPASSWORD=

pause