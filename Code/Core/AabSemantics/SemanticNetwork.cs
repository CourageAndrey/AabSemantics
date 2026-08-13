using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using AabSemantics.Contexts;
using AabSemantics.Localization;
using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// Default in-memory <see cref="ISemanticNetwork"/>. Its constructor wires up the rules that
	/// keep the network coherent: removing a concept removes the statements about it, adding a
	/// statement adopts any concept it mentions, and statements owned by the system context
	/// cannot be added or removed.
	/// </summary>
	public class SemanticNetwork : ISemanticNetwork
	{
		#region Properties

		/// <summary>Localized name of the network.</summary>
		public ILocalizedString Name
		{ get; }

		/// <summary>Context the network's knowledge lives in.</summary>
		public ISemanticNetworkContext Context
		{ get; }

		/// <summary>Concepts known to the network.</summary>
		public IRepository<IConcept> Concepts
		{ get; }

		/// <summary>Statements known to the network.</summary>
		public IRepository<IStatement> Statements
		{ get; }

		/// <summary>Extension modules attached to the network, keyed by module name.</summary>
		public IDictionary<String, IExtensionModule> Modules
		{ get; }

		#endregion

		/// <summary>Creates an empty network with no modules attached.</summary>
		/// <param name="language">Language for text the network produces.</param>
		public SemanticNetwork(ILanguage language)
		{
			Modules = new Dictionary<String, IExtensionModule>();

			var name = new LocalizedStringVariable();
			name.SetLocale(language.Culture, Strings.NewKbName);
			Name = name;

			var systemContext = new SystemContext(language);

			var concepts = new Repository<IConcept>();
			concepts.ItemRemoved += (sender, args) =>
			{
				foreach (var statement in Statements.Where(r => r.GetChildConcepts().Contains(args.Item)).ToList())
				{
					Statements.Remove(statement);
				}
			};
			Concepts = concepts;

			var statements = new Repository<IStatement>();
			statements.ItemAdded += (sender, args) =>
			{
				if (args.Item.Context == null)
				{
					var context = Context as IContext ?? systemContext;
					args.Item.Context = context;
				}
				args.Item.Context.Scope.Add(args.Item);

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

		/// <summary>Formats the network as a caption followed by its name.</summary>
		/// <returns>Diagnostic string.</returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} : {1}", Strings.TostringSemanticNetwork, Name);
		}
	}
}