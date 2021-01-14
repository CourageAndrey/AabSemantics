namespace Inventor.Client.ViewModels
{
	public class Concept
	{
		public LocalizedString Name
		{ get; }

		public LocalizedString Hint
		{ get; }

		public Concept()
			: this(new LocalizedString(), new LocalizedString())
		{ }

		public Concept(Core.Base.Concept concept)
			: this(LocalizedString.From(concept.Name), LocalizedString.From(concept.Hint))
		{ }

		public Concept(LocalizedString name, LocalizedString hint)
		{
			Name = name;
			Hint = hint;
		}
	}
}
