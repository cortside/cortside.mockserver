using System;
using System.Net.Http;
using Cortside.MockServer.AccessControl;
using FluentAssertions;
using Xunit;

namespace Cortside.MockServer.Tests {
    public class AccessControlTest {
        private readonly HttpClient client;
        private readonly MockHttpServer server;

        public AccessControlTest() {
            server = MockHttpServer.CreateBuilder(Guid.NewGuid().ToString())
                .AddModule(new IdentityServerMock("./Data/discovery.json", "./Data/jwks.json"))
                .AddModule(new SubjectMock("./Data/subjects.json"))
                .Build();

            client = server.CreateClient();
        }

        [Fact]
        public void ShouldBeStarted() {
            //assert
            server.IsStarted.Should().BeTrue();
            client.BaseAddress.Should().Be(server.Url);
        }
    }
}
