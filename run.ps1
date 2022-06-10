[cmdletBinding()]
Param()

Push-Location "$PSScriptRoot/src/Cortside.WireMock.WebApi"

cmd /c start cmd /k "title Cortside.WireMock.WebApi & dotnet run"

Pop-Location
