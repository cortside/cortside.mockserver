using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Cortside.MockServer.Tests {
    public class ReadStaticMappingsTest {
        private readonly HttpClient client;
        private readonly MockHttpServer server;

        public ReadStaticMappingsTest() {
            server = MockHttpServer.CreateBuilder(Guid.NewGuid().ToString())
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
        public async Task ShouldMatchStaticMapping() {
            //arrange
            var uri = new Uri(client.BaseAddress + "WsIdentity/VerificationOfOccupancy");
            var body = "User.ReferenceCode=appId&User.GLBPurpose=1&User.DLPurpose=3&User.MaxWaitSeconds=30&User.MaxWaitSecondsSpecified=1&Options.AttributesVersionRequest=VOOATTRV1&Options.IncludeModel=1&Options.IncludeModelSpecified=1&Options.IncludeReportSpecified=0&SearchBy.Name.First=Chayanne&SearchBy.Name.Last=Sullivan&SearchBy.Address.StreetAddress1=31%20HENRY%20WAY%20ST&SearchBy.Address.City=Hawkinsville&SearchBy.Address.State=GA&SearchBy.Address.Zip5=31036&SearchBy.SSN=566637173";
            var bodyContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            //act
            var response = await client.PostAsync(uri, bodyContent);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("VerificationOfOccupancyResponseEx");
        }
    }
}
