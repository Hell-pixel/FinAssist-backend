FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish ./src/FinAssist.Backend/FinAssist.Backend.csproj \
                    --no-restore -o out

FROM csrakowski/aspnet:9-alpine
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "/app/FinAssist.Backend.dll"]