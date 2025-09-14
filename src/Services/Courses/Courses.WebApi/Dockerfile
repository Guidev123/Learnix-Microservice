FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Services/Courses/Courses.WebApi/Courses.WebApi.csproj", "src/Services/Courses/Courses.WebApi/"]
COPY ["src/Services/Courses/Courses.Infrastructure/Courses.Infrastructure.csproj", "src/Services/Courses/Courses.Infrastructure/"]
COPY ["src/Building Blocks/Learnix.Commons.WebApi/Learnix.Commons.WebApi.csproj", "src/Building Blocks/Learnix.Commons.WebApi/"]

RUN dotnet restore "src/Services/Courses/Courses.WebApi/Courses.WebApi.csproj"

COPY . .

WORKDIR "/src/src/Services/Courses/Courses.WebApi"
RUN dotnet build "Courses.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Courses.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courses.WebApi.dll"]