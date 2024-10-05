using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

namespace Storage.API.Controllers;

[ApiController]
[Route("[controller]")]
public class Controller
{
	[HttpGet(nameof(Get))]
	public int Get()
	{
		Debug.WriteLine("qwe");
		return 123;
	}
}