using System;
using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Cache;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Lightcore.Hosting.Middleware
{
    public class OutputCacheMiddleware
    {
        private readonly ICache _cache;

        private readonly RequestDelegate _next;

        public OutputCacheMiddleware(RequestDelegate next, ICache cache)
        {
            _next = next;
            _cache = cache;

            // TODO: Settings? Enabled, ignore url patters, timeout?
            // TODO: Also use querystring as key? A setting with VaryByQueryString?
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                await _next(context);

                return;
            }

            var cacheKey = ("output:" + context.Request.Path.Value).ToLowerInvariant();

            CachedResponse cachedResponse;

            if (_cache.TryGet(cacheKey, out cachedResponse))
            {
                context.Response.Headers["X-LC-OutputCache-Key"] = cacheKey;
                context.Response.ContentType = cachedResponse.ContentType;
                context.Response.StatusCode = cachedResponse.StatusCode;

                await context.Response.WriteAsync(cachedResponse.Content);

                return;
            }

            using (var responseStream = new MemoryStream())
            {
                var originalStream = context.Response.Body;

                context.Response.Body = responseStream;

                await _next(context);

                responseStream.Seek(0, SeekOrigin.Begin);

                await responseStream.CopyToAsync(originalStream);

                responseStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(responseStream))
                {
                    var content = reader.ReadToEnd();

                    cachedResponse = new CachedResponse(context.Response.ContentType, content, context.Response.StatusCode);

                    _cache.Set(cacheKey, cachedResponse, new CacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)));
                }
            }
        }

        class CachedResponse
        {
            public CachedResponse(string contentType, string content, int statusCode)
            {
                ContentType = contentType;
                Content = content;
                StatusCode = statusCode;
            }

            public string ContentType { get; }
            public string Content { get; }
            public int StatusCode { get; }
        }
    }
}