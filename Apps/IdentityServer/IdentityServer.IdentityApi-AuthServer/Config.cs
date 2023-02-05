// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace IdentityServer.IdentityApi_AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resource_api1"){Scopes={"api1.read","api1.write","api1.update"},ApiSecrets=new[]{ new Secret("secretapi1".Sha256())}},
                new ApiResource("resource_api2"){Scopes={"api2.read","api2.write","api2.update"},ApiSecrets=new[]{ new Secret("secretapi2".Sha256())}},
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
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
                new ApiScope("api2.update","Api 2 için update izni"),

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()//profile claims
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource(){Name="CountryAndCity",DisplayName="Country and City",Description="Kullanıcının ülke ve şehir bilgisi",
                    UserClaims=new [] {"country","city"}},
                new IdentityResource(){Name="Roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"} },
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
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.Email,
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
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.Email,
                        "api1.read","api2.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
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
                    ClientId="js-client",
                    ClientName="SPA Angular uygulaması",
                    RequireClientSecret=false,
                    RequirePkce=true,
                    AllowedGrantTypes=GrantTypes.Code,
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.Email,
                        "api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                    AccessTokenLifetime=2*60*60,
                    RedirectUris={"http://localhost:4200/callback"},
                    AllowedCorsOrigins={"http://localhost:4200"},
                    PostLogoutRedirectUris={ "http://localhost:4200" }

                },
                 new Client()
                {
                    ClientId="Client1-ResourceOwner-Mvc",
                    ClientName="Client1-ResourceOwner-Mvc MVC app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.Email,
                        "api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles",IdentityServerConstants.LocalApi.ScopeName},
                    AccessTokenLifetime=2*60*60,

                    AllowOfflineAccess=true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                },
            };
        }
    }
}