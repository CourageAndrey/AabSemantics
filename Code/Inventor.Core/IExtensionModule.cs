using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public interface IExtensionModule
	{
		String Name
		{ get; }

		ICollection<String> Dependencies
		{ get; }

		void AttachTo(ISemanticNetwork semanticNetwork);
	}

	public abstract class ExtensionModule : IExtensionModule
	{
		public String Name
		{ get; }

		public ICollection<String> Dependencies
		{ get; }

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

			Attach(semanticNetwork);

			semanticNetwork.Modules[Name] = this;
		}

		protected abstract void Attach(ISemanticNetwork semanticNetwork);
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

		public ModuleException(String moduleName, String maessage)
			: base(maessage)
		{
			ModuleName = moduleName;
		}

		#endregion
	}
}
