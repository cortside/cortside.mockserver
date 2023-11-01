using System;
using System.Collections.Generic;
using Cortside.MockServer.Mocks.Models;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Cortside.MockServer.Mocks {
    public class CatalogMock : IMockHttpMock {
        private static readonly Random rnd = new Random();
        private readonly List<CatalogItem> catalog = new List<CatalogItem>();

        public CatalogMock() {
        }

        public CatalogMock(IList<CatalogItem> items) {
            catalog.AddRange(items);
        }

        public void Configure(WireMockServer server) {
            server
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

            var item = catalog.Find(x => x.ItemId == id);
            if (item != null) {
                return item;
            }

            return new CatalogItem() {
                ItemId = id,
                Name = $"Item {itemId}",
                Sku = itemId,
                UnitPrice = new decimal(rnd.Next(10000) / 100.0)
            };
        }
    }
}
