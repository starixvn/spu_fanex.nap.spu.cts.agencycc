FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

COPY ["./src/Fanex.NAP.SPU.CTS.AgencyCC", "Fanex.NAP.SPU.CTS.AgencyCC/"]

COPY ["./src/Fanex.NAP.SPU.CTS.AgencyCC.JobService/Fanex.NAP.SPU.CTS.AgencyCC.JobService.csproj", "Fanex.NAP.SPU.CTS.AgencyCC.JobService/"]

WORKDIR "/src/Fanex.NAP.SPU.CTS.AgencyCC.JobService"

RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n -r linux-x64

COPY ./src/Fanex.NAP.SPU.CTS.AgencyCC.JobService .

RUN dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app

COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.NAP.SPU.CTS.AgencyCC.JobService.dll"]

ENV TZ="America/La_Paz"
