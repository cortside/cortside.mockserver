using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cortside.MockServer.Tests.Mocks;
using Cortside.MockServer.Tests.Mocks.Models;
using FluentAssertions;
using Xunit;

namespace Cortside.MockServer.Tests {
    public class CatalogTest {
        private readonly HttpClient client;
        private readonly MockHttpServer server;

        public CatalogTest() {
            var items = new List<CatalogItem>() {
                new CatalogItem() {
                    ItemId = Guid.Parse("d6ab90f2-bf58-4919-a118-07b08f80f7cf"),
                    Sku = "PAPPY-25",
                    Name = "Pappy Van Winkle 25 Year",
                    UnitPrice = 59999.99M,
                    ImageUrl = "https://www.oldripvanwinkle.com/app/uploads/2016/09/bourbon-bg-10year-2.jpg"
                }
            };
            var mock = new CatalogMock(items);
            server = MockHttpServer.CreateBuilder(Guid.NewGuid().ToString())
                .AddMock(mock)
                .Build();

            client = server.CreateClient();
        }

        [Fact]
        public void ShouldBeStarted() {
            //assert
            server.IsStarted.Should().BeTrue();
            client.BaseAddress.Should().Be(server.Url);
        }

        [Fact]
        public async Task ShouldGetItemAsync() {
            //arrange
            const string itemId = "d6ab90f2-bf58-4919-a118-07b08f80f7cf";
            const string url = $"/api/v1/items/{itemId}";

            //act
            var response = await client.GetAsync(url);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain(itemId);
            content.Should().Contain("PAPPY-25");
        }
    }
}
