# Sobre DivCalc(Nombre temporal)
Aplicación GUI y CLI en C# para calcular reglas de divisibilidad, 3ª revisión.

Seguramente sea cambiado a CalcDiv, ya que existe una aplicación de móvil con el mismo nombre, aunque es completamente distinta, y CalcDiv solo parece ser usada por una librería de R.
Le cambiaré el nombre una vez haya terminado con el refactoring y saque la siguiente versión de la aplicación CLI.

Este es un proyecto en el que llevo trabajando desde 2021 en C++, y más tarde en Java. Empecé a trabajar en el ya que las reglas de divisibilidad me han parecido interesantes desde hace bastante tiempo, y con lo que he investigado y trabajado con ellas para crear estas aplicaciones, me resultan aún más interesantes y me pregunto si hay más tipos de los que aún no he oido hablar.

La verdad es que tener uno de los 3 repositorios en [divisibility-rules](https://github.com/topics/divisibility-rules) me parece algo increible, en que haya 3 y solo 3, y la verdad es que espero poder tener una aplicación competente, aunque a nadie le importe, porque ahora mismo da algo de pena.

## Datos sobre DivCalc

Si os fijais vereis que este repositorio tiene dos colaboradores [inbaluma](https://github.com/inbaluma) y [JonniThorpe](https://github.com/JonniThorpe).

La lógica de la aplicación la he desarrollado yo por mi cuenta y seguramente seguirá siendo así, pero ellos han desarrollado pruebas unitarias para un trabajo que hicimos juntos para la universidad en la que teníamos que hacerle pruebas a alguna aplicación que tuvieramos. Esto me motivo a terminar la versión 0.1 y a refactorizarla después.

La nota del trabajo sigue pendiente.

# Planes futuros para DivCalc

Si mirais el código en el momento en el que se subió este leéme, vereis que es pequeño pero bastante mal diseñado.

Estoy seguro de que podría seguir trabajando con esto pero la verdad es que quiero mejorarla para facilitar el mantenimiento de la aplicación, y la verdad es que me sentí un poco mal dandole este código a mis compañeros de grupo para la uni.

El plan es refactorizar esta aplicación para que tenga las mismas funcionalidades y más, como varios idiomas para no perder lo que ya tengo, usando las buenas prácticas que debería seguir una aplicación de CLI.
Aquí hay algunos cambios planeados:
  - Rehacer la clase principal para usar un parseador de argumentos
  - Cambiar la estructura de los argumentos para que se parezca a otras aplicaciones de CLI
    - Mantendré la combinación xd por motivos evidentes
    - En concreto haré que los argumentos no se pasen en el mismo sitio sino que deban ser colocados de una forma más típica
  - Argumento para cambiar de idioma
  - Separación del output de la aplicación y la salida de consola
    - Incluye al formateo del output a JSON

# Como usar e interpretar DivCalc

Las reglas de divisibilidad se aplican a números en su base, que llamaremos base o "raiz" en varias partes del código, para ver si son divisibles entre un número entero, que llamaremos divisor y en el código aparece a veces como "num".

Las reglas de coeficientes pueden tener una longitud arbitraria, llamada en varias partes del código como "coeficientes" o "cantidad".

Para aplicar las reglas de coeficientes pondré un ejemplo:

Supongamos que ejecutas: `DivCalc.exe -d 7 10 2`, deberías obtener `2, 3`

Lo que significa esto es que has buscado la regla de divisibilidad de 7 en base 10 y quieres aplicar una regla de coeficientes de longitud 2.

Como ejemplo calcularemos si 10534 es divisible entre 7:

Separamos 10534 en dos partes, la de la derecha tan larga como la regla.

Obtenemos 105 y 34.

Ahora multiplicamos cada número de la parte derecha por su elemento de la regla en la misma posición y los sumamos todos.

Es decir, tendremos 105 y 3 * 2 + 4 * 3, que es igual a 18, al sumarlo con la parte izquierda nos queda 123.

Una vez sumados vemos si el número obtenido es divisible entre el divisor, si no podemos determinamos repetimos todos los pasos anteriores al número obtenido.

Por brevedad, terminaremos aquí sabiendo que 123 no es divisible entre 7.

Todos los números obtenidos serán divisiblies entre el divisor si, y solo si, el número original lo era.

Por lo tanto 10534 en decimal no es divisible entre 7, ya que 123 no lo es.

Hay una cantidad infinita de reglas de coeficientes para cualquier base, divisor y longitud, por defecto se devuelve la de menores valores absolutos.

El resto de reglas obtenidas con -x se explicarán más tarde
