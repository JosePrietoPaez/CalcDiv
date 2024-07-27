using Operaciones;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TestCalculadora {

	[TestFixture]
	internal class ReglaTests {

		[Test]
		public void Regla_Constructor_CreaEInicializaPropiedades() {
			long divisor = 10, @base = 7;
			int longitud = 3;
			string nombre = "Nombre";
			List<long> coeficientes = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);

			Regla reglaObtenida = new(divisor, @base, longitud, nombre);

			Assert.Multiple(() => {
				Assert.That(reglaObtenida.Nombre, Is.EqualTo(nombre));
				Assert.That(reglaObtenida.Divisor, Is.EqualTo(divisor));
				Assert.That(reglaObtenida.Base, Is.EqualTo(@base));
				Assert.That(reglaObtenida.Longitud, Is.EqualTo(longitud));
				Assert.That(reglaObtenida.Coeficientes, Is.EqualTo(coeficientes));
				Assert.That(reglaObtenida.Coeficientes, Is.Not.SameAs(coeficientes));
			});
		}

		[Test]
		public void Regla_ConstructorCoeficientes_CreaEInicializaPropiedades() {
			long divisor = 30, @base = 17;
			int longitud = 4;
			string nombre = "Nombre";
			List<long> coeficientes = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);

			Regla reglaObtenida = new(divisor, @base, coeficientes, nombre);

			Assert.Multiple(() => {
				Assert.That(reglaObtenida.Nombre, Is.EqualTo(nombre));
				Assert.That(reglaObtenida.Divisor, Is.EqualTo(divisor));
				Assert.That(reglaObtenida.Base, Is.EqualTo(@base));
				Assert.That(reglaObtenida.Longitud, Is.EqualTo(longitud));
				Assert.That(reglaObtenida.Coeficientes, Is.EqualTo(coeficientes));
				Assert.That(reglaObtenida.Coeficientes, Is.Not.SameAs(coeficientes));
			});
		}

		[Test]
		public void Regla_JSON_MuestraTodasLasPropiedades() {
			long divisor = 10, @base = 13;
			int longitud = 5;
			string nombre = "Nombre";
			List<long> regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);

			Regla reglaObtenida = new(divisor, @base, regla, nombre);

			JsonNode nodo = JsonNode.Parse(JsonSerializer.Serialize(reglaObtenida))!;
			Assert.Multiple(() => {
				Assert.That((long)nodo["base"], Is.EqualTo(@base));
				Assert.That((long)nodo["divisor"], Is.EqualTo(divisor));
				Assert.That((string)nodo["name"], Is.EqualTo(nombre));
				JsonArray coeficientes = nodo["coefficients"]!.AsArray();
				Assert.That(coeficientes, Has.Count.EqualTo(longitud));
				for (int i = 0; i < coeficientes.Count; i++) {
					Assert.That((long)coeficientes[i]!, Is.EqualTo(regla[i]));
				}
			});
		}

	}
}
