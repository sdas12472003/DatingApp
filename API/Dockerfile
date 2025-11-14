# ------------ Build Stage ------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj first
COPY API/API.csproj ./API/

RUN dotnet restore ./API/API.csproj

# Copy the rest of the code
COPY . .

WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish


# ------------ Runtime Stage ------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Render provides PORT dynamically
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

COPY --from=build /app/publish .

EXPOSE 5000
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
