$name = "Lightsoft"
$packageFolders = Get-ChildItem -Path "src" -Directory | Select-Object -ExpandProperty Name

$apiKey = "<your_api_key_with_unlist_permission>"

foreach ($folder in $packageFolders) {

  $packageId = "$name.$folder"
  
  $json = Invoke-WebRequest -Uri "https://api.nuget.org/v3-flatcontainer/$PackageId/index.json" | ConvertFrom-Json
	
  foreach($version in $json.versions) {
	Write-Host "Unlisting $packageId, Ver $version"
	dotnet nuget delete $packageId $version --source https://api.nuget.org/v3/index.json --non-interactive --api-key $apiKey
  }
}