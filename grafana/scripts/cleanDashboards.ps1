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

        $json = $db | ConvertTo-Json -Depth 10 

        $json | Out-File -FilePath $filePath
    }
}

prettier $base* --write