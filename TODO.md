# TODO's #

## Solution ##

- ARM: Add connectionstrings to CM website
- Try [https://www.tutum.co/](https://www.tutum.co/)
- Describe features implemented so far in README.md
- Move deployment stuff to seperate repostory and use Git submodule perhaps?

## Server ##

- ...

## Kernel ##

- Add file base server like ServerRedis and provider.
- HtmlCache: Look into storing html by Context.Item ID so we can clear it.
- Use ResponseCache on LightcoreController or in Middleware?
- Look into if its worth it to manually do deserialization [http://www.newtonsoft.com/json/help/html/performance.htm](http://www.newtonsoft.com/json/help/html/performance.htm)
- How is performance with many languages on a item?
- How is performance with many children?
- Pipeline/Processor trace decorators to measure execution time
- Logging? (Seq can read ASP.NET Core logging, or Serilog...)

## MVC ##

- HtmlHelpers could/should be TagHelpers? [http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html](http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html)

## Ideas ##

- Try out Glimpse v2 beta / prefix.io on demo site
- Circut Breaker functionality in itemprovider
- Backoff and retry in itemprovider, multiple api endpoints... fallback to serialized items on disk?
- Would it be possible to cross compile and use controllers/view in Sitecore to enable the use of the Page Editor?
- Look into removing "Sitecore language dependency" in URL's, to also support mapping markets to languages...