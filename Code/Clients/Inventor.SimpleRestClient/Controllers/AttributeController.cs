using Microsoft.AspNetCore.Mvc;

using Inventor.Semantics.Metadata;

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
		public IEnumerable<Semantics.Xml.Attribute> Get()
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			return Repositories.Attributes
				.Definitions
				.Values
				.Select(definition => Semantics.Xml.Attribute.Save(definition.AttributeValue));
		}
	}
}
