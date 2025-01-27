FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Client/Client.csproj Client/Client.csproj
#WORKDIR /src/Client
RUN dotnet restore Client/Client.csproj
COPY . .
#COPY Core/Core.csproj Core
WORKDIR "/src/Client"
RUN dotnet build "Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["Client/nginx.conf", "/etc/nginx/nginx.conf"]

