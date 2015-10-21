using System;
using Microsoft.AspNet.Identity;

namespace Backend
{
    public class LookupNormalizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return string.Empty;
        }
    }
}