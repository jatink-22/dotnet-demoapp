using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PostgresConnectivityApp.Models;

namespace PostgresConnectivityApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace PostgresConnectivityApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string message;

            // Retrieve connection string from configuration
            var connectionString = _configuration.GetConnectionString("PostgreSqlConnection");

            try
            {
                // Establish connection to PostgreSQL database
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                // Test query to confirm connectivity
                using var command = new NpgsqlCommand("SELECT 'Connection successful!' AS message;", connection);
                var result = command.ExecuteScalar();

                message = result?.ToString() ?? "No response from database.";
            }
            catch (Exception ex)
            {
                // Handle any errors during connection
                message = $"Database connection failed: {ex.Message}";
            }

            // Pass message to view
            ViewBag.Message = message;
            return View();
        }
    }
}
