using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>
	/// Base XML surrogate of a concept attribute. Attributes are stateless, so a surrogate carries
	/// no data: the element name alone identifies which attribute it is.
	/// <para>
	/// Note the naming: <see cref="Load"/> goes from surrogate to attribute, while
	/// <see cref="Save"/> goes from attribute to surrogate.
	/// </para>
	/// </summary>
	[XmlType]
	public abstract class Attribute
	{
		/// <summary>Returns the attribute this surrogate stands for.</summary>
		/// <returns>The attribute instance.</returns>
		public abstract IAttribute Load();

		/// <summary>Returns the surrogate registered for an attribute.</summary>
		/// <param name="attribute">Attribute to convert.</param>
		/// <returns>The shared surrogate instance from the attribute's metadata.</returns>
		/// <exception cref="System.NotSupportedException">The attribute's type is not registered.</exception>
		public static Attribute Save(IAttribute attribute)
		{
			var definition = Repositories.Attributes.Definitions.GetSuitable(attribute);
			return definition.GetSerializationSettings<AttributeXmlSerializationSettings>().Xml;
		}
	}

	/// <summary>XML surrogate of one concrete attribute type.</summary>
	/// <typeparam name="AttributeT">Attribute type represented.</typeparam>
	[XmlType]
	public abstract class Attribute<AttributeT> : Attribute
		where AttributeT : IAttribute
	{
		/// <summary>Returns the attribute this surrogate stands for.</summary>
		/// <returns>The attribute instance.</returns>
		public override IAttribute Load()
		{
			return LoadTyped();
		}

		/// <summary>Returns the attribute this surrogate stands for, strongly typed.</summary>
		/// <returns>The attribute instance, normally the type's shared singleton.</returns>
		public abstract AttributeT LoadTyped();
	}
}