namespace WebsiteStatusApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _httpClient;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        //sc.exe create WebsiteStatus binpath= C:\workerservice\WebsiteStatusApp.exe start= auto in powershell to run the app

        //to uninstall stop the service first
        //sc.exe delete WebsiteStatus
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);  
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            _logger.LogInformation("The service has been stopped");
            return base.StopAsync(cancellationToken);   
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var result = await _httpClient.GetAsync("https://www.google.com");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. ", result.StatusCode);
                }
                else
                {
                    _logger.LogError("The website is down", result.StatusCode);
                }
                await Task.Delay(60*1000, stoppingToken);
            }
        }
    }
}