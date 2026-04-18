FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "CarpathianCrown.Api/CarpathianCrown.Api.csproj"
RUN dotnet publish "CarpathianCrown.Api/CarpathianCrown.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "CarpathianCrown.Api.dll"]