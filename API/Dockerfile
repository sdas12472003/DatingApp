# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all files
COPY . .

# Restore and publish
WORKDIR /src/API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app and config
COPY --from=build /app/publish .
COPY API/appsettings.Production.json ./appsettings.Production.json

# Ensure the SQLite DB file is included (if pre-generated)
COPY API/datingapp.db ./datingapp.db

# Optional: expose port (if needed by Render)
EXPOSE 80

ENTRYPOINT ["dotnet", "API.dll"]
