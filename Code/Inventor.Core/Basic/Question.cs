using System;

namespace Inventor.Core
{
	public abstract class Question
	{ }

	#region Attributes

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class QuestionDescriptorAttribute : Attribute
	{ }

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

	#endregion
}