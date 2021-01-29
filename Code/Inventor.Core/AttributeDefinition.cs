using System;

namespace Inventor.Core
{
	public class AttributeDefinition
	{
		#region Properties

		public Type AttributeType
		{ get; }

		public IAttribute AttributeValue
		{ get; }

		private readonly Func<ILanguage, String> _attributeNameGetter;

		#endregion

		#region Constructors

		public AttributeDefinition(Type attributeType, IAttribute attributeValue, Func<ILanguage, String> attributeNameGetter)
		{
			AttributeType = attributeType;
			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _attributeNameGetter(language);
		}
	}
}