using System.Collections.Generic;
using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Metadata;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Editable view over a concept: its identifier, localized name and hint, and its attributes.</summary>
	public class Concept : IKnowledgeViewModel
	{
		#region Properties

		/// <summary>Editable view over the concept's name.</summary>
		public LocalizedString Name
		{ get; }

		/// <summary>Identifier of the edited concept.</summary>
		public string ID
		{ get; set; }

		/// <summary>Editable view over the concept's hint.</summary>
		public LocalizedString Hint
		{ get; }

		/// <summary>Attributes selectable for the concept, each flagged as checked or not.</summary>
		public List<ConceptAttribute> Attributes
		{ get; } = new List<ConceptAttribute>();

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new concept.</summary>
		/// <param name="language">Language the dialog is localized in.</param>
		public Concept(ILanguage language)
			: this(null,
				new LocalizedStringVariable(new Dictionary<string, string> { { language.Culture, string.Empty }, }), new LocalizedStringVariable())
		{ }

		/// <summary>Creates a view model bound to an existing concept.</summary>
		/// <param name="concept">Concept to edit.</param>
		public Concept(AabSemantics.Concepts.Concept concept)
			: this(concept.ID, LocalizedString.From(concept.Name), LocalizedString.From(concept.Hint))
		{
			BoundObject = concept;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier; empty when creating a new concept.</param>
		/// <param name="name">Editable name.</param>
		/// <param name="hint">Editable hint.</param>
		public Concept(string id, LocalizedString name, LocalizedString hint)
		{
			Name = name;
			ID = id;
			Hint = hint;
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>The concept being edited, or <c>null</c> while creating a new one.</summary>
		public AabSemantics.Concepts.Concept BoundObject
		{ get; private set; }

		/// <summary>Builds the dialog used to edit this concept.</summary>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="semanticNetwork">Network the concept belongs to.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>An unshown dialog.</returns>
		public Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
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

		private void updateAttributes(IMetadataRepository<AttributeDefinition> attributeRepository, ILanguage language)
		{
			Attributes.Clear();
			Attributes.Add(new ConceptAttribute(AttributeDefinition.None, language, BoundObject == null || BoundObject.Attributes.Count == 0));
			foreach (var attributeDefinition in attributeRepository.Definitions.Values)
			{
				Attributes.Add(new ConceptAttribute(attributeDefinition, language, BoundObject != null && BoundObject.Attributes.Contains(attributeDefinition.Value)));
			}
		}

		/// <summary>Creates the concept from the edited values and adds it to the network.</summary>
		/// <param name="semanticNetwork">Network to add the concept to.</param>
		/// <returns>The created concept.</returns>
		public object ApplyCreate(ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.Concepts.Add(BoundObject = new Concepts.Concept(ID, Name.Create(), Hint.Create()));

			foreach (var attribute in Attributes)
			{
				if (attribute.IsOn && attribute.Value != null)
				{
					BoundObject.WithAttribute(attribute.Value);
				}
			}

			return BoundObject;
		}

		/// <summary>Writes the edited values onto the bound concept.</summary>
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
