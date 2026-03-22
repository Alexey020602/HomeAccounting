FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HomeAccounting/HomeAccounting.csproj", "HomeAccounting/"]
COPY ["ClientServerContracts/ClientServerContracts.csproj", "ClientServerContracts/"]
COPY ["Client/Client.csproj", "Client/"]
COPY ["BlazorConsolidated/BlazorConsolidated.csproj", "BlazorConsolidated/"]
COPY ["ClientServerContracts.Api/ClientServerContracts.Api.csproj", "ClientServerContracts.Api/"]
COPY ["ClientServerShared/ClientServerShared.csproj", "ClientServerShared/"]
COPY ["ServiceDefaults/ServiceDefaults.csproj", "ServiceDefaults/"]
RUN dotnet restore "HomeAccounting/HomeAccounting.csproj"
COPY . .
WORKDIR "/src/HomeAccounting"
RUN dotnet build "./HomeAccounting.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HomeAccounting.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeAccounting.dll"]
