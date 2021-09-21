using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using Inventor.Core.Contexts;
using Inventor.Core.Localization;
using Inventor.Core.Utils;

namespace Inventor.Core
{
	public class SemanticNetwork : ISemanticNetwork
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public ISemanticNetworkContext Context
		{ get; }

		public IKeyedCollection<IConcept> Concepts
		{ get; }

		public IKeyedCollection<IStatement> Statements
		{ get; }

		public IDictionary<String, IExtensionModule> Modules
		{ get; }

		public event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		public event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		public event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		public event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

		#endregion

		public SemanticNetwork(ILanguage language)
		{
			Modules = new Dictionary<String, IExtensionModule>();

			var name = new LocalizedStringVariable();
			name.SetLocale(language.Culture, Strings.NewKbName);
			Name = name;

			var systemContext = new SystemContext(language);

			var concepts = new EventCollection<IConcept>();
			concepts.ItemAdded += (sender, args) =>
			{
				var handler = Volatile.Read(ref ConceptAdded);
				if (handler != null)
				{
					handler(sender, args);
				}
			};
			concepts.ItemRemoved += (sender, args) =>
			{
				var handler = Volatile.Read(ref ConceptRemoved);
				if (handler != null)
				{
					handler(sender, args);
				}
				foreach (var statement in Statements.Where(r => r.GetChildConcepts().Contains(args.Item)).ToList())
				{
					Statements.Remove(statement);
				}
			};
			Concepts = concepts;

			var statements = new EventCollection<IStatement>();
			statements.ItemAdded += (sender, args) =>
			{
				if (args.Item.Context == null)
				{
					var context = Context as IContext ?? systemContext;
					args.Item.Context = context;
				}
				args.Item.Context.Scope.Add(args.Item);

				var handler = Volatile.Read(ref StatementAdded);
				if (handler != null)
				{
					handler(sender, args);
				}
				foreach (var concept in args.Item.GetChildConcepts())
				{
					if (!Concepts.Contains(concept))
					{
						Concepts.Add(concept);
					}
				}
			};
			statements.ItemRemoved += (sender, args) =>
			{
				if (args.Item.Context == Context || args.Item.Context == systemContext)
				{
					args.Item.Context.Scope.Remove(args.Item);
					args.Item.Context = null;
				}

				var handler = Volatile.Read(ref StatementRemoved);
				if (handler != null)
				{
					handler(sender, args);
				}
			};
			Statements = statements;

			Context = systemContext.Instantiate(this);

			EventHandler<CancelableItemEventArgs<IStatement>> systemStatementProtector = (sender, args) =>
			{
				if (args.Item.Context != null && args.Item.Context.IsSystem)
				{
					args.IsCanceled = true;
				}
			};
			statements.ItemAdding += systemStatementProtector;
			statements.ItemRemoving += systemStatementProtector;
		}

		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} : {1}", Strings.TostringSemanticNetwork, Name);
		}
	}
}