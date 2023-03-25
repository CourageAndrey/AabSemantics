using System.Collections.Generic;
using System.Windows;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.WPF.Controls;
using Inventor.Semantics.WPF.Dialogs;

namespace Inventor.Semantics.WPF.ViewModels
{
	public class Concept : IKnowledgeViewModel
	{
		#region Properties

		public LocalizedString Name
		{ get; }

		public string ID
		{ get; set; }

		public LocalizedString Hint
		{ get; }

		public List<ConceptAttribute> Attributes
		{ get; } = new List<ConceptAttribute>();

		#endregion

		#region Constructors

		public Concept(ILanguage language)
			: this(null,
				new LocalizedStringVariable(new Dictionary<string, string> { { language.Culture, string.Empty }, }), new LocalizedStringVariable())
		{ }

		public Concept(Semantics.Concepts.Concept concept)
			: this(concept.ID, LocalizedString.From(concept.Name), LocalizedString.From(concept.Hint))
		{
			BoundObject = concept;
		}

		public Concept(string id, LocalizedString name, LocalizedString hint)
		{
			Name = name;
			ID = id;
			Hint = hint;
		}

		#endregion

		#region Implementation of IViewModel

		public Semantics.Concepts.Concept BoundObject
		{ get; private set; }

		public Window CreateEditDialog(Window owner, Semantics.ISemanticNetwork semanticNetwork, ILanguage language)
		{
			updateAttributes(Repositories.Attributes, language);
			var control = new ConceptControl
			{
				EditValue = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<IWpfUiModule>().Misc.Concept,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		private void updateAttributes(Semantics.IRepository<AttributeDefinition> attributeRepository, ILanguage language)
		{
			Attributes.Clear();
			Attributes.Add(new ConceptAttribute(AttributeDefinition.None, language, BoundObject == null || BoundObject.Attributes.Count == 0));
			foreach (var attributeDefinition in attributeRepository.Definitions.Values)
			{
				Attributes.Add(new ConceptAttribute(attributeDefinition, language, BoundObject != null && BoundObject.Attributes.Contains(attributeDefinition.Value)));
			}
		}

		public object ApplyCreate(Semantics.ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.Concepts.Add(BoundObject = new Semantics.Concepts.Concept(ID, Name.Create(), Hint.Create()));

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
			BoundObject.UpdateIdIfAllowed(ID);
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
