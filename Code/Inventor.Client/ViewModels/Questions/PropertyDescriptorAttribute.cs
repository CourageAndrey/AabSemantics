using System;

namespace Inventor.Client.ViewModels.Questions
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class PropertyDescriptorAttribute : Attribute
	{
		#region Properties

		public bool Required
		{ get; set; }

		public string NamePath
		{ get; set; }

		#endregion

		public PropertyDescriptorAttribute(bool required, string namePath)
		{
			Required = required;
			NamePath = namePath;
		}
	}
}