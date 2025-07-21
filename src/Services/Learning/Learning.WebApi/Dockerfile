FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Learning/Learning.WebApi/Learning.WebApi.csproj", "src/Services/Learning/Learning.WebApi/"]
RUN dotnet restore "./src/Services/Learning/Learning.WebApi/Learning.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/Learning/Learning.WebApi"
RUN dotnet build "./Learning.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Learning.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Learning.WebApi.dll"]