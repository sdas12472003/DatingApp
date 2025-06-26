# # Build Stage
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY . .
# WORKDIR /src/API
# RUN dotnet restore
# RUN dotnet publish -c Release -o /app/publish

# # Runtime Stage
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
# WORKDIR /app
# COPY --from=build /app/publish .

# # ✅ Make sure the production config file is included
# COPY API/appsettings.Production.json ./appsettings.Production.json

# ENTRYPOINT ["dotnet", "API.dll"]
# ------------ Build Stage ------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the .csproj file first for efficient caching
COPY API/API.csproj ./API/

# Restore dependencies
RUN dotnet restore ./API/API.csproj

# Copy the rest of the source code
COPY . .

# Set working directory to where your Program.cs is
WORKDIR /src/API

# Publish the project
RUN dotnet publish -c Release -o /app/publish


# ------------ Runtime Stage ------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app from build container
COPY --from=build /app/publish .

# Optional: Include production config
COPY API/appsettings.Production.json ./appsettings.Production.json

ENTRYPOINT ["dotnet", "API.dll"]
