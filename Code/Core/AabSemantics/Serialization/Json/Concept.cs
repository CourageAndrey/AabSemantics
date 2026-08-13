using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AabSemantics.Serialization.Json
{
	/// <summary>JSON surrogate of a concept.</summary>
	[DataContract]
	public class Concept
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[DataMember]
		public String ID
		{ get; set; }

		/// <summary>Localized display name.</summary>
		[DataMember]
		public LocalizedString Name
		{ get; set; }

		/// <summary>Localized tooltip text.</summary>
		[DataMember]
		public LocalizedString Hint
		{ get; set; }

		/// <summary>Element names of the concept's attributes; attributes carry no data of their own.</summary>
		[DataMember]
		public List<String> Attributes
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public Concept()
		{ }

		/// <summary>Converts a concept into its surrogate.</summary>
		/// <param name="concept">Concept to convert.</param>
		/// <exception cref="KeyNotFoundException">One of the concept's attribute types is not registered.</exception>
		public Concept(IConcept concept)
		{
			ID = concept.ID;
			Name = new LocalizedString(concept.Name);
			Hint = new LocalizedString(concept.Hint);
			Attributes = concept.Attributes.ToJson();
		}

		#endregion

		/// <summary>
		/// Restores the concept from the surrogate. Name and hint always come back as editable
		/// per-locale strings, even if the original was a computed constant.
		/// </summary>
		/// <returns>A newly created concept with its attributes attached.</returns>
		/// <exception cref="InvalidOperationException">One of the attribute element names is unknown.</exception>
		public IConcept Load()
		{
			var name = new Localization.LocalizedStringVariable();
			Name.LoadTo(name);

			var hint = new Localization.LocalizedStringVariable();
			Hint.LoadTo(hint);

			return new Concepts.Concept(ID, name, hint).WithAttributes(Attributes.ToAttributes());
		}
	}
}
