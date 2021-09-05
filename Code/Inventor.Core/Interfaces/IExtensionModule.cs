using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Metadata;

namespace Inventor.Core
{
	public interface IExtensionModule
	{
		String Name
		{ get; }

		ICollection<String> Dependencies
		{ get; }

		void AttachTo(ISemanticNetwork semanticNetwork);

		void RegisterMetadata();
	}

	public abstract class ExtensionModule : IExtensionModule
	{
		public String Name
		{ get; }

		public ICollection<String> Dependencies
		{ get; }

		public Boolean IsMetadataRegistered
		{ get { return Repositories.Modules.ContainsKey(Name); } }

		protected ExtensionModule(String name, ICollection<String> dependencies = null)
		{
			Name = name;
			Dependencies = dependencies ?? Array.Empty<String>();
		}

		public void AttachTo(ISemanticNetwork semanticNetwork)
		{
			if (semanticNetwork.Modules.ContainsKey(Name)) return;

			var missingDependencies = semanticNetwork.GetMissingDependencies(this);
			if (missingDependencies.Count > 0)
			{
				throw new ModuleException(Name, $"Impossible to apply \"{Name}\" module. Required modules \"{String.Join("\", \"", missingDependencies)}\" are not loaded.");
			}

			if (!IsMetadataRegistered)
			{
				RegisterMetadata();
			}

			Attach(semanticNetwork);

			semanticNetwork.Modules[Name] = this;
		}

		protected virtual void Attach(ISemanticNetwork semanticNetwork)
		{ }

		public virtual void RegisterMetadata()
		{
			if (!IsMetadataRegistered)
			{
				RegisterAttributes();
				RegisterStatements();
				RegisterQuestions();

				Repositories.Modules[Name] = this;
			}
		}

		protected virtual void RegisterAttributes()
		{ }

		protected virtual void RegisterStatements()
		{ }

		protected virtual void RegisterQuestions()
		{ }
	}

	public static class ExtensionModuleRegistrationHelper
	{
		public static ISemanticNetwork WithModule<ModuleT>(this ISemanticNetwork semanticNetwork)
			where ModuleT : IExtensionModule, new()
		{
			var module = new ModuleT();
			module.AttachTo(semanticNetwork);
			return semanticNetwork;
		}

		public static ICollection<String> GetMissingDependencies(this ISemanticNetwork semanticNetwork, IExtensionModule module)
		{
			return module.Dependencies.Except(semanticNetwork.Modules.Keys).ToList();
		}

		public static ISemanticNetwork WithModules(this ISemanticNetwork semanticNetwork, ICollection<IExtensionModule> modules)
		{
			var modulesToApply = new List<IExtensionModule>(modules);

			while (modulesToApply.Count > 0)
			{
				var applied = new List<IExtensionModule>();
				foreach (var module in modulesToApply)
				{
					if (semanticNetwork.GetMissingDependencies(module).Count == 0)
					{
						module.AttachTo(semanticNetwork);
						applied.Add(module);
					}
				}

				if (applied.Count > 0)
				{
					modulesToApply.RemoveAll(applied.Contains);
				}
				else
				{
					throw new ModuleException(String.Join("; ", modulesToApply.Select(m => m.Name)), $"Impossible to apply {modulesToApply.Count} modules because they have unresolved dependencies.");
				}
			}

			return semanticNetwork;
		}
	}

	public class ModuleException : Exception
	{
		#region Properties

		public String ModuleName
		{ get; }

		#endregion

		#region Constructors

		public ModuleException(String moduleName, String message)
			: base(message)
		{
			ModuleName = moduleName;
		}

		#endregion
	}
}
