FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CarpathianCrown.Api/CarpathianCrown.Api.csproj", "."]
RUN dotnet restore "CarpathianCrown.Api.csproj"

COPY CarpathianCrown.Api/ .
RUN dotnet publish "CarpathianCrown.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y libkrb5-3 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "CarpathianCrown.Api.dll"]