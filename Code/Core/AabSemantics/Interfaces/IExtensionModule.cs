using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;

namespace AabSemantics
{
	/// <summary>
	/// A pluggable bundle of domain knowledge: the attributes, concepts, statements, questions
	/// and localization a semantic network gains when the module is attached to it.
	/// Most implementations derive from <see cref="ExtensionModule"/> rather than implementing this directly.
	/// </summary>
	public interface IExtensionModule
	{
		/// <summary>
		/// Name identifying the module, used as its key in registries and in dependency lists.
		/// </summary>
		String Name
		{ get; }

		/// <summary>
		/// Names of modules that must already be attached before this one can be.
		/// </summary>
		ICollection<String> Dependencies
		{ get; }

		/// <summary>
		/// Registers the module with a semantic network, after verifying its dependencies
		/// and registering its metadata.
		/// </summary>
		/// <param name="semanticNetwork">Network to extend.</param>
		void AttachTo(ISemanticNetwork semanticNetwork);

		/// <summary>
		/// Publishes the module's types into the process-wide metadata repositories.
		/// Metadata is global, so this runs at most once per module regardless of how many
		/// networks the module is attached to.
		/// </summary>
		void RegisterMetadata();

		/// <summary>
		/// Declares the language extension types the module contributes, keyed by extension name.
		/// They let the module add its own localized strings to every supported language.
		/// </summary>
		/// <returns>Extension name to implementing type; empty when the module adds no strings.</returns>
		IDictionary<String, Type> GetLanguageExtensions();
	}

	/// <summary>
	/// Base class for extension modules. It sequences dependency checking, metadata registration
	/// and attachment, leaving subclasses to override only the <c>Register*</c> hooks they need.
	/// </summary>
	public abstract class ExtensionModule : IExtensionModule
	{
		/// <summary>
		/// Name identifying the module.
		/// </summary>
		public String Name
		{ get; }

		/// <summary>
		/// Names of modules that must be attached before this one.
		/// </summary>
		public ICollection<String> Dependencies
		{ get; }

		/// <summary>
		/// <c>true</c> once the module's metadata has been published process-wide.
		/// </summary>
		public Boolean IsMetadataRegistered
		{ get { return Repositories.Modules.ContainsKey(Name); } }

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="name">Name identifying the module.</param>
		/// <param name="dependencies">Names of prerequisite modules; <c>null</c> means none.</param>
		protected ExtensionModule(String name, ICollection<String> dependencies = null)
		{
			Name = name;
			Dependencies = dependencies ?? Array.Empty<String>();
		}

		/// <summary>
		/// Attaches the module to a network: verifies dependencies, registers metadata if it has
		/// not been registered yet, runs <see cref="Attach"/>, and records the module on the network.
		/// Attaching a module that is already present does nothing.
		/// </summary>
		/// <param name="semanticNetwork">Network to extend.</param>
		/// <exception cref="ModuleException">A prerequisite module is not attached to the network.</exception>
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

		/// <summary>
		/// Hook for seeding the network with the module's own concepts and statements.
		/// Called once per network, after metadata registration. Does nothing by default.
		/// </summary>
		/// <param name="semanticNetwork">Network being extended.</param>
		protected virtual void Attach(ISemanticNetwork semanticNetwork)
		{ }

		/// <summary>
		/// Publishes the module's language, attributes, concepts, statements, questions and
		/// answers into the process-wide repositories. Idempotent: subsequent calls do nothing.
		/// </summary>
		public virtual void RegisterMetadata()
		{
			if (!IsMetadataRegistered)
			{
				RegisterLanguage();
				RegisterAttributes();
				RegisterConcepts();
				RegisterStatements();
				RegisterQuestions();
				RegisterAnswers();

				Repositories.Modules[Name] = this;
			}
		}

		/// <summary>
		/// Declares the language extension types the module contributes. Returns nothing by default.
		/// </summary>
		/// <returns>Extension name to implementing type.</returns>
		public virtual IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>();
		}

		/// <summary>
		/// Hook for registering the module's localized strings. Does nothing by default.
		/// </summary>
		protected virtual void RegisterLanguage()
		{ }

		/// <summary>
		/// Hook for registering the module's attribute definitions. Does nothing by default.
		/// </summary>
		protected virtual void RegisterAttributes()
		{ }

		/// <summary>
		/// Hook for registering the module's system concepts. Does nothing by default.
		/// </summary>
		protected virtual void RegisterConcepts()
		{ }

		/// <summary>
		/// Hook for registering the module's statement definitions. Does nothing by default.
		/// </summary>
		protected virtual void RegisterStatements()
		{ }

		/// <summary>
		/// Hook for registering the module's question definitions. Does nothing by default.
		/// </summary>
		protected virtual void RegisterQuestions()
		{ }

		/// <summary>
		/// Hook for registering the module's answer definitions. Does nothing by default.
		/// </summary>
		protected virtual void RegisterAnswers()
		{ }
	}

	/// <summary>
	/// Fluent helpers for attaching modules to a semantic network.
	/// </summary>
	public static class ExtensionModuleRegistrationHelper
	{
		/// <summary>
		/// Creates a module and attaches it to the network.
		/// </summary>
		/// <typeparam name="ModuleT">Module type; must have a parameterless constructor.</typeparam>
		/// <param name="semanticNetwork">Network to extend.</param>
		/// <returns>The same network, to allow call chaining.</returns>
		/// <exception cref="ModuleException">A prerequisite module is not attached yet.</exception>
		public static ISemanticNetwork WithModule<ModuleT>(this ISemanticNetwork semanticNetwork)
			where ModuleT : IExtensionModule, new()
		{
			var module = new ModuleT();
			module.AttachTo(semanticNetwork);
			return semanticNetwork;
		}

		/// <summary>
		/// Lists the module's prerequisites that the network does not have yet.
		/// </summary>
		/// <param name="semanticNetwork">Network to inspect.</param>
		/// <param name="module">Module whose dependencies are checked.</param>
		/// <returns>Names of the missing modules; empty when the module can be attached.</returns>
		public static ICollection<String> GetMissingDependencies(this ISemanticNetwork semanticNetwork, IExtensionModule module)
		{
			return module.Dependencies.Except(semanticNetwork.Modules.Keys).ToList();
		}

		/// <summary>
		/// Attaches several modules, repeatedly applying whichever ones have their dependencies
		/// satisfied. This means the caller does not have to supply them in dependency order.
		/// </summary>
		/// <param name="semanticNetwork">Network to extend.</param>
		/// <param name="modules">Modules to attach, in any order.</param>
		/// <returns>The same network, to allow call chaining.</returns>
		/// <exception cref="ModuleException">
		/// A pass attached nothing while modules remained, meaning their dependencies are
		/// missing from the set or form a cycle.
		/// </exception>
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

	/// <summary>
	/// Thrown when a module cannot be attached, typically because its dependencies are unresolved.
	/// </summary>
	public class ModuleException : Exception
	{
		#region Properties

		/// <summary>
		/// Name of the offending module, or a semicolon-separated list when several are involved.
		/// </summary>
		public String ModuleName
		{ get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates the exception.
		/// </summary>
		/// <param name="moduleName">Name of the offending module.</param>
		/// <param name="message">Description of what went wrong.</param>
		public ModuleException(String moduleName, String message)
			: base(message)
		{
			ModuleName = moduleName;
		}

		#endregion
	}
}
