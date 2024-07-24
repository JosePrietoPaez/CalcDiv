# Sobre *CalcDiv*

Aplicación CLI en C# para calcular reglas de divisibilidad.

Este es un proyecto en el que he estado trabajando desde 2021, inicialmente en C++ y luego en Java. Mi motivación es mejorar mis capacidades de programación y aprender durante el camino.

Es bastante raro que en [divisibility-rules](https://github.com/topics/divisibility-rules) solo haya tres repositorios, uno siendo este, aunque este sea un nicho muy pequeño quiero contribuir algo decente.

Aunque la idea de esta aplicación fuera propia y no haya buscado otros proyectos similares,
recomiendo echarle un vistazo a [grand-unified-divisibility-rule](https://github.com/lemiorhan/grand-unified-divisibility-rule) de [@lemiorhan](https://github.com/lemiorhan),
explica en bastante detalle muchos conceptos usados en *CalcDiv*, aunque está en inglés.

Sin embargo, este proyecto quiere facilitar la búsqueda de reglas de divisibilidad para cualquier base entera y una cantidad arbitraria de coeficientes.
Además, pretende buscar reglas que no usen coeficientes.

---

## Cómo usar e interpretar *CalcDiv*

Nos harán falta unos conceptos para entender la aplicación y las reglas de divisibilidad.

### Terminología y definiciones

`Regla de divisibilidad` - Una serie de cálculos que nos permiten averiguar si un número es divisible entre otro sin hacer la división.

Téngase en cuenta que las reglas de divisibilidad dependen de la representación del número y el número por el que (no, ya que estamos usando una regla de divisibilidad,) queremos dividir.
Debido a esto no hay reglas de divisibilidad en unario.

Llamaremos a las reglas de divisibilidad solamente reglas para simplificar.

`Dividendo` - El número que queremos dividir, este número no afecta a la regla, solo al resultado.

`Divisor` - El número por el que dividimos, es uno de los valores que afectan la regla.

`Base` - La base de la representación posicional del dividendo, generalmente es diez, ya que usamos el sistema de numeración decimal, este es el segundo valor que afecta a la regla

*CalcDiv* solo funciona para bases enteras mayores que 1.

Todas las reglas obtenidas para un cierto divisor son equivalentes, aunque sean para bases distintas, pero serán distintas.

Por ejemplo, si queremos saber si 754 es divisible entre 12, en base 10, el dividiendo sería 754, el divisor 12 y la base 10.

*CalcDiv* no ofrece por ahora, y seguramente nunca, reglas para sistemas de numeración no posicionales, como el romano.

Los otros tipos de reglas se explican en la aplicación al obtenerlas.

`Coeficiente` - Un número que se multiplica por otro número, usado en las reglas de coeficientes.

`Reglas de coeficientes` - Reglas que, usando una cantidad concreta de coeficientes, determina si el dividendo es divisible entre el divisor.

Los coeficientes son una sucesión que se repite, distinta para cada regla.
Estos coeficientes se pueden modificar para crear una cantidad ilimitada de reglas válidas.

`Resto` - La cantidad restante al realizar una división entera. Un dividendo divisible entre el divisor da resto cero.

`Factores primos` - Representan un entero son los números primos que, al multiplicarse entre sí, se pueden usar para obtener el entero mencionado. Usados para reglas que no son de coeficientes.

`Primeras cifras` - Las cifras más próximas a la de las unidades.

Por ejemplo, las tres primeras cifras de 82490 son 4, 9 y 0, porque 0 es la cifra de las unidades.

Estas definiciones deberían bastar para entender el resto del documento y la aplicación.

### Como usar *CalcDiv* y usar las reglas de coeficientes

Para aprender a usar *CalcDiv* usaremos un ejemplo:

Veremos si 12735, en base 10, es divisible entre 7, usando 2 coeficientes.

Podríamos ejecutar la aplicación sin argumentos e introducir estos datos en el diálogo,
u obtener el resultado directamente con la opción `--direct-output`, o también `-d`.

Tendremos que ejecutar el siguiente comando, suponiendo que el nombre del ejecutable es `CalcDivCLI.exe` y no se modifica:
`CalcDivCLI.exe --direct-output 7 10 2`, o podemos usar `-d` para abreviar.

7 es el divisor, 10 es la base y 2 la cantidad de coeficientes.

Obtendremos una sucesión de coeficientes al ejecutar el comando. En este caso, `-2, -3`.

Apliquemos esta regla con 12735:

1. Tenemos que separar las primeras cifras de las demás. La cantidad de cifras es la cantidad de coeficientes.
	- En este ejemplo, nos quedan 127 y 35.
2. Multiplicamos las primeras cifras por el coeficiente en la misma posición. Después sumamos los productos.
	- En este ejemplo, como los coeficientes son -2 y -3 y las cifras son 3 y 5.
	- Sus productos son -6 y -15, la suma de ambos es -21.
3. Calculamos la suma del número obtenido con la otra parte del dividendo. Esta suma es divisible entre el divisor si, y solo si, el dividendo original lo es.
	- En este ejemplo, sumamos 127 y -21, obtenemos 106.

Si no podemos saber trivialmente si el número obtenido es divisible o no entre el divisor, aplicamos recursivamente la misma regla, sustituyendo el dividendo por el número obtenido hasta que seamos capaces.

Volvemos a aplicar la regla, esta vez para 106:
1. Tras separar las cifras nos quedan 1 y 06.
2. Tras multiplicar las primeras cifras por los coeficientes y sumarlos obtenemos -18.
3. La suma de 1 y -18 es -17.

Trivialmente, -17, en base 10, no es divisible entre 7.
Por lo tanto, ni 106 ni 12735, el dividendo original, lo son.

---

## Datos sobre *CalcDiv*

En este repositorio verás dos colaboradores: [inbaluma](https://github.com/inbaluma) y [JonniThorpe](https://github.com/JonniThorpe).

La lógica de la aplicación la he desarrollado yo por mi cuenta y seguramente seguirá siendo así. Sin embargo, ellos han desarrollado pruebas unitarias para un trabajo universitario en el que teníamos que probar alguna aplicación que tuviéramos. Esto me motivó a terminar la versión 0.1 y a refactorizarla después.

Sacamos un 7 de 10, así que, fue bien, dadas las circunstancias.

## Planes futuros para *CalcDiv*

Tras la version 0.1 he estado refactorizando muchas partes del código para facilitar su mantenimiento y desarrollo.

Mis planes para características futuras son más pequeñas que las introducidas en la versión 0.3, y seguiré refactorizando el código, ya que hay muchas partes que tienen problemas.

Quiero crear una versión GUI de la aplicación, parecida a lo que creé en Java.
