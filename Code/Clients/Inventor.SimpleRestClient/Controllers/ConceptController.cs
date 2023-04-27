using Microsoft.AspNetCore.Mvc;

using Inventor.Semantics;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;
using Inventor.Semantics.Utils;

namespace Inventor.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class ConceptController : ControllerBase
	{
		private readonly ILogger<ConceptController> _logger;
		private readonly IDataService _dataService;

		public ConceptController(ILogger<ConceptController> logger, IDataService dataService)
		{
			_logger = logger.EnsureNotNull(nameof(logger));
			_dataService = dataService.EnsureNotNull(nameof(dataService));
		}

		[HttpGet(Name = "GetConcept")]
		public IEnumerable<Concept> Get([FromQuery] string id)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			var concepts = string.IsNullOrEmpty(id)
				? semanticNetwork.Concepts.Where(concept => !ConceptIdResolver.SystemConceptsById.ContainsKey(concept.ID)).ToList() as ICollection<IConcept>
				: new[] { semanticNetwork.Concepts[id] };

			return concepts.Select(concept => new Concept(concept));
		}

		[HttpPut(Name = "PutConcept")]
		public void Put([FromBody] Concept concept)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			semanticNetwork.Concepts.Add(concept.Load());
		}

		[HttpDelete(Name = "DeleteConcept")]
		public void Delete([FromQuery] string id)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			semanticNetwork.Statements.Remove(semanticNetwork.Statements[id]);
		}
	}
}
