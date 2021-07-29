using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Inventor.Core.Localization;
using Inventor.Core.Utils;

namespace Inventor.Core.Base
{
	public class KnowledgeBase : IKnowledgeBase
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public IKnowledgeBaseContext Context
		{ get; }

		public ICollection<IConcept> Concepts
		{ get; }

		public ICollection<IStatement> Statements
		{ get; }

		public event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		public event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		public event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		public event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

		#endregion

		public KnowledgeBase(ILanguage language)
		{
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

			foreach (var concept in SystemConcepts.GetAll())
			{
				Concepts.Add(concept);
			}

			Context = systemContext.Instantiate(this, new StatementRepository(), new QuestionRepository(), new AttributeRepository());

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
			return String.Format("{0} : {1}", Strings.TostringKnowledgeBase, Name);
		}

		#region Serialization

		public static KnowledgeBase Load(String fileName, ILanguage language)
		{
			return fileName.DeserializeFromFile<Xml.KnowledgeBase>().Load(language);
		}

		public void Save(String fileName)
		{
			new Xml.KnowledgeBase(this).SerializeToFile(fileName);
		}

		public event EventHandler Changed;

		private void raiseChanged()
		{
			var handler = Volatile.Read(ref Changed);
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion
	}
}
