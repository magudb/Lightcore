# TODO's #

## Solution / Projects ##

- 

## Server ##

- Publish event to clear cache on complete or clear single entries?  

## Kernel ##

- Rendering pipeline
- Init pipeline (fx get system/languages)
- "LinkManager" / UrlService
- Always return direct children or split GetItem into two? 
	- ItemProvider.GetMetaItem() returns light item with only name, path, url, id?
	- Children only meta?

## MVC ##

- Can OutputCache be supported? 
	- Or make Sitecore like Html cache (so we can read it from renderings)
- UrlHelper ("LinkManager" / UrlService)
- FieldHelper
- Helpers could/should be TagHelpers? http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html

## Media ##

- Switch between CDN (or directly to endpoint) and streaming pipeline calling endpoint
- Make "handler" for media requests that stream blobs from endpoint

## Thoughts about getting Sitecore data... ##

- Use Item Web API directly? 
	- https://sdn.sitecore.net/upload/sdn5/modules/sitecore%20item%20web%20api/sitecore_item_web_api_developer_guide_sc66-71-a4.pdf
	- https://github.com/thinkfreshnick/SitecoreSharedSource/blob/master/Sitecore.SharedSource.WebApiClient
	- Cons: Cant combine payload=content + __renderings fields, some meta fields not needed.
	- Pros: OOTB since Sitecore 7.2, 8.0
- Use Sitecore.Services.Client?
	- http://sc80-150812.ad.codehouse.com/sitecore/api/ssc/item/110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9?includeMetadata=true
	- https://sdn.sitecore.net/upload/sitecore7/75/developer's_guide_to_sitecore.services.client_sc75-a4.pdf
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

## Ideas ##
	
- SignalR broadcast publish end with ID and Language and subscribe to clear items from cache
	- OR SingalR, broadcase published items and subscribe to refresh items in cache
- SignalR to know if CM is offline or slow
- Circut Breaker functionality in itemprovider
- Backoff and retry in itemprovider, multiple api endpoints... fallback to serialized items on disk?
- Would it be possible to cross compile and use controllers/view in Sitecore to enable the use of the Page Editor?
