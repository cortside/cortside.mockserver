using System;
using System.Collections.Generic;
using System.IO;
using Cortside.AspNetCore.Common.Paging;
using Cortside.MockServer.Tests.Mocks.Models;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Cortside.MockServer.Tests.Mocks {
    public class CatalogMock : IMockHttpMock {
        private static readonly Random rnd = new Random();
        private readonly PagedList<CatalogItem> catalog = new PagedList<CatalogItem>() { Items = new List<CatalogItem>() };

        public CatalogMock() {
        }

        public CatalogMock(string filename) {
            var items = JsonConvert.DeserializeObject<List<CatalogItem>>(File.ReadAllText(filename));
            catalog.Items.AddRange(items);
            catalog.PageNumber = 1;
            catalog.PageSize = catalog.Items.Count;
            catalog.TotalItems = catalog.Items.Count;
        }

        public CatalogMock(IList<CatalogItem> items) {
            catalog.Items.AddRange(items);
            catalog.PageNumber = 1;
            catalog.PageSize = catalog.Items.Count;
            catalog.TotalItems = catalog.Items.Count;
        }

        public void Configure(MockHttpServer server) {
            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/api/v1/items")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_ => JsonConvert.SerializeObject(catalog))
                );

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/api/v1/items/*")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(r => JsonConvert.SerializeObject(GetItem(r.PathSegments[3])))
                );
        }

        private CatalogItem GetItem(string itemId) {
            if (!Guid.TryParse(itemId, out var id)) {
                id = Guid.NewGuid();
            }

            var item = catalog.Items.Find(x => x.ItemId == id);
            if (item != null) {
                return item;
            }

            return new CatalogItem() {
                ItemId = id,
                Name = $"Item {itemId}",
                Sku = itemId,
                UnitPrice = new decimal(rnd.Next(10000) / 100.0),
                ImageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={itemId}"
            };
        }
    }
}
