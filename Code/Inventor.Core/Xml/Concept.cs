using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class Concept
	{
		#region Properties

		[XmlAttribute]
		public Int32 ID
		{ get; set; }

		[XmlElement]
		public LocalizedString Name
		{ get; set; }

		[XmlElement]
		public LocalizedString Hint
		{ get; set; }

		[XmlArray(nameof(Attributes))]
		[XmlArrayItem("IsBoolean", typeof(IsBooleanAttribute))]
		[XmlArrayItem("IsComparisonSign", typeof(IsComparisonSignAttribute))]
		[XmlArrayItem("IsProcess", typeof(IsProcessAttribute))]
		[XmlArrayItem("IsSequenceSign", typeof(IsSequenceSignAttribute))]
		[XmlArrayItem("IsSign", typeof(IsSignAttribute))]
		[XmlArrayItem("IsValue", typeof(IsValueAttribute))]
		public List<Attribute> Attributes
		{ get; set; } = new List<Attribute>();

		#endregion

		#region Constructors

		public Concept()
		{ }

		public Concept(IConcept concept, IDictionary<IConcept, Int32> conceptsCache)
		{
			Name = new LocalizedString(concept.Name);
			Hint = new LocalizedString(concept.Hint);
			Attributes = concept.Attributes.Select(a => Attribute.Save(a)).ToList();

			conceptsCache[concept] = ID = conceptsCache.Count;
		}

		#endregion

		public IConcept Load()
		{
			var name = new Localization.LocalizedStringVariable();
			Name.LoadTo(name);

			var hint = new Localization.LocalizedStringVariable();
			Hint.LoadTo(hint);

			var concept = new Base.Concept(name, hint);
			foreach (var attribute in Attributes)
			{
				concept.Attributes.Add(attribute.Load());
			}
			return concept;
		}
	}
}
