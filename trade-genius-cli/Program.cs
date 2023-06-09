﻿using System.Diagnostics;

if (args.Length == 0)
{
    Console.WriteLine("insufficient params.");
    return;
}

string firstArg = args[0];
if (firstArg == "install" || firstArg == "i" || firstArg == "update" || firstArg == "u")
{
    await InstallTemplates();
    return;
}

if (firstArg == "api")
{
    if (args.Length != 3)
    {
        Console.WriteLine("invalid command. use something like : trade-genius api new <projectname>");
        return;
    }

    string command = args[1];
    string projectName = args[2];
    if (command == "n" || command == "new")
    {
        await BootstrapWebApiSolution(projectName);
    }

    return;
}

if (firstArg == "wasm")
{
    if (args.Length != 3)
    {
        Console.WriteLine("invalid command. use something like : trade-genius wasm new <projectname>");
        return;
    }

    string command = args[1];
    string projectName = args[2];
    if (command == "n" || command == "new")
    {
        await BootstrapBlazorWasmSolution(projectName);
    }

    return;
}

async Task InstallTemplates()
{
    WriteSuccessMessage("installing trade-genius dotnet webapi template...");
    var apiPsi = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "new install FullStackHero.WebAPI.Boilerplate"
    };
    using var apiProc = Process.Start(apiPsi)!;
    await apiProc.WaitForExitAsync();

    Console.WriteLine("installing trade-genius blazor wasm template...");
    var wasmPsi = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "new install FullStackHero.BlazorWebAssembly.Boilerplate"
    };
    using var wasmProc = Process.Start(wasmPsi)!;
    await wasmProc.WaitForExitAsync();

    WriteSuccessMessage("installed the required templates.");
    Console.WriteLine("get started by using : trade-genius <type> new <projectname>.");
    Console.WriteLine("note : <type> can be api, wasm.");
}

async Task BootstrapWebApiSolution(string projectName)
{
    Console.WriteLine($"bootstraping fullstackhero dotnet webapi project for you at \"./{projectName}\"...");
    var psi = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = $"new trade-genius-api -n {projectName} -o {projectName} -v=q"
    };
    using var proc = Process.Start(psi)!;
    await proc.WaitForExitAsync();
    WriteSuccessMessage($"trade-genius-api project {projectName} successfully created.");
    WriteSuccessMessage("application ready! build something amazing!");
}

async Task BootstrapBlazorWasmSolution(string projectName)
{
    Console.WriteLine($"bootstraping fullstackhero blazor wasm solution for you at \"./{projectName}\"...");
    var psi = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = $"new trade-genius-blazor -n {projectName} -o {projectName} -v=q"
    };
    using var proc = Process.Start(psi)!;
    await proc.WaitForExitAsync();
    WriteSuccessMessage($"fullstackhero blazor wasm solution {projectName} successfully created.");
    WriteSuccessMessage("application ready! build something amazing!");
}

void WriteSuccessMessage(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(message);
    Console.ResetColor();
}