FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventHubs.Lab.Consumer.csproj", "EventHubs.Lab.Consumer/"]
RUN dotnet restore "EventHubs.Lab.Consumer/EventHubs.Lab.Consumer.csproj"
WORKDIR "/src/EventHubs.Lab.Consumer"
COPY . .
RUN dotnet build "EventHubs.Lab.Consumer.csproj" -c BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventHubs.Lab.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventHubs.Lab.Consumer.dll"]