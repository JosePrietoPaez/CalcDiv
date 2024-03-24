using System;
using System.Collections;

namespace ProgramaDivisibilidad
{
	internal enum Flag {
		Directo = DatosFlags.DIRECTO,
		Ayuda = DatosFlags.AYUDA,
		Inverso = DatosFlags.INVERSO,
		AyudaCorta = DatosFlags.CORTA,
		Todas = DatosFlags.TODOS,
		Nombre = DatosFlags.NOMBRE,
		Expandido = DatosFlags.EXPANDIDO
	}
	internal static class DatosFlags {
		public const int DIRECTO = 0, AYUDA = 1,INVERSO = 2,CORTA = 3, TODOS = 4, NOMBRE = 5,EXPANDIDO = 6;
		public const char DIRECTO_C = 'd', AYUDA_C = 'H', INVERSO_C = 'r', CORTA_C = 'h', TODOS_C = 't', NOMBRE_C = 'n', EXPANDIDO_C = 'x';
		private static readonly char[] _todos = [DIRECTO_C,AYUDA_C,INVERSO_C,CORTA_C,TODOS_C,NOMBRE_C,EXPANDIDO_C];
		internal static ulong StringAIndice(string array) {
			ulong flags = 0UL;
			int indice = 0;
			foreach(var flag in _todos) {
				if (array.Contains(flag)) {
					flags |= 1UL << indice;
				}
				indice++;
			}
			return flags;
		}

		internal static bool NumeroArgumentosCorrecto(int numArgs, bool[] flags) {
			bool numeroCorrecto = true;
			if (flags[DIRECTO]) {
				if (flags[NOMBRE]) {
					numeroCorrecto = numArgs == 4;
				} else {
					numeroCorrecto = numArgs == 3;
				}
			} else if (flags[NOMBRE]) {
				numeroCorrecto = numArgs == 1;
			}
			return numeroCorrecto;
		}
	}
}
