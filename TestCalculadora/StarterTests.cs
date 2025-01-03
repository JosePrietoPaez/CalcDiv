namespace TestAPI;
using Divisibility;
using ModosEjecucion;
using Operaciones;

[TestFixture]
public class StarterTests {
	[SetUp]
	public void Setup() {
	}

	[Test]
	public void CalculateRule_Manual_ThrowsException() {
		Assert.Throws<NotImplementedException>(() => {
			DivisibilityCalculator.CalculateRule(new OpcionesManual());
		});
	}

	[Test]
	public void CalculateRule_Dialog_ThrowsException() {
		Assert.Throws<NotImplementedException>(() => {
			DivisibilityCalculator.CalculateRule(new OpcionesDialogo());
		});
	}

	[Test]
	public void CalculateRule_CorrectSingle_ReturnsRuleAndCorrectExecution() {
		// Arrange
		long divisor = 3,
			@base = 10,
			length = 1;
		var options = new OpcionesDirecto() {
			Base = @base,
			Divisor = divisor,
			Longitud = (int?)length,
			Dividendo = null,
			JSON = false,
			ReglasVariadas = false
		};

		// Act
		var result = DivisibilityCalculator.CalculateRule(options);

		// Assert
		Assert.Multiple(() => {
			Assert.That(result.Item1, Is.EqualTo(EstadoEjecucion.CORRECTA));
			Assert.That(result.Item2.Count(), Is.EqualTo(1));
			Assert.That(result.Item2.First().Divisor, Is.EqualTo(divisor));
			Assert.That(result.Item2.First().Base, Is.EqualTo(@base));
			Assert.That(result.Item2.First() is ReglaCoeficientes);
			Assert.That((result.Item2.First() as ReglaCoeficientes)!.Longitud, Is.EqualTo((int?)length));
		});
	}

	[Test]
	public void CalculateRule_CorrectMultple_ReturnsRulesAndCorrectExecution() {
		// Arrange
		long[] divisors = [3, 5],
			bases = [10, 25];
		var options = new OpcionesVarias() {
			Dividendo = null,
			ReglasVariadas = true,
			VariasReglas = [string.Join(',', divisors.Select(n => n.ToString())),
			string.Join(',', divisors.Select(n => n.ToString()))]
		};

		// Act
		var result = DivisibilityCalculator.CalculateRule(options);

		// Assert
		Assert.Multiple(() => {
			Assert.That(result.Item1, Is.EqualTo(EstadoEjecucion.CORRECTA));
			Assert.That(result.Item2.Count(), Is.EqualTo(4));
		});
	}
}
