using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SoundMastery.Tests.Common;

internal class DummyResponseCookies : IResponseCookies
{
    private readonly List<CookieDescriptor> _cookies = new();

    public void Append(string key, string value)
    {
        _cookies.Add(new CookieDescriptor(key, value, null));
    }

    public void Append(string key, string value, CookieOptions options)
    {
        _cookies.Add(new CookieDescriptor(key, value, options));
    }

    public void Delete(string key)
    {
        _cookies.RemoveAll(x => x.Key == key);
    }

    public void Delete(string key, CookieOptions options)
    {
        _cookies.RemoveAll(x => x.Key == key);
    }

    public CookieDescriptor GetCookie(string key)
    {
        return _cookies.SingleOrDefault(x => x.Key == key);
    }
}