# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app/publish .
COPY /src/API/appsettings.Production.json ./appsettings.Production.json
ENTRYPOINT ["dotnet", "API.dll"]
