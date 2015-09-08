# Installation instructions

## PowerShell 5 and Windows 10

Install from [PowerShell Gallery](https://www.powershellgallery.com/packages/Gac)
```powershell
Install-Module Gac
```

## Chocolatey

Install from [Chocolatey](https://chocolatey.org/packages/PowerShellGAC)
```powershell
choco install powershellgac
```

## Otherwise

1. Download PowerShell GAC
2. Right click the zip file and choose Properties. Click Unblock and Ok. Or use PowerShell to unblock with [Unblock-File](https://technet.microsoft.com/en-us/library/hh849924.aspx). 
  ![example of properties](http://blogs.technet.com/resized-image.ashx/__size/550x0/__key/communityserver-blogs-components-weblogfiles/00-00-00-76-18/6813.HSG_2D00_7_2D00_18_2D00_11_2D00_02.png)
3. Extract zip file in "My Documents\WindowsPowerShell\Modules" so the files are in "My Documents\WindowsPowerShell\Modules\Gac".
4. Open PowerShell verify the module can be found with `Get-Module -ListAvailable`.
   Make sure the PowerShell ExecutionPolicy is set to RemoteSigned or lower or the module fails to load. This can be done by calling `Set-ExecutionPolicy RemoteSigned`
5. Use `Import-Module Gac` to import the module in PowerShell 2.0 
   This is not needed in PowerShell 3.0, since it auto loads modules it can find.

Change `$env:PSModulePath` when installing in a different path or for all users. Using the complete path when calling `Import-Module` also works.
