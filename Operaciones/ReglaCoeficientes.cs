﻿using Operaciones.Recursos;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad de coeficientes.
	/// </summary>
	/// <remarks>
	/// Esta clase contiene propiedades y métodos para gestionar su tipo y propiedades.
	/// </remarks>
	public class ReglaCoeficientes(
		[Range(0, long.MaxValue)] long divisor
		, [Range(2, long.MaxValue)] long @base
		, [Range(0,int.MaxValue)] int coeficientes): ReglaAplicable {

		/// <summary>
		/// Crea una regla con los coeficientes ya obtenidos, no se comprueba que sean correctos.
		/// </summary>
		/// <param name="divisor">Divisor de la regla</param>
		/// <param name="base">Base de la representación de los dividendos</param>
		/// <param name="coeficientes">Lista de coeficientes</param>
		public ReglaCoeficientes(long divisor, long @base, IEnumerable<long> coeficientes)
			: this(divisor, @base, coeficientes.Count()) {
			if (!coeficientes.Any()) throw new ArgumentException(
				TextoCalculos.ReglaVaciaError, nameof(coeficientes));
			_coeficientes = new(coeficientes);
		}

		private List<long>? _coeficientes = null;

		private int _longitud = coeficientes;

		[JsonPropertyName("divisor")]
		[Range(0, long.MaxValue)]
		public override long Divisor => divisor;
		
		[JsonPropertyName("base")]
		[Range(2, long.MaxValue)]
		public override long Base => @base;

		/// <summary>
		/// Esta propiedad indica la longitud de la regla, será 0 siempre que no se pueda calcular.
		/// </summary>
		/// <remarks>
		/// Al cambiar la longitud, se tendrá que volver a acceder a <see cref="Coeficientes"/> para actualizarlo.
		/// </remarks>
		[JsonIgnore]
		public int Longitud { 
			get {
				if (!Calculos.SonCoprimos(Divisor, Base))
					return 0;
				return _longitud;
			}
			set => _longitud = value; 
		}

		/// <summary>
		/// Esta propiedad permite obtener los coeficientes de la regla para ser utilizados.
		/// </summary>
		/// <remarks>
		/// Si se cambia <see cref="Longitud"/> puede ser posible que se recalcule.
		/// </remarks>
		[JsonPropertyName("coefficients")]
		public List<long> Coeficientes { 
			get {
				if (Calculos.SonCoprimos(Divisor, Base)) {
					if (_coeficientes is null || _coeficientes.Count < Longitud) {
						if (Divisor < 1) throw new InvalidOperationException();
						_coeficientes = CalcularRegla();
					}
					return _coeficientes[..Longitud];
				}
				return [];
			}
		}

		[JsonPropertyName("rule-explained")]
		public override string ReglaExplicada { get => TextoCalculos.ReglaExpliacadaCoeficientes; }

		[JsonPropertyName("type")]
		public override CasosDivisibilidad Tipo => CasosDivisibilidad.COEFFICIENTS;

		public override string AplicarRegla(long dividendo) {
			StringBuilder sb = new();
			long dividendoMenor = dividendo,
				dividendoActual;
			int iteracionesRestantes = int.MaxValue; // Se cambiará si el actual es mayor que el menor y hará una cantidad limitada
			bool minimoLocalEncontrado = false
				, saltarBucle = false;
			InsertarMensajeBase(sb, dividendo);
			do {
				
				dividendoActual = Math.Abs(ObtenerNuevoDividendo(dividendoMenor, sb));
				if (iteracionesRestantes == 0 && minimoLocalEncontrado) {
					saltarBucle = true;
				}
				if (dividendoActual < dividendoMenor) {
					dividendoMenor = dividendoActual;
				} else {
					minimoLocalEncontrado = true;
					iteracionesRestantes = 2;
					sb.AppendFormat(TextoCalculos.MensajeAplicarMinimoEncontrado, dividendoActual, dividendoMenor).AppendLine();
				}
				iteracionesRestantes--;
				sb.AppendLine();
			} while (dividendoMenor > LimiteTrivialidadEstimado 
			& !saltarBucle); // Mientras sea demasiado grande o no tengamos un mínimo que nos oblique a parar
			InsertarMensajeFin(sb, dividendo, dividendoMenor);
			return sb.ToString();
		}

		private long ObtenerNuevoDividendo(long dividendo, StringBuilder sb) { // Escribe en el sb y devuelve un nuevo dividendo
			sb.AppendFormat(TextoCalculos.MensajeAplicarInicio, Divisor, Base, LongAStringCondicional(dividendo), Longitud).AppendLine();
			long parteIzquierda = dividendo / Calculos.PotenciaEntera(Base, Longitud),
				parteDerecha = Calculos.IntervaloCifras(dividendo, Base, 0, (byte)Longitud);
			sb.AppendFormat(TextoCalculos.MensajeAplicarSeparar, Longitud, LongAStringCondicional(parteIzquierda), LongAStringCondicional(parteDerecha))
				.AppendLine()
				.AppendLine(TextoCalculos.MensajeAplicarMultiplicarCoeficientes);
			if (Calculos.Cifras(parteDerecha, Base) < Longitud) {
				sb.AppendFormat(TextoCalculos.MensajeAplicarParteDerechaPequeña, parteDerecha).AppendLine();
			}
			long productoTotal = CalcularNuevoDividendoYPonerEnBuilder(parteDerecha, sb),
				nuevoDividendo = productoTotal + parteIzquierda; // Paso dos
			sb.AppendLine(TextoCalculos.MensajeAplicarSuma)
				.AppendLine(LongAStringCondicional(productoTotal) + " + " + LongAStringCondicional(parteIzquierda)
				+ " = " + LongAStringCondicional(nuevoDividendo));
			return nuevoDividendo;
		}

		private long CalcularNuevoDividendoYPonerEnBuilder(long parteDerecha, StringBuilder sb) {
			byte indiceCoeficientes = 0;
			long resultadoProducto = 0;
			// Se itera al revés por las cifras ya que están en el orden inverso
			for (byte cifras = Calculos.Cifras(parteDerecha, Base); indiceCoeficientes < cifras - 1; indiceCoeficientes++) {
				long coeficienteActual = Coeficientes[indiceCoeficientes],
					cifraActual = Calculos.Cifra(parteDerecha, (byte)(cifras - indiceCoeficientes - 1), Base);
				sb.Append(LongAStringCondicional(coeficienteActual) + " * " + LongAStringCondicional(cifraActual) + " + "); // O sea coeficiente * cifra +
				resultadoProducto += coeficienteActual * cifraActual;
			}
			sb.Append(Coeficientes[indiceCoeficientes] + " * " + LongAStringCondicional(Calculos.Cifra(parteDerecha, 0, Base)))
				.AppendLine(" = " + LongAStringCondicional(resultadoProducto += Coeficientes[indiceCoeficientes] * Calculos.Cifra(parteDerecha, 0, Base)));
			return resultadoProducto;
		}

		private List<long> CalcularRegla() {
			List<long> regla = Calculos.ReglaDivisibilidadOptima(Divisor, Longitud, Base).Coeficientes;
			Longitud = regla.Count;
			return regla;
		}

		public override string ToString() {
			return string.Join(", ", Coeficientes);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Coeficientes);
		}

		public override bool Equals(object? obj) {
			return obj is ReglaCoeficientes coeficientes &&
				   Divisor == coeficientes.Divisor &&
				   Base == coeficientes.Base &&
				   Longitud == coeficientes.Longitud &&
				   EqualityComparer<List<long>>.Default.Equals(Coeficientes, coeficientes.Coeficientes);
		}
	}
}