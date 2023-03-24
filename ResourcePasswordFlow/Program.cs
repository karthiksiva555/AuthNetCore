using System.Text.Json.Serialization;
using RestSharp;

// set up variables 
const string identityAuthority = "https://geotab-test.eu.auth0.com";
const string identityClientId = "0NYTn9dl8T0pMrzvdp6rQIwaxjc5gflD";
const string identityClientSecret = "3gjjVkvyhyHP43c6fce0ouNPDTPzxmpIicRjF9fpneOrbd9hKBnA3Z7dLlAbfNGk";
const string managementApiLink = identityAuthority + "/api/v2/";
const string userName = "authuser1@gmail.com";
const string password = "Testuser@123";

var token = await GetTokenAsync(userName, password);
if (!string.IsNullOrEmpty(token))
{
    var userId = await GetUserIdFromToken(token);
    Console.WriteLine(userId);
}

// Get token 
async Task<string?> GetTokenAsync(string username, string password)
{
    var identityClient = new RestClient(identityAuthority);
    var request = new RestRequest("oauth/token", Method.Post);
    request.AddHeader("content-type", "application/json");
    request.AddParameter("application/json", "{\"client_id\":\""+ identityClientId + "\",\"client_secret\":\"" + identityClientSecret + "\",\"audience\":\""+ managementApiLink + "\"," +
                                             "\"grant_type\":\"password\", \"username\": \""+ username+"\", \"password\":\""+ password + "\", \"scope\": \"openid\"}", ParameterType.RequestBody);
    var response = await identityClient.PostAsync<IdentityTokenResponse>(request);
    return response!.AccessToken;
}

// Get user Id from userinfo endpoint
async Task<string?> GetUserIdFromToken(string accessToken)
{
    var identityClient = new RestClient(identityAuthority);
    var request = new RestRequest("/userinfo", Method.Post);
    request.AddHeader("content-type", "application/json");
    request.AddHeader("Authorization", "Bearer "+ accessToken);
    var response = await identityClient.GetAsync<UserInfoResponse>(request);
    return response!.UserId;
}

record IdentityTokenResponse {
        
    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }
        
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }
    
    [JsonPropertyName("id_token")]
    public string? IdToken { get; init; }
}

record UserInfoResponse
{
    [JsonPropertyName("sub")]
    public string? UserId { get; init; }
        
    [JsonPropertyName("email")]
    public string? UserName { get; init; }
}