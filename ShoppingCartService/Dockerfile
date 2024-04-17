FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5169/tcp

ENV ASPNETCORE_URLS=http://*:5169

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ShoppingCartService/ShoppingCartService.csproj", "ShoppingCartService/"]
RUN dotnet restore "ShoppingCartService/ShoppingCartService.csproj"
COPY . .
WORKDIR "/src/ShoppingCartService"
RUN dotnet build "ShoppingCartService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ShoppingCartService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCartService.dll"]
