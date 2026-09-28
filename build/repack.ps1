$ErrorActionPreference = "Stop"

$root = Split-Path $PSScriptRoot -Parent
$name = "Mackmilan.CleanArchitecture.Solution.Template.0.0.0.nupkg"
$pkg  = Join-Path $root "artifacts\$name"

Set-Location $root

dotnet new uninstall Mackmilan.CleanArchitecture.Solution.Template 2>$null
nuget pack "CleanArchitecture.nuspec" -NoDefaultExcludes -OutputDirectory "artifacts"
dotnet new install $pkg --force
Remove-Item $pkg -Force 2>$null
