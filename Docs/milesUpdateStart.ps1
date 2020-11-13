Set-Variable host -option Constant -value "localhost"
Set-Variable port -option Constant -value 1746
Set-Variable action -option Constant -value "/api"

$Logfile = "C:\InternalAPI\api.log"

Function LogWrite
{
   Param ([string]$logstring)

   Add-content $Logfile -value $logstring
}

$url = "https://" + $host + ":" + $port + $action

Invoke-RestMethod -Method 'Get' -Uri $url