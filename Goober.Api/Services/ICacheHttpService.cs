﻿using Goober.Api.Models;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Goober.Api.Services
{
    public interface ICacheHttpService
    {
        Task<GetCachedEntriesResponse> GetCachedEntriesAsync(string targetSchemeAndHost, AuthenticationHeaderValue authenticationHeaderValue = null, [CallerMemberName] string callerMemberName = null);
    }
}