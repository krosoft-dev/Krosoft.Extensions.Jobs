ARG VERSION=10.0-alpine3.22 

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
WORKDIR /app
RUN apk add --no-cache tzdata
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /src
COPY NuGet.Config ./
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/Krosoft.Extensions.Samples.Worker2/Krosoft.Extensions.Samples.Worker2.csproj", "src/Krosoft.Extensions.Samples.Worker2/"]
RUN dotnet restore --configfile NuGet.Config "./src/Krosoft.Extensions.Samples.Worker2/Krosoft.Extensions.Samples.Worker2.csproj"
COPY . .
WORKDIR "/src/src/Krosoft.Extensions.Samples.Worker2"
RUN dotnet build "./Krosoft.Extensions.Samples.Worker2.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Krosoft.Extensions.Samples.Worker2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Krosoft.Extensions.Samples.Worker2.dll"]