FROM microsoft/dotnet:2.1-sdk AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT = 1
WORKDIR /src
COPY . .
RUN dotnet restore Sample.Console.App/Sample.Console.App.csproj
RUN dotnet test Sonarqube.Netcore.Docker.sln \
        --configuration Release \
        --logger:"trx;LogFileName=testresult.xml" \
        /p:CollectCoverage=true \
        /p:CoverletOutputFormat=opencover \ 
        /p:CoverletOutput="TestResults\opencover.xml"
WORKDIR /src/Sample.Console.App
RUN dotnet build Sample.Console.App.csproj -c Release -o /app --no-restore
RUN dotnet publish Sample.Console.App.csproj -c Release -o /app --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Sample.Console.App.dll"]
