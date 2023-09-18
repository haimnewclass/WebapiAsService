
// generate classs BackgroundWorkerService that implement IHostedService 
using System.Diagnostics;
using System.Threading;

public class BackgroundWorkerService : BackgroundService
{
   
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 

        while (!stoppingToken.IsCancellationRequested)
        {
            // write to console
            Console.WriteLine("Hello World");
            //sleep for 5 seconds
            await Task.Delay(5000,stoppingToken);
        }
    }
}




public static class WindowsServiceHelpers
{
    public static bool IsWindowsService()
    {
        bool isService = false;

        using (var process = Process.GetCurrentProcess())
        {
            isService = process.SessionId == 0 && process.ProcessName == "YOUR_SERVICE_NAME";
        }

        return isService;
    }
}