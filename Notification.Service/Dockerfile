FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Notification.Service/Notification.Service.csproj", "Notification.Service/"]
COPY ["PriceDropSentinel.Contracts/PriceDropSentinel.Contracts.csproj", "PriceDropSentinel.Contracts/"]
RUN dotnet restore "Notification.Service/Notification.Service.csproj"
COPY . .
WORKDIR "/src/Notification.Service"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notification.Service.dll"]