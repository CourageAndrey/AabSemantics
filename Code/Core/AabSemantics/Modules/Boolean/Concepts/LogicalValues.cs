using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Localization;

namespace AabSemantics.Modules.Boolean.Concepts
{
	/// <summary>
	/// The two logical values as concepts, plus conversions between them and <see cref="System.Boolean"/>.
	/// Both are system concepts with fixed identifiers and carry <see cref="IsBooleanAttribute"/>.
	/// </summary>
	public static class LogicalValues
	{
		#region Properties

		/// <summary>The logical "true" concept.</summary>
		public static readonly IConcept True = new SystemConcept(
			$"{{{nameof(LogicalValues)}.{nameof(True)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageBooleanModule, ILanguageConcepts>().SystemConceptNames.True),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageBooleanModule, ILanguageConcepts>().SystemConceptHints.True));

		/// <summary>The logical "false" concept.</summary>
		public static readonly IConcept False = new SystemConcept(
			$"{{{nameof(LogicalValues)}.{nameof(False)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageBooleanModule, ILanguageConcepts>().SystemConceptNames.False),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageBooleanModule, ILanguageConcepts>().SystemConceptHints.False));

		/// <summary>Both logical values; the helpers below reject any concept outside this set.</summary>
		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			True,
			False,
		};

		#endregion

		private static void ensureSuits(this IConcept value)
		{
			if (!All.Contains(value))
			{
				throw new InvalidOperationException("This method can work only with logical values.");
			}
		}

		/// <summary>Returns the opposite logical value.</summary>
		/// <param name="value">One of the two logical values.</param>
		/// <returns>The other logical value.</returns>
		/// <exception cref="InvalidOperationException"><paramref name="value"/> is not a logical value.</exception>
		public static IConcept Invert(this IConcept value)
		{
			ensureSuits(value);

			if (value == True)
			{
				return False;
			}
			else if (value == False)
			{
				return True;
			}
			else
			{
				return value;
			}
		}

		/// <summary>Converts a logical value concept into a CLR boolean.</summary>
		/// <param name="value">One of the two logical values.</param>
		/// <returns><c>true</c> for <see cref="True"/>, <c>false</c> for <see cref="False"/>.</returns>
		/// <exception cref="InvalidOperationException"><paramref name="value"/> is not a logical value.</exception>
		public static System.Boolean ToBoolean(this IConcept value)
		{
			ensureSuits(value);

			if (value == True)
			{
				return true;
			}
			else if (value == False)
			{
				return false;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Converts a CLR boolean into the matching logical value concept.</summary>
		/// <param name="value">Value to convert.</param>
		/// <returns><see cref="True"/> or <see cref="False"/>.</returns>
		public static IConcept ToLogicalValue(this System.Boolean value)
		{
			return value
				? True
				: False;
		}

		static LogicalValues()
		{
			foreach (var concept in All)
			{
				concept.WithAttribute(IsBooleanAttribute.Value);
			}
		}
	}
}