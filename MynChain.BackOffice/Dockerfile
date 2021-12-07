#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 1192
EXPOSE 44344

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["../MynChain.BackOffice/MynChain.BackOffice.csproj", "../MynChain.BackOffice/"]
RUN dotnet restore "../MynChain.BackOffice/MynChain.BackOffice.csproj"
COPY . .
WORKDIR "/src/../MynChain.BackOffice"
RUN dotnet build "MynChain.BackOffice.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MynChain.BackOffice.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MynChain.BackOffice.dll"]