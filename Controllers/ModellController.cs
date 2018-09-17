using System.Collections;
using Microsoft.AspNetCore.Mvc;
using template.Data;
using template.Extensions;
using template.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Primitives;

namespace template.Controllers
{
	[Route("api/models")]
	public class ModellController : Controller
	{

		[HttpGet]
		[Route("")]
		public IActionResult GetAllModels([FromQuery] int pageNumber = 1, int pageSize = 10)
		{
			string header = Request.Headers["Accept-Language"];
			Console.WriteLine(header);

			Envelope<ModelDTO> envelope = new Envelope<ModelDTO>();
			envelope.PageNumber = pageNumber;
			envelope.PageSize = pageSize;
			List<ModelDTO> temp = new List<ModelDTO>();

			DataContext.Models.ToLightWeight().ForEach(m =>
			{
				m.Links.AddReference("self", $"http://localhost:5000/api/models/{m.Id}");
				temp.Add(m);
			});

			envelope.Items = temp.Skip((pageNumber - 1) * pageSize).Take(pageSize);
			double maxPages = (double)temp.Count() / (double)pageSize;
			envelope.MaxPages = (int)Math.Ceiling(maxPages);
			return Ok(envelope);
		}

		[HttpGet]
		[Route("{id:int}")]
		public IActionResult GetModelById(int id)
		{
			ModelDetailsDTO model = DataContext.Models.ToDetails().FirstOrDefault(m => m.Id == id);
			model.Links.AddReference("self", $"http://localhost:5000/api/models/{id}");
			return Ok(model);
		}

	}
}