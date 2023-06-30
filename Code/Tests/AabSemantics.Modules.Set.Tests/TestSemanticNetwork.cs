using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests
{
	public static class TestSemanticNetworkExtension
	{
		public static SetTestSemanticNetwork CreateSetTestData(this ISemanticNetwork semanticNetwork)
		{
			return new SetTestSemanticNetwork(semanticNetwork);
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