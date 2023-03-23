using Microsoft.AspNetCore.Mvc;

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
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
		}

		[HttpGet(Name = "GetSemanticNetwork")]
		public String Get()
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			var snapshot =  new Semantics.Serialization.Json.SemanticNetwork(semanticNetwork);

			return snapshot.SerializeToJsonString();
		}
	}
}
