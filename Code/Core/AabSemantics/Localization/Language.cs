using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Metadata;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Localization
{
	/// <summary>
	/// Serializable <see cref="ILanguage"/>: the root of a language file. Nested bundles are
	/// exposed twice — as concrete <c>*Xml</c> properties the serializer writes, and as read-only
	/// interface properties the engine reads.
	/// </summary>
	[Serializable, XmlType(RootName), XmlRoot(RootName)]
	public class Language : ILanguage
	{
		#region Constants

		/// <summary>Name of the XML root element of a language file.</summary>
		[XmlIgnore]
		internal const String RootName = "Language";
		[XmlIgnore]
		private const String DefaultCulture = "en-US";
		[XmlIgnore]
		private const String DefaultName = "English";

		#endregion

		#region Xml Properties

		/// <summary>Attribute names, in serializable form.</summary>
		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		/// <summary>Statement wordings, in serializable form.</summary>
		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		/// <summary>Question wordings, in serializable form.</summary>
		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		/// <summary>
		/// Per-module string bundles, in serializable form. Deserializing these requires
		/// <see cref="PrepareModulesToSerialization{LanguageT}"/> to have run.
		/// </summary>
		[XmlArray(nameof(Extensions))]
		public List<LanguageExtension> ExtensionsXml
		{ get; set; } = new List<LanguageExtension>();

		#endregion

		#region Interface Properties

		/// <summary>Path the language was loaded from; empty for the built-in default.</summary>
		[XmlIgnore]
		public String FileName
		{ get; protected set; }

		/// <summary>Display name of the language, written in that language itself.</summary>
		[XmlAttribute]
		public String Name
		{ get; set; }

		/// <summary>Culture identifier, e.g. <c>en-US</c>.</summary>
		[XmlAttribute]
		public String Culture
		{ get; set; }

		/// <summary>Attribute names.</summary>
		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		/// <summary>Statement wordings.</summary>
		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		/// <summary>Question wordings.</summary>
		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		/// <summary>Per-module string bundles.</summary>
		[XmlIgnore]
		public ICollection<LanguageExtension> Extensions
		{ get { return ExtensionsXml; } }

		#endregion

		/// <summary>
		/// The built-in English language, used whenever no other one matches. It carries no
		/// module extensions, so module-specific strings must come from a loaded language file.
		/// </summary>
		[XmlIgnore]
		public static Language Default
		{ get; protected set; }

		static Language()
		{
			Default = new Language
			{
				FileName = String.Empty,
				Name = DefaultName,
				Culture = DefaultCulture,

				AttributesXml = LanguageAttributes.CreateDefault(),
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}

		/// <summary>Returns the language's display name.</summary>
		/// <returns>The value of <see cref="Name"/>.</returns>
		public override String ToString()
		{
			return Name;
		}

		/// <summary>
		/// Teaches the XML serializer which concrete types the registered modules use for their
		/// string bundles. Must run after the modules have registered their metadata and before a
		/// language file is read, otherwise module extensions cannot be deserialized. Repeated
		/// calls for the same language type do nothing.
		/// </summary>
		/// <typeparam name="LanguageT">Language type being prepared.</typeparam>
		public static void PrepareModulesToSerialization<LanguageT>()
			where LanguageT : class, ILanguage
		{
			var languageType = typeof(LanguageT);
			if (!_preparedToSerialization.Contains(languageType))
			{
				var overrides = new Dictionary<String, Type>();
				foreach (var module in Repositories.Modules.Values)
				{
					foreach (var extension in module.GetLanguageExtensions())
					{
						overrides[extension.Key] = extension.Value;
					}
				}

				languageType.DefineTypeOverrides(new[]
				{
					new XmlHelper.PropertyTypes(nameof(ExtensionsXml), languageType, overrides),
				});

				_preparedToSerialization.Add(languageType);
			}
		}

		private static ICollection<Type> _preparedToSerialization = new HashSet<Type>();
	}
}
