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
            };
        }

        public static IEnumerable<TestUser> GetTestUsers()//Test kullanıcıları
        {
            return new List<TestUser>()
            {
                new TestUser{SubjectId="1",Username="Elbatra",Password="password",Claims=new List<Claim>()
                    {
                        new Claim("given_name","Semih"),
                        new Claim("family_name","Daghan")
                    }
                },
                new TestUser{SubjectId="2",Username="Eldodi",Password="password",Claims=new List<Claim>()
                    {
                        new Claim("given_name","Sea of"),
                        new Claim("family_name","Thieves")
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
                }
            };
        }
    }
}
