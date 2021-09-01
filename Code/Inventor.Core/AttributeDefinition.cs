using System;

namespace Inventor.Core
{
	public class AttributeDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		public IAttribute AttributeValue
		{ get; }

		public Xml.Attribute Xml
		{ get; }

		private readonly Func<ILanguage, String> _attributeNameGetter;

		#endregion

		#region Constructors

		public AttributeDefinition(Type type, IAttribute attributeValue, Func<ILanguage, String> attributeNameGetter, Xml.Attribute xml)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (attributeValue == null) throw new ArgumentNullException(nameof(attributeValue));
			if (!type.IsInstanceOfType(attributeValue)) throw new InvalidCastException();
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));
			if (xml == null) throw new ArgumentNullException(nameof(xml));

			Type = type;
			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
			Xml = xml;
		}

		private AttributeDefinition()
		{
			Type = typeof(NoAttribute);
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