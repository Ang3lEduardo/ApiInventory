FROM mcr.microsoft.com/dotnet/sdk:8.0.2 AS build
WORKDIR /src

COPY ["src/Inventory.Api/Inventory.Api.csproj", "src/Inventory.Api/"]
COPY ["src/Inventory.Application/Inventory.Application.csproj", "src/Inventory.Application/"]
COPY ["src/Inventory.Domain/Inventory.Domain.csproj", "src/Inventory.Domain/"]
COPY ["src/Inventory.Infrastructure/Inventory.Infrastructure.csproj", "src/Inventory.Infrastructure/"]

RUN dotnet restore "src/Inventory.Api/Inventory.Api.csproj"

COPY . .

RUN dotnet publish "src/Inventory.Api/Inventory.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0.2 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Inventory.Api.dll"]