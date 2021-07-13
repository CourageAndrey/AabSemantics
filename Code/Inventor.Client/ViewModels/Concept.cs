using System.Collections.Generic;
using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;

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

		public Concept()
			: this(new LocalizedStringVariable(), new LocalizedStringVariable())
		{ }

		public Concept(Core.Base.Concept concept)
			: this(LocalizedString.From(concept.Name), LocalizedString.From(concept.Hint))
		{
			_boundObject = concept;
		}

		public Concept(LocalizedString name, LocalizedString hint)
		{
			Name = name;
			Hint = hint;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Base.Concept _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, ILanguage language)
		{
			updateAttributes(knowledgeBase.Context.AttributeRepository, language);
			var control = new ConceptControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
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
			Attributes.Add(new ConceptAttribute(Core.AttributeDefinition.None, language, _boundObject == null || _boundObject.Attributes.Count == 0));
			foreach (var attributeDefinition in attributeRepository.AttributeDefinitions.Values)
			{
				Attributes.Add(new ConceptAttribute(attributeDefinition, language, _boundObject != null && _boundObject.Attributes.Contains(attributeDefinition.AttributeValue)));
			}
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Concepts.Add(_boundObject = new Core.Base.Concept(Name.Create(), Hint.Create()));

			foreach (var attribute in Attributes)
			{
				if (attribute.IsOn && attribute.Value != null)
				{
					_boundObject.Attributes.Add(attribute.Value);
				}
			}
		}

		public void ApplyUpdate()
		{
			Name?.Apply(_boundObject.Name);
			Hint?.Apply(_boundObject.Hint);

			_boundObject.Attributes.Clear();
			foreach (var attribute in Attributes)
			{
				if (attribute.IsOn && attribute.Value != null)
				{
					_boundObject.Attributes.Add(attribute.Value);
				}
			}
		}

		#endregion
	}
}
