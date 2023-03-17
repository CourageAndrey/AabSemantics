using Microsoft.AspNetCore.Mvc;

using Inventor.Semantics;
using Inventor.Semantics.Xml;

namespace Inventor.SimpleRestClient.Controllers
{
	[ApiController, Route("[controller]")]
	public class AskQuestionController : ControllerBase
	{
		private readonly ILogger<AskQuestionController> _logger;
		private readonly IDataService _dataService;

		public AskQuestionController(ILogger<AskQuestionController> logger, IDataService dataService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
		}

		[HttpGet(Name = "GetAskQuestion")]
		public Answer Get([FromBody] Question question)
		{
			var semanticNetwork = _dataService.GetSemanticNetwork();

			var conceptsCache = new Dictionary<String, IConcept>();
			foreach (var concept in semanticNetwork.Concepts)
			{
				conceptsCache[concept.ID] = concept;
			}
			var conceptIdResolver = new ConceptIdResolver(conceptsCache);

			var deserializedQuestion = question.Save(conceptIdResolver);
			var answer = deserializedQuestion.Ask(semanticNetwork.Context);
			var serializedAnswer = Answer.Load(answer, semanticNetwork.Context.Language);

			return serializedAnswer;
		}
	}
}
