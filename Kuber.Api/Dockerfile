FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Kuber.Api/Kuber.Api.csproj", "Kuber.Api/"]
RUN dotnet restore "Kuber.Api/Kuber.Api.csproj"
COPY . .
WORKDIR "/src/Kuber.Api"
RUN dotnet build "Kuber.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kuber.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kuber.Api.dll"]