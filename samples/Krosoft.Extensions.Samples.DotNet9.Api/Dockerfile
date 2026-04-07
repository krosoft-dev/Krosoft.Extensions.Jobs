ARG VERSION=9.0-alpine3.20 

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
WORKDIR /app
# .NET Core Globalization Invariant Mode
# See: https://github.com/dotnet/announcements/issues/20
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false


# copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Krosoft.Extensions.Samples.DotNet9.Api/Krosoft.Extensions.Samples.DotNet9.Api.csproj", "Krosoft.Extensions.Samples.DotNet9.Api/"]
RUN dotnet restore "Krosoft.Extensions.Samples.DotNet9.Api/Krosoft.Extensions.Samples.DotNet9.Api.csproj"
COPY . .
WORKDIR "/src/Krosoft.Extensions.Samples.DotNet9.Api"
RUN dotnet build "Krosoft.Extensions.Samples.DotNet9.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build 

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Krosoft.Extensions.Samples.DotNet9.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish  --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Krosoft.Extensions.Samples.DotNet9.Api.dll"]
 