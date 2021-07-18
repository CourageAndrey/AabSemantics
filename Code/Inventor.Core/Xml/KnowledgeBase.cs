using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[Serializable, XmlRoot(nameof(KnowledgeBase))]
	public class KnowledgeBase
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
		[XmlArrayItem("Comparison", typeof(ComparisonStatement))]
		[XmlArrayItem("Group", typeof(GroupStatement))]
		[XmlArrayItem("HasPart", typeof(HasPartStatement))]
		[XmlArrayItem("HasSign", typeof(HasSignStatement))]
		[XmlArrayItem("Is", typeof(IsStatement))]
		[XmlArrayItem("Processes", typeof(ProcessesStatement))]
		[XmlArrayItem("SignValue", typeof(SignValueStatement))]
		public List<Statement> Statements
		{ get; set; } = new List<Statement>();

		#endregion

		#region Constructors

		public KnowledgeBase()
		{ }

		public KnowledgeBase(Base.KnowledgeBase knowledgeBase)
		{
			Name = new LocalizedString(knowledgeBase.Name);

			var systemConcepts = new HashSet<IConcept>(SystemConcepts.GetAll());
			var conceptsCache = new Dictionary<IConcept, Int32>();
			Concepts = knowledgeBase.Concepts.Except(systemConcepts).Select(concept => new Concept(concept, conceptsCache)).ToList();

			var conceptIdResolver = new LoadIdResolver(conceptsCache);
			Statements = knowledgeBase.Statements.Select(statement => Statement.Load(statement, conceptIdResolver)).ToList();
		}

		#endregion

		public Base.KnowledgeBase Load(ILanguage language)
		{
			var result = new Base.KnowledgeBase(language);
			Name.LoadTo(result.Name);

			var conceptsCache = new Dictionary<Concept, IConcept>();
			foreach (var concept in Concepts)
			{
				result.Concepts.Add(conceptsCache[concept] = concept.Load());
			}

			var conceptIdResolver = new SaveIdResolver(conceptsCache);
			foreach (var statement in Statements)
			{
				result.Statements.Add(statement.Save(conceptIdResolver));
			}

			return result;
		}
	}
}
