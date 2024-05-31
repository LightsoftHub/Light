$packageIdList = @('package_name_1','package_name_2')

$apiKey = "<your_api_key_with_unlist_permission>"

foreach ($packageId in $packageIdList) {

  $json = Invoke-WebRequest -Uri "https://api.nuget.org/v3-flatcontainer/$PackageId/index.json" | ConvertFrom-Json
	
  foreach($version in $json.versions) {
	Write-Host "Unlisting $packageId, Ver $version"
	dotnet nuget delete $packageId $version --source https://api.nuget.org/v3/index.json --non-interactive --api-key $apiKey
  }
}