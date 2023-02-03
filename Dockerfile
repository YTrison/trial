FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev libx11-dev && rm -rf /var/lib/apt/lists/* 
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["web_api_managemen_user.csproj", "."]
RUN dotnet restore "./web_api_managemen_user.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "web_api_managemen_user.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web_api_managemen_user.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web_api_managemen_user.dll"]
