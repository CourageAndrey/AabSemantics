namespace AabSemantics.Modules.Processes.Attributes
{
	/// <summary>Marks a concept as a process, i.e. something with a start and a finish.</summary>
	public class IsProcessAttribute : IAttribute
	{
		private IsProcessAttribute()
		{ }

		/// <summary>The single shared instance of the attribute.</summary>
		public static readonly IsProcessAttribute Value = new IsProcessAttribute();
	}
}
