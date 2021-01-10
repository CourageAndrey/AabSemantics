using System;

namespace Inventor.Core
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class PropertyDescriptorAttribute : Attribute
	{
		#region Properties

		public Boolean Required
		{ get; set; }

		public String NamePath
		{ get; set; }

		#endregion

		public PropertyDescriptorAttribute(Boolean required, String namePath)
		{
			Required = required;
			NamePath = namePath;
		}
	}
}