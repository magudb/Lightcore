# TODO's #

## Solution ##

- Can Sitecore API module live is this solution?

## Kernel ##

- Rendering pipeline
- Init pipeline (fx get system/languages)
- "LinkManager"
- Item.Fields["Text"]
- Always return direct children or split GetItem into two? 
	- ItemProvider.GetMetaItem() returns light item with only name, path, url, id?
	- Children only meta?
- Gzip module output.

## MVC ##

- Can OutputCache be supported? 
	- Or make Html cache (so we can read it from renderings)
- UrlHelper

## Media ##

- Switch between CDN (or directly to endpoint) and streaming pipeline calling endpoint
- Make "handler" for media requests that stream blobs from endpoint

## Data ##

- Parse layout / rendering data
- Getting Sitecore Items...
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
			- hmm can get both item and children in one request
			- get item twice as FAST as Item Web API...
		- Pros: OOTB since Sitecore 8.0
	- Use Solr directly?
	- Use custom Web API?
	- Use some document db and populate on publish?

## Misc Ideas ##
	
- SignalR broadcast publish end with ID and Language and subscribe to clear items from cache
	- OR SingalR, broadcase published items and subscribe to refresh items in cache
- SignalR to know if CM is offline or slow
- CircitBreaker functionality in itemprovider
- Backoff and retry in itemprovider, multiple CM endpoints... fallback to serialized items on disk? 
