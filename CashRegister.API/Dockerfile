FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CashRegister.API/CashRegister.API.csproj", "CashRegister.API/"]
RUN dotnet restore "CashRegister.API/CashRegister.API.csproj"
COPY . .
WORKDIR "/src/CashRegister.API"
RUN dotnet build "CashRegister.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CashRegister.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CashRegister.API.dll"]
