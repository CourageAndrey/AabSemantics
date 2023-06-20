using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Json
{
	[DataContract]
	public class SemanticNetwork
	{
		#region Properties

		[DataMember]
		public LocalizedString Name
		{ get; set; }

		[DataMember]
		public List<Concept> Concepts
		{ get; set; } = new List<Concept>();

		[DataMember]
		public List<Statement> Statements
		{ get; set; } = new List<Statement>();

		[DataMember]
		public List<String> Modules
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		public SemanticNetwork()
		{
			Name = new LocalizedString();
		}

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

		public AabSemantics.SemanticNetwork Load(ILanguage language)
		{
			var result = new AabSemantics.SemanticNetwork(language);
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

		static SemanticNetwork()
		{
			RefreshMetadata();
		}

		public static void RefreshMetadata()
		{
			var semanticNetworkType = typeof(SemanticNetwork);
			var serializer = new DataContractJsonSerializer(
				semanticNetworkType,
				Repositories.Statements.GetJsonTypes());
			semanticNetworkType.DefineCustomJsonSerializer(serializer);
		}
	}

	public static class SemanticNetworkJsonExtensions
	{
		public static AabSemantics.SemanticNetwork LoadSemanticNetworkFromJson(this String fileName, ILanguage language)
		{
			var jsonSnapshot = fileName.DeserializeFromJsonFile<SemanticNetwork>();
			return jsonSnapshot.Load(language);
		}

		public static void SaveToJson(this ISemanticNetwork semanticNetwork, String fileName)
		{
			var jsonSnapshot = new SemanticNetwork(semanticNetwork);
			jsonSnapshot.SerializeToJsonFile(fileName);
		}
	}
}
