using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Cortside.MockServer.AccessControl;
using FluentAssertions;
using Xunit;

namespace Cortside.MockServer.Tests {
    public class AccessControlTest {
        private readonly HttpClient client;
        private readonly MockHttpServer server;

        public AccessControlTest() {
            server = MockHttpServer.CreateBuilder(Guid.NewGuid().ToString())
                .AddMock(new IdentityServerMock("./Data/discovery.json", "./Data/jwks.json"))
                .AddMock(new SubjectMock("./Data/subjects.json"))
                .Build();

            client = server.CreateClient();
        }

        [Fact]
        public void ShouldBeStarted() {
            //assert
            server.IsStarted.Should().BeTrue();
            client.BaseAddress.Should().Be(server.Url);
        }

        [Theory]
        [InlineData("ShoppingCart", "GetOrders")]
        [InlineData("SqlReport", "CanGetReports")]
        public async Task ShouldHaveTwoPoliciesAsync(string policy, string permission) {
            //arrange
            const string subjectId = "132953b2-f6a7-4c1d-8da1-2b3c3dafe1c5";
            var url = $"runtime/policy/{policy}?version=1";

            var uri = new Uri(client.BaseAddress + url);
            var body = subjectId;
            var bodyContent = new StringContent(body, Encoding.UTF8, "application/json");

            //act
            var response = await client.PostAsync(uri, bodyContent);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain(permission);
        }

        [Fact]
        public async Task ShouldSupportAuthorizationApiAsync() {
            //arrange
            const string subjectId = "222953b2-f6a7-4c1d-8da1-2b3c3da55555";
            var policyId = "bfa68daf-cd0d-4b76-b43b-a2bb9183ca17";
            var url = $"api/v1/policies/{policyId}/evaluate";

            var uri = new Uri(client.BaseAddress + url);
            var body = subjectId;

            //act
            var response = await client.PostAsJsonAsync(uri, body);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("AdjustOrder");
        }
    }
}
