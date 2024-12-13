# FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
# WORKDIR /app

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src
# COPY ["ElsKuruAarThorSand.csproj", "./"]
# RUN dotnet restore "./ElsKuruAarThorSand.csproj"
# COPY . .
# WORKDIR "/src/"
# RUN dotnet build "ElsKuruAarThorSand.csproj" -c Release -o /app/build
# # Build the final image using the base image and the published output

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "elskuruaarthorsand.dll"]
# Command to build: docker build -t elskuruaarthorsandapp .

# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["ElsKuruAarThorSand.csproj", "./"]
# RUN dotnet restore "./ElsKuruAarThorSand.csproj"
COPY . .
RUN dotnet build "ElsKuruAarThorSand.csproj" -c Release -o /app/build

# Copy the SQLite database file to the container
COPY Infrastructure/Data/shop.db /app/publish/Infrastructure/Data/shop.db
RUN chmod -R 777 /app/publish/Infrastructure/Data
# Publish stage
FROM build AS publish
RUN dotnet publish "ElsKuruAarThorSand.csproj" -c Release -o /app/publish

# Final runtime image with ASP.NET Core support
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElsKuruAarThorSand.dll"]