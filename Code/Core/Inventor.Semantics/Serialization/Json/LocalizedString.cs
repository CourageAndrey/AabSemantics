using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Json
{
	[DataContract]
	public class LocalizedString
	{
		#region Properties

		[DataMember]
		public Dictionary<String, String> Values
		{ get; set; }

		#endregion

		#region Constructors

		public LocalizedString(ILocalizedString source)
			: this(source.AsDictionary())
		{ }

		public LocalizedString(Dictionary<String, String> values)
		{
			Values = values;
		}

		public LocalizedString()
			: this(new Dictionary<String, String>())
		{ }

		#endregion

		public void LoadTo(ILocalizedString destination)
		{
			var variable = (LocalizedStringVariable) destination;
			variable.Clear();

			foreach (var value in Values)
			{
				variable.SetLocale(value.Key, value.Value);
			}
		}
	}
}
