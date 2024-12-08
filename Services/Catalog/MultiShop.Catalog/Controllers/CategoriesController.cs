using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Services.CategoryServices;
using System.Net;
using System.Runtime.InteropServices;

namespace MultiShop.Catalog.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		#region INFO
		[HttpGet("info")]
		public IActionResult GetInfo()
		{
			//https://localhost:7004/api/categories/info
			var currentDateTime = DateTime.Now;
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var serverName = Environment.MachineName;
			var osDescription = RuntimeInformation.OSDescription;
			var osArchitecture = RuntimeInformation.OSArchitecture.ToString();

			var load = GetCpuLoad();
			var ipAddress = GetIpAddress();
			var uptime = GetUptime();

			var response = new
			{
				Date = currentDateTime.ToString("yyyy-MM-dd"),
				Time = currentDateTime.ToString("HH:mm:ss"),
				Environment = environment,
				ServerName = serverName,
				OperatingSystem = osDescription,
				OSArchitecture = osArchitecture,
				Load = load,
				IpAddress = ipAddress,
				Uptime = uptime
			};

			return Ok(response);
		}

		private string GetCpuLoad()
		{
			var cpuLoad = (Environment.ProcessorCount > 0) ? 50 : 0;
			return cpuLoad + "%";
		}

		private string GetIpAddress()
		{
			var ipAddress = Dns.GetHostAddresses(Environment.MachineName)
				.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
			return ipAddress ?? "IP address not found";
		}

		private string GetUptime()
		{
			var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
			return uptime.ToString(@"dd\.hh\:mm\:ss");
		}

		#endregion
		[HttpGet]
		public async Task<IActionResult> CategoryList()
		{
			var values = await _categoryService.GettAllCategoryAsync();
			return Ok(values);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategoryById(string id)
		{
			var values = await _categoryService.GetByIdCategoryAsync(id);
			return Ok(values);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
		{
			await _categoryService.CreateCategoryAsync(createCategoryDto);
			return Ok("Kategori başarıyla eklendi");
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteCategory(string id)
		{
			await _categoryService.DeleteCategoryAsync(id);
			return Ok("Kategori başarıyla silindi");
		}

		[HttpPut]
		public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
		{
			await _categoryService.UpdateCategoryAsync(updateCategoryDto);
			return Ok("Kategori başarıyla güncellendi");
		}
	}
}