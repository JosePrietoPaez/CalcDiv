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
			ReglasCoeficientes = true
		};

		// Act
		var (State, Rules) = DivisibilityCalculator.CalculateRule(options);

		// Assert
		Assert.Multiple(() => {
			Assert.That(State, Is.EqualTo(ExitState.NO_ERROR));
			Assert.That(Rules.Count(), Is.EqualTo(1));
			Assert.That(Rules.First().Divisor, Is.EqualTo(divisor));
			Assert.That(Rules.First().Base, Is.EqualTo(@base));
			Assert.That(Rules.First() is ReglaCoeficientes);
			Assert.That((Rules.First() as ReglaCoeficientes)!.Longitud, Is.EqualTo((int?)length));
		});
	}

	[Test]
	public void CalculateSingleRule_CorrectSingleDefaultBase_ReturnsRuleAndCorrectExecution() {
		// Arrange
		long divisor = 3;
		int length = 1;

		// Act
		var (State, Rule) = DivisibilityCalculator.CalculateSingleRule(divisor, coefficientRule: true, length: length);

		// Assert
		Assert.Multiple(() => {
			Assert.That(State, Is.EqualTo(ExitState.NO_ERROR));
			Assert.That(Rule.Divisor, Is.EqualTo(divisor));
			Assert.That(Rule.Base, Is.EqualTo(10));
			Assert.That(Rule is ReglaCoeficientes);
			Assert.That((Rule as ReglaCoeficientes)!.Longitud, Is.EqualTo((int?)length));
		});
	}

	[Test]
	public void CalculateRule_CorrectMultiple_ReturnsRulesAndCorrectExecution() {
		// Arrange
		long[] divisors = [3, 5],
			bases = [10, 25];
		var options = new OpcionesVarias() {
			Dividendo = null,
			ReglasCoeficientes = false,
			VariasReglas = [string.Join(',', divisors.Select(n => n.ToString())),
			string.Join(',', divisors.Select(n => n.ToString()))]
		};

		// Act
		var (State, Rules) = DivisibilityCalculator.CalculateRule(options);

		// Assert
		Assert.Multiple(() => {
			Assert.That(State, Is.EqualTo(ExitState.NO_ERROR));
			Assert.That(Rules.Count(), Is.EqualTo(4));
		});
	}

	[Test]
	public void CalculateMultipleRules_CorrectVaried_ReturnsRulesAndCorrectExecution() {
		// Arrange
		long[] divisors = [3, 7],
			bases = [10, 25];
		// Act
		var (State, Rules) = DivisibilityCalculator.CalculateMultipleRules(divisors, bases);
		// Assert
		Assert.Multiple(() => {
			Assert.That(State, Is.EqualTo(ExitState.NO_ERROR));
			Assert.That(Rules.Count(), Is.EqualTo(4));
			Assert.That(Rules.All(r => r is not ReglaCoeficientes));
		});
	}

	[Test]
	public void CalculateMultipleRules_NotFullyCoprime_ReturnsValidRulesAndPartialErrorExecution() {
		// Arrange
		long[] divisors = [3, 6],
			bases = [10, 25];
		// Act
		var (State, Rules) = DivisibilityCalculator.CalculateMultipleRules(divisors, bases, true);
		// Assert
		Assert.Multiple(() => {
			Assert.That(State, Is.EqualTo(ExitState.PARTIAL_ERROR_MULTIPLE));
			Assert.That(Rules.Count(), Is.EqualTo(3));
			Assert.That(Rules.All(r => r is ReglaCoeficientes));
		});
	}
}
