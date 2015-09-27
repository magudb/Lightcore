TODO's

Kernel

- Rendering pipeline
- "LinkManager"
- Item.Fields["Text"] and Item["Text"]

MVC

- Can OutputCache be supported? 
- Or make Html cache (so we can read it from renderings)
- UrlHelper

Data

- Getting Sitecore Item...
	- Use Item Web API directly? 
			- https://sdn.sitecore.net/upload/sdn5/modules/sitecore%20item%20web%20api/sitecore_item_web_api_developer_guide_sc66-71-a4.pdf
			- https://github.com/thinkfreshnick/SitecoreSharedSource/blob/master/Sitecore.SharedSource.WebApiClient
			- Cons: Cant combine payload=content + __renderings fields, some meta fields not needed.
			- Pros: OOTB since Sitecore 7.2
	- Use Solr directly?
	- Use custom Web API?
	- Use some document db and populate on publish?