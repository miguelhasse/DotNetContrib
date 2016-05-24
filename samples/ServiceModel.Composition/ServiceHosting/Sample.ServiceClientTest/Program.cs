using System;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var managementService = new ManagementGateway("ManagementService", "miguel.hasse", "");
			managementService.ServerEvent += OnServerEvent;
            
			try { managementService.CreateSession(Guid.NewGuid()); }
			catch (Exception ex) { Console.WriteLine(ex.GetBaseException().Message); }

			try { managementService.CreateSession(Guid.NewGuid()); }
			catch (Exception ex) { Console.WriteLine(ex.GetBaseException().Message); }
            /*
			var cashierService = new CashierGateway("CashierService");

			var parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = 5
			};
            Parallel.For(0, 20, parallelOptions, async n =>
			{
				try
				{
					////foreach (var hotel in (await cashierService.GetEnvironment()).Hotels)
					////	Console.WriteLine(hotel.Description);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.GetBaseException().Message);
				}
			});
            */

            var performanceService = new PerformanceGateway("PerformanceService", "miguel.hasse", "");
            var performanceData = performanceService.GetData();

            Console.ReadLine();
			managementService.ServerEvent -= OnServerEvent;
		}

		private static void OnServerEvent(object sender, CallbackEventArgs e)
		{
			Console.WriteLine("Callback: {0}", e.Data);
		}
	}
}
