using System;
using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Semantics;
using Inventor.Semantics.Metadata;

namespace Inventor.Test.Metadata
{
	[TestFixture]
	public class RepositoriesTest
	{
		[Test]
		public void ImpossibleToSetNullModules()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Modules = null);

			Assert.DoesNotThrow(() => Repositories.Modules = new Dictionary<string, IExtensionModule>());
		}

		[Test]
		public void ImpossibleToSetNullAttributes()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Attributes = null);

			Assert.DoesNotThrow(() => Repositories.Attributes = new Repository<AttributeDefinition>());
		}

		[Test]
		public void ImpossibleToSetNullStatements()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Statements = null);

			Assert.DoesNotThrow(() => Repositories.Statements = new Repository<StatementDefinition>());
		}

		[Test]
		public void ImpossibleToSetNullQuestions()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Questions = null);

			Assert.DoesNotThrow(() => Repositories.Questions = new Repository<QuestionDefinition>());
		}
	}
}
