using Microsoft.AspNetCore.Mvc;

using Inventor.Semantics;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class StatementController : ControllerBase
	{
		private readonly ILogger<StatementController> _logger;
		private readonly IDataService _dataService;

		public StatementController(ILogger<StatementController> logger, IDataService dataService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
		}

		[HttpGet(Name = "GetStatement")]
		public IEnumerable<Statement> Get([FromQuery] string id)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			var statements = string.IsNullOrEmpty(id)
				? semanticNetwork.Statements as ICollection<IStatement>
				: new[] { semanticNetwork.Statements[id] };

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

			semanticNetwork.Concepts.Remove(semanticNetwork.Concepts[id]);
		}
	}
}
