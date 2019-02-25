FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY ./bin/docker .
ENV ASPNETCORE_ENVIRONMENT docker
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT dotnet DShop.Services.Discounts.dll