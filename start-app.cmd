@echo off

cd C:\Projects\highlights-parser\Sources\UI\HighlightsParser.ConsoleApp
call dotnet build && dotnet run "%1"
cd C:\Projects\highlights-parser