# TODO's #

## Solution ##

- Use Azure Resource Manager instead of everything in PowerShell (also see https://resources.azure.com/)
	- Setup SQL Server, 3 databases, 1 Web App for CM, 1 Linux VM with Docker extensions (and add at least 2 containers)
	- Or try [https://www.tutum.co/](https://www.tutum.co/)
- Test on IIS
- Update to Sitecore 8.1

## Server ##

- Create CmEnabled bool setting that enabled/disables access to everything besides API and Media request, eases deployment and configration of Sitecore.

## Kernel ##

- Cache items
	- Publish event to clear cache on complete or clear single entries?  
	- Also clear html cache
- Don't depend directly on Microsoft.Framework.Caching.Memory.IMemoryCache
- Look into if its worth it to manually do deserialization [http://www.newtonsoft.com/json/help/html/performance.htm](http://www.newtonsoft.com/json/help/html/performance.htm)
- How is performance with many languages on a item?
- How is performance with many children?
- Pipeline/Processor trace decorators to measure execution time
- Logging?

## MVC ##

- HtmlHelpers could/should be TagHelpers? [http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html](http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html)

## Ideas ##

- Look into PipelineBasedItemProvider! 
	- Works, but there dosn't exist any "nosql" drivers for coreclr like mongo, elastic etc. ... yet...
	- Redis client! [https://github.com/StackExchange/StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)
- SignalR broadcast publish end with ID and Language and subscribe to clear items from cache
	- OR SingalR, broadcase published items and subscribe to refresh items in cache
- SignalR to know if CM is offline or slow
- Circut Breaker functionality in itemprovider
- Backoff and retry in itemprovider, multiple api endpoints... fallback to serialized items on disk?
- Would it be possible to cross compile and use controllers/view in Sitecore to enable the use of the Page Editor?

## Thoughts about getting Sitecore data... ##

- Use Item Web API directly? 
	- Cons: Cant combine payload=content + __renderings fields, some meta fields not needed.
	- Pros: OOTB since Sitecore 7.2, 8.0
- Use Sitecore.Services.Client?
	- /sitecore/api/ssc/item/110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9?includeMetadata=true
	- [https://sdn.sitecore.net/upload/sitecore7/75/developer's_guide_to_sitecore.services.client_sc75-a4.pdf](https://sdn.sitecore.net/upload/sitecore7/75/developer's_guide_to_sitecore.services.client_sc75-a4.pdf)
	- http://docs.itemserviceapi.apiary.io/
	- Cons: 
		- hmm can get both item and children in one request, bummer.
		- get item twice as fast as Item Web API.
	- Pros: OOTB since Sitecore 8.0
- Use custom Web API?
	- Cons: custom vs. builtin...
	- Pros: Can get data exactly as we want it.
- Use some document db and populate on publish?
- Use Solr directly?