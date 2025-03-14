using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace AirbnbFrontend.Pages
{
	public class HostPropertyModel : PageModel
	{
		[BindProperty] public string Title { get; set; }
		[BindProperty] public string About { get; set; }
		[BindProperty] public string StreetAddress { get; set; }
		[BindProperty] public string City { get; set; }
		[BindProperty] public string State { get; set; }
		[BindProperty] public string ZipCode { get; set; }
		[BindProperty] public int Bedrooms { get; set; }
		[BindProperty] public decimal Bathrooms { get; set; }
		[BindProperty] public int SquareFeet { get; set; }
		[BindProperty] public int PricePerNight { get; set; }
		public string Message { get; set; }

		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HostPropertyModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = httpClient;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IActionResult> OnPostAsync()
		{
			// ✅ Hardcoded UserId (Ensure it's a valid GUID string)
			var userId = "e8e20f27-465f-491e-8c8f-3fd548ea9c14";

			if (!Guid.TryParse(userId, out var parsedUserId))
			{
				Message = "❌ Error: UserId is not a valid GUID!";
				return Page();
			}

			Console.WriteLine($"📢 Sending UserId: {userId}");

			
			var newProperty = new
			{
				Owner = parsedUserId,  
				Title = Title,
				About = About,
				StreetAddress = StreetAddress,
				City = City,
				State = State,
				ZipCode = ZipCode,
				Bedrooms = Bedrooms,
				Bathrooms = Bathrooms,
				SquareFeet = SquareFeet,
				PricePerNight = PricePerNight,
				Photos = new string[] { }
			};

			var jsonContent = JsonConvert.SerializeObject(newProperty);
			Console.WriteLine($"📢 Request Payload: {jsonContent}"); // ✅ Print JSON request before sending

			var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			try
			{
				var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5013/api/properties")
				{
					Content = stringContent
				};

				
				request.Headers.Add("Cookie", $"UserId={userId}");

				
				var response = await _httpClient.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{
					Message = "🎉 Property added successfully!";
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					Message = $"❌ Error adding property! Server response: {response.StatusCode} - {errorResponse}";
					Console.WriteLine($"⚠️ API Response: {errorResponse}");
				}
			}
			catch (HttpRequestException httpEx)
			{
				Message = $"❌ Network Error: {httpEx.Message}";
				Console.WriteLine($"⚠️ Network Error: {httpEx.Message}");
			}
			catch (Exception ex)
			{
				Message = $"❌ Unexpected Error: {ex.Message}";
				Console.WriteLine($"⚠️ Unexpected Error: {ex}");
			}

			return Page();
		}
	}
}
