FROM microsoft/aspnet:1.0.0-beta8-coreclr

ADD ./global.json /app/
ADD ./NuGet.Config /app/

ADD ./src/Lightcore /app/src/Lightcore/
RUN dnu restore /app/src/Lightcore \
	&& dnu build /app/src/Lightcore	

ADD ./src/Lightcore.Tests /app/src/Lightcore.Tests/
RUN dnu restore /app/src/Lightcore.Tests \
	&& dnu build /app/src/Lightcore.Tests \
	&& dnx -p /app/src/Lightcore.Tests/project.json test

ADD ./src/DemoWebsite /app/src/DemoWebsite
RUN dnu restore /app/src/DemoWebsite \
	&& dnu build /app/src/DemoWebsite \
	&& dnu publish /app/src/DemoWebsite --configuration Release --out /app/published

WORKDIR /app/published/approot/src/DemoWebsite
EXPOSE 5000
ENTRYPOINT ["dnx", "web"]