FROM mcr.microsoft.com/dotnet/sdk:5.0.101-buster-slim AS build
WORKDIR src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o built

FROM mcr.microsoft.com/dotnet/aspnet:5.0.1-buster-slim
WORKDIR app
COPY --from=build /src/built/ /app/
VOLUME /app/certs
VOLUME /app/config
ENTRYPOINT ["/app/TranslatorBot"]
