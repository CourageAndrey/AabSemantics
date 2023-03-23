using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	[Serializable]
	public class SemanticNetwork
	{
		#region Properties

		public LocalizedString Name
		{ get; set; }

		public List<Concept> Concepts
		{ get; set; } = new List<Concept>();

		public List<Statement> Statements
		{ get; set; } = new List<Statement>();

		public List<String> Modules
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		public SemanticNetwork()
		{ }

		public SemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			Name = new LocalizedString(semanticNetwork.Name);

			Modules = semanticNetwork.Modules.Keys.ToList();

			Concepts = semanticNetwork.Concepts
				.Where(concept => !ConceptIdResolver.SystemConceptsById.ContainsKey(concept.ID))
				.Select(concept => new Concept(concept))
				.ToList();

			Statements = semanticNetwork.Statements.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		public Semantics.SemanticNetwork Load(ILanguage language)
		{
			var result = new Semantics.SemanticNetwork(language);
			Name.LoadTo(result.Name);

			result.WithModules(Repositories.Modules.Values.Where(module => Modules.Contains(module.Name)).ToList());

			var conceptsCache = new Dictionary<String, IConcept>();
			foreach (var concept in Concepts)
			{
				result.Concepts.Add(conceptsCache[concept.ID] = concept.Load());
			}

			var conceptIdResolver = new ConceptIdResolver(conceptsCache);
			foreach (var statement in Statements)
			{
				result.Statements.Add(statement.Save(conceptIdResolver));
			}

			return result;
		}
	}
}
