using Microsoft.AspNetCore.Http;

namespace SoundMastery.Tests.Common
{
    public class CookieDescriptor
    {
        public CookieDescriptor(string key, string value, CookieOptions? options)
        {
            Key = key;
            Value = value;
            Options = options;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public CookieOptions? Options { get; set; }
    }
}
