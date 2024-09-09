namespace Cortside.MockServer.Builder {
    public interface IMockHttpServerBuilder {
        MockHttpServer Build();

        MockHttpServerOptions Options { get; }

        public IMockHttpServerBuilder AddMock<T>() where T : IMockHttpMock, new();

        public IMockHttpServerBuilder AddMock<T>(T instance) where T : IMockHttpMock;
    }
}
