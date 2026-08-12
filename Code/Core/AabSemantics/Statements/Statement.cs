using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Utils;

namespace AabSemantics.Statements
{
	public abstract class Statement : IStatement
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public String ID
		{ get; private set; }

		public IContext Context
		{ get; set; }

		public ILocalizedString Hint
		{ get; }

		#endregion

		public abstract IEnumerable<IConcept> GetChildConcepts();

		public sealed override String ToString()
		{
			return this.GetTypeWithId();
		}

		protected Statement(String id, ILocalizedString name, ILocalizedString hint = null)
		{
			Update(id);

			Name = name.EnsureNotNull(nameof(name));

			Hint = hint ?? LocalizedString.Empty;
		}

		public void Update(String id)
		{
			ID = id.EnsureIdIsSet();
		}

		public abstract Task<Boolean> CheckUniqueAsync(IEnumerable<IStatement> statements);

#pragma warning disable 659
		public abstract override Boolean Equals(Object obj);

		public override Int32 GetHashCode()
		{
// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
#pragma warning restore 659
	}

	public abstract class Statement<StatementT> : Statement, IEquatable<StatementT>
		where StatementT : Statement<StatementT>
	{
		protected Statement(String id, LocalizedString name, LocalizedString hint = null)
			: base(id, name, hint)
		{ }

		public sealed override async Task<Boolean> CheckUniqueAsync(IEnumerable<IStatement> statements)
		{
			return await Task.Run(() =>
			{
				bool found = false;
				foreach (var _ in statements.OfType<StatementT>().Where(s => Equals(s)))
				{
					if (!found)
					{
						found = true;
					}
					else
					{
						return Task.FromResult(false);
					}
				}

				return Task.FromResult(true);
			}).ConfigureAwait(false);
		}

		public abstract Boolean Equals(StatementT other);

#pragma warning disable 659
		public sealed override Boolean Equals(Object obj)
		{
			return Equals(obj as StatementT);
		}

		public override Int32 GetHashCode()
		{
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
#pragma warning restore 659
	}
}
