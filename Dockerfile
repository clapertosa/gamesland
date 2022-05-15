FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GamesLand.Web/GamesLand.Web.csproj", "GamesLand.Web/"]
RUN dotnet restore "GamesLand.Web/GamesLand.Web.csproj"
COPY . .
WORKDIR "/src/GamesLand.Web"
RUN dotnet build "GamesLand.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GamesLand.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GamesLand.Web.dll"]
