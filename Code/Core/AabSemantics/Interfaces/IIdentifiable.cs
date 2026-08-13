using System;

namespace AabSemantics
{
	/// <summary>
	/// Anything addressable by a stable string identifier. Identifiers are what serialization
	/// formats and text anchors reference, so they must survive a save/load round trip.
	/// </summary>
	public interface IIdentifiable
	{
		/// <summary>
		/// Identifier, unique within the semantic network the item belongs to.
		/// </summary>
		String ID
		{ get; }
	}

	/// <summary>
	/// Helpers for producing and formatting identifiers.
	/// </summary>
	public static class IdHelper
	{
		/// <summary>
		/// Returns the value unchanged, or a fresh GUID when it is null or empty.
		/// Use it in constructors so that every item ends up with an identifier
		/// whether or not the caller supplied one.
		/// </summary>
		/// <param name="value">Candidate identifier; may be null or empty.</param>
		/// <returns>The original value, or a newly generated GUID string.</returns>
		public static String EnsureIdIsSet(this String value)
		{
			return !String.IsNullOrEmpty(value)
				? value
				: Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Formats the item as <c>TypeName [ID]</c>, for diagnostics and exception messages.
		/// </summary>
		/// <param name="instance">Item to format.</param>
		/// <returns>Human-readable type-and-identifier string.</returns>
		public static String GetTypeWithId(this IIdentifiable instance)
		{
			return $"{instance.GetType().Name} [{instance.ID}]";
		}
	}
}
