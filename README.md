# Sobre DivCalc (Nombre temporal)

Aplicación GUI y CLI en C# para calcular reglas de divisibilidad, 3ª revisión.

Seguramente sea cambiado a CalcDiv, ya que existe una aplicación móvil con el mismo nombre, aunque es completamente distinta. Además, CalcDiv solo parece ser usado por una librería de R. Cambiaré el nombre una vez termine el refactoring y lance la siguiente versión de la aplicación CLI.

Este es un proyecto en el que he estado trabajando desde 2021, inicialmente en C++ y luego en Java. Empecé a trabajar en él porque las reglas de divisibilidad me parecen interesantes. Con la investigación y el desarrollo que he realizado para crear estas aplicaciones, las encuentro aún más fascinantes y me pregunto si existen más tipos que aún no conozco.

Es bastante raro que en [divisibility-rules](https://github.com/topics/divisibility-rules) solo haya tres repositorios. Espero poder tener una aplicación competente, aunque a nadie le importe demasiado, porque actualmente está un poco mal.

## Datos sobre DivCalc

En este repositorio verás dos colaboradores: [inbaluma](https://github.com/inbaluma) y [JonniThorpe](https://github.com/JonniThorpe).

La lógica de la aplicación la he desarrollado yo por mi cuenta y seguramente seguirá siendo así. Sin embargo, ellos han desarrollado pruebas unitarias para un trabajo universitario en el que teníamos que probar alguna aplicación que tuviéramos. Esto me motivó a terminar la versión 0.1 y a refactorizarla después.

La nota del trabajo sigue pendiente.

## Planes futuros para DivCalc

Si revisas el código en el momento en que se subió este README, verás que es pequeño pero bastante mal diseñado.

Podría seguir trabajando con esto, pero quiero mejorarlo para facilitar el mantenimiento de la aplicación. Me sentí un poco mal entregando este código a mis compañeros de grupo para la universidad.

El plan es refactorizar esta aplicación para que tenga las mismas funcionalidades y más, como soporte para varios idiomas, usando las buenas prácticas que debería seguir una aplicación de CLI. Aquí hay algunos cambios planeados:
  - Rehacer la clase principal para usar un parseador de argumentos.
  - Cambiar la estructura de los argumentos para que se parezca a otras aplicaciones de CLI.
    - Mantendré la combinación `xd` por motivos evidentes.
    - Haré que los argumentos no se pasen en el mismo sitio sino que deban ser colocados de una forma más típica.
  - Agregar un argumento para cambiar de idioma.
  - Separar el output de la aplicación de la salida de consola.
    - Esto incluye el formateo del output a JSON.

## Cómo usar e interpretar DivCalc

Las reglas de divisibilidad se aplican a números en su base, que llamaremos "base" o "raíz" en varias partes del código, para ver si son divisibles entre un número entero, que llamaremos "divisor" y en el código aparece a veces como "num".

Las reglas de coeficientes pueden tener una longitud arbitraria, llamada en varias partes del código como "coeficientes" o "cantidad".

Para aplicar las reglas de coeficientes pondré un ejemplo:

Supongamos que ejecutas: `DivCalc.exe -d 7 10 2`, deberías obtener `2, 3`.

Esto significa que has buscado la regla de divisibilidad de 7 en base 10 y quieres aplicar una regla de coeficientes de longitud 2.

Como ejemplo, calcularemos si 10534 es divisible entre 7:

1. Separamos 10534 en dos partes: la parte derecha tan larga como la regla.
   - Obtenemos 105 y 34.
2. Multiplicamos cada número de la parte derecha por su elemento de la regla en la misma posición y los sumamos todos.
   - Es decir, tendremos 105 y 3 * 2 + 4 * 3, que es igual a 18.
   - Al sumarlo con la parte izquierda nos queda 123.
3. Vemos si el número obtenido es divisible entre el divisor.
   - Si no podemos determinarlo, repetimos todos los pasos anteriores con el número obtenido.

Por brevedad, terminaremos aquí sabiendo que 123 no es divisible entre 7.

Todos los números obtenidos serán divisibles entre el divisor si, y solo si, el número original lo era.

Por lo tanto, 10534 en decimal no es divisible entre 7, ya que 123 no lo es.

Hay una cantidad infinita de reglas de coeficientes para cualquier base, divisor y longitud. Por defecto, se devuelve la de menores valores absolutos.

El resto de reglas obtenidas con `-x` se explicarán más tarde.
