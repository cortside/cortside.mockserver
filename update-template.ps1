# make sure latest version of cortside.templates is installed
dotnet new --install cortside.templates

# update powershell scripts from root
dotnet new cortside-api-powershell --force --output ./ --name Cortside.WireMock --company Cortside --product WireMock

# update .editorconfig
dotnet new cortside-api-editorconfig --force --output ./ --name Cortside.WireMock --company Cortside --product WireMock

# update dockerfile and supporting shell and powershell scripts
if (Test-Path -path "deploy") {
	dotnet new cortside-api-deploy --force --output ./ --name Cortside.WireMock --company Cortside --product WireMock
}
