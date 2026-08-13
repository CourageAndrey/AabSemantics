namespace AabSemantics.Modules.Processes.Attributes
{
	/// <summary>Marks a concept as one of the sequence signs.</summary>
	public class IsSequenceSignAttribute : IAttribute
	{
		private IsSequenceSignAttribute()
		{ }

		/// <summary>The single shared instance of the attribute.</summary>
		public static readonly IsSequenceSignAttribute Value = new IsSequenceSignAttribute();
	}
}
