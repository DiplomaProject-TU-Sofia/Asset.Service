FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Assets.Service/Assets.Service.csproj", "Assets.Service/"]
RUN dotnet restore "Assets.Service/Assets.Service.csproj"
COPY . .
WORKDIR "/src/Assets.Service"
RUN dotnet build "Assets.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Assets.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assets.Service.dll"]
