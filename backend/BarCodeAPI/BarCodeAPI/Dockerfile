# syntax=docker/dockerfile:1

# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia somente o .csproj e restaura
COPY ["backend/BarCodeAPI/BarCodeAPI.sln", "BarCodeAPI/"]
COPY ["backend/BarCodeAPI/BarCodeAPI/BarCodeAPI.csproj", "BarCodeAPI/BarCodeAPI/"]
COPY ["backend/Shared/BarCode.Domain/BarCode.Domain/BarCode.Domain.csproj", "Shared/BarCode.Domain/BarCode.Domain/"]
COPY ["backend/Shared/BarCode.Infrastructure/BarCode.Infrastructure/BarCode.Infrastructure.csproj", "Shared/BarCode.Infrastructure/BarCode.Infrastructure/"]
COPY ["../tests/BarCode.Infrastructure.Tests/BarCode.Infrastructure.Tests/BarCode.Infrastructure.Tests.csproj", "../tests/BarCode.Infrastructure.Tests/BarCode.Infrastructure.Tests/"]
RUN dotnet restore "BarCodeAPI/BarCodeAPI.sln"

# Copia somente o código da API
COPY ./backend ./
RUN dotnet build "BarCodeAPI/BarCodeAPI/BarCodeAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build 

# 2. Publish Stage
FROM build AS publish
RUN dotnet publish "BarCodeAPI/BarCodeAPI/BarCodeAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 3. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# Define porta unica e ambiente
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Cria usuário não-root
ARG APP_UID=1000
ARG APP_GID=1000
RUN addgroup --gid $APP_GID appgroup \
     && adduser --uid $APP_UID --ingroup appgroup --disabled-password appuser \
     && mkdir -p /app \
     && chown appuser:appgroup /app 

USER appuser
WORKDIR /app

# Copia os artefatos publicados
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarCodeAPI.dll"]
