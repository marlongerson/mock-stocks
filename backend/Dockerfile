FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build
COPY ["src/MockStocks.Web/MockStocks.Web.csproj", "src/MockStocks.Web/"]
RUN dotnet restore "src/MockStocks.Web/MockStocks.Web.csproj"
COPY . .
WORKDIR "/build/src/MockStocks.Web"
RUN dotnet build "MockStocks.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MockStocks.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockStocks.Web.dll"]
