namespace Cortside.MockServer {
    public interface IMockHttpServerBuilder {
        MockHttpServer Build();

        MockHttpServerOptions Options { get; }

        public IMockHttpServerBuilder AddMock<T>() where T : IMockHttpMock, new();

        public IMockHttpServerBuilder AddMock<T>(T instance) where T : IMockHttpMock;
    }
}
