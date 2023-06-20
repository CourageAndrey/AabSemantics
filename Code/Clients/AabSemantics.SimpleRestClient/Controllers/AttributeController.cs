using Microsoft.AspNetCore.Mvc;

using AabSemantics.Metadata;
using AabSemantics.Serialization.Json;
using AabSemantics.Utils;

namespace AabSemantics.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class AttributeController : ControllerBase
	{
		private readonly ILogger<AttributeController> _logger;
		private readonly IDataService _dataService;

		public AttributeController(ILogger<AttributeController> logger, IDataService dataService)
		{
			_logger = logger.EnsureNotNull(nameof(logger));
			_dataService = dataService.EnsureNotNull(nameof(dataService));
		}

		[HttpGet(Name = "GetAttribute")]
		public IEnumerable<String> Get()
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			return Repositories.Attributes
				.Definitions
				.Values
				.Select(definition => definition.Value)
				.ToJson();
		}
	}
}
