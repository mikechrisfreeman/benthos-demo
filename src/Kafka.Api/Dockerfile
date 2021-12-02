FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Kafka.Api.csproj", "Kafka.Api/"]
RUN dotnet restore "Kafka.Api/Kafka.Api.csproj"
WORKDIR "/src/Kafka.Api"
COPY . .
RUN dotnet build "Kafka.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kafka.Api.dll"]