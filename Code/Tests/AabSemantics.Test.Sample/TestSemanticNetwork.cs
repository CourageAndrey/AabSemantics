using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Test.Sample
{
	public class TestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		public MathematicsTestSemanticNetwork Mathematics
		{ get; }

		public ProcessesTestSemanticNetwork Processes
		{ get; }

		public SetTestSemanticNetwork Set
		{ get; }

		#region Subject Areas

		public IConcept SubjectArea_Transport
		{ get { return Set.SubjectArea_Transport; } }

		public IConcept SubjectArea_Numbers
		{ get; }

		public IConcept SubjectArea_Processes
		{ get; }

		#endregion

		#region Base Concepts

		public IConcept Base_Vehicle
		{ get { return Set.Base_Vehicle; } }

		#endregion

		#region Signs

		public IConcept Sign_MotorType
		{ get { return Set.Sign_MotorType; } }

		public IConcept Sign_AreaType
		{ get { return Set.Sign_AreaType; } }

		#endregion

		#region Motor Types

		public IConcept MotorType_Muscles
		{ get { return Set.MotorType_Muscles; } }

		public IConcept MotorType_Steam
		{ get { return Set.MotorType_Steam; } }

		public IConcept MotorType_Combustion
		{ get { return Set.MotorType_Combustion; } }

		public IConcept MotorType_Jet
		{ get { return Set.MotorType_Jet; } }

		#endregion

		#region Area Types

		public IConcept AreaType_Ground
		{ get { return Set.AreaType_Ground; } }

		public IConcept AreaType_Water
		{ get { return Set.AreaType_Water; } }

		public IConcept AreaType_Air
		{ get { return Set.AreaType_Air; } }

		#endregion

		#region Certain Transportation Devices

		public IConcept Vehicle_Bicycle
		{ get { return Set.Vehicle_Bicycle; } }

		public IConcept Vehicle_Curragh
		{ get { return Set.Vehicle_Curragh; } }

		public IConcept Vehicle_SteamLocomotive
		{ get { return Set.Vehicle_SteamLocomotive; } }

		public IConcept Vehicle_Steamboat
		{ get { return Set.Vehicle_Steamboat; } }

		public IConcept Vehicle_Car
		{ get { return Set.Vehicle_Car; } }

		public IConcept Vehicle_Motorcycle
		{ get { return Set.Vehicle_Motorcycle; } }

		public IConcept Vehicle_Fighter
		{ get { return Set.Vehicle_Fighter; } }

		public IConcept Vehicle_Airbus
		{ get { return Set.Vehicle_Airbus; } }

		public IConcept Vehicle_JetFighter
		{ get { return Set.Vehicle_JetFighter; } }

		#endregion

		#region Car parts

		public IConcept Part_Engine
		{ get { return Set.Part_Engine; } }

		public IConcept Part_Wheels
		{ get { return Set.Part_Wheels; } }

		public IConcept Part_Body
		{ get { return Set.Part_Body; } }

		#endregion

		#region Comparable Values

		public IConcept Number0
		{ get { return Mathematics.Number0; } }

		public IConcept NumberZero
		{ get { return Mathematics.NumberZero; } }

		public IConcept NumberNotZero
		{ get { return Mathematics.NumberNotZero; } }

		public IConcept Number1
		{ get { return Mathematics.Number1; } }

		public IConcept Number1or2
		{ get { return Mathematics.Number1or2; } }

		public IConcept Number2
		{ get { return Mathematics.Number2; } }

		public IConcept Number2or3
		{ get { return Mathematics.Number2or3; } }

		public IConcept Number3
		{ get { return Mathematics.Number3; } }

		public IConcept Number3or4
		{ get { return Mathematics.Number3or4; } }

		public IConcept Number4
		{ get { return Mathematics.Number4; } }

		#endregion

		#region Processes

		public IConcept ProcessA
		{ get { return Processes.ProcessA; } }

		public IConcept ProcessB
		{ get { return Processes.ProcessB; } }

		#endregion

		#endregion

		public TestSemanticNetwork(ILanguage language)
		{
			SemanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			((LocalizedStringVariable) SemanticNetwork.Name).SetLocale("ru-RU", "Тестовая база знаний");
			((LocalizedStringVariable) SemanticNetwork.Name).SetLocale("en-US", "Test knowledgebase");

			Mathematics = SemanticNetwork.CreateMathematicsTestData();
			Processes = SemanticNetwork.CreateProcessesTestData();
			Set = SemanticNetwork.CreateSetTestData();

			SemanticNetwork.Concepts.Add(SubjectArea_Numbers = new Concept(nameof(SubjectArea_Numbers), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Числа" },
				{ "en-US", "Numbers" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Числа." },
				{ "en-US", "Numbers." },
			})));

			SemanticNetwork.Concepts.Add(SubjectArea_Processes = new Concept(nameof(SubjectArea_Processes), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Процессы" },
				{ "en-US", "Processes" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Процессы." },
				{ "en-US", "Processes." },
			})));

			SemanticNetwork.DeclareThat(SubjectArea_Numbers).IsSubjectAreaOf(new[]
			{
				Number0,
				NumberZero,
				NumberNotZero,
				Number1,
				Number1or2,
				Number2,
				Number2or3,
				Number3,
				Number3or4,
				Number4,
			});

			SemanticNetwork.DeclareThat(SubjectArea_Processes).IsSubjectAreaOf(new[]
			{
				ProcessA,
				ProcessB,
			});
		}
	}

	public static class TestSemanticNetworkExtension
	{
		public static MathematicsTestSemanticNetwork CreateMathematicsTestData(this ISemanticNetwork semanticNetwork)
		{
			return new MathematicsTestSemanticNetwork(semanticNetwork);
		}

		public static ProcessesTestSemanticNetwork CreateProcessesTestData(this ISemanticNetwork semanticNetwork)
		{
			return new ProcessesTestSemanticNetwork(semanticNetwork);
		}

		public static SetTestSemanticNetwork CreateSetTestData(this ISemanticNetwork semanticNetwork)
		{
			return new SetTestSemanticNetwork(semanticNetwork);
		}
	}

	public class MathematicsTestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		#region Comparable Values

		public IConcept Number0
		{ get; }

		public IConcept NumberZero
		{ get; }

		public IConcept NumberNotZero
		{ get; }

		public IConcept Number1
		{ get; }

		public IConcept Number1or2
		{ get; }

		public IConcept Number2
		{ get; }

		public IConcept Number2or3
		{ get; }

		public IConcept Number3
		{ get; }

		public IConcept Number3or4
		{ get; }

		public IConcept Number4
		{ get; }

		#endregion

		#endregion

		public MathematicsTestSemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			#region Semantic network

			SemanticNetwork = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<MathematicsModule>();

			#endregion

			#region Comparable Values

			Func<String, LocalizedStringVariable> getString = text => new LocalizedStringVariable(
				new Dictionary<String, String>
				{
					{ "ru-RU", text },
					{ "en-US", text },
				});
			Func<Int32, LocalizedStringVariable> getStringByNumber = number => getString(number.ToString());

			SemanticNetwork.Concepts.Add(Number0 = new Concept("0", getStringByNumber(0), getStringByNumber(0)));

			SemanticNetwork.Concepts.Add(NumberZero = new Concept("_0_", getString("_0_"), getString("_0_")));

			SemanticNetwork.Concepts.Add(NumberNotZero = new Concept("!0", getString("!0"), getString("!0")));

			SemanticNetwork.Concepts.Add(Number1 = new Concept("1", getStringByNumber(1), getStringByNumber(1)));

			SemanticNetwork.Concepts.Add(Number1or2 = new Concept("1 || 2", getString("1 || 2"), getString("1 || 2")));

			SemanticNetwork.Concepts.Add(Number2 = new Concept("2", getStringByNumber(2), getStringByNumber(2)));

			SemanticNetwork.Concepts.Add(Number2or3 = new Concept("2 || 3", getString("2 || 3"), getString("2 || 3")));

			SemanticNetwork.Concepts.Add(Number3 = new Concept("3", getStringByNumber(3), getStringByNumber(3)));

			SemanticNetwork.Concepts.Add(Number3or4 = new Concept("3 || 4", getString("3 || 4"), getString("3 || 4")));

			SemanticNetwork.Concepts.Add(Number4 = new Concept("4", getStringByNumber(4), getStringByNumber(4)));

			#endregion

			#region Concept Attributes

			Number0.WithAttribute(IsValueAttribute.Value);
			NumberZero.WithAttribute(IsValueAttribute.Value);
			NumberNotZero.WithAttribute(IsValueAttribute.Value);
			Number1.WithAttribute(IsValueAttribute.Value);
			Number1or2.WithAttribute(IsValueAttribute.Value);
			Number2.WithAttribute(IsValueAttribute.Value);
			Number2or3.WithAttribute(IsValueAttribute.Value);
			Number3.WithAttribute(IsValueAttribute.Value);
			Number3or4.WithAttribute(IsValueAttribute.Value);
			Number4.WithAttribute(IsValueAttribute.Value);

			#endregion

			#region Statements

			SemanticNetwork.DeclareThat(Number0).IsEqualTo(NumberZero);
			SemanticNetwork.DeclareThat(NumberNotZero).IsNotEqualTo(NumberZero);
			SemanticNetwork.DeclareThat(Number0).IsLessThan(Number1);
			SemanticNetwork.DeclareThat(Number1).IsLessThan(Number2);
			SemanticNetwork.DeclareThat(Number3).IsGreaterThan(Number2);
			SemanticNetwork.DeclareThat(Number4).IsGreaterThan(Number3);
			SemanticNetwork.DeclareThat(Number1).IsLessThanOrEqualTo(Number1or2);
			SemanticNetwork.DeclareThat(Number1or2).IsLessThanOrEqualTo(Number2);
			SemanticNetwork.DeclareThat(Number2).IsLessThanOrEqualTo(Number2or3);
			SemanticNetwork.DeclareThat(Number3).IsGreaterThanOrEqualTo(Number2or3);
			SemanticNetwork.DeclareThat(Number4).IsGreaterThanOrEqualTo(Number3or4);
			SemanticNetwork.DeclareThat(Number3or4).IsGreaterThanOrEqualTo(Number3);

			#endregion
		}
	}

	public class ProcessesTestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		#region Processes

		public IConcept ProcessA
		{ get; }

		public IConcept ProcessB
		{ get; }

		#endregion

		#endregion

		public ProcessesTestSemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			#region Semantic network

			SemanticNetwork = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<ProcessesModule>();

			#endregion

			#region Processes
			
			Func<String, LocalizedStringVariable> getString = text => new LocalizedStringVariable(
				new Dictionary<String, String>
				{
					{ "ru-RU", text },
					{ "en-US", text },
				});

			SemanticNetwork.Concepts.Add(ProcessA = new Concept(nameof(ProcessA), getString("Process A")));
			SemanticNetwork.Concepts.Add(ProcessB = new Concept(nameof(ProcessB), getString("Process B")));

			#endregion

			#region Concept Attributes

			ProcessA.WithAttribute(IsProcessAttribute.Value);
			ProcessB.WithAttribute(IsProcessAttribute.Value);

			#endregion

			#region Statements

			SemanticNetwork.DeclareThat(ProcessA).StartsBeforeOtherStarted(ProcessB);
			SemanticNetwork.DeclareThat(ProcessA).FinishesAfterOtherFinished(ProcessB);

			#endregion
		}
	}

	public class SetTestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		#region Subject Areas

		public IConcept SubjectArea_Transport
		{ get; }

		#endregion

		#region Base Concepts

		public IConcept Base_Vehicle
		{ get; }

		#endregion

		#region Signs

		public IConcept Sign_MotorType
		{ get; }

		public IConcept Sign_AreaType
		{ get; }

		#endregion

		#region Motor Types

		public IConcept MotorType_Muscles
		{ get; }

		public IConcept MotorType_Steam
		{ get; }

		public IConcept MotorType_Combustion
		{ get; }

		public IConcept MotorType_Jet
		{ get; }

		#endregion

		#region Area Types

		public IConcept AreaType_Ground
		{ get; }

		public IConcept AreaType_Water
		{ get; }

		public IConcept AreaType_Air
		{ get; }

		#endregion

		#region Certain Transportation Devices

		public IConcept Vehicle_Bicycle
		{ get; }

		public IConcept Vehicle_Curragh
		{ get; }

		public IConcept Vehicle_SteamLocomotive
		{ get; }

		public IConcept Vehicle_Steamboat
		{ get; }

		public IConcept Vehicle_Car
		{ get; }

		public IConcept Vehicle_Motorcycle
		{ get; }

		public IConcept Vehicle_Fighter
		{ get; }

		public IConcept Vehicle_Airbus
		{ get; }

		public IConcept Vehicle_JetFighter
		{ get; }

		#endregion

		#region Car parts

		public IConcept Part_Engine
		{ get; }

		public IConcept Part_Wheels
		{ get; }

		public IConcept Part_Body
		{ get; }

		#endregion

		#endregion

		public SetTestSemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			#region Semantic network

			SemanticNetwork = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<SetModule>();

			#endregion

			#region Subject Areas

			SemanticNetwork.Concepts.Add(SubjectArea_Transport = new Concept(nameof(SubjectArea_Transport), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспорт" },
				{ "en-US", "Transport" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Средства передвижения." },
				{ "en-US", "Vehicles." },
			})));

			#endregion

			#region Base Concepts

			SemanticNetwork.Concepts.Add(Base_Vehicle = new Concept(nameof(Base_Vehicle), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспортное средство" },
				{ "en-US", "Vehicle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устройство для перевозки людей и/или грузов." },
				{ "en-US", "System which is indended for transportation of humans and cargo." },
			})));

			#endregion

			#region Signs

			SemanticNetwork.Concepts.Add(Sign_MotorType = new Concept(nameof(Sign_MotorType), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Движитель" },
				{ "en-US", "Mover" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Система, обеспечивающая движение." },
				{ "en-US", "Initiator of movement." },
			})));

			SemanticNetwork.Concepts.Add(Sign_AreaType = new Concept(nameof(Sign_AreaType), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда передвижения" },
				{ "en-US", "Movement area" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда, для которой предназначено транспортное средство." },
				{ "en-US", "Place, where vehicles can move." },
			})));

			#endregion

			#region Motor types

			SemanticNetwork.Concepts.Add(MotorType_Muscles = new Concept(nameof(MotorType_Muscles), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мускульная сила" },
				{ "en-US", "Muscles" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использования для приведения в движение мускульной силы: собственной, других людей, животных." },
				{ "en-US", "To use own muscles in order to move." },
			})));

			SemanticNetwork.Concepts.Add(MotorType_Steam = new Concept(nameof(MotorType_Steam), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровая тяга" },
				{ "en-US", "Steam engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы нагретого пара." },
				{ "en-US", "To use steam engine to move." },
			})));

			SemanticNetwork.Concepts.Add(MotorType_Combustion = new Concept(nameof(MotorType_Combustion), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Внутреннее сгорание" },
				{ "en-US", "Combustion engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы топлива, сжигаемого в закрытых цилиндрах." },
				{ "en-US", "To use combustion engine to move." },
			})));

			SemanticNetwork.Concepts.Add(MotorType_Jet = new Concept(nameof(MotorType_Jet), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивная тяга" },
				{ "en-US", "Jet engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Выталкивание вещества в обратном направлении, обычно сжигаемого топлива." },
				{ "en-US", "To use jet engine to move." },
			})));

			#endregion

			#region Area types

			SemanticNetwork.Concepts.Add(AreaType_Ground = new Concept(nameof(AreaType_Ground), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Земля" },
				{ "en-US", "Ground" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Наземный транспорт." },
				{ "en-US", "Plain ground." },
			})));

			SemanticNetwork.Concepts.Add(AreaType_Water = new Concept(nameof(AreaType_Water), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Вода" },
				{ "en-US", "Water" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Плавучий транспорт." },
				{ "en-US", "Water surface." },
			})));

			SemanticNetwork.Concepts.Add(AreaType_Air = new Concept(nameof(AreaType_Air), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Воздух" },
				{ "en-US", "Air" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Возможность полёта." },
				{ "en-US", "Fly in air." },
			})));

			#endregion

			#region Certain Transportation Devices

			SemanticNetwork.Concepts.Add(Vehicle_Bicycle = new Concept(nameof(Vehicle_Bicycle), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Велосипед" },
				{ "en-US", "Bicycle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсный даритель радости." },
				{ "en-US", "Two wheels of fun." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Curragh = new Concept(nameof(Vehicle_Curragh), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Курага" },
				{ "en-US", "Curragh" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Первая человеческая потуга создать лодку." },
				{ "en-US", "It is not a bot itself yet." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_SteamLocomotive = new Concept(nameof(Vehicle_SteamLocomotive), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровоз" },
				{ "en-US", "Steam locomotive" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип локомотива." },
				{ "en-US", "Obsolete train." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Steamboat = new Concept(nameof(Vehicle_Steamboat), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Пароход" },
				{ "en-US", "Steamboat" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип корабля." },
				{ "en-US", "Obsolete boat type." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Car = new Concept(nameof(Vehicle_Car), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Автомобиль" },
				{ "en-US", "Car" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Четырёхколёсное механическое т/с." },
				{ "en-US", "4-wheels standard vehicle." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Motorcycle = new Concept(nameof(Vehicle_Motorcycle), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мотоцикл" },
				{ "en-US", "Motorcycle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсное механическое т/с, возможно с коляской." },
				{ "en-US", "Half of a car." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Fighter = new Concept(nameof(Vehicle_Fighter), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Поршневой истребитель" },
				{ "en-US", "Fighter" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший самолёт для ведения воздушного боя." },
				{ "en-US", "Obsolete aircraft." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Airbus = new Concept(nameof(Vehicle_Airbus), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Аэробус" },
				{ "en-US", "Airbus" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Гражданский самолёт для перевозки пассажиров." },
				{ "en-US", "Large civil airplane." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_JetFighter = new Concept(nameof(Vehicle_JetFighter), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивный истребитель" },
				{ "en-US", "Jet fighter" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Современный самолёт для ведения воздушного боя." },
				{ "en-US", "Modern aircraft." },
			})));

			#endregion

			#region Car parts

			SemanticNetwork.Concepts.Add(Part_Engine = new Concept(nameof(Part_Engine), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двигатель" },
				{ "en-US", "Engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двигатель." },
				{ "en-US", "Engine." },
			})));

			SemanticNetwork.Concepts.Add(Part_Wheels = new Concept(nameof(Part_Wheels), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Колёса" },
				{ "en-US", "Wheels" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Колёса." },
				{ "en-US", "Wheels." },
			})));

			SemanticNetwork.Concepts.Add(Part_Body = new Concept(nameof(Part_Body), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Кузов" },
				{ "en-US", "Car body" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Кузов." },
				{ "en-US", "Car body." },
			})));

			#endregion

			#region Concept Attributes

			Sign_MotorType.WithAttribute(IsSignAttribute.Value);
			Sign_AreaType.WithAttribute(IsSignAttribute.Value);

			MotorType_Muscles.WithAttribute(IsValueAttribute.Value);
			MotorType_Steam.WithAttribute(IsValueAttribute.Value);
			MotorType_Combustion.WithAttribute(IsValueAttribute.Value);
			MotorType_Jet.WithAttribute(IsValueAttribute.Value);
			AreaType_Ground.WithAttribute(IsValueAttribute.Value);
			AreaType_Water.WithAttribute(IsValueAttribute.Value);
			AreaType_Air.WithAttribute(IsValueAttribute.Value);

			#endregion

			#region Statements

			SemanticNetwork.DeclareThat(SubjectArea_Transport).IsSubjectAreaOf(new[]
			{
				Base_Vehicle,
				Sign_MotorType,
				MotorType_Muscles,
				MotorType_Steam,
				MotorType_Combustion,
				MotorType_Jet,
				Sign_AreaType,
				AreaType_Ground,
				AreaType_Water,
				AreaType_Air,
				Vehicle_Bicycle,
				Vehicle_Curragh,
				Vehicle_SteamLocomotive,
				Vehicle_Steamboat,
				Vehicle_Car,
				Vehicle_Motorcycle,
				Vehicle_Fighter,
				Vehicle_Airbus,
				Vehicle_JetFighter,
				Part_Body,
				Part_Engine,
				Part_Wheels,
			});

			SemanticNetwork.DeclareThat(Base_Vehicle).HasSigns(new[]
			{
				Sign_MotorType,
				Sign_AreaType,
			});

			SemanticNetwork.DeclareThat(Sign_MotorType).IsAncestorOf(new[]
			{
				MotorType_Muscles,
				MotorType_Steam,
				MotorType_Combustion,
				MotorType_Jet,
			});
			SemanticNetwork.DeclareThat(Sign_AreaType).IsAncestorOf(new[]
			{
				AreaType_Ground,
				AreaType_Water,
				AreaType_Air,
			});
			SemanticNetwork.DeclareThat(Base_Vehicle).IsAncestorOf(new[]
			{
				Vehicle_Bicycle,
				Vehicle_Curragh,
				Vehicle_SteamLocomotive,
				Vehicle_Steamboat,
				Vehicle_Car,
				Vehicle_Motorcycle,
				Vehicle_Fighter,
				Vehicle_Airbus,
				Vehicle_JetFighter,
			});

			SemanticNetwork.DeclareThat(Vehicle_Bicycle).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Muscles },
				{ Sign_AreaType, AreaType_Ground },
			});
			SemanticNetwork.DeclareThat(Vehicle_Curragh).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Muscles },
				{ Sign_AreaType, AreaType_Water },
			});
			SemanticNetwork.DeclareThat(Vehicle_SteamLocomotive).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Steam },
				{ Sign_AreaType, AreaType_Ground },
			});
			SemanticNetwork.DeclareThat(Vehicle_Steamboat).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Steam },
				{ Sign_AreaType, AreaType_Water },
			});
			SemanticNetwork.DeclareThat(Vehicle_Car).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Combustion },
				{ Sign_AreaType, AreaType_Ground },
			});
			SemanticNetwork.DeclareThat(Vehicle_Motorcycle).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Combustion },
				{ Sign_AreaType, AreaType_Ground },
			});
			SemanticNetwork.DeclareThat(Vehicle_Fighter).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Combustion },
				{ Sign_AreaType, AreaType_Air },
			});
			SemanticNetwork.DeclareThat(Vehicle_Airbus).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Jet },
				{ Sign_AreaType, AreaType_Air },
			});
			SemanticNetwork.DeclareThat(Vehicle_JetFighter).HasSignValues(new Dictionary<IConcept, IConcept>
			{
				{ Sign_MotorType, MotorType_Jet },
				{ Sign_AreaType, AreaType_Air },
			});

			SemanticNetwork.DeclareThat(Vehicle_Car).HasParts(new[]
			{
				Part_Body,
				Part_Engine,
				Part_Wheels,
			});

			#endregion
		}
	}
}