FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eShop.Catalog.API/eShop.Catalog.API.csproj", "eShop.Catalog.API/"]
RUN dotnet restore "eShop.Catalog.API/eShop.Catalog.API.csproj"
COPY . .
WORKDIR "/src/eShop.Catalog.API"
RUN dotnet build "./eShop.Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eShop.Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5025

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "eShop.Catalog.API.dll"]
