using System;

using AabSemantics.Localization;

namespace AabSemantics.Concepts
{
	/// <summary>
	/// Built-in concept contributed by a module. Its identifier is fixed, because serialized
	/// data and module code refer to it by that value.
	/// </summary>
	public class SystemConcept : Concept
	{
		#region Constructors

		/// <summary>Creates a built-in concept.</summary>
		/// <param name="id">Fixed identifier.</param>
		/// <param name="name">Localized display name.</param>
		/// <param name="hint">Localized tooltip text.</param>
		public SystemConcept(String id, LocalizedStringConstant name, LocalizedStringConstant hint)
			: base(id, name, hint)
		{ }

		#endregion

		/// <summary>Accepts only the identifier the concept already has.</summary>
		/// <param name="id">Identifier to set.</param>
		/// <exception cref="NotSupportedException"><paramref name="id"/> differs from the current one.</exception>
		public override void UpdateIdIfAllowed(String id)
		{
			if (id != ID)
			{
				throw new NotSupportedException();
			}
		}
	}
}
