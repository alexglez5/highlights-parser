@echo off

cd C:\Projects\text-parser\Sources\TextParser\
call dotnet build
cd C:\Projects\text-parser\Sources\TextParser\bin\Debug\netcoreapp3.1\
call TextParser.exe "C:\\Projects\\dev-refs\\Scripts\\utils\\" "kindleHighlightsSource.txt" "C:\\Projects\\dev-refs\\Scripts\\utils\\" kindleHighlightsOutput.md
cd C:\Projects\text-parser\