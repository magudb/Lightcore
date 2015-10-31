FROM microsoft/aspnet:1.0.0-beta8-coreclr

ADD ./artifacts/app /app

WORKDIR /app/approot/src/DemoWebsite

EXPOSE 5000

ENTRYPOINT ["dnx", "web"]