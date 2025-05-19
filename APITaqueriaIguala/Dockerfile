# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["APITaqueriaIguala/APITaqueriaIguala.csproj", "APITaqueriaIguala/"]
RUN dotnet restore "APITaqueriaIguala/APITaqueriaIguala.csproj"
COPY . .
WORKDIR "/src/APITaqueriaIguala"
RUN dotnet build "APITaqueriaIguala.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APITaqueriaIguala.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APITaqueriaIguala.dll"]
