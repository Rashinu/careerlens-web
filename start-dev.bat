@echo off
title CareerLens Dev Starter

echo CareerLens baslatiliyor...
echo.

echo [1/3] Docker baslatiliyor (PostgreSQL + Redis)...
cd /d "%~dp0backend"
docker compose up -d
echo Docker hazir.
echo.

echo [2/3] Backend API baslatiliyor (localhost:5100)...
start "CareerLens API" cmd /k "cd /d "%~dp0backend\CareerLens.API" && dotnet run --launch-profile http"
timeout /t 15 /nobreak >nul

echo [3/3] Frontend baslatiliyor (localhost:3000)...
start "CareerLens Frontend" cmd /k "cd /d "%~dp0frontend" && npm run dev"

echo.
echo ================================
echo  Hazir!
echo  Frontend : http://localhost:3000
echo  Backend  : http://localhost:5100
echo  Swagger  : http://localhost:5100/swagger
echo  Hangfire : http://localhost:5100/hangfire
echo ================================
echo.
pause
