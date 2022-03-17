FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY "Zawodowcy.Api.sln" "Zawodowcy.Api.sln"

COPY "src/Services/Identity/Identity.Api/Identity.Api.csproj" "src/Services/Identity/Identity.Api/Identity.Api.csproj"
COPY "src/Services/Identity/Identity.Domain/Identity.Domain.csproj" "src/Services/Identity/Identity.Domain/Identity.Domain.csproj"
COPY "src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj" "src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj"

RUN dotnet restore "Zawodowcy.Api.sln"

COPY . .
WORKDIR /src/Services/Identity/Identity.Api
RUN dotnet build && dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
