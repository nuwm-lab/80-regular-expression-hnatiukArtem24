using System;


namespace LabWork;


// Даний проект є шаблоном для виконання лабораторних робіт
// з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
// Необхідно змінювати і дописувати код лише в цьому проекті
// Відео-інструкції щодо роботи з github можна переглянути
// за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos
internal static class Program
{
    /// <summary>
    /// Entry point - demonstrates Builder pattern by creating several aircraft.
    /// </summary>
    static void Main(string[] args)
    {
        try
        {
            // Демонстрація використання патерну Builder для проєктування літаків
            var director = new AircraftDirector();


            var passengerBuilder = new PassengerPlaneBuilder();
            director.SetBuilder(passengerBuilder);


            // Побудувати регіональний пасажирський літак
            director.ConstructRegionalPassengerPlane();
            var regional = passengerBuilder.Build();
            Console.WriteLine(regional);


            // Побудувати довго-габаритний пасажирський літак
            director.ConstructLongHaulPassengerPlane();
            var longHaul = passengerBuilder.Build();
            Console.WriteLine(longHaul);


            // Побудувати вантажний літак через власний builder
            var cargoBuilder = new CargoPlaneBuilder();
            director.SetBuilder(cargoBuilder);
            director.ConstructHeavyCargoPlane();
            var cargo = cargoBuilder.Build();
            Console.WriteLine(cargo);


            Console.WriteLine("Builder demo completed.");
        }
        catch (ArgumentException ex)
        {
            // Argument problems indicate programming or input errors; report and exit with non-zero code.
            Console.Error.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            Environment.ExitCode = 1;
            return;
        }
        catch (InvalidOperationException ex)
        {
            // Validation / state errors produced by builders or product validation.
            Console.Error.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            Environment.ExitCode = 1;
            return;
        }
        // Note: we intentionally do not swallow all exceptions here. Unexpected exceptions should surface
        // so they can be observed during development and handled appropriately in production with
        // a proper logging/monitoring solution.
    }
}





using System;
using Xunit;


namespace LabWork.Tests
{
    public class BuilderUsageTests
    {
        [Fact]
        public void DirectBuilderUsage_WithoutDirector_CreatesValidAircraft()
        {
            // Arrange
            var builder = new PassengerPlaneBuilder();

            // Act - build aircraft directly without Director
            builder.SetEngine(new Engine("Test Engine", 100));
            builder.SetWings(new Wings("Test Wings", 30.0));
            builder.SetInterior(new Interior("Test Interior", 50));

            var aircraft = builder.Build();

            // Assert
            Assert.NotNull(aircraft);
            var json = aircraft.ToJson();
            Assert.Contains("Test Engine", json);
            Assert.Contains("Test Wings", json);
            Assert.Contains("Test Interior", json);
        }


        [Fact]
        public void BuildWithoutRequiredParts_ThrowsInvalidOperationException()
        {
            // Arrange
            var builder = new PassengerPlaneBuilder();

            // Act/Assert - try to build without setting any parts
            var ex = Assert.Throws<InvalidOperationException>(() => builder.Build());
            Assert.Equal("Aircraft missing engine.", ex.Message);

            // Set only engine
            builder.SetEngine(new Engine("Test Engine", 100));
            ex = Assert.Throws<InvalidOperationException>(() => builder.Build());
            Assert.Equal("Aircraft missing wings.", ex.Message);

            // Set wings but no interior
            builder.SetWings(new Wings("Test Wings", 30.0));
            ex = Assert.Throws<InvalidOperationException>(() => builder.Build());
            Assert.Equal("Aircraft missing interior.", ex.Message);
        }


        [Fact]
        public void Reset_ClearsAllParts()
        {
            // Arrange
            var builder = new CargoPlaneBuilder();
            builder.SetEngine(new Engine("Test Engine", 100));
            builder.SetWings(new Wings("Test Wings", 30.0));
            builder.SetInterior(new Interior("Test Interior", 4));

            // Act
            builder.Reset();

            // Assert - should throw because all parts were cleared
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }
    }
}


using System;


namespace LabWork
{
    /// <summary>
    /// Concrete builder for cargo/utility aircraft.
    /// </summary>
    public sealed class CargoPlaneBuilder : IAircraftBuilder
    {
        private Engine? _engine;
        private Wings? _wings;
        private Interior? _interior;


        public void Reset()
        {
            _engine = null;
            _wings = null;
            _interior = null;
        }


        public void SetEngine(Engine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }


        public void SetWings(Wings wings)
        {
            _wings = wings ?? throw new ArgumentNullException(nameof(wings));
        }


        public void SetInterior(Interior interior)
        {
            _interior = interior ?? throw new ArgumentNullException(nameof(interior));
            // Cargo planes may have a utilitarian interior; still store it
        }


        public Aircraft Build()
        {
            if (_engine is null) throw new InvalidOperationException("Aircraft missing engine.");
            if (_wings is null) throw new InvalidOperationException("Aircraft missing wings.");
            if (_interior is null) throw new InvalidOperationException("Aircraft missing interior.");


            var result = new Aircraft(_engine, _wings, _interior);
            Reset();
            return result;
        }
    }
}



using System;


namespace LabWork
{
    /// <summary>
    /// Concrete builder for passenger aircraft.
    /// </summary>
    public sealed class PassengerPlaneBuilder : IAircraftBuilder
    {
        private Engine? _engine;
        private Wings? _wings;
        private Interior? _interior;


        public void Reset()
        {
            _engine = null;
            _wings = null;
            _interior = null;
        }


        public void SetEngine(Engine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }


        public void SetWings(Wings wings)
        {
            _wings = wings ?? throw new ArgumentNullException(nameof(wings));
        }


        public void SetInterior(Interior interior)
        {
            _interior = interior ?? throw new ArgumentNullException(nameof(interior));
        }


        public Aircraft Build()
        {
            if (_engine is null) throw new InvalidOperationException("Aircraft missing engine.");
            if (_wings is null) throw new InvalidOperationException("Aircraft missing wings.");
            if (_interior is null) throw new InvalidOperationException("Aircraft missing interior.");


            var result = new Aircraft(_engine, _wings, _interior);
            Reset();
            return result;
        }
    }
}



using System;
using LabWork;
using Xunit;


namespace Tests
{
    public class AircraftJsonAndDirectorTests
    {
        [Fact]
        public void Aircraft_ToJson_Contains_EngineModel()
        {
            var director = new AircraftDirector();
            var builder = new PassengerPlaneBuilder();
            director.SetBuilder(builder);
            director.ConstructRegionalPassengerPlane();
            var aircraft = builder.Build();


            var json = aircraft.ToJson();
            Assert.Contains("TurboFan X200", json);
            Assert.Contains("Thrust", json);
        }


        [Fact]
        public void Director_Throws_When_Builder_Not_Set()
        {
            var director = new AircraftDirector();
            Assert.Throws<InvalidOperationException>(() => director.ConstructRegionalPassengerPlane());
        }
    }
}



using Xunit;


namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // trivial passing test to satisfy template
            Assert.True(true);
        }
    }
}



Рис.6. Текст програми UnitTest1

using System;
using LabWork;
using Xunit;


namespace Tests
{
    public class AircraftBuilderTests
    {
        [Fact]
        public void Director_Constructs_RegionalPassengerPlane()
        {
            var director = new AircraftDirector();
            var builder = new PassengerPlaneBuilder();
            director.SetBuilder(builder);
            director.ConstructRegionalPassengerPlane();
            var aircraft = builder.Build();
            Assert.NotNull(aircraft);
            var s = aircraft.ToString();
            Assert.Contains("TurboFan X200", s);
        }


        [Fact]
        public void Builder_Throws_On_Null_Engine()
        {
            var builder = new PassengerPlaneBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.SetEngine(null!));
        }


        [Fact]
        public void Build_Throws_When_Missing_Parts()
        {
            var builder = new PassengerPlaneBuilder();
            // do not set any parts
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }
    }
}







using System;


namespace LabWork
{
    /// <summary>
    /// Director orchestrates builder steps for common aircraft configurations.
    /// Use <see cref="SetBuilder"/> to provide a concrete builder before calling Construct* methods.
    /// </summary>
    public class AircraftDirector
    {
        private IAircraftBuilder? _builder;


        // Magic strings centralized for easier maintenance
        private const string TurboFanX200 = "TurboFan X200";
        private const string TurboFanZ900 = "TurboFan Z900";
        private const string TurbopropH700 = "Turboprop H700";


        /// <summary>
        /// The builder used by the director. External code should call <see cref="SetBuilder"/> to assign.
        /// </summary>
        public IAircraftBuilder Builder
        {
            get => _builder ?? throw new InvalidOperationException("Builder not set on Director.");
            private set => _builder = value ?? throw new ArgumentNullException(nameof(value));
        }


        /// <summary>
        /// Assigns the builder the director will use. Performs null validation.
        /// </summary>
        /// <param name="builder">A concrete <see cref="IAircraftBuilder"/> to use.</param>
        public void SetBuilder(IAircraftBuilder builder)
        {
            Builder = builder; // setter performs null-check; external callers use this method to set builder
        }


        /// <summary>
        /// Construct a regional passenger aircraft by orchestrating builder steps.
        /// </summary>
        public void ConstructRegionalPassengerPlane()
        {
            EnsureBuilder();
            // a typical regional passenger configuration
            Builder.Reset();
            Builder.SetEngine(new Engine(TurboFanX200, 120));
            Builder.SetWings(new Wings("High-lift", 28.4));
            Builder.SetInterior(new Interior("Comfort", 80));
        }


        /// <summary>
        /// Construct a long-haul passenger aircraft.
        /// </summary>
        public void ConstructLongHaulPassengerPlane()
        {
            EnsureBuilder();
            Builder.Reset();
            Builder.SetEngine(new Engine(TurboFanZ900, 300));
            Builder.SetWings(new Wings("Sweep", 60.0));
            Builder.SetInterior(new Interior("Lux", 250));
        }


        /// <summary>
        /// Construct a heavy cargo aircraft.
        /// </summary>
        public void ConstructHeavyCargoPlane()
        {
            EnsureBuilder();
            Builder.Reset();
            Builder.SetEngine(new Engine(TurbopropH700, 180));
            Builder.SetWings(new Wings("Straight - reinforced", 40.0));
            Builder.SetInterior(new Interior("Utility", 4));
        }


        private void EnsureBuilder()
        {
            if (_builder is null) throw new InvalidOperationException("Builder not set on Director.");
        }
    }
}



Рис.8. Текст програми AircraftDirector

using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace LabWork
{
    /// <summary>
    /// Product class that represents a constructed aircraft.
    /// Parts are encapsulated and can only be set by builders in the same assembly.
    /// </summary>
    public sealed class Aircraft
    {
        private readonly Engine _engine;
        private readonly Wings _wings;
        private readonly Interior _interior;


        /// <summary>
        /// Creates a new Aircraft with all required parts.
        /// Internal to ensure only builders in this assembly can create instances.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any part is null.</exception>
        internal Aircraft(Engine engine, Wings wings, Interior interior)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _wings = wings ?? throw new ArgumentNullException(nameof(wings));
            _interior = interior ?? throw new ArgumentNullException(nameof(interior));
        }


        private void Validate()
        {
            // Since constructor validates, this should never happen unless reflection is used
            if (_engine is null)
            {
                throw new InvalidOperationException("Aircraft missing engine.");
            }


            if (_wings is null)
            {
                throw new InvalidOperationException("Aircraft missing wings.");
            }


            if (_interior is null)
            {
                throw new InvalidOperationException("Aircraft missing interior.");
            }
        }


        /// <summary>
        /// Returns a human-readable description of the aircraft.
        /// This method validates the built state and will throw if parts are missing.
        /// </summary>
        public override string ToString()
        {
            // Protect against accidental use of incomplete product
            Validate();


            var sb = new StringBuilder();
            sb.AppendLine("Aircraft configuration:");
            sb.AppendLine($" Engine: {_engine}");
            sb.AppendLine($" Wings: {_wings}");
            sb.AppendLine($" Interior: {_interior}");
            return sb.ToString();
        }


        /// <summary>
        /// Returns a JSON representation of the aircraft (read-only snapshot).
        /// </summary>
        public string ToJson()
        {
            Validate();


            var dto = new
            {
                Engine = new { Model = _engine!.Model, Thrust = _engine!.Thrust },
                Wings = new { Type = _wings!.WingType, Span = _wings!.Span },
                Interior = new { Style = _interior!.Style, Seats = _interior!.Seats }
            };


            return JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
