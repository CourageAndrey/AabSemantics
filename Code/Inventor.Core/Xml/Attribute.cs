using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public abstract class Attribute
	{
		public abstract IAttribute Load();

		public static Attribute Save(IAttribute attribute)
		{
			if (attribute is Attributes.IsBooleanAttribute)
			{
				return new IsBooleanAttribute();
			}
			else if (attribute is Attributes.IsComparisonSignAttribute)
			{
				return new IsComparisonSignAttribute();
			}
			else if (attribute is Attributes.IsProcessAttribute)
			{
				return new IsProcessAttribute();
			}
			else if (attribute is Attributes.IsSequenceSignAttribute)
			{
				return new IsSequenceSignAttribute();
			}
			else if (attribute is Attributes.IsSignAttribute)
			{
				return new IsSignAttribute();
			}
			else if (attribute is Attributes.IsValueAttribute)
			{
				return new IsValueAttribute();
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}
}