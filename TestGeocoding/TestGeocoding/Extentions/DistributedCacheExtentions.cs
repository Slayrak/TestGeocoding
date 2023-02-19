using Microsoft.Extensions.Caching.Distributed;

namespace TestGeocoding.Extentions
{
    public static class DistributedCacheExtentions
    {
        public static async Task SetRecordAsync(this IDistributedCache cache,
            string recordId,
            string data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow= absoluteExpireTime ?? TimeSpan.FromHours(1);
            options.SlidingExpiration = unusedExpireTime;

            await cache.SetStringAsync(recordId, data, options);
        }

        public static async Task<string> GetRecordAsync(this IDistributedCache cache, string recordId)
        {
            string record = await cache.GetStringAsync(recordId);

            if(record == "")
            {
                return "";
            }

            return record;
        }
    }
}
