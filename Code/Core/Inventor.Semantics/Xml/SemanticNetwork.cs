using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Xml
{
	[Serializable, XmlRoot(nameof(SemanticNetwork))]
	public class SemanticNetwork
	{
		#region Properties

		[XmlElement]
		public LocalizedString Name
		{ get; set; }

		[XmlArray(nameof(Concepts))]
		[XmlArrayItem(nameof(Concept))]
		public List<Concept> Concepts
		{ get; set; } = new List<Concept>();

		[XmlArray(nameof(Statements))]
		public List<Statement> Statements
		{ get; set; } = new List<Statement>();

		[XmlArray(nameof(Modules))]
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

			var conceptsCache = new Dictionary<Concept, IConcept>();
			foreach (var concept in Concepts)
			{
				result.Concepts.Add(conceptsCache[concept] = concept.Load());
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
			var semanticNetworkType = typeof(SemanticNetwork);
			var attributeOverrides = new XmlAttributeOverrides();

			var attributeAttributes = new XmlAttributes();
			foreach (var definition in Repositories.Attributes.Definitions.Values)
			{
				attributeAttributes.XmlElements.Add(new XmlElementAttribute(definition.XmlElementName, definition.XmlType));
			}
			attributeOverrides.Add(typeof(Concept), "Attributes", attributeAttributes);

			var statementAttributes = new XmlAttributes();
			foreach (var definition in Repositories.Statements.Definitions.Values)
			{
				statementAttributes.XmlElements.Add(new XmlElementAttribute(definition.XmlElementName, definition.XmlType));
			}
			attributeOverrides.Add(semanticNetworkType, "Statements", statementAttributes);

			var serializer = new XmlSerializer(semanticNetworkType, attributeOverrides);
			semanticNetworkType.DefineCustomSerializer(serializer);
		}
	}

	public static class SemanticNetworkXmlExtensions
	{
		public static Semantics.SemanticNetwork LoadSemanticNetworkFromXml(this String fileName, ILanguage language)
		{
			var xmlSnapshot = fileName.DeserializeFromFile<SemanticNetwork>();
			return xmlSnapshot.Load(language);
		}

		public static void Save(this ISemanticNetwork semanticNetwork, String fileName)
		{
			var xmlSnapshot = new SemanticNetwork(semanticNetwork);
			xmlSnapshot.SerializeToFile(fileName);
		}
	}
}
