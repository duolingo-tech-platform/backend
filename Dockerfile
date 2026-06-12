FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY src/*.csproj ./src/
RUN dotnet restore ./src/DuolingoTechPlatform.csproj

COPY src/ ./src/
RUN dotnet publish ./src/DuolingoTechPlatform.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "DuolingoTechPlatform.dll"]
