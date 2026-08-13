using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>XML surrogate of a concept.</summary>
	[XmlType]
	public class Concept
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[XmlAttribute]
		public String ID
		{ get; set; }

		/// <summary>Localized display name.</summary>
		[XmlElement]
		public LocalizedString Name
		{ get; set; }

		/// <summary>Localized tooltip text.</summary>
		[XmlElement]
		public LocalizedString Hint
		{ get; set; }

		/// <summary>Surrogates of the concept's attributes.</summary>
		[XmlArray(nameof(Attributes))]
		public List<Attribute> Attributes
		{ get; set; } = new List<Attribute>();

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public Concept()
		{ }

		/// <summary>Converts a concept into its surrogate.</summary>
		/// <param name="concept">Concept to convert.</param>
		/// <exception cref="NotSupportedException">One of the concept's attribute types is not registered.</exception>
		public Concept(IConcept concept)
		{
			Name = new LocalizedString(concept.Name);
			Hint = new LocalizedString(concept.Hint);
			Attributes = concept.Attributes.Select(a => Attribute.Save(a)).ToList();
			ID = concept.ID;
		}

		#endregion

		/// <summary>
		/// Restores the concept from the surrogate. Name and hint always come back as editable
		/// per-locale strings, even if the original was a computed constant.
		/// </summary>
		/// <returns>A newly created concept with its attributes attached.</returns>
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
