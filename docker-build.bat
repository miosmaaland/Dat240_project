@echo off

REM Build the Docker image
docker-compose build

REM Start the container
docker-compose up
