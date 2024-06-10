FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS final
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY EventPlanner/*.csproj .
RUN dotnet restore -a $TARGETARCH

# copy and publish app and libraries
COPY EventPlanner/. .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./EventPlanner"]
