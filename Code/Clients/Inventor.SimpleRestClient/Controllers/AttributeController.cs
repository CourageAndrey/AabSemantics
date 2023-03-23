using Microsoft.AspNetCore.Mvc;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class AttributeController : ControllerBase
	{
		private readonly ILogger<AttributeController> _logger;
		private readonly IDataService _dataService;

		public AttributeController(ILogger<AttributeController> logger, IDataService dataService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
		}

		[HttpGet(Name = "GetAttribute")]
		public IEnumerable<String> Get()
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			return Repositories.Attributes
				.Definitions
				.Values
				.Select(definition => definition.AttributeValue)
				.ToJson();
		}
	}
}
