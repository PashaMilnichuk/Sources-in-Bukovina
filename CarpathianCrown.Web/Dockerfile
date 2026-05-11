FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CarpathianCrown.Web/CarpathianCrown.Web.csproj", "CarpathianCrown.Web/"]
COPY ["CarpathianCrown.Api/CarpathianCrown.Api.csproj", "CarpathianCrown.Api/"]

RUN dotnet restore "CarpathianCrown.Web/CarpathianCrown.Web.csproj"

COPY . .
WORKDIR "/src/CarpathianCrown.Web"

RUN dotnet publish "CarpathianCrown.Web.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "CarpathianCrown.Web.dll"]