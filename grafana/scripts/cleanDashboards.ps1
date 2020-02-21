function Format-Json(
    [Parameter(Mandatory, ValueFromPipeline)][String] $json
) {
    $indent = 0;
    ($json -Split "`n" | ForEach-Object {
            if ($_ -match '[\}\]]\s*,?\s*$') {
                # This line ends with ] or }, decrement the indentation level
                $indent--
            }
            $line = ('  ' * $indent) + $($_.TrimStart() -replace '":  (["{[])', '": $1' -replace ':  ', ': ')
            if ($_ -match '[\{\[]\s*$') {
                # This line ends with [ or {, increment the indentation level
                $indent++
            }
            $line
        }) -Join "`n"
}

$base = '../dashboards/'

(Get-ChildItem -Path $base).Name | ForEach-Object {
    $filePath = ($base + $_)
    $file = Get-Content -Path $filePath -Raw
    $db = $file | ConvertFrom-Json

    if (-not $db.dashboard) {
        $db.timezone = ""
        $db.time.from = "now-1h"
        $db.time.to = "now"
        $db.style = "dark"
        $db.id = $null
        $db.PSObject.properties.remove('__inputs')
        $db.PSObject.properties.remove('__requires')

        $db.panels | ForEach-Object {
            $_.datasource = $null
        }

        $newFile = @{
            'dashboard' = $db
        }

        $json = $newFile | ConvertTo-Json -Depth 10 

        Format-Json -json $json | Out-File -FilePath $filePath
    }
}
