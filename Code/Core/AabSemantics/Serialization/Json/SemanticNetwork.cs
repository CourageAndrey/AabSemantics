using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Json
{
	/// <summary>
	/// JSON surrogate of a whole semantic network. Modules are stored by name only, so loading
	/// requires those modules to be registered in the process.
	/// </summary>
	[DataContract]
	public class SemanticNetwork
	{
		#region Properties

		/// <summary>Localized name of the network.</summary>
		[DataMember]
		public LocalizedString Name
		{ get; set; }

		/// <summary>Surrogates of the network's concepts, excluding module-provided system concepts.</summary>
		[DataMember]
		public List<Concept> Concepts
		{ get; set; } = new List<Concept>();

		/// <summary>Surrogates of the network's statements.</summary>
		[DataMember]
		public List<Statement> Statements
		{ get; set; } = new List<Statement>();

		/// <summary>Names of the modules the network was built with.</summary>
		[DataMember]
		public List<String> Modules
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public SemanticNetwork()
		{
			Name = new LocalizedString();
		}

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

		/// <summary>
		/// Rebuilds the serializer so it knows the currently registered statement types. Call it
		/// again after registering further statement types, otherwise those cannot be saved.
		/// </summary>
		public static void RefreshMetadata()
		{
			var semanticNetworkType = typeof(SemanticNetwork);
			var serializer = new DataContractJsonSerializer(
				semanticNetworkType,
				Repositories.Statements.GetJsonTypes());
			semanticNetworkType.DefineCustomJsonSerializer(serializer);
		}
	}

	/// <summary>Reading and writing a whole semantic network as a JSON file.</summary>
	public static class SemanticNetworkJsonExtensions
	{
		/// <summary>Loads a semantic network from a JSON file.</summary>
		/// <param name="fileName">Path to read from.</param>
		/// <param name="language">Language for the restored network.</param>
		/// <returns>The restored network.</returns>
		public static AabSemantics.SemanticNetwork LoadSemanticNetworkFromJson(this String fileName, ILanguage language)
		{
			var jsonSnapshot = fileName.DeserializeFromJsonFile<SemanticNetwork>();
			return jsonSnapshot.Load(language);
		}

		/// <summary>Saves a semantic network to a JSON file, overwriting it.</summary>
		/// <param name="semanticNetwork">Network to save.</param>
		/// <param name="fileName">Path to write to.</param>
		public static void SaveToJson(this ISemanticNetwork semanticNetwork, String fileName)
		{
			var jsonSnapshot = new SemanticNetwork(semanticNetwork);
			jsonSnapshot.SerializeToJsonFile(fileName);
		}
	}
}
