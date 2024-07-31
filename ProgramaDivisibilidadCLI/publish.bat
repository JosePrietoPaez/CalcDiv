dotnet clean ProgramaDivisibilidadCLI.csproj --configuration Release
rmdir ArchivosCompilados /q /s
md ArchivosCompilados
IF "%~1" == "1" GOTO comprimir
dotnet publish -p:PublishSingleFile=true --runtime win-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime win-x86 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime osx-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime linux-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime win-x64 --self-contained false --output bin/Release/net8.0/win-x64/publish-no-sc
dotnet publish -p:PublishSingleFile=true --runtime win-x86 --self-contained false --output bin/Release/net8.0/win-x86/publish-no-sc
dotnet publish -p:PublishSingleFile=true --runtime osx-x64 --self-contained false --output bin/Release/net8.0/osx-x64/publish-no-sc
dotnet publish -p:PublishSingleFile=true --runtime linux-x64 --self-contained false --output bin/Release/net8.0/linux-x64/publish-no-sc
:comprimir
7z a -tzip -mx9 -mm=LZMA .\ArchivosCompilados\calc_div_windows_64bit_self_contained.zip .\bin\Release\net8.0\win-x64\publish\CalcDivCLI*
7z a -tzip -mx9 -mm=LZMA .\ArchivosCompilados\calc_div_windows_32bit_self_contained.zip .\bin\Release\net8.0\win-x86\publish\CalcDivCLI*
7z a -tzip -mx9 -mm=LZMA .\ArchivosCompilados\calc_div_osx_64bit_self_contained.zip .\bin\Release\net8.0\osx-x64\publish\CalcDivCLI*
7z a -ttar .\ArchivosCompilados\calc_div_linux_64bit_self_contained.tar .\bin\Release\net8.0\linux-x64\publish\CalcDivCLI*
7z a -tgzip -mx9 .\ArchivosCompilados\calc_div_linux_64bit_self_contained.tar.gz .\ArchivosCompilados\calc_div_linux_64bit_self_contained.tar
7z a -tzip .\ArchivosCompilados\calc_div_windows_64bit.zip .\bin\Release\net8.0\win-x64\publish-no-sc\CalcDivCLI*
7z a -tzip .\ArchivosCompilados\calc_div_windows_32bit.zip .\bin\Release\net8.0\win-x64\publish-no-sc\CalcDivCLI*
7z a -tzip .\ArchivosCompilados\calc_div_osx_64bit.zip .\bin\Release\net8.0\win-x64\publish-no-sc\CalcDivCLI*
7z a -ttar .\ArchivosCompilados\calc_div_linux_64bit.tar .\bin\Release\net8.0\linux-x64\publish-no-sc\CalcDivCLI*
7z a -tgzip -mx9 .\ArchivosCompilados\calc_div_linux_64bit.tar.gz .\ArchivosCompilados\calc_div_linux_64bit.tar