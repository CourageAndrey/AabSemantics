using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Text.Primitives
{
	/// <summary>
	/// A localizable sentence: a format string chosen per language, plus the knowledge items its
	/// anchors stand for. The format string is resolved at render time, so the same node prints
	/// correctly in any language.
	/// </summary>
	public class FormattedText : TextBase
	{
		#region Properties

		/// <summary>Selects the format string from a language.</summary>
		public Func<ILanguage, String> Formatter
		{ get; }

		/// <summary>Knowledge items the format string refers to, keyed by anchor token.</summary>
		public IDictionary<String, IKnowledge> Parameters
		{ get; }

		#endregion

		/// <summary>Creates a localizable sentence.</summary>
		/// <param name="formatter">Selects the format string from a language.</param>
		/// <param name="parameters">Referenced knowledge items; copied. An empty map when <c>null</c>.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="formatter"/> is <c>null</c>.</exception>
		public FormattedText(Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters = null)
		{
			Formatter = formatter.EnsureNotNull(nameof(formatter));
			Parameters = parameters != null
				? new Dictionary<String, IKnowledge>(parameters)
				: new Dictionary<String, IKnowledge>();
		}

		/// <summary>Returns the sentence's own references.</summary>
		/// <returns>Anchor token to referenced item.</returns>
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return Parameters;
		}
	}
}