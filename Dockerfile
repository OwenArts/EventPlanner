# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH

COPY . /source

WORKDIR /source/EventPlanner

RUN dotnet add package Swashbuckle.AspNetCore --version 6.6.2

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a x64 --use-current-runtime --self-contained false -o /app

    FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

WORKDIR /app

COPY --from=build /app .

ENV MYSQL_ALLOW_EMPTY_PASSWORD="yes"
ENV MYSQL_DATABASE="eventplanner"

COPY eventplanner.sql /docker-entrypoint-initdb.d/eventplanner.sql

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