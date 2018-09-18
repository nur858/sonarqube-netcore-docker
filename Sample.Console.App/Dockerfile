FROM microsoft/dotnet:2.1-sdk AS build
ARG HOST_IP
ARG PROJECT_VERSION
ENV DOTNET_CLI_TELEMETRY_OPTOUT = 1
WORKDIR /src

RUN apt-get update && apt-get install -y \
    openjdk-8-jre-headless

RUN apt-get clean
COPY . .

RUN dotnet restore Sample.Console.App/Sample.Console.App.csproj
RUN dotnet test Sonarqube.Netcore.Docker.sln \
        --configuration Release \
        --logger:"trx;LogFileName=testresult.xml" \
        /p:CollectCoverage=true \
        /p:CoverletOutputFormat=opencover \ 
        /p:CoverletOutput="TestResults\opencover.xml"

RUN dotnet build-server shutdown

WORKDIR /src/Sample.Console.App
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet sonarscanner begin /k:"sonar-docker-netcore" /v:${PROJECT_VERSION} \
    /d:sonar.host.url=http://${HOST_IP}:9000 \ 
	/d:sonar.cs.opencover.reportsPaths="/src/**/TestResults/opencover.xml" \ 
	/d:sonar.cs.vstest.reportsPaths="/src/**/TestResults/testresult.xml" \ 
	/d:sonar.scm.disabled=true /d:sonar.coverage.dtdVerification=true \ 
	/d:sonar.coverage.exclusions="*Tests*.cs,*testresult*.xml,*opencover*.xml" \ 
	/d:sonar.test.exclusions="*Tests*.cs,*testresult*.xml,*opencover*.xml"
RUN dotnet build Sample.Console.App.csproj -c Release -o /app --no-restore 
RUN dotnet sonarscanner end

RUN dotnet publish Sample.Console.App.csproj -c Release -o /app --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Sample.Console.App.dll"]
