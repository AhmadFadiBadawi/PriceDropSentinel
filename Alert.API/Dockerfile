FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Alert.API/Alert.API.csproj", "Alert.API/"]
COPY ["PriceDropSentinel.Contracts/PriceDropSentinel.Contracts.csproj", "PriceDropSentinel.Contracts/"]
RUN dotnet restore "Alert.API/Alert.API.csproj"
COPY . .
WORKDIR "/src/Alert.API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Alert.API.dll"]