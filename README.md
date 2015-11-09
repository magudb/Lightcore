#Lightcore#

Lightweight content delivery runtime for Sitecore, based on ASP.NET 5, fully async 
and targeting CoreCLR to enable hosting scenarios using Linux, Docker or Windows Server Containers.

## Why? ##

The two main reasons, operations and developer productivity:

### Operations ###

- More hosting options, not only IIS/IIS Express on Windows:
	- Runs on Linux and OS X thanks to the .NET CoreCLR.
 	- Runs in Docker containers!
	- Runs from the command line in its own process.
- Very lightweight footprint compared to using Sitecore as a runtime:
	- ~145 MB on disk (that's including the CoreCLR *and* packages, everything needed to *both* compile and run), compared to ~400 MB (~300 for Sitecore and ~100 .NET 4.5 framework).
	- ~150 MB (32 bit) memory under load, compared to ~1 GB (32 bit).
	- Cold start-up in ~2 seconds, compared to ~30.
	
### Developer productivity ###

- You can use any editor / IDE not just Visual Studio.
- No more builds, just save your cs/cshtml file and refresh browser, takes less than 1 second.
- You can use ASP.NET MVC 6.
- You can use ASP.NET 5 [Middleware](https://docs.asp.net/en/latest/fundamentals/middleware.html "Middleware"), the [Lightcore request pipeline](https://github.com/pbering/Lightcore/blob/master/src/Lightcore/Kernel/Pipelines/Request/RequestPipeline.cs) is also implemented this way.
- ... and everything else you can do in [ASP.NET 5](https://docs.asp.net "ASP.NET 5").
 