dotnet publish -p:PublishSingleFile=true --runtime win-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime win-x86 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime osx-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime linux-x64 --self-contained