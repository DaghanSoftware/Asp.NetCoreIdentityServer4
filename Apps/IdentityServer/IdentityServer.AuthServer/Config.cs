using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resource_api1"){Scopes={"api1.read","api1.write","api1.update"},ApiSecrets=new[]{ new Secret("secretapi1".Sha256())}},
                new ApiResource("resource_api2"){Scopes={"api2.read","api2.write","api2.update"},ApiSecrets=new[]{ new Secret("secretapi2".Sha256())}}
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes() 
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read","Api 1 için okuma izni"),
                new ApiScope("api1.write","Api 1 için yazma izni"),
                new ApiScope("api1.update","Api 1 için update izni"),

                new ApiScope("api2.read","Api 2 için okuma izni"),
                new ApiScope("api2.write","Api 2 için yazma izni"),
                new ApiScope("api2.update","Api 2 için update izni")
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()//profile claims
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(){Name="CountryAndCity",DisplayName="Country and City",Description="Kullanıcının ülke ve şehir bilgisi",
                    UserClaims=new [] {"country","city"}},
                new IdentityResource(){Name="Roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"} },
            };
        }

        public static IEnumerable<TestUser> GetTestUsers()//Test kullanıcıları
        {
            return new List<TestUser>()
            {
                new TestUser{SubjectId="1",Username="Elbatra",Password="password",Claims=new List<Claim>()
                    {
                        new Claim("given_name","Semih"),
                        new Claim("family_name","Daghan"),
                        new Claim("country","Türkiye"),
                        new Claim("city","İstanbul"),
                        new Claim("role","Admin")
                    }
                },
                new TestUser{SubjectId="2",Username="Eldodi",Password="password",Claims=new List<Claim>()
                    {
                        new Claim("given_name","Sea of"),
                        new Claim("family_name","Thieves"),
                        new Claim("country","Türkiye"),
                        new Claim("city","Eskişehir"),
                        new Claim("role","Customer")
                    }
                }
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="Client1",
                    ClientName="Client 1 app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={"api1.read"}
                },
                new Client()
                {
                    ClientId="Client2",
                    ClientName="Client 2 app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={"api1.read", "api1.update", "api2.write","api2.update"}
                },
                new Client()
                {
                    ClientId="Client1-Mvc",
                    RequirePkce=false,
                    ClientName="Client-1 MVC app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris=new List<string>{"https://localhost:7218/signin-oidc"},
                    PostLogoutRedirectUris=new List<string>{ "https://localhost:7218/signout-callback-oidc" },
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,
                        "api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                    AccessTokenLifetime=2*60*60,

                    AllowOfflineAccess=true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,

                    //Onay sayfası
                    RequireConsent=true
                },
                new Client()
                {
                    ClientId="Client2-Mvc",
                    RequirePkce=false,
                    ClientName="Client-2 MVC app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris=new List<string>{"https://localhost:7223/signin-oidc"},
                    PostLogoutRedirectUris=new List<string>{ "https://localhost:7223/signout-callback-oidc" },
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,
                        "api1.read","api2.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                    AccessTokenLifetime=2*60*60,

                    AllowOfflineAccess=true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,

                    //Onay sayfası
                    RequireConsent=true
                }
            };
        }
    }
}
