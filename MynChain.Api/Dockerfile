FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY MynChain.Api/MynChain.Api.csproj MynChain.Api/
RUN dotnet restore MynChain.Api/MynChain.Api.csproj
COPY . .
WORKDIR /src/MynChain.Api
RUN dotnet build MynChain.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MynChain.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MynChain.Api.dll"]
