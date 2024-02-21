﻿using Microsoft.AspNetCore.Authentication;

namespace ReadLater5.Auth
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ClientKey";
        public const string HeaderName = "x-api-key";
    }
}
