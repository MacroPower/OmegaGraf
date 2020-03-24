$base = '../branding/'

(Get-ChildItem -Path $base -Recurse -Include *.svg).FullName | ForEach-Object {
    $filePath = $_
    $tempFile = $filePath -replace '.svg','-tmp.svg'

    scour -i $filePath -o $tempFile --enable-viewboxing --enable-id-stripping --enable-comment-stripping --shorten-ids --indent=none

    Remove-Item -Path $filePath
    Move-Item -Path $tempFile -Destination $filePath
}

