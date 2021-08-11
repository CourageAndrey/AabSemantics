﻿using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class AddConceptCommand : BaseEditCommand
	{
		#region Properties

		public ViewModels.Concept ViewModel
		{ get; }

		public IConcept NewItem
		{ get; private set; }

		#endregion

		public AddConceptCommand(ViewModels.Concept viewModel, ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
		{
			ViewModel = viewModel;
		}

		public override void Apply()
		{
			if (NewItem == null)
			{
				NewItem = (IConcept) ViewModel.ApplyCreate(SemanticNetwork);
			}
			else
			{
				SemanticNetwork.Concepts.Add(NewItem);
			}
		}

		public override void Rollback()
		{
			SemanticNetwork.Concepts.Remove(NewItem);
		}
	}
}