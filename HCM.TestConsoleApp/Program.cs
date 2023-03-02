﻿using IdentityModel.Client;
using System.Net.Http.Headers;

Thread.Sleep(10000);

using var identityServerHttpClient = new HttpClient
{
    BaseAddress = new Uri(Environment.GetEnvironmentVariable("AUTHENTICATION__AUTHORITY")!)
};

var discoveryDocumentResponse = await identityServerHttpClient.GetDiscoveryDocumentAsync();

Console.WriteLine(discoveryDocumentResponse.TokenEndpoint);

var tokenResponse = await identityServerHttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discoveryDocumentResponse.TokenEndpoint,
    ClientId = Environment.GetEnvironmentVariable("AUTHENTICATION__CLIENTID")!,
    ClientSecret = Environment.GetEnvironmentVariable("AUTHENTICATION__CLIENTSECRET")!,
    Scope = "https://human-capital-management.com/api"
});

Console.WriteLine(tokenResponse.AccessToken);

using var httpClient = new HttpClient
{
    DefaultRequestHeaders =
    {
        Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken)
    }
};

Console.WriteLine(await httpClient.GetStringAsync("https://api:7001/WeatherForecast"));