namespace Cortside.MockServer {
    public interface IMockHttpServerBuilder {
        MockHttpServer Build();

        MockHttpServerOptions Options { get; }

        public IMockHttpServerBuilder AddModule<T>() where T : IMockHttpMock, new();

        public IMockHttpServerBuilder AddModule<T>(T instance) where T : IMockHttpMock;
    }
}
