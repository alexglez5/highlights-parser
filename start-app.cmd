@echo off

cd C:\Dev\highlights-parser\Sources\UI\HighlightsParser.ConsoleApp
call dotnet build && dotnet run "%1"
cd C:\Dev\highlights-parser