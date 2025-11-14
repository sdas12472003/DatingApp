# ------------ Build Stage ------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj
COPY API/API.csproj ./API/

# Restore
RUN dotnet restore ./API/API.csproj

# Copy everything
COPY . .

# Publish
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish


# ------------ Runtime Stage ------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 5000

ENTRYPOINT ["dotnet", "API.dll"]
