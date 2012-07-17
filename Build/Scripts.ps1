properties {
    $Project = "AssertEx"
    $ScriptDir = Resolve-Path "."
    $BaseDir = Join-Path $ScriptDir ".."
    $SolutionFile = "$BaseDir\AssertExLib.sln"
    $OutputDir = "$BaseDir\temp\"
    $PackageDir = "$BaseDir\temp\package"
    $ArtifactsDir = "$BaseDir\artifacts"
    $BaseVersion = "0.0.1"
    $Version = "$BaseVersion" 
    $PatchNumber = (git rev-list --all | wc -l).trim() # TODO: better implementation
    $AssemblyVersion = "$BaseVersion.$PatchNumber"
}

function Log-Message ($message) {
    write-host $message -foregroundcolor "green"
}

function Set-Version {
    param([Parameter(Mandatory=$true)][string]$assembly_info,[Parameter(Mandatory=$true)][string]$Version)

    $infile=get-content $assembly_info
    $regex = New-Object System.Text.RegularExpressions.Regex "\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"
    $replace = $regex.Replace($infile,$version)
    set-content -Value $replace $assembly_info
}

function Create-IfNotFound {
    param([Parameter(Mandatory=$true)][string]$Directory)

    if ((Test-Path $Directory -PathType Container) -ne $true) {
        mkdir $Directory | out-null 
    }
}

function Copy-Item-Force {
    param([Parameter(Mandatory=$true)][string]$from,
          [Parameter(Mandatory=$true)][string]$to)

          New-Item -type File -Force $to 
          Copy-Item -Force $from $to
}

function Clear-Recursively {
    param([Parameter(Mandatory=$true)][string]$Directory)

    if (Test-Path $Directory -PathType Container) {
        ri $Directory -Recurse | out-null 
    }
}

function Output-Library {
    param([Parameter(Mandatory=$true)][string]$library)

    # add portable assembly to multiple paths
    $folders = "net40", "net45", "sl50", "winrt45"
    ForEach ($folder in $folders) {

        $newFolder = "$PackageDir\lib\$folder"

        if (-not (Test-Path $newFolder)) {
            New-Item -Type directory "$newFolder" | out-null 
        }
        
        Copy-Item $library $newFolder | out-null 
    }
}

function Add-NuSpec {
    param([Parameter(Mandatory=$true)][string]$PackageNameFile)

    $nuspecFile = "$BaseDir\build\$PackageNameFile"
    cp $nuspecFile $PackageDir

    Log-Message "Modifying placeholder in nuspec file"
    $Spec = [xml](get-content "$PackageDir\$PackageNameFile")
    $Spec.package.metadata.version = ([string]$Spec.package.metadata.version).Replace("{version}",$Version)
    $Spec.Save("$PackageDir\$PackageNameFile")
}

function Create-Package {
    param([Parameter(Mandatory=$true)][string]$PackageNameFile)

    Log-Message "Creating package using nuget.exe"
    exec { ..\.nuget\NuGet.exe pack "$PackageDir\$PackageNameFile" -Output $ArtifactsDir }
}

task default -depends test

task clean {
    Log-Message "Clearing tracked and untracked files"
    git checkout -- $BaseDir
    git clean -df $BaseDir
    Clear-Recursively $OutputDir
    Clear-Recursively $ArtifactsDir
}

task version -depends clean {
    Log-Message "Updating version to $version"
    foreach ($assemblyinfo in $(get-childitem -Path $BaseDir -Include "AssemblyInfo.cs" -recurse)) {
        Set-Version -assembly_info "$assemblyinfo" -Version $AssemblyVersion
    }
}   

task compile -depends clean,version  {
    Log-Message "Building solution"
    msbuild $SolutionFile "/p:OutDir=$OutputDir" "/p:Configuration=Release"  "/verbosity:quiet"
}

task test -depends compile  {
   exec { ..\Tools\xunit\xunit.console.clr4.x86.exe "$OutputDir\UnitTests.dll" }
}

task package -depends test {

    Create-IfNotFound $PackageDir
    Create-IfNotFound $ArtifactsDir

    Output-Library "$OutputDir\AssertExLib.dll"

    $PackageNameFile = "AssertEx.nuspec"

    Add-NuSpec $PackageNameFile
    Create-Package $PackageNameFile
    
    Clear-Recursively $PackageDir
}

task create-release -depends package {
    Log-Message "Clearing tracked files"
    git checkout -- $BaseDir
}