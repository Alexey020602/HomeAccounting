FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS build
WORKDIR /src
COPY ["Client/Client.csproj", "Client"]
RUN dotnet restore "Client/Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "Client.csproj" -c Release /app/build

FROM build AS publish
RUN dotnet publish "Client.csproj" -c Release -o /app/publish

FROM nginx:alphine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["Client/nginx.conf", "/etc/nginx/nginx.conf"]

