using Microsoft.AspNetCore.Mvc;

using Inventor.Semantics.Serialization.Json;
using Inventor.Semantics.Utils;

namespace Inventor.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class SemanticNetworkController : ControllerBase
	{
		private readonly ILogger<SemanticNetworkController> _logger;
		private readonly IDataService _dataService;

		public SemanticNetworkController(ILogger<SemanticNetworkController> logger, IDataService dataService)
		{
			_logger = logger.EnsureNotNull(nameof(logger));
			_dataService = dataService.EnsureNotNull(nameof(dataService));
		}

		[HttpGet(Name = "GetSemanticNetwork")]
		public String Get()
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			var snapshot = new SemanticNetwork(semanticNetwork);

			return snapshot.SerializeToJsonString();
		}
	}
}
