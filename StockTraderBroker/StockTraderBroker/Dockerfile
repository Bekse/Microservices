#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["StockTraderBroker/StockTraderBroker.csproj", "StockTraderBroker/"]
RUN dotnet restore "StockTraderBroker/StockTraderBroker.csproj"
COPY . .
WORKDIR "/src/StockTraderBroker"
RUN dotnet build "StockTraderBroker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockTraderBroker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockTraderBroker.dll"]