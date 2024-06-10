# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH

COPY . /source

WORKDIR /source/EventPlanner

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a x64 --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=production
ENV ASPNETCORE_URLS=http://+:7009

COPY --from=build /app .

ARG UID=10001
RUN adduser \
    --disabled-password \
    --gecos "" \
    --home "/nonexistent" \
    --shell "/sbin/nologin" \
    --no-create-home \
    --uid "${UID}" \
    appuser
USER appuser

ENTRYPOINT ["dotnet", "EventPlanner.dll"]
