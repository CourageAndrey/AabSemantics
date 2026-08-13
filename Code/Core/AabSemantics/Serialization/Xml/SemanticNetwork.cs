using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>
	/// XML surrogate of a whole semantic network — the root element of a saved knowledge base.
	/// Modules are stored by name only, so loading requires those modules to be registered in
	/// the process.
	/// </summary>
	[Serializable, XmlRoot(nameof(SemanticNetwork))]
	public class SemanticNetwork
	{
		#region Properties

		/// <summary>Localized name of the network.</summary>
		[XmlElement]
		public LocalizedString Name
		{ get; set; }

		/// <summary>
		/// Surrogates of the network's concepts, excluding module-provided system concepts, which
		/// are recreated by the modules themselves rather than stored.
		/// </summary>
		[XmlArray(nameof(Concepts))]
		[XmlArrayItem(nameof(Concept))]
		public List<Concept> Concepts
		{ get; set; } = new List<Concept>();

		/// <summary>Surrogates of the network's statements.</summary>
		[XmlArray(nameof(Statements))]
		public List<Statement> Statements
		{ get; set; } = new List<Statement>();

		/// <summary>Names of the modules the network was built with.</summary>
		[XmlArray(nameof(Modules))]
		public List<String> Modules
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public SemanticNetwork()
		{ }

		/// <summary>Converts a semantic network into its surrogate.</summary>
		/// <param name="semanticNetwork">Network to convert.</param>
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

		/// <summary>
		/// Rebuilds the network: attaches the named modules, then restores concepts and finally
		/// statements, since statements reference concepts by identifier.
		/// </summary>
		/// <param name="language">Language for the restored network.</param>
		/// <returns>The restored network.</returns>
		/// <exception cref="ModuleException">A stored module name is registered but its dependencies are unresolved.</exception>
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
			typeof(SemanticNetwork).DefineTypeOverrides(new[]
			{
				new XmlHelper.PropertyTypes(nameof(Concept.Attributes), typeof(Concept), Repositories.Attributes.Definitions.Values.ToDictionary(
					definition => definition.GetSerializationSettings<AttributeXmlSerializationSettings>().XmlElementName,
					definition => definition.GetSerializationSettings<AttributeXmlSerializationSettings>().XmlType)),
				new XmlHelper.PropertyTypes(nameof(Statements), typeof(SemanticNetwork), Repositories.Statements.Definitions.Values.ToDictionary(
					definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlElementName,
					definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlType)),
			});
		}
	}

	/// <summary>Reading and writing a whole semantic network as an XML file.</summary>
	public static class SemanticNetworkXmlExtensions
	{
		/// <summary>Loads a semantic network from an XML file.</summary>
		/// <param name="fileName">Path to read from.</param>
		/// <param name="language">Language for the restored network.</param>
		/// <returns>The restored network.</returns>
		public static AabSemantics.SemanticNetwork LoadSemanticNetworkFromXml(this String fileName, ILanguage language)
		{
			var xmlSnapshot = fileName.DeserializeFromXmlFile<SemanticNetwork>();
			return xmlSnapshot.Load(language);
		}

		/// <summary>Saves a semantic network to an XML file, overwriting it.</summary>
		/// <param name="semanticNetwork">Network to save.</param>
		/// <param name="fileName">Path to write to.</param>
		public static void SaveToXml(this ISemanticNetwork semanticNetwork, String fileName)
		{
			var xmlSnapshot = new SemanticNetwork(semanticNetwork);
			xmlSnapshot.SerializeToXmlFile(fileName);
		}
	}
}
