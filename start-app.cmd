@echo off

cd C:\Projects\text-parser\Sources\TextParser\
call dotnet build
call TextParser.exe "C:\\Projects\\dev-refs\\Scripts\\utils\\" "kindleHighlightsSource.txt" "C:\\Projects\\dev-refs\\Scripts\\utils\\" kindleHighlightsOutput.md
cd C:\Projects\text-parser\