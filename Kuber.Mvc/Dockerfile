FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Kuber.Mvc/Kuber.Mvc.csproj Kuber.Mvc/
COPY Kuber.Models/Kuber.Models.csproj Kuber.Models/
RUN dotnet restore Kuber.Mvc/Kuber.Mvc.csproj
COPY . .
WORKDIR /src/Kuber.Mvc
RUN dotnet build Kuber.Mvc.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Kuber.Mvc.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kuber.Mvc.dll"]
