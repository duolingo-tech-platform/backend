# Gera o zip para upload no AWS Elastic Beanstalk
$zipPath = "..\deploy-backend.zip"
if (Test-Path $zipPath) { Remove-Item $zipPath }
Compress-Archive -Path "Dockerfile", ".dockerignore", "src" -DestinationPath $zipPath
Write-Host "Criado: deploy-backend.zip em $(Resolve-Path $zipPath)"
