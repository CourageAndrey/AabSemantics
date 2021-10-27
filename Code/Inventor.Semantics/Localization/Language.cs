using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Localization
{
	[Serializable, XmlType(RootName), XmlRoot(RootName)]
	public class Language : ILanguage
	{
		#region Constants

		[XmlIgnore]
		internal const String RootName = "Language";
		[XmlIgnore]
		private const String DefaultCulture = "en-US";
		[XmlIgnore]
		private const String DefaultName = "English";

		#endregion

		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		[XmlArray(nameof(Extensions))]
		public List<LanguageExtension> ExtensionsXml
		{ get; set; } = new List<LanguageExtension>();

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public String FileName
		{ get; protected set; }

		[XmlAttribute]
		public String Name
		{ get; set; }

		[XmlAttribute]
		public String Culture
		{ get; set; }

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		[XmlIgnore]
		public ICollection<LanguageExtension> Extensions
		{ get { return ExtensionsXml; } }

		#endregion

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

		public override String ToString()
		{
			return Name;
		}

		public static void PrepareModulesToSerialization<LanguageT>()
			where LanguageT : class, ILanguage
		{
			var languageType = typeof(LanguageT);
			if (!_preparedToSerialization.Contains(languageType))
			{
				var attributeOverrides = new XmlAttributeOverrides();

				var moduleAttributes = new XmlAttributes();
				foreach (var module in Repositories.Modules.Values)
				{
					foreach (var extension in module.GetLanguageExtensions())
					{
						moduleAttributes.XmlElements.Add(new XmlElementAttribute(extension.Key, extension.Value));
					}
				}
				attributeOverrides.Add(languageType, nameof(Extensions), moduleAttributes);
				attributeOverrides.Add(languageType, nameof(ExtensionsXml), moduleAttributes);

				var serializer = new XmlSerializer(languageType, attributeOverrides);
				languageType.DefineCustomSerializer(serializer);

				_preparedToSerialization.Add(languageType);
			}
		}

		private static ICollection<Type> _preparedToSerialization = new HashSet<Type>();
	}
}
