namespace AabSemantics.Extensions.WPF.ViewModels
{
	public class ConceptWithKey
	{
		public string Key
		{ get; set; }

		public ConceptItem Concept
		{ get; set; }

		public ConceptWithKey(string key, ConceptItem concept)
		{
			Key = key;
			Concept = concept;
		}
	}
}
