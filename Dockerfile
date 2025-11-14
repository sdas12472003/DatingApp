# ------------ Build Stage ------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the csproj for restore caching
COPY API/API.csproj ./API/

# Restore
RUN dotnet restore ./API/API.csproj

# Copy everything else
COPY . .

# Publish
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish


# ------------ Runtime Stage ------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Render provides the PORT dynamically
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

COPY --from=build /app/publish .

EXPOSE 5000
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
