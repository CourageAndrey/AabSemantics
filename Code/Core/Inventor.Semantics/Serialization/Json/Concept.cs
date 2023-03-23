using System;
using System.Collections.Generic;

namespace Inventor.Semantics.Serialization.Json
{
	[Serializable]
	public class Concept
	{
		#region Properties

		public String ID
		{ get; set; }

		public LocalizedString Name
		{ get; set; }

		public LocalizedString Hint
		{ get; set; }

		public List<String> Attributes
		{ get; set; } = new List<String>();

		#endregion

		#region Constructors

		public Concept()
		{ }

		public Concept(IConcept concept)
		{
			Name = new LocalizedString(concept.Name);
			Hint = new LocalizedString(concept.Hint);
			Attributes = concept.Attributes.ToJson();
			ID = concept.ID;
			
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
