# ------------------------------
# Build Stage
# ------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY . .
WORKDIR /src/API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ------------------------------
# Runtime Stage
# ------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS final
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Optional: If appsettings.Production.json is NOT in publish dir
# Copy it explicitly if needed (ensure path is correct)
COPY API/appsettings.Production.json ./appsettings.Production.json

ENTRYPOINT ["dotnet", "API.dll"]
