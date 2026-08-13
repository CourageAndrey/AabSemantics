namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Row of the custom statement editor, pairing a role name with the chosen concept.</summary>
	public class ConceptWithKey
	{
		/// <summary>Role name.</summary>
		public string Key
		{ get; set; }

		/// <summary>The concept in question.</summary>
		public ConceptItem Concept
		{ get; set; }

		/// <summary>Creates the row.</summary>
		/// <param name="key">Role name.</param>
		/// <param name="concept">Concept filling the role.</param>
		public ConceptWithKey(string key, ConceptItem concept)
		{
			Key = key;
			Concept = concept;
		}
	}
}
