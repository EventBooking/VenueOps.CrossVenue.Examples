$env:CvcAwsBucket = 'cvc.sample.data'
$env:CvcCluster = 'indv'
$env:CvcRegion = 'us-east-1'
$env:CvcTenantId = 'account-3421-A'
$env:CvcAwsAccessKey = 'AKIA4M4F7WRHXJKGQYVK'

$env:CvcDir = ''
$env:CvcAwsSecretKey = 'SECRET KEY SENT SEPARATELY'

PUSHD src
PUSHD CVC.Reader
dotnet run
POPD
POPD
