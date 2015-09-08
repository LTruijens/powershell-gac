# Table View with FileVersion

This table view adds a FileVersion column to the default table view of assembly names

## EXAMPLES
```powershell
Get-GacAssembly | Format-Table -View FileVersion
```
This example returns all assemblies in the GAC and shows the default columns plus the FileVersion
