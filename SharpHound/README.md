# SharpHound
C# Data Collector for the BloodHound Project.

### General Information
| Project Source | Our Version | DotNet Version |
|:-:|:-:|:-:|
| https://github.com/BloodHoundAD/SharpHound | 1.0.5 | 4.6.2 |

### Edit & Update Instructions
For updates to the new version, the following changes must be made in the new version.
* Remove BloodHound keyword from ZIP filename
    * File Name: OutputWriter.cs
    * Line: 145
    * ```var filename = string.IsNullOrEmpty(_context.ZipFilename) ? "Enum" : _context.ZipFilename;```
* Change ZIP extension to bin
    * File Name: BaseContext.cs
    * Line: 105
    * ```if (extension is "json" or "zip" or "bin" && Flags.RandomizeFilenames)```
* Getting the ZIP Filename
    * File Name: OutputWriter.cs
    * Line: 187
    * ```_context.Logger.LogInformation("PARSE_ZIP_FILE:{finalPath}", resolvedFileName);```
* Getting the CACHE Filename
    * File Name: Sharphound.cs
    * Line: 179
    * ```context.Logger.LogInformation("PARSE_CACHE_FILE:{Path}", path);```
* Remove application icon from Project Properties.

### Compile & Debug Instructions
* SharpHound uses multiple third party libraries, so Nuget needs to be set up on MSVC first. For this, the steps in the link should be followed: https://docs.microsoft.com/en-us/nuget/api/overview#service-index
* It works successfully in Debug mode. No extra settings are needed for debugging. In addition, the output can be detailed with the Verbose (v) parameter. The verbose mode default value is 2 and lower is means more verbose.

### Execution Notes
* If no parameter/value is given for output files in any way; `PARSE_ZIP_FILE` and `CACHE_ZIP_FILE` parameters will start with `.\` in the output. Therefore, the `--outputdirectory` parameter should be used and given an appropriate value.

### Sample Output
```
$ SharpHound.exe --outputdirectory C:\Users\Public\

2022-06-27T11:14:34.2569538-07:00|INFORMATION|Resolved Collection Methods: Group, LocalAdmin, Session, Trusts, ACL, Container, RDP, ObjectProps, DCOM, SPNTargets, PSRemote
2022-06-27T11:14:34.2629562-07:00|INFORMATION|Initializing SharpHound at 11:14 AM on 6/27/2022
2022-06-27T11:14:34.4599563-07:00|INFORMATION|PARSE_CACHE_FILE:C:\Users\Public\MzRlYWUyYjktYWNkNS00ODM0LTllNTgtN2IzZjcwZWQwYzIy.bin
2022-06-27T11:14:34.4889578-07:00|INFORMATION|Flags: Group, LocalAdmin, Session, Trusts, ACL, Container, RDP, ObjectProps, DCOM, SPNTargets, PSRemote
2022-06-27T11:14:34.7069587-07:00|INFORMATION|Beginning LDAP search for 
2022-06-27T11:14:34.7409580-07:00|INFORMATION|Producer has finished, closing LDAP channel
2022-06-27T11:14:34.7429536-07:00|INFORMATION|LDAP channel closed, waiting for consumers
2022-06-27T11:15:04.8144875-07:00|INFORMATION|Status: 0 objects finished (+0 0)/s -- Using 35 MB RAM
2022-06-27T11:15:34.8104892-07:00|INFORMATION|Status: 119 objects finished (+119 1.983333)/s -- Using 42 MB RAM
2022-06-27T11:15:36.6025274-07:00|INFORMATION|Consumers finished, closing output channel
2022-06-27T11:15:36.6444886-07:00|INFORMATION|Output channel closed, waiting for output task to complete
Closing writers
2022-06-27T11:15:36.8124803-07:00|INFORMATION|Status: 132 objects finished (+13 2.129032)/s -- Using 40 MB RAM
2022-06-27T11:15:36.8124803-07:00|INFORMATION|Enumeration finished in 00:01:02.1101889
2022-06-27T11:15:36.9274830-07:00|INFORMATION|PARSE_ZIP_FILE:C:\Users\Public\20220627111515_Enum.bin
2022-06-27T11:15:36.9724854-07:00|INFORMATION|SharpHound Enumeration Completed at 11:15 AM on 6/27/2022! Happy Graphing!

```