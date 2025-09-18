using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using NUnit.Framework;
using FakeStoreApp.Models;
using FakeStoreApp.Controllers;

namespace FakeStoreApp;

[TestFixture]
public class ProductRepoServiceTests
{

    private ProductsRepoService _service;
    private Mock<IMemoryCache> _memoryCacheMock;
    private Mock<ILogger<ProductsRepoService>> _loggerMock;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _loggerMock = new Mock<ILogger<ProductsRepoService>>();

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var MockProducts = new List<ProductModel>
        {
            new ProductModel
            {
                Id = 1,
                Title = "Mock Title",
                Description = "Mock Description",
                Category = "Mock Category",
                Price = 0.00m
            }
        };

        handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(MockProducts)
                });

        _httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("")
        };

        object dummy;
        _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out dummy!))
            .Returns(false);

        _service = new ProductsRepoService(_httpClient, _loggerMock.Object, _memoryCacheMock.Object);
    }

    [Test]
    public async Task GetAllProductsAsnycTest()
    {
        var products = await _service.GetAllProductsAsync();

        
        Assert.Pass();
    }
}
