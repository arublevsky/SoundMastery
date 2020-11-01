[CmdletBinding()]
Param(
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Set-Location -Path "src/server/SoundMastery/SoundMastery.Migration"
& dotnet run $BuildArguments -WorkingDirectory 
Set-Location -Path "./../../../../"