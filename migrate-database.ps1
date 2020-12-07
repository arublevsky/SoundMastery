[CmdletBinding()]
Param(
    [Parameter(Position=0,Mandatory=$true,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Set-Location -Path "src/server/SoundMastery/SoundMastery.Migration"
& dotnet restore
& dotnet build
& dotnet run $BuildArguments
Set-Location -Path "./../../../../"