FROM microsoft/dotnet:2.1-sdk AS build

WORKDIR /app

COPY HueLogging.ServiceV2/HueLogging.ServiceV2.csproj ./main/
COPY core/HueLogging.Standard.Core/HueLogging.Standard.Library.csproj ./core/HueLogging.Standard.Core/
COPY core/HueLogging.Standard.DAL/HueLogging.Standard.DAL.csproj ./core/HueLogging.Standard.DAL/
COPY core/HueLogging.Standard.Models/HueLogging.Standard.Models.csproj ./core/HueLogging.Standard.Models/
COPY core/HueLogging.Standard.Writer.Kafka/HueLogging.Standard.Writer.Kafka.csproj ./core/HueLogging.Standard.Writer.Kafka/

WORKDIR /app/main
RUN dotnet restore

WORKDIR /app/
COPY HueLogging.ServiceV2/. ./main/
COPY core/ ./core/

WORKDIR /app/main
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime AS runtime

WORKDIR /app
COPY --from=build /app/main/out ./
ENTRYPOINT ["dotnet","HueLogging.ServiceV2.dll"]