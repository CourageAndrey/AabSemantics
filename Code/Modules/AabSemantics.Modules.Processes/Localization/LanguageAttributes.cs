﻿using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Processes.Localization
{
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		String IsProcess
		{ get; }

		String IsSequenceSign
		{ get; }
	}

	[XmlType("ProcessesAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String IsProcess
		{ get; set; }

		[XmlElement]
		public String IsSequenceSign
		{ get; set; }

		#endregion

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
