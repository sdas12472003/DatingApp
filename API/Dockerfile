# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY . .
RUN dotnet restore "API.csproj"
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
