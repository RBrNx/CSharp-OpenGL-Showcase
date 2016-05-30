param($installPath, $toolsPath, $package, $project)
$projectFullName = $project.FullName
$debugString = "install.ps1 executing for " + $projectFullName
Write-Host $debugString
 
$fileInfo = new-object -typename System.IO.FileInfo -ArgumentList $projectFullName
$projectDirectory = $fileInfo.DirectoryName
$tempDirectory = "PencilTemp"
$sourceDirectory = "$projectDirectory\$tempDirectory"
Write-Host $sourceDirectory

if(test-path $sourceDirectory -pathtype container)
{
 $destinationDirectory = "$projectDirectory\..\bin\Debug"
 Write-Host $destinationDirectory

 Write-Host "Copying files from $sourceDirectory to $destinationDirectory"
 robocopy $sourceDirectory $destinationDirectory /XO /e
 
 $destinationDirectory = "$projectDirectory\..\bin\Release"
 Write-Host $destinationDirectory

 Write-Host "Copying files from $sourceDirectory to $destinationDirectory"
 robocopy $sourceDirectory $destinationDirectory /XO /e
 
 $destinationDirectory = "$projectDirectory\bin\Debug"
 Write-Host $destinationDirectory

 Write-Host "Copying files from $sourceDirectory to $destinationDirectory"
 robocopy $sourceDirectory $destinationDirectory /XO /e
 
 $destinationDirectory = "$projectDirectory\bin\Release"
 Write-Host $destinationDirectory

 Write-Host "Copying files from $sourceDirectory to $destinationDirectory"
 robocopy $sourceDirectory $destinationDirectory /XO /e
 
 Write-Host "Removing $tempDirectory from project."
 $tempDirectoryProjectItem = $project.ProjectItems.Item($tempDirectory)
 $tempDirectoryProjectItem.Remove()
 
 Write-Host "Deleting $sourceDirectory"
 rmdir $sourceDirectory /q /s
}
 
$debugString = "install.ps1 complete" + $projectFullName
Write-Host $debugString