using System;
using System.Net;
using System.Net.Http;
using Moq;
using Moq.Contrib.HttpClient;

namespace HealthyHands.Tests.Helpers;


public struct HttpRequest
{
    public HttpMethod Method{ get; set; }
    public string RequestUri { get; set; }
}

public struct HttpResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Response { get; set; }
}

public class MockHttpHelper
{
    private readonly string _baseAddress;

    public MockHttpHelper(string baseAddress)
    {
        _baseAddress = baseAddress;
    }

    public Mock<HttpMessageHandler> CreateMessageHandler(HttpRequest httpRequest, HttpResponse httpResponse)
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var requestUri = _baseAddress + httpRequest.RequestUri;
        
        if (httpResponse.Response == "")
        {
            mockHttpMessageHandler.SetupRequest(httpRequest.Method, requestUri)
                .ReturnsResponse(httpResponse.StatusCode);
        }
        else
        {
            mockHttpMessageHandler.SetupRequest(httpRequest.Method, requestUri)
                .ReturnsResponse(httpResponse.StatusCode, httpResponse.Response);
        }

        return mockHttpMessageHandler;
    }

    public HttpClient CreateClient(Mock<HttpMessageHandler> mockHttpMessageHandler)
    {
        var mockClient = mockHttpMessageHandler.CreateClient();
        mockClient.BaseAddress = new Uri(_baseAddress);

        return mockClient;
    }
}