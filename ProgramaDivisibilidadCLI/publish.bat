dotnet publish -p:PublishSingleFile=true --runtime win-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime win-x86 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime osx-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime linux-x64 --self-contained
dotnet publish -p:PublishSingleFile=true --runtime win-x64 --self-contained false --output bin/Release/net8.0/win-x64/publish-no-sc
dotnet publish -p:PublishSingleFile=true --runtime win-x86 --self-contained false --output bin/Release/net8.0/win-x86/publish-no-sc
dotnet publish -p:PublishSingleFile=true --runtime osx-x64 --self-contained false --output bin/Release/net8.0/osx-x64/publish-no-sc
dotnet publish -p:PublishSingleFile=true --runtime linux-x64 --self-contained false --output bin/Release/net8.0/linux-x64/publish-no-sc