using System.Collections.Generic;
using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	public class Concept : IKnowledgeViewModel
	{
		#region Properties

		public LocalizedString Name
		{ get; }

		public LocalizedString Hint
		{ get; }

		public List<ConceptAttribute> Attributes
		{ get; } = new List<ConceptAttribute>();

		#endregion

		#region Constructors

		public Concept(ILanguage language)
			: this(
				new LocalizedStringVariable(new Dictionary<string, string> { { language.Culture, string.Empty }, }),
				new LocalizedStringVariable())
		{ }

		public Concept(Core.Base.Concept concept)
			: this(LocalizedString.From(concept.Name), LocalizedString.From(concept.Hint))
		{
			BoundObject = concept;
		}

		public Concept(LocalizedString name, LocalizedString hint)
		{
			Name = name;
			Hint = hint;
		}

		#endregion

		#region Implementation of IViewModel

		public Core.Base.Concept BoundObject
		{ get; private set; }

		public Window CreateEditDialog(Window owner, Core.ISemanticNetwork semanticNetwork, ILanguage language)
		{
			updateAttributes(semanticNetwork.Context.AttributeRepository, language);
			var control = new ConceptControl
			{
				EditValue = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.Misc.Concept,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		private void updateAttributes(Core.IAttributeRepository attributeRepository, ILanguage language)
		{
			Attributes.Clear();
			Attributes.Add(new ConceptAttribute(Core.AttributeDefinition.None, language, BoundObject == null || BoundObject.Attributes.Count == 0));
			foreach (var attributeDefinition in attributeRepository.AttributeDefinitions.Values)
			{
				Attributes.Add(new ConceptAttribute(attributeDefinition, language, BoundObject != null && BoundObject.Attributes.Contains(attributeDefinition.AttributeValue)));
			}
		}

		public object ApplyCreate(Core.ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.Concepts.Add(BoundObject = new Core.Base.Concept(Name.Create(), Hint.Create()));

			foreach (var attribute in Attributes)
			{
				if (attribute.IsOn && attribute.Value != null)
				{
					BoundObject.WithAttribute(attribute.Value);
				}
			}

			return BoundObject;
		}

		public void ApplyUpdate()
		{
			Name?.Apply(BoundObject.Name);
			Hint?.Apply(BoundObject.Hint);

			BoundObject.WithoutAttributes();
			foreach (var attribute in Attributes)
			{
				if (attribute.IsOn && attribute.Value != null)
				{
					BoundObject.WithAttribute(attribute.Value);
				}
			}
		}

		#endregion
	}
}
