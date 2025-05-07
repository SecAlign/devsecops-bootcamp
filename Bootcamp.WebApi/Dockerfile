FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

COPY shared/publish .
ENTRYPOINT ["dotnet", "Bootcamp.WebApi.dll"]
