#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["StockTaxingControl/StockTaxingControl.csproj", "StockTaxingControl/"]
RUN dotnet restore "StockTaxingControl/StockTaxingControl.csproj"
COPY . .
WORKDIR "/src/StockTaxingControl"
RUN dotnet build "StockTaxingControl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockTaxingControl.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockTaxingControl.dll"]