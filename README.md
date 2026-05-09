# Vale of Eternity

## Instalación y uso

Todo el contenido del proyecto está disponible aquí en el repositorio y usa **Unity 6 (6000.2.15 LTS)**.

Al no estar publicada todavía ninguna versión ejecutable del prototipo, ni enlazado ningún video con las pruebas realizadas, es necesario abrir el proyecto en Unity y usarlo desde allí.

## Introducción

Este proyecto es una adaptación del homónimo juego de mesa (sin expansiones).

En este proyecto los distintos jugadores están controlados por una inteligencia artificial.

Esta inteligencia artificial se basa en probabilidades. Y buscará sinergias para obtener la mayor cantidad de puntos posibles. Denegando puntos a los rivales si considera que es la mejor estrategia.

Se considera que el juego es un punto de partida interesante porque contiene una gran cantidad de pequeñas decisiones con repercusiones a medio o largo plazo.


## Sinopsis del Juego

En este juego los distintos jugadores son cazadores de criaturas. Deberan capturarlas o comerciar con ellas para obtener el máximo número de puntos posibles.

La característica más interesante del juego es su sistema de divisa. Pues hay monedas de 3 tipos. De valor 1, 3 y 6. Cada jugador solo puede tener hasta cuatro piedras en cualquier momento y al gastar cualquiera no obtendrá cambio.

Cada carta tiene una familia (color), un coste y uno o varios efectos.

## Reglas de Juego

El juego se desarrolla en rondas. Hasta jugar 10 o hasta que algún jugador obtenga 60 puntos.

Este juego tiene un mazo compartido por todos los jugadores.

### Inicio de Partida

Se decide el jugador que comenzará de forma aleatoria. Este obtiene el token de primer jugador.

Cada jugador comienza con tantos puntos como número de jugador sea durante el primer turno. En una partida de cuatro jugadores empezarán con 1, 2, 3 y 4. Siendo el jugador con un punto aquel con el token de primer jugador.

A partir de ahora numeraremos los jugadores con su número respecto al jugador con el token de primer jugador. En sentido antihorario.

Dentro de cada ronda hay varias fases:

- Fase de Caza
- Fase de Juego
- Fase de Activación
- Fase Final

### Fase de Caza

Durante la fase de caza se revelan tantas cartas del mazo como el doble de jugadores esten jugando.

Estas cartas se colocan en el mercado.

Durante esta fase cada jugador tendrá que colocar uno de sus marcadores en una de las cartas en el mercado.
Esta fase acaba cuando todas las cartas reveladas tengan un marcador de jugador sobre ellas.

Los jugadores se tornan poniendo marcadores en este orden:
>> 1 > 2 > 3 > 4 > 4 > 3 > 2 > 1

Siendo 1 el jugador con el token de primer jugador, y 2 el jugador a su derecha.

### Fase de Juego

Cada jugador jugara su turno en sentido antihorario. En este orden:
>> 1 > 2 > 3 > 4

Durante su turno cada jugador puede hacer cualquier número de las siguientes acciones en cualquier orden.

- **Jugar una carta**. Pagando el coste de una carta en la mano colocala en la mesa.
- **Capturar una carta**. Quita uno de tus marcadores de jugador de una de las cartas del mercado y ponla en tu mano.
- **Vende una carta**. Quita uno de tus marcadores de jugador de una de las cartas del mercado, colocala en descartes. Gana la divisa correspondiente.
- **Eliminar una carta**. Paga igual valor al número de ronda. Descarta una de las cartas en tu mesa.

### Fase de activación

Activa los efectos de final de turno de las cartas en tu zona de la mesa.
Puedes elegir el orden en el que resolver los efectos. Pero es obligatorio activar todos.

### Fase Final

Suma uno al número de ronda.

El siguiente jugador en sentido antihorario consigue el token de primer jugador.

### Jugar una carta

Antes de jugar una carta hay que pagar su coste. Descartando las monedas que se usen para pagar por su coste de antemano.

Las cartas puede tener uno o más de los siguientes efectos:

- **Instantaneo** al jugarse este efecto se hace de inmediato. En caso de ser un efecto que cuente criaturas en mesa. La criatura con este efecto se cuenta.
- **Infinito** mientras esta carta está en la mesa se hace este efecto se aplica. En caso de ser un efecto que cuente criaturas. Esta criatura no se cuenta en el efecto.
- **Final** al final de cada turno haz este efecto

_El número máximo de cartas en mesa es igual al número de ronda_

### Vender una carta

Al vender una carta gana las siguientes monedas según la familia de la carta:

- **Rojo** 3 monedas de 1 de valor
- **Verde** 4 monedas de 1 de valor
- **Azul** 1 moneda de 3 de valor
- **Rosa** 1 moneda de 3 de valor y 1 de 1 de valor
- **Dragones** 1 moneda de 6 de valor

En caso de obtener más monedas de las que puede tener un jugador, el jugador podrá elegir que monedas de las posibles obtener.

En caso de obtener monedas en exceso no se pueden eliminar monedas de aquellas que ya se tenían. Se tendrán que escoger siempre de las obtenidas al vender la carta.

## Punto de partida

Se parte únicamente de las reglas en formato físico del juego, y de las imagenes de las cartas y objetos extraidas de la [wiki](https://valeofeternity.wiki.gg) del juego.

El proyecto de unity parte de un proyecto vacio.

## Planteamiento del problema

La práctica consiste en desarrollar este juego para un soporte digital en la que todos los jugadores estén controlados por la IA desarrollada.

A. En la pantalla se pueden ver las distintas zonas del juego claramente diferenciadas. Estas son mercado, y tablero y mano de cada jugador. El tablero de todos los jugadores será visible en todo momento. Se podrá seleccionar la mano de que jugador ver presionando sobre el número del jugador. Y un marcador de un ojo sustituirá el número del jugador cuya mano se esté viendo.
Jugablemente, todas las cartas tienen sus efectos definidos. Se pueden ver la divisa que posee cada jugador en su zona y los puntos de victoria actuales que posee.

B. Durante la fase de caza, cada jugador escoge cartas a su debido tiempo hasta tener 2. Las cartas se escogeran teniendo en cuenta posibles sinergias con cartas en la mesa o mano del jugador correspondiente, su precio de venta o si con alguna carta otro de los jugadores podría sacar una gran ventaja. El parametro decisivo viene dado por una lista de prioridades ordenadas.

C. Durante la fase de juego, cada jugador escoge cartas con su marcador en el mercado y las pone en su mano o las vende. Además de jugar cartas de entre estas o que ya tuviese en su mano o eliminar alguna de la mesa. Tomarán tantas acciones de las anteriores como pueda o crea oportuno para maximizar los puntos ganados o crear una situación favorable en turnos posteriores. Que cartas jugar viene dado por las sinergias con cartas en la mesa, o requerimientos para jugar o aprovechar los efectos de las cartas con el fin de obtener más puntos que los rivales al final de la partida. Todo esto gobernado por la lista de prioridades.

D. Durante la fase de relojes, caja jugador escoge el orden en que resolver los efectos de relojes de las cartas en su mesa. Intentando maximizar el número de puntos o la obtención de divisa o cartas en mano para el próximo turno. El parametro decisivo viene dado por una lista de prioridades.

E. Cada jugador defina una lista de prioridades ordenadas a seguir. Estas prioridades se recalculan en los momentos en los que una acción del propio jugador o una acción de un oponente cambie el estado de su mesa, mano o divisa. Todas las acciones posibles de un jugador se rigen por esta lista de prioridades. El algoritmo que calcula las prioridades tendrá como misión maximizar la obtención de puntos relativa al resto de jugadores. En algunos casos esto implicará jugar efectos que no obtengan la mayor cantidad de puntos, pero que frenen el avance de otro jugador.
Para confirmar el comportamiento de la IA, se dispondrá de una métricas visibles durante la partida y al final de ella. Estas incluyen, puntos de victoria finales y por turno; obtención y gasto de divisa por turno; y cartas vendidas en contraste con las cartas puestas en mano o robadas.

## Diseño de la Solución

Se harán algoritmos distintos para cada fase de la partida.

### Selección de Cartas

Un algoritmo de selección de cartas tomará tres parametros. Una lista de cartas, un escalado y un número variable de prioridades

Para cada prioridad dada evaluará los individuos asignandoles una puntuación (un número entero).
Se guardará la puntuación de cada individuo para cada propiedad.

Cuando este completamente evaluado hará una suma ponderada. Obteniendo una puntación final para cada individuo. La ponderación de cada prioridad vendrá dada por el parametro escalado. La primera prioridad será aquella con menor ponderación generalmente y la última aquella con mayor ponderación.

#### Escalado

Algunos escalados posibles son:

- Lineal (1,2,3,...)
- Potencias de 2 (1,2,4,...)
- Sucesión de Fibonnacci (1,2,3,5,...)
- Constante (1,1,1,...)

#### Prioridades

Algunas prioridades son:

- Puede ser pagada
- Alto coste
- Bajo coste
- Efectos finales
- Efectos instantaneos
- Valor por familia
- Número de piedras por familia
- Sinergias con monedas de 1
- Sinergias con monedas de 3
- Sinergias con monedas de 6
- Sinergias con familia rosa
- Sinergias con familia roja
- Sinergias con familia verde
- Sinergias con familia azul
- Sinergias con familia dragones
- Sinergias con número de familias
- Sinergias con número de cartas en mesa
- Sinergias con número de cartas en mano

## Implementación

## Conclusiones

## Referencias

Por día de uso

### 07/05/2026

- [https://www.geeksforgeeks.org/c-sharp/text-textmeshpro-in-unity/](https://www.geeksforgeeks.org/c-sharp/text-textmeshpro-in-unity/)
- [https://stackoverflow.com/questions/44390454/what-is-the-use-of-region-and-endregion-in-c](https://stackoverflow.com/questions/44390454/what-is-the-use-of-region-and-endregion-in-c)
- [https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Image.html](https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Image.html)
- [https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject.AddComponent.html](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject.AddComponent.html)
- [https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject-ctor.html](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject-ctor.html)
- [https://discussions.unity.com/t/destroy-all-children-of-object/92016](https://discussions.unity.com/t/destroy-all-children-of-object/92016)
- [https://discussions.unity.com/t/how-to-get-list-of-child-game-objects/34696](https://discussions.unity.com/t/how-to-get-list-of-child-game-objects/34696)

### 08/05/2026

- [https://www.geeksforgeeks.org/c-sharp/lambda-expressions-in-c-sharp/](https://www.geeksforgeeks.org/c-sharp/lambda-expressions-in-c-sharp/)

### 09/05/2026

- [https://valeofeternity.wiki.gg](https://valeofeternity.wiki.gg)
- [https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Events.UnityEvent.html](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Events.UnityEvent.html)
- [https://docs.unity3d.com/6000.0/Documentation/ScriptReference/30_search.html?q=unityAction](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/30_search.html?q=unityAction)
- [https://boardgamegeek.com/filepage/262447/english-rules](https://boardgamegeek.com/filepage/262447/english-rules)
- [Vale of Eternity Rulebook](VOE_RULEBOOK_EN.pdf)