using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public interface ILanguage
	{
		String Name
		{ get; }

		String Culture
		{ get; }

		ILanguageAttributes Attributes
		{ get; }

		ILanguageStatements Statements
		{ get; }

		ILanguageQuestions Questions
		{ get; }

		ICollection<LanguageExtension> Extensions
		{ get; }
	}

	public static class LanguagesExtensions
	{
		public static ExtensionT GetExtension<ExtensionT>(this ILanguage language)
		{
			return language.Extensions.OfType<ExtensionT>().First();
		}
	}
}
