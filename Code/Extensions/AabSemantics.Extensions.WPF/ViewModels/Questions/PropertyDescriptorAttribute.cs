using System;

namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>
	/// Marks a question view model's property as an editable parameter, telling the dialog whether
	/// it is mandatory and where to find its caption in the language files.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class PropertyDescriptorAttribute : Attribute
	{
		#region Properties

		/// <summary>Whether the parameter must be filled in.</summary>
		public bool Required
		{ get; set; }

		/// <summary>Dot-separated path to the parameter's caption inside the language.</summary>
		public string NamePath
		{ get; set; }

		#endregion

		/// <summary>Describes an editable question parameter.</summary>
		/// <param name="required">Whether the parameter must be filled in.</param>
		/// <param name="namePath">Dot-separated path to the caption inside the language, as understood by <c>GetBoundText</c>.</param>
		public PropertyDescriptorAttribute(bool required, string namePath)
		{
			Required = required;
			NamePath = namePath;
		}
	}
}