using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Processes.Localization
{
	/// <summary>Names of the attributes contributed by the processes module.</summary>
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		/// <summary>Name of the "is a process" attribute.</summary>
		String IsProcess
		{ get; }

		/// <summary>Name of the "is a sequence sign" attribute.</summary>
		String IsSequenceSign
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAttributes"/>, loaded from a language file.</summary>
	[XmlType("ProcessesAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		/// <summary>Name of the "is a process" attribute.</summary>
		[XmlElement]
		public String IsProcess
		{ get; set; }

		/// <summary>Name of the "is a sequence sign" attribute.</summary>
		[XmlElement]
		public String IsSequenceSign
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsProcess = "Is Process",
				IsSequenceSign = "Is Processes Sequence Sign",
			};
		}
	}
}
