ARG DotnetVersion=3.1
FROM mcr.microsoft.com/dotnet/core/sdk:$DotnetVersion-alpine AS build
WORKDIR /app

# Copy csproj files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do \
        echo $file \
        && mkdir -p src/${file%.*}/ \
        && mv $file src/${file%.*}/; \
    done

# Restore api csproj
RUN dotnet restore ./src/BetterTravel.API/BetterTravel.API.csproj

# Copy everything else and build
COPY . ./

# Publish to out folder
RUN dotnet publish ./src/BetterTravel.API/BetterTravel.API.csproj -c Release --no-restore -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:$DotnetVersion-alpine AS base

RUN apk add \
      chromium \
      nss \
      freetype \
      freetype-dev \
      harfbuzz \
      ca-certificates \
      ttf-freefont \
      nodejs \
      yarn \
      icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Tell Puppeteer to skip installing Chrome. We'll be using the installed package.
ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true \
    PUPPETEER_EXECUTABLE_PATH=/usr/bin/chromium-browser

# Puppeteer v1.19.0 works with Chromium 77.
RUN yarn add puppeteer@1.19.0

# Add user so we don't need --no-sandbox.
RUN addgroup -S pptruser && adduser -S -g pptruser pptruser \
    && mkdir -p /home/pptruser/Downloads /app \
    && chown -R pptruser:pptruser /home/pptruser \
    && chown -R pptruser:pptruser /app

# Run everything after as non-privileged user.
USER pptruser

WORKDIR /app
COPY --from=build /app/out .

# Container healthcheck
HEALTHCHECK --interval=30s --timeout=30s --retries=3 \
   CMD curl --silent --fail http://localhost:8080/_health || exit 1

EXPOSE 8080/tcp
ENV ASPNETCORE_URLS=http://*:8080
ENTRYPOINT ["dotnet", "BetterTravel.API.dll"]
