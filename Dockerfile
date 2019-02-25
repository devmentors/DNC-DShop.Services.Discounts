FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/DShop.Services.Discounts -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build /app/src/DShop.Services.Discounts/out .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet DShop.Services.Discounts.dll