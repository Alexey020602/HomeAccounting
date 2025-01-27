FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MigrationService/MigrationService.csproj", "MigrationService/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["FnsChecksApi/FnsChecksApi.csproj", "FnsChecksApi/"]
COPY ["DataBase/DataBase.csproj", "DataBase/"]
RUN dotnet restore "MigrationService/MigrationService.csproj"
COPY . .
WORKDIR "/src/MigrationService"
RUN dotnet build "MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MigrationService.dll"]
