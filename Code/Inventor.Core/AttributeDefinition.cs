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

		public Xml.Attribute Xml
		{ get; }

		private readonly Func<ILanguage, String> _attributeNameGetter;

		#endregion

		#region Constructors

		public AttributeDefinition(Type attributeType, IAttribute attributeValue, Func<ILanguage, String> attributeNameGetter, Xml.Attribute xml)
		{
			if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
			if (attributeValue == null) throw new ArgumentNullException(nameof(attributeValue));
			if (!attributeType.IsInstanceOfType(attributeValue)) throw new InvalidCastException();
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));
			if (xml == null) throw new ArgumentNullException(nameof(xml));

			AttributeType = attributeType;
			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
			Xml = xml;
		}

		private AttributeDefinition()
		{
			AttributeType = typeof(NoAttribute);
			AttributeValue = new NoAttribute();
			_attributeNameGetter = language => language.Attributes.None;
			Xml = null;
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