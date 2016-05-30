param($installPath, $toolsPath, $package, $project)

$projectFullName = $project.FullName
$debugString = "uninstall.ps1 executing for " + $projectFullName
Write-Host $debugString
 
$fileInfo = new-object -typename System.IO.FileInfo -ArgumentList $projectFullName
$projectDirectory = $fileInfo.DirectoryName
 
if(test-path $sourceDirectory -pathtype container)
{
 $destinationDirectory = "$projectDirectory\..\bin\Debug\natives32"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\..\bin\Debug\natives64"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\..\bin\Release\natives32"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\..\bin\Release\natives64"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\bin\Debug\natives32"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\bin\Debug\natives64"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\bin\Release\natives32"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
 
 $destinationDirectory = "$projectDirectory\bin\Release\natives64"
 Write-Host $destinationDirectory

 Write-Host "Removing dependency files from $destinationDirectory"
 rmdir $destinationDirectory /s /q
}
 
$debugString = "uninstall.ps1 complete" + $projectFullName
Write-Host $debugString