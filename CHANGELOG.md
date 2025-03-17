# Release 8.0

* Updated powershell scripts to latest versions from coeus/shoppingcart-api
* Standardized library build files and resolved code coverage issues
* Update target framework to net8.0
* Update all dependency nuget packages
* Add/Fix build badges
* Transition to use Shouldly instead of FluentAssertions
* Add support for mocking of Cortside.Authorization policies

|Commit|Date|Author|Message|
|---|---|---|---|
| 8c7d39a | <span style="white-space:nowrap;">2024-09-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 0979862 | <span style="white-space:nowrap;">2024-09-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 35c29bb | <span style="white-space:nowrap;">2024-12-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add badges to README.md
| 6deaabc | <span style="white-space:nowrap;">2025-01-22</span> | <span style="white-space:nowrap;">=</span> |  authorization-api
| 49c3117 | <span style="white-space:nowrap;">2025-01-22</span> | <span style="white-space:nowrap;">=</span> |  more clearly fake token
| 0fa7f1a | <span style="white-space:nowrap;">2025-01-22</span> | <span style="white-space:nowrap;">=</span> |  (origin/feature/ENG-4482-authorization) fake tokens & coverlet change
| 4bb21bd | <span style="white-space:nowrap;">2025-01-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #14 from cortside/feature/ENG-4482-authorization
| eed5229 | <span style="white-space:nowrap;">2025-02-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  cleanup
| ac270b7 | <span style="white-space:nowrap;">2025-02-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'develop' of github.com:cortside/cortside.mockserver into develop
| 559934e | <span style="white-space:nowrap;">2025-02-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to net8
| d17d1f5 | <span style="white-space:nowrap;">2025-03-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/8.0, origin/develop, develop) Use Shouldly instead of FluentAssertions because of new licensing; update nuget packages;
****

# Release 6.2

* Update nuget dependencies to latest stable versions
* Add support for child policies in SubjectMock


|Commit|Date|Author|Message|
|---|---|---|---|
| d73f2df | <span style="white-space:nowrap;">2023-11-08</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 8c6cfd5 | <span style="white-space:nowrap;">2023-11-08</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| e1f880e | <span style="white-space:nowrap;">2024-08-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to latest nuget packages; update scripts
| 5f99b62 | <span style="white-space:nowrap;">2024-08-27</span> | <span style="white-space:nowrap;">troychorton</span> |  [add-child-policies-to-subject-mock] added child policies to subject mock
| ef274ce | <span style="white-space:nowrap;">2024-08-27</span> | <span style="white-space:nowrap;">troychorton</span> |  [add-child-policies-to-subject-mock] update nuget apikey
| 06cc84f | <span style="white-space:nowrap;">2024-08-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #12 from cortside/feature/add-child-policies-to-subject-mock
| 8080be8 | <span style="white-space:nowrap;">2024-09-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/6.2, origin/develop, develop) update scripts; update nuget packages;
****

# Release 6.1

* Update nuget dependencies to latest stable versions
* Introduced new MockHttpServerBuilder to make setting up the server a little easier and more fluent
	```csharp
	server = MockHttpServer.CreateBuilder(Guid.NewGuid().ToString())
		.AddMock(new IdentityServerMock("./Data/discovery.json", "./Data/jwks.json"))
		.AddMock(new SubjectMock("./Data/subjects.json"))
		.Build();
	```
	
	This replaces the previous method like this:
	```csharp
	// OLD style that has been replaced
	server = new MockHttpServer(name)
		.ConfigureBuilder(new SubjectMock("./Data/subjects.json"))
		.ConfigureBuilder<TestMock>()
		.ConfigureBuilder<DelegationGrantMock>();

	server.WaitForStart();	
	```
* Added MockHttpServer method CreateClient that returns a preconfigured HttpClient instance
	```csharp
	client = server.CreateClient();
	```
* Add an extension method to simplify creating a stand alone server
	```csharp
	public static class Program {
		public static Task Main(string[] args) {
			// setup global default json serializer settings
			JsonConvert.DefaultSettings = JsonNetUtility.GlobalDefaultSettings;

			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, false)
				.AddJsonFile("appsettings.local.json", true, false)
				.AddJsonFile("build.json", false, false)
				.Build();

			var options = configuration.GetSection("MockHttpServerOptions").Get<MockHttpServerOptions>();
			var build = configuration.GetSection("Build").Get<BuildModel>();
			var connectionString = configuration["Database:ConnectionString"];

			var server = MockHttpServer.CreateBuilder(options)
				.AddMock<CommonMock>()
				.AddMock(new IdentityServerMock("./Data/discovery.json", "./Data/jwks.json"))
				.AddMock(new SubjectMock("./Data/subjects.json"))
				.AddMock(new CatalogMock("./Data/items.json"))
				.Build();

			return server.WaitForCancelKeyPressAsync();
		}
	}
	```

	with following section in appsettings.json:
	```json
	"MockHttpServerOptions": {
		"Port": 5001,
		"CorsPolicyOptions": "AllowAll",
		"StartAdminInterface": false,
		"AllowCSharpCodeMatcher": false,
		"ReadStaticMappings": false
	}
	```
* Change interface needed for mock classes from IMockHttpServerBuilder to IMockHttpMock
* Removed use of Serilog in favor of solely relying on Microsoft.Extensions.Logging
	
	
|Commit|Date|Author|Message|
|---|---|---|---|
| a974867 | <span style="white-space:nowrap;">2023-08-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| b1c11ca | <span style="white-space:nowrap;">2023-09-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| a033176 | <span style="white-space:nowrap;">2023-10-31</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-9] add MockHttpServerBuilder and cleanup configuration; add test project with some initial tests
| 0532ee2 | <span style="white-space:nowrap;">2023-11-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-9] add logging and ability to get to Logger and WireMock server
| cda9c06 | <span style="white-space:nowrap;">2023-11-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-9] cleanup and moving catalog mock back to tests
| 2f51bd3 | <span style="white-space:nowrap;">2023-11-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-9] rename AddModule to AddMock
| 23a0954 | <span style="white-space:nowrap;">2023-11-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-9] add ability to use options object in builder
| 0495917 | <span style="white-space:nowrap;">2023-11-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-9] better organize namespace
| d9e8858 | <span style="white-space:nowrap;">2023-11-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to jdk17 for sonar
| 5573c29 | <span style="white-space:nowrap;">2023-11-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/ISSUE-9, ISSUE-9) update to jdk17+ for sonar
| 0fb9c96 | <span style="white-space:nowrap;">2023-11-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #10 from cortside/ISSUE-9
| 671992d | <span style="white-space:nowrap;">2023-11-08</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/6.1, origin/develop, develop) update to latest nuget packages
****

# Release 6.0

* Update version number to match framework version (6.x)
* Update projects to be net6.0
* Update nuget dependencies to latest stable versions

|Commit|Date|Author|Message|
|---|---|---|---|
| 3b76172 | <span style="white-space:nowrap;">2023-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 8c37e6a | <span style="white-space:nowrap;">2023-06-20</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 6364fcb | <span style="white-space:nowrap;">2023-06-26</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [net6] update to net6
| 2d80f69 | <span style="white-space:nowrap;">2023-06-26</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/net6, net6) [net6] use vs2022 builder image
| 74ed0a4 | <span style="white-space:nowrap;">2023-06-26</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #7 from cortside/net6
| f185e41 | <span style="white-space:nowrap;">2023-07-17</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version to 6.x to be in line with dotnet and net6 version numbers
| ee20347 | <span style="white-space:nowrap;">2023-08-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/6.0, origin/develop, develop) update to latest nuget packages
****

# Release 1.2

|Commit|Date|Author|Message|
|---|---|---|---|
| b1b05cc | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 1c9bf6e | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 7f5f2e8 | <span style="white-space:nowrap;">2023-03-22</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  make ids connect/token endpoint more specific to client_credentials flow, allowing for other flows to be added to the server
| 1697b1b | <span style="white-space:nowrap;">2023-04-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add health check endpoint mocks
| 572cdb3 | <span style="white-space:nowrap;">2023-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/1.2, origin/develop, develop) update powershell scripts
****

# Release 1.1
|Commit|Date|Author|Message|
|---|---|---|---|
| efb28c4 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20220713] updated nuget packages
| 6872da4 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| 6c189c5 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #3 from cortside/feature/BOT-20220713
| bd4fc13 | <span style="white-space:nowrap;">2022-08-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  make route matching a little more selecting before assuming authorization header has base64 encoded value
| 143b765 | <span style="white-space:nowrap;">2022-12-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget api key
| 5bbb5fc | <span style="white-space:nowrap;">2022-12-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'develop' of github.com:cortside/cortside.mockserver into develop
| 2c86eb6 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20230102] updated nuget packages
| 8250405 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/feature/BOT-20230102, feature/BOT-20230102) update version in prep for next release; update helper scripts
| 30a4ea4 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #4 from cortside/feature/BOT-20230102
| e8a1783 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/1.1, origin/develop, develop) initial changelog
****
# Release 1.0
|Commit|Date|Author|Message|
|---|---|---|---|
| 0e066f1 | <span style="white-space:nowrap;">2022-06-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (master) Initial commit
| 9a68556 | <span style="white-space:nowrap;">2022-06-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  initial commit extracted from coeus
| 6f24c93 | <span style="white-space:nowrap;">2022-06-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  set version to 1.0
| 3d5fd18 | <span style="white-space:nowrap;">2022-06-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages
| 657befb | <span style="white-space:nowrap;">2022-06-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to latest wiremock
| 9948e64 | <span style="white-space:nowrap;">2022-06-29</span> | <span style="white-space:nowrap;">ruifang</span> |  add content type
| ffbd877 | <span style="white-space:nowrap;">2022-07-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #1 from cortside/feature/template
| 6fb7335 | <span style="white-space:nowrap;">2022-07-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/master, origin/HEAD) Merge pull request #2 from cortside/develop
| efb28c4 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20220713] updated nuget packages
| 6872da4 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| 6c189c5 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #3 from cortside/feature/BOT-20220713
| bd4fc13 | <span style="white-space:nowrap;">2022-08-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  make route matching a little more selecting before assuming authorization header has base64 encoded value
| 143b765 | <span style="white-space:nowrap;">2022-12-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget api key
| 5bbb5fc | <span style="white-space:nowrap;">2022-12-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'develop' of github.com:cortside/cortside.mockserver into develop
| 2c86eb6 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20230102] updated nuget packages
| 8250405 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/feature/BOT-20230102, feature/BOT-20230102) update version in prep for next release; update helper scripts
| 30a4ea4 | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> develop, origin/develop) Merge pull request #4 from cortside/feature/BOT-20230102
****
