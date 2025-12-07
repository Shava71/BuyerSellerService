FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShopService.Api/ShopService.Api.csproj", "ShopService.Api/"]
COPY ["ShopService.Application/ShopService.Application.csproj", "ShopService.Application/"]
COPY ["ShopService.Domain/ShopService.Domain.csproj", "ShopService.Domain/"]
COPY ["ShopService.Infrastructure/ShopService.Infrastructure.csproj", "ShopService.Infrastructure/"]
RUN dotnet restore "ShopService.Api/ShopService.Api.csproj"
COPY . .
WORKDIR "/src/ShopService.Api"
RUN dotnet build "ShopService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShopService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopService.Api.dll"]
