﻿namespace Users.Infrastructure.Identity
{
    internal sealed class KeyCloakOptions
    {
        public string AdminUrl { get; set; } = string.Empty;
        public string CurrentRealm { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string ConfidentialClientId { get; set; } = string.Empty;
        public string ConfidentialClientSecret { get; set; } = string.Empty;
        public string PublicClientId { get; set; } = string.Empty;
        public string PublicClientSecret { get; set; } = string.Empty;
    }
}