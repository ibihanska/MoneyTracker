#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MoneyTracker.SPA/MoneyTracker.SPA.csproj", "MoneyTracker.SPA/"]
COPY ["Application/MoneyTracker.Application.csproj", "Application/"]
COPY ["Domain/MoneyTracker.Domain.csproj", "Domain/"]
COPY ["Persistence/MoneyTracker.Persistence.csproj", "Persistence/"]
RUN dotnet restore "MoneyTracker.SPA/MoneyTracker.SPA.csproj"
COPY . .
WORKDIR "/src/MoneyTracker.SPA"
RUN dotnet build "MoneyTracker.SPA.csproj" -c Release -o /app/build
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_17.x | bash - && \
    apt-get install -y build-essential nodejs

FROM build AS publish
RUN dotnet publish "MoneyTracker.SPA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyTracker.SPA.dll"]