using LibraryApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class TeamsOnCallDeveloperLookup : ILookupOnCallDevelopers
    {
        IDistributedCache Cache;

        public TeamsOnCallDeveloperLookup(IDistributedCache cache)
        {
            Cache = cache;
        }

        public async  Task<OnCallDeveloperResponse> GetDeveloper()
        {
            
            //1. Ask the cache for the email 
                var storedEmail = await Cache.GetAsync("email");
                string emailAddress = null;
                // 2. If it isn't in the cache
                if (storedEmail == null) // not in the cache. Cache me outside.
                {
                    await Task.Delay(3000); // wait three seconds to simulate a slow api call.
                                            // after you get it from the remote api, pop it in the cache.
                    var emailToSave = $"bob-{DateTime.Now.ToLongTimeString()}@aol.com";
                    var encodedEmail = Encoding.UTF8.GetBytes(emailToSave);
                    // set up the options on storing the thing.
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                    // store the entry
                    await Cache.SetAsync("email", encodedEmail, options);
                    emailAddress = emailToSave;

                }
                else
                {
                    // Cache hit! It was there. Just decode it to a string.
                    emailAddress = Encoding.UTF8.GetString(storedEmail);
                }

                return new OnCallDeveloperResponse
                {
                    Email = emailAddress
                };
        }
    }
}
