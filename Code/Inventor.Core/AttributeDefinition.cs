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
			if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
			if (attributeValue == null) throw new ArgumentNullException(nameof(attributeValue));
			if (!attributeType.IsInstanceOfType(attributeValue)) throw new InvalidCastException();
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));

			AttributeType = attributeType;
			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
		}

		private AttributeDefinition()
		{
			AttributeType = typeof(NoAttribute);
			AttributeValue = new NoAttribute();
			_attributeNameGetter = language => language.Attributes.None;
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _attributeNameGetter(language);
		}

		public static readonly AttributeDefinition None = new AttributeDefinition();

		private class NoAttribute : IAttribute
		{ }
	}
}