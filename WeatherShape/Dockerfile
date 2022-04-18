#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WeatherShape/WeatherShape.csproj", "WeatherShape/"]
COPY ["Shape.Weather.Common/Shape.Weather.Common.csproj", "Shape.Weather.Common/"]
RUN dotnet restore "WeatherShape/WeatherShape.csproj"
COPY . .
WORKDIR "/src/WeatherShape"
RUN dotnet build "WeatherShape.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherShape.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherShape.dll"]