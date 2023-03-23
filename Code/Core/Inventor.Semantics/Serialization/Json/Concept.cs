using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Inventor.Semantics.Serialization.Json
{
	[DataContract]
	public class Concept
	{
		#region Properties

		[DataMember]
		public String ID
		{ get; set; }

		[DataMember]
		public LocalizedString Name
		{ get; set; }

		[DataMember]
		public LocalizedString Hint
		{ get; set; }

		[DataMember]
		public List<String> Attributes
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		public Concept()
		{ }

		public Concept(IConcept concept)
		{
			ID = concept.ID;
			Name = new LocalizedString(concept.Name);
			Hint = new LocalizedString(concept.Hint);
			Attributes = concept.Attributes.ToJson();
		}

		#endregion

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
