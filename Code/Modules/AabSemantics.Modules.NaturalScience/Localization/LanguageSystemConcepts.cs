using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.NaturalScience.Localization
{
	public interface ILanguageSystemConcepts
	{
		String Science
		{ get; }

		String Physics
		{ get; }

		String Chemistry
		{ get; }

		String Astronomy
		{ get; }

		String Biology
		{ get; }

		String ChemicalElement
		{ get; }
	}

	[XmlType("NaturalScienseSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		[XmlElement]
		public String Science
		{ get; set; }

		[XmlElement]
		public String Physics
		{ get; set; }

		[XmlElement]
		public String Chemistry
		{ get; set; }

		[XmlElement]
		public String Astronomy
		{ get; set; }

		[XmlElement]
		public String Biology
		{ get; set; }

		[XmlElement]
		public String ChemicalElement
		{ get; set; }

		#endregion

		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				Science = "Science",
				Physics = "Physics",
				Chemistry = "Chemistry",
				Astronomy = "Astronomy",
				Biology = "Biology",
				ChemicalElement = "Chemical element",
			};
		}

		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				Science = "Systematic discipline that builds and organises knowledge in the form of testable hypotheses and predictions about the universe.",
				Physics = "Scientific study of matter, its fundamental constituents, its motion and behavior through space and time, and the related entities of energy and force.",
				Chemistry = "Scientific study of the properties and behavior of matter.",
				Astronomy = "Natural science that studies celestial objects and the phenomena that occur in the cosmos.",
				Biology = "Scientific study of life.It is a natural science with a broad scope but has several unifying themes that tie it together as a single, coherent field.",
				ChemicalElement = "Chemical substance whose atoms all have the same number of protons.",
			};
		}
	}
}
