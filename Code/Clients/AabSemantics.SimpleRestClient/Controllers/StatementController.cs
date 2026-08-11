using Microsoft.AspNetCore.Mvc;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;
using AabSemantics.Utils;

namespace AabSemantics.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class StatementController : ControllerBase
	{
		private readonly ILogger<StatementController> _logger;
		private readonly IDataService _dataService;

		public StatementController(ILogger<StatementController> logger, IDataService dataService)
		{
			_logger = logger.EnsureNotNull(nameof(logger));
			_dataService = dataService.EnsureNotNull(nameof(dataService));
		}

		[HttpGet(Name = "GetStatement")]
		public IEnumerable<Statement> Get([FromQuery] string id)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			IEnumerable<IStatement> statements = string.IsNullOrEmpty(id)
				? semanticNetwork.Statements
				: new[] { semanticNetwork.Statements.GetItem(id) };

			return statements.Select(statement => Statement.Load(statement));
		}

		[HttpPut(Name = "PutStatement")]
		public void Put([FromBody] Statement statement)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			var conceptsCache = new Dictionary<String, IConcept>();
			foreach (var concept in semanticNetwork.Concepts)
			{
				conceptsCache[concept.ID] = concept;
			}
			var conceptIdResolver = new ConceptIdResolver(conceptsCache);

			semanticNetwork.Statements.Add(statement.Save(conceptIdResolver));
		}

		[HttpDelete(Name = "DeleteStatement")]
		public void Delete([FromQuery] string id)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			semanticNetwork.Concepts.Remove(semanticNetwork.Concepts.GetItem(id));
		}
	}
}
