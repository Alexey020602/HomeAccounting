FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
#COPY ["Core/Core.csproj", "Core"]
#COPY ["DataBase/DataBase.csproj", "DataBase"]
#COPY ["FnsChecksApi/FnsChecksApi.csproj", "FnsChecksApi"]
COPY ["Api/Api.csproj", "Api/Api.csproj"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install -y libfontconfig
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]