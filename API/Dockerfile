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
# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only csproj first (improves Docker caching)
COPY API/API.csproj ./API/
RUN dotnet restore ./API/API.csproj

# Now copy the rest of the source
COPY . .

WORKDIR /app/API
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Copy Production config file
COPY API/appsettings.Production.json ./appsettings.Production.json

ENTRYPOINT ["dotnet", "API.dll"]
