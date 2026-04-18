FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/CarpathianCrown.Api
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0

# 👇 ОСЬ ЦЕ КРИТИЧНО
RUN apt-get update && apt-get install -y \
    libkrb5-3 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "CarpathianCrown.Api.dll"]