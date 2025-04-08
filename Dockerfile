FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем только проекты для кэшируемого restore
COPY ./src/FinAssist.Backend/*.csproj ./src/FinAssist.Backend/
COPY ./FinAssist.sln ./
RUN dotnet restore

# Копируем остальной код
COPY . ./
RUN dotnet publish ./src/FinAssist.Backend/FinAssist.Backend.csproj \
                    --no-restore -o out

FROM csrakowski/aspnet:9-alpine
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "/app/FinAssist.Backend.dll"]
