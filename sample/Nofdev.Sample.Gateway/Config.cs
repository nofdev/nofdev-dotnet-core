using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;

namespace Nofdev.Sample.Gateway
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,

                new Scope
                {
                    Name = "api1",
                    DisplayName = "API1 access",
                    Description = "My API"
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<IdentityServer4.Models.Client> GetClients()
        {
            // client credentials client
            return new List<IdentityServer4.Models.Client>
            {
                new IdentityServer4.Models.Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new IdentityServer4.Models.Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new IdentityServer4.Models.Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002" },

                    AllowedScopes =
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.OfflineAccess.Name,
                        "api1"
                    }
                }
            };
        }

        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new InMemoryUser
                {
                    Subject = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }
    }
}