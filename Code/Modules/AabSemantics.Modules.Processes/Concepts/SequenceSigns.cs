using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Processes.Concepts
{
	/// <summary>
	/// The fifteen sequence signs as concepts, plus the algebra over them: how a sign reverts when
	/// the processes swap, which signs follow from a given one, which combinations contradict, and
	/// how two signs compose across a shared process.
	/// </summary>
	public static class SequenceSigns
	{
		#region Properties

		/// <summary>The "A starts after B started" sequence sign.</summary>
		public static readonly IConcept StartsAfterOtherStarted = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(StartsAfterOtherStarted)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.StartsAfterOtherStarted),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.StartsAfterOtherStarted));

		/// <summary>The "A starts when B starts" sequence sign.</summary>
		public static readonly IConcept StartsWhenOtherStarted = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(StartsWhenOtherStarted)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.StartsWhenOtherStarted),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.StartsWhenOtherStarted));

		/// <summary>The "A starts before B starts" sequence sign.</summary>
		public static readonly IConcept StartsBeforeOtherStarted = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(StartsBeforeOtherStarted)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.StartsBeforeOtherStarted),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.StartsBeforeOtherStarted));

		/// <summary>The "A finishes after B started" sequence sign.</summary>
		public static readonly IConcept FinishesAfterOtherStarted = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(FinishesAfterOtherStarted)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.FinishesAfterOtherStarted),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.FinishesAfterOtherStarted));

		/// <summary>The "A finishes when B starts" sequence sign.</summary>
		public static readonly IConcept FinishesWhenOtherStarted = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(FinishesWhenOtherStarted)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.FinishesWhenOtherStarted),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.FinishesWhenOtherStarted));

		/// <summary>The "A finishes before B starts" sequence sign.</summary>
		public static readonly IConcept FinishesBeforeOtherStarted = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(FinishesBeforeOtherStarted)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.FinishesBeforeOtherStarted),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.FinishesBeforeOtherStarted));

		/// <summary>The "A starts after B finished" sequence sign.</summary>
		public static readonly IConcept StartsAfterOtherFinished = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(StartsAfterOtherFinished)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.StartsAfterOtherFinished),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.StartsAfterOtherFinished));

		/// <summary>The "A starts when B finishes" sequence sign.</summary>
		public static readonly IConcept StartsWhenOtherFinished = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(StartsWhenOtherFinished)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.StartsWhenOtherFinished),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.StartsWhenOtherFinished));

		/// <summary>The "A starts before B finishes" sequence sign.</summary>
		public static readonly IConcept StartsBeforeOtherFinished = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(StartsBeforeOtherFinished)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.StartsBeforeOtherFinished),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.StartsBeforeOtherFinished));

		/// <summary>The "A finishes after B finished" sequence sign.</summary>
		public static readonly IConcept FinishesAfterOtherFinished = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(FinishesAfterOtherFinished)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.FinishesAfterOtherFinished),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.FinishesAfterOtherFinished));

		/// <summary>The "A finishes when B finishes" sequence sign.</summary>
		public static readonly IConcept FinishesWhenOtherFinished = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(FinishesWhenOtherFinished)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.FinishesWhenOtherFinished),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.FinishesWhenOtherFinished));

		/// <summary>The "A finishes before B finishes" sequence sign.</summary>
		public static readonly IConcept FinishesBeforeOtherFinished = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(FinishesBeforeOtherFinished)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.FinishesBeforeOtherFinished),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.FinishesBeforeOtherFinished));

		/// <summary>The "A causes B" sequence sign.</summary>
		public static readonly IConcept Causes = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(Causes)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.Causes),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.Causes));

		/// <summary>The "A is caused by B" sequence sign.</summary>
		public static readonly IConcept IsCausedBy = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(IsCausedBy)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.IsCausedBy),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.IsCausedBy));

		/// <summary>The "A runs simultaneously with B" sequence sign.</summary>
		public static readonly IConcept SimultaneousWith = new SystemConcept(
			$"{{{nameof(SequenceSigns)}.{nameof(SimultaneousWith)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptNames.SimultaneousWith),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageProcessesModule, ILanguageConcepts>().SystemConceptHints.SimultaneousWith));

		/// <summary>All fifteen signs; every helper below rejects concepts outside this set.</summary>
		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherStarted,
			FinishesWhenOtherStarted,
			FinishesBeforeOtherStarted,
			StartsAfterOtherFinished,
			StartsWhenOtherFinished,
			StartsBeforeOtherFinished,
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
			Causes,
			IsCausedBy,
			SimultaneousWith,
		};

		/// <summary>Signs constraining when the first process starts.</summary>
		public static readonly ICollection<IConcept> StartSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			StartsAfterOtherFinished,
			StartsWhenOtherFinished,
			StartsBeforeOtherFinished,
		};

		/// <summary>Signs constraining when the first process finishes.</summary>
		public static readonly ICollection<IConcept> FinishSigns = new HashSet<IConcept>
		{
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
			FinishesAfterOtherStarted,
			FinishesWhenOtherStarted,
			FinishesBeforeOtherStarted,
		};

		/// <summary>Signs measured against the start of the second process.</summary>
		public static readonly ICollection<IConcept> RelatedToStartSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherStarted,
			FinishesWhenOtherStarted,
			FinishesBeforeOtherStarted,
		};

		/// <summary>Signs measured against the finish of the second process.</summary>
		public static readonly ICollection<IConcept> RelatedToFinishSigns = new HashSet<IConcept>
		{
			StartsAfterOtherFinished,
			StartsWhenOtherFinished,
			StartsBeforeOtherFinished,
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
		};

		/// <summary>Signs placing the first event strictly after the second.</summary>
		public static readonly ICollection<IConcept> AfterSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			FinishesAfterOtherStarted,
			StartsAfterOtherFinished,
			FinishesAfterOtherFinished,
		};

		/// <summary>Signs placing the two events at the same moment.</summary>
		public static readonly ICollection<IConcept> WhenSigns = new HashSet<IConcept>
		{
			StartsWhenOtherStarted,
			FinishesWhenOtherStarted,
			StartsWhenOtherFinished,
			FinishesWhenOtherFinished,
		};

		/// <summary>Signs placing the first event strictly before the second.</summary>
		public static readonly ICollection<IConcept> BeforeSigns = new HashSet<IConcept>
		{
			StartsBeforeOtherStarted,
			FinishesBeforeOtherStarted,
			StartsBeforeOtherFinished,
			FinishesBeforeOtherFinished,
		};

		/// <summary>Signs that compose across a shared process, driving transitive inference.</summary>
		public static readonly ICollection<IConcept> TransitiveSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
		};

		/// <summary>Signs that cannot relate a process to itself, e.g. "starts before it starts".</summary>
		public static readonly ICollection<IConcept> SelfInvalidSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherFinished,
			FinishesBeforeOtherFinished,
			StartsAfterOtherFinished,
			FinishesBeforeOtherStarted,
		};

		/// <summary>
		/// Lookup table of the sign that follows from combining two signs across a shared process,
		/// indexed by the transitive sign and then the child sign. Absent entries mean nothing follows.
		/// </summary>
		public static readonly IDictionary<IConcept, IDictionary<IConcept, IConcept>> ValidSequenceCombinations;

		#endregion

		private static void EnsureSuits(this IConcept sign)
		{
			if (!All.Contains(sign))
			{
				throw new InvalidOperationException("This method can work only with process sequence signs.");
			}
		}

		/// <summary>Returns the sign that holds when the two processes swap places.</summary>
		/// <param name="sign">Sign to revert.</param>
		/// <returns>The mirrored sign; symmetric signs are returned unchanged.</returns>
		/// <exception cref="InvalidOperationException"><paramref name="sign"/> is not a sequence sign.</exception>
		public static IConcept Revert(this IConcept sign)
		{
			EnsureSuits(sign);

			if (sign == StartsAfterOtherStarted)
			{
				return StartsBeforeOtherStarted;
			}
			else if (sign == StartsBeforeOtherStarted)
			{
				return StartsAfterOtherStarted;
			}
			else if (sign == FinishesAfterOtherStarted)
			{
				return StartsBeforeOtherFinished;
			}
			else if (sign == FinishesWhenOtherStarted)
			{
				return StartsWhenOtherFinished;
			}
			else if (sign == FinishesBeforeOtherStarted)
			{
				return StartsAfterOtherFinished;
			}
			else if (sign == StartsAfterOtherFinished)
			{
				return FinishesBeforeOtherStarted;
			}
			else if (sign == StartsWhenOtherFinished)
			{
				return FinishesWhenOtherStarted;
			}
			else if (sign == StartsBeforeOtherFinished)
			{
				return FinishesAfterOtherStarted;
			}
			else if (sign == FinishesAfterOtherFinished)
			{
				return FinishesBeforeOtherFinished;
			}
			else if (sign == FinishesBeforeOtherFinished)
			{
				return FinishesAfterOtherFinished;
			}
			else if (sign == Causes)
			{
				return IsCausedBy;
			}
			else if (sign == IsCausedBy)
			{
				return Causes;
			}
			else
			{
				return sign;
			}
		}

		/// <summary>
		/// Composes two sequence signs sharing a middle process into one relating the outer processes,
		/// which is how the module derives new sequences transitively.
		/// </summary>
		/// <param name="transitiveSign">Sign relating the first process to the middle one; must be transitive.</param>
		/// <param name="childSign">Sign relating the middle process to the last one.</param>
		/// <returns>The derived sign, or <c>null</c> when nothing follows from the pair.</returns>
		public static IConcept TryToCombineMutualSequences(IConcept transitiveSign, IConcept childSign)
		{
			EnsureSuits(transitiveSign);
			EnsureSuits(childSign);

			IDictionary<IConcept, IConcept> d;
			IConcept resultSign;
			return ValidSequenceCombinations.TryGetValue(transitiveSign, out d) && d.TryGetValue(childSign, out resultSign)
				? resultSign
				: null;
		}

		/// <summary>Determines whether a set of signs asserted about the same process pair is self-contradictory.</summary>
		/// <param name="signs">Signs recorded for one pair of processes.</param>
		/// <returns><c>true</c> if the signs cannot all hold together.</returns>
		/// <exception cref="InvalidOperationException">One of the signs is not a sequence sign.</exception>
		public static async Task<System.Boolean> ContradictsAsync(this ICollection<IConcept> signs)
		{
			foreach (var sign in signs)
			{
				EnsureSuits(sign);
			}

			var foundStartToStartSigns = await signs.Where(s => StartSigns.Contains(s) && RelatedToStartSigns.Contains(s)).Distinct().ToListAsync();
			var foundStartToFinishSigns = await signs.Where(s => StartSigns.Contains(s) && RelatedToFinishSigns.Contains(s)).Distinct().ToListAsync();
			var foundFinishToStartSigns = await signs.Where(s => FinishSigns.Contains(s) && RelatedToStartSigns.Contains(s)).Distinct().ToListAsync();
			var foundFinishToFinishSigns = await signs.Where(s => FinishSigns.Contains(s) && RelatedToFinishSigns.Contains(s)).Distinct().ToListAsync();
			return	(signs.Contains(StartsBeforeOtherStarted) && signs.Contains(StartsAfterOtherFinished)) ||
					(signs.Contains(FinishesBeforeOtherStarted) && signs.Contains(FinishesAfterOtherFinished)) ||
					foundStartToStartSigns.Count > 1 ||
					foundStartToFinishSigns.Count > 1 ||
					foundFinishToStartSigns.Count > 1 ||
					foundFinishToFinishSigns.Count > 1;
		}

		/// <summary>Blocking counterpart of <see cref="ContradictsAsync"/>.</summary>
		/// <param name="signs">Signs recorded for one pair of processes.</param>
		/// <returns><c>true</c> if the signs cannot all hold together.</returns>
		public static System.Boolean Contradicts(this ICollection<IConcept> signs)
		{
			return TaskHelper.AwaitDetached(() => ContradictsAsync(signs));
		}

		/// <summary>Returns the signs implied by a given one, e.g. "finishes before B starts" implies "starts before B starts".</summary>
		/// <param name="sign">Sign to draw consequences from.</param>
		/// <returns>The implied signs; empty when the sign implies nothing further.</returns>
		/// <exception cref="InvalidOperationException"><paramref name="sign"/> is not a sequence sign.</exception>
		public static ICollection<IConcept> Consequently(this IConcept sign)
		{
			EnsureSuits(sign);

			if (sign == StartsBeforeOtherStarted)
			{
				return new[] { StartsBeforeOtherFinished };
			}
			else if (sign == FinishesBeforeOtherStarted)
			{
				return new[] { StartsBeforeOtherStarted, StartsBeforeOtherFinished, FinishesBeforeOtherFinished };
			}
			else if (sign == StartsAfterOtherFinished)
			{
				return new[] { FinishesAfterOtherFinished, FinishesAfterOtherStarted, StartsAfterOtherStarted };
			}
			else if (sign == FinishesAfterOtherFinished)
			{
				return new[] { FinishesAfterOtherStarted };
			}
			else
			{
				return Array.Empty<IConcept>();
			}
		}

		static SequenceSigns()
		{
			foreach (var concept in All)
			{
				concept.WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsSequenceSignAttribute.Value });
			}

			ValidSequenceCombinations = new Dictionary<IConcept, IDictionary<IConcept, IConcept>>();

			Action<IConcept, IConcept, IConcept> setValidCombination = (transitiveSign, childSign, resultSign) =>
			{
				IDictionary<IConcept, IConcept> d;
				if (!ValidSequenceCombinations.TryGetValue(transitiveSign, out d))
				{
					ValidSequenceCombinations[transitiveSign] = d = new Dictionary<IConcept, IConcept>();
				}
				d.Add(childSign, resultSign);
			};

			foreach (var combination in new[]
			{
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsWhenOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsWhenOtherFinished, StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsWhenOtherStarted, StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsWhenOtherFinished, StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsWhenOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsWhenOtherFinished, StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsWhenOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsWhenOtherFinished, FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsWhenOtherStarted, FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsWhenOtherFinished, FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsWhenOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsWhenOtherFinished, FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesWhenOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesWhenOtherFinished, StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesWhenOtherStarted, StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesWhenOtherFinished, StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesWhenOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesWhenOtherFinished, StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesWhenOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesWhenOtherFinished, FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesWhenOtherStarted, FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesWhenOtherFinished, FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesWhenOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesWhenOtherFinished, FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(Causes, Causes, Causes),
				new Tuple<IConcept, IConcept, IConcept>(IsCausedBy, IsCausedBy, IsCausedBy),
				new Tuple<IConcept, IConcept, IConcept>(SimultaneousWith, SimultaneousWith, SimultaneousWith),

				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, FinishesBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, FinishesBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, FinishesBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, FinishesBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, StartsAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, StartsAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, StartsAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, StartsAfterOtherFinished, FinishesAfterOtherFinished),
			})
			{
				setValidCombination(combination.Item1, combination.Item2, combination.Item3);
			}
		}
	}
}