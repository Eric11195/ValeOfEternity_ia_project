# Vale of Eternity

Este proyecto es una adaptación del homónimo juego de mesa.

En este proyecto los distintos jugadores están controlados por una inteligencia artificial.

Esta inteligencia artificial se basa en probabilidades. Y buscará sinergias para obtener la mayor cantidad de puntos posibles. Denegando puntos a los rivales si considera que es la mejor estrategia.

Se considera que el juego es un punto de partida interesante porque contiene una gran cantidad de pequeñas decisiones con repercusiones a medio o largo plazo.

## Overview del Juego

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

### Vender una carta

Al vender una carta gana las siguientes monedas según la familia de la carta:

- **Rojo** 3 monedas de 1 de valor
- **Verde** 4 monedas de 1 de valor
- **Azul** 1 moneda de 3 de valor
- **Rosa** 1 moneda de 3 de valor y 1 de 1 de valor
- **Dragones** 1 moneda de 6 de valor

En caso de obtener más monedas de las que puede tener un jugador, el jugador podrá elegir que monedas de las posibles obtener.

En caso de obtener monedas en exceso no se pueden eliminar monedas de aquellas que ya se tenían. Se tendrán que escoger siempre de las obtenidas al vender la carta.

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
- Sinergias con número de cartas en mano.
- Sinergias con familia rosa
- Sinergias con familia roja
- Sinergias con familia verde
- Sinergias con familia azul
- Sinergias con familia dragones
- Sinergias con número de familias
- Sinergias con número de cartas en mesa

## Implementación

## Conclusiones

## Referencias

Por día de uso

### 07/05/2026
- https://www.geeksforgeeks.org/c-sharp/text-textmeshpro-in-unity/
- https://stackoverflow.com/questions/44390454/what-is-the-use-of-region-and-endregion-in-c
- https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Image.html
- https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject.AddComponent.html
- https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject-ctor.html
- https://discussions.unity.com/t/destroy-all-children-of-object/92016
- https://discussions.unity.com/t/how-to-get-list-of-child-game-objects/34696