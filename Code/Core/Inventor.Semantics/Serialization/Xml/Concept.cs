using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Inventor.Semantics.Serialization.Xml
{
	[XmlType]
	public class Concept
	{
		#region Properties

		[XmlAttribute]
		public String ID
		{ get; set; }

		[XmlElement]
		public LocalizedString Name
		{ get; set; }

		[XmlElement]
		public LocalizedString Hint
		{ get; set; }

		[XmlArray(nameof(Attributes))]
		public List<Attribute> Attributes
		{ get; set; } = new List<Attribute>();

		#endregion

		#region Constructors

		public Concept()
		{ }

		public Concept(IConcept concept)
		{
			Name = new LocalizedString(concept.Name);
			Hint = new LocalizedString(concept.Hint);
			Attributes = concept.Attributes.Select(a => Attribute.Save(a)).ToList();
			ID = concept.ID;
		}

		#endregion

		public IConcept Load()
		{
			var name = new Localization.LocalizedStringVariable();
			Name.LoadTo(name);

			var hint = new Localization.LocalizedStringVariable();
			Hint.LoadTo(hint);

			return new Concepts.Concept(ID, name, hint).WithAttributes(Attributes.Select(a => a.Load()));
		}
	}
}
