using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public abstract class Question
	{
		#region Supported questions

		public static readonly IDictionary<Func<string>, Func<Question>> AllSupportedTypes = new Dictionary<Func<string>, Func<Question>>();

		static Question()
		{
			var propertyNames = new Dictionary<string, Func<string>>();
			foreach (var property in typeof (LanguageQuestionNames).GetProperties())
			{
				propertyNames[property.Name] = () => (string) property.GetValue(LanguageEx.CurrentEx.QuestionNames);
			}
			foreach (var questionType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof (Question).IsAssignableFrom(t) && !t.IsAbstract))
			{
				AllSupportedTypes[propertyNames[questionType.Name]] = () => Activator.CreateInstance(questionType) as Question;
			}
		}

		#endregion
	}

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