$ScriptDir = Split-Path -Path $MyInvocation.MyCommand.Path -Parent

remove-module [p]sake
import-module "$ScriptDir\Tools\psake\psake.psm1"
invoke-psake "$ScriptDir\Build\Scripts.ps1" create-release