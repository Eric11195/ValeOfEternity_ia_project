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

En este juego los distintos jugadores son cazadores de criaturas fantásticas. Deberan capturarlas o comerciar con ellas para obtener el máximo número de puntos posibles.

La característica más interesante del juego es su sistema de divisa. Pues hay monedas de 3 tipos. De valor 1, 3 y 6. Cada jugador solo puede tener hasta cuatro piedras en cualquier momento y al gastar cualquiera no obtendrá cambio.

Cada carta tiene una familia (color), un coste y uno o varios efectos.

## Reglas de Juego

Estas reglas están enfocadas a entender el proyecto, aquellas cosas enfocadas al juego físico han sido obviadas. Para una vista de las reglas completas haz clic [aquí](VOE_RULEBOOK_EN.pdf).

El juego se desarrolla en rondas. Hasta jugar 10 o hasta que algún jugador obtenga 60 puntos.

Este juego tiene un mazo compartido por todos los jugadores.

### Inicio de Partida

Se decide el jugador que comenzará de forma aleatoria. Este obtiene el token de primer jugador.

Cada jugador comienza con tantos puntos como número de jugador sea durante el primer turno. En una partida de cuatro jugadores empezarán con 1, 2, 3 y 4. Siendo el jugador con un punto aquel con el token de primer jugador.

A partir de ahora numeraremos los jugadores con su número respecto al jugador con el token de primer jugador. En sentido antihorario.

Dentro de cada ronda hay varias fases:

```mermaid
---
title: Game Cycle
---
stateDiagram-v2
[*] --> RandomPlayerGetsFirstPlayerMarker
RandomPlayerGetsFirstPlayerMarker --> MarketPhase: Round 1
state MarketPhase{
  [*] --> TwoCardPerPlayerAreRevealed
  TwoCardPerPlayerAreRevealed --> FirstPlayerChoosesFirstCards
  FirstPlayerChoosesFirstCards --> SecondPlayerChoosesFirstCards
  SecondPlayerChoosesFirstCards --> ThirdPlayerChoosesFirstCard
  ThirdPlayerChoosesFirstCard --> FourthPlayerChoosesFirstAndSecondCard
  FourthPlayerChoosesFirstAndSecondCard --> ThirdPlayerChoosesSecondCard
  ThirdPlayerChoosesSecondCard --> SecondPlayerChoosesSecondCard
  SecondPlayerChoosesSecondCard --> FirstPlayerChoosesSecondCard
  FirstPlayerChoosesSecondCard --> [*]
}
MarketPhase --> PlayPhase
state PlayPhase{
  [*] --> FirstPlayerPlays
  FirstPlayerPlays --> SecondPlayerPlays
  SecondPlayerPlays --> ThirdPlayerPlays
  ThirdPlayerPlays --> FourthPlayerPlays
  FourthPlayerPlays --> [*]
}
PlayPhase --> ClockPhase
state ClockPhase{
  [*] --> FirstPlayerActivatedClocks
  FirstPlayerActivatedClocks --> SecondPlayerActivatedClocks
  SecondPlayerActivatedClocks --> ThirdPlayerActivatedClocks
  ThirdPlayerActivatedClocks --> FourthPlayerActivatedClocks
  FourthPlayerActivatedClocks --> [*]
}
ClockPhase --> CleanupPhase
state CleanupPhase{
  [*] --> Round+=1
  Round+=1 --> FirstPlayerMarkerMovesToNextPlayer
  FirstPlayerMarkerMovesToNextPlayer --> [*]
}
state if_state <<choice>>
CleanupPhase --> if_state
if_state --> EndGame: Player has 60 or more points or Round>10
if_state --> MarketPhase: Round < 10 && No player has >=60 points
```

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

__El número máximo de cartas en mesa es igual al número de ronda__

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

## Planteamiento del Problema

La práctica consiste en desarrollar este juego para un soporte digital en la que todos los jugadores estén controlados por la IA desarrollada.

- **A.** En la pantalla se pueden ver las distintas zonas del juego claramente diferenciadas. Estas son mercado, tablero y mano de cada jugador. El tablero de todos los jugadores será visible en todo momento. Se podrá seleccionar la mano de que jugador ver presionando sobre el número del jugador. Y un marcador de un ojo sustituirá el número del jugador cuya mano se esté viendo.
Jugablemente, todas las cartas tienen sus efectos definidos. Se pueden ver la divisa que posee cada jugador en su zona y los puntos de victoria actuales que posee.
Además, un panel scrolleable dará cuenta de todas las acciones y decisiones de todos los jugadores.

- **B.** Durante la fase de caza, cada jugador escoge cartas a su debido tiempo hasta tener 2. Las cartas se escogeran teniendo en cuenta posibles sinergias con cartas en la mesa o mano del jugador correspondiente, su precio de venta o si con alguna carta otro de los jugadores podría sacar una gran ventaja. El parametro decisivo viene dado por la prioridad actual del jugador.

- **C.** Durante la fase de juego, cada jugador escoge cartas con su marcador en el mercado y las pone en su mano o las vende. Además de jugar cartas de entre estas o que ya tuviese en su mano o eliminar alguna de la mesa. Tomarán tantas acciones de las anteriores como pueda o crea oportuno para maximizar los puntos ganados o crear una situación favorable en turnos posteriores. Que cartas jugar viene dado por las sinergias con cartas en la mesa, requerimientos para jugarlas o aprovechar los efectos de las cartas con el fin de obtener más puntos que los rivales al final de la partida. Todo esto gobernado por la lista de prioridades.

- **D.** Durante la fase de relojes, caja jugador escoge el orden en que resolver los efectos de relojes de las cartas en su mesa. Intentando maximizar el número de puntos o la obtención de divisa o cartas en mano para el próximo turno. El parametro decisivo viene dado por una lista de prioridades.
Este algoritmo usará como base el algoritmo de resolución de problemas con incertidumbre **[Monte Carlo Rollout](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search)**

- **E.** Cada jugador defina una prioridad a seguir. Estas prioridades se recalculan en los momentos en los que una acción del propio jugador o una acción de un oponente cambie el estado de su mesa, mano o divisa. Todas las acciones posibles de un jugador se rigen por la prioridad actual. El algoritmo que calcula las prioridades tendrá como misión maximizar la obtención de puntos relativa al resto de jugadores. En algunos casos esto implicará jugar efectos que no obtengan la mayor cantidad de puntos, pero que frenen el avance de otro jugador.

Para confirmar el comportamiento de la IA, se dispondrán de métricas visibles durante la partida y al final de ella. Estas incluyen, puntos de victoria finales y por turno; obtención y gasto de divisa por turno; y cartas vendidas en contraste con las cartas puestas en mano o robadas.

## Diseño de la Solución

El flujo de juego y las llamadas a todas las funciones necesarias para el funcionamiento de está aplicación se encuentran en el fichero [GameManager.cs](VOE/Assets/Scripts/GameManager.cs)

### Apartado A

#### Identificación de Zonas de Juego

Una imagen de fondo nos permitirá ver delimitadas las distintas zonas de juego.
Con el mercado estando a la derecha del todo. La mano del jugador siendo la parte de abajo de la pantalla y las zonas de cada jugador en el espacio que queda, donde colocaremos las cartas de su zona, así como la divisa que tengan.

#### Zonas de Cartas

En cada zona un objeto con un componente de tipo [CardAreaManager](VOE/Assets/Scripts/CardAreaManager.cs) será el encargado de crear las cartas, objetos con un [CardComponent.cs](VOE/Assets/Scripts/CardComponent.cs) y colocarlas en la posición correcta. **CardAreaManager** tiene varios parametros, filas y columnas, y 2 transforms. Estos 2 transforms delimitan el limite inferior izquierdo y el limite superior derecho de la zona. De forma que las cartas se coloquen automáticamente en la posición adecuada en función de esto y su índice en el array que las contiene mediante una interpolación entre ambas posiciones.

#### Representación de Divisa

En la zona de cada jugador un objeto con un componente de tipo [StoneRepresentator](VOE/Assets/Scripts/StoneRepresentator.cs) se encargará de mostrar la divisa de cada jugador. Teniendo en cuenta que aunque el límite de divisa sea de 4 piedras/monedas. Hay cartas que podrían ampliarlo.

#### Cartas

Las cartas son una gran parte del trabajo. El juego cuenta con 68 cartas únicas, cada una con efectos específicos.

##### Información Fundamental

Cada una está identificada unequívocamente por un [CardNameId](VOE/Assets/Scripts/CardsEnum.cs). Y se asocian por el valor de este a la posición del array de recurso de carta (nombre de imagen) en [FileNames.cs](VOE/Assets/Scripts/FileNames.cs).

##### Parámetros de las Cartas

Todos los parametros de cada carta del juego están guardados en [CardData.cs](VOE/Assets/Scripts/CardData.cs). Cada carta tiene:

- una familia -> en enumerado entre Rojo, Azul, Verde, Rosa y Dragones
- un precio -> valor numérico
- enabler flags -> enumerado que contiene que tipo de estrategias permiten
- payoff flags -> enumerado que contiene que tipo de estrategias recompensan. De forma que al tener varias cartas con enabler flags de un tipo, las cartas con payoff flags de ese mismo tipo serán más poderosas
- funciones de:
  - reloj
  - al entrar al tablero
  - al salir del tablero

###### Funciones de Cartas

Las funciones de cada carta están almacenadas en [CardFunc.cs](VOE/Assets/Scripts/CardFunc.cs). Clasificadas en regiones según la carta de la que provienen y en clock, enter, exit y trigger.

- Clock son funciones de final de turno
- Enter son funciones al entrar en el tablero
- Exit son funciones al salir del tablero
- Trigger son funciones a las que se suscribirán distintas UnityActions de los jugadores. De forma que al entrar o al salir alguna carta del tablero el jugador se podría suscribir a una de estas funciones. En cartas por ejemplo que al pagar costes con monedas de cierto tipo proporcionan puntos de victoria.

###### Flags de Cartas

Cada carta almacena dos enteros que pueden contener distintas flags.

Las clasificamos con estas etiquetas para que la IA pueda buscar coincidencias y evaluarlas correctamente.

- EnablerFlags -> permite saber que sinergias habilita esta carta.
- PayoffFlags -> permite saber que sinergias mejoran el efecto de esta carta.

De forma que conviene tener el máximo número de cartas con enabler flags de un tipo, para hacer que el efecto de una carta con PayoffFlags de ese tipo sea lo más poderoso posible.

Los arquetipos y sinergías que se tendrán en cuenta, intentando categorizar todos los del juego, se pueden encontrar en [CardFlags.cs](VOE/Assets/Scripts/CardFlags.cs) serán:

- big_hand -> estrategia que te recompensa por tener muchas cartas en mano
- familyR -> estrategia que te recompensa por tener muchas cartas rojas
- familyB -> estrategia que te recompensa por tener muchas cartas azules
- familyG -> estrategia que te recompensa por tener muchas cartas verdes
- familyP -> estrategia que te recompensa por tener muchas cartas rosas
- familyD -> estrategia que te recompensa por tener muchas cartas dragones
- stones1 -> estrategia que te recompensa por tener o usar piedras/monedas de valor 1
- stones3 -> estrategia que te recompensa por tener o usar piedras/monedas de valor 3
- stones6 -> estrategia que te recompensa por tener o usar piedras/monedas de valor 6
- number_of_families -> estrategia que te recompensa por tener el mayor número de cartas de distintas familias en tu mesa
- clocks -> estrategia que te recompensa por tener muchas cartas con efectos de reloj o final de turno en tu mesa
- etbs -> término proveniente de magic: the gathering. Significa efectos de (Enter The Battlefield), es decir de ponerse en el tablero. Por lo que da nombre a la estrategia que recompensa tener muchas cartas con efectos al entrar
- recursion -> estrategia que te recompensa por poner cartas de tu mesa en tu mano, para poder volverlas a usar. Posiblemente por tener efectos al entrar poderosos y tener bajo coste
- removal -> no es una estrategia en si, pero marca a las cartas que pueden eliminar cartas enemigas. Para desestabilizar sus planes.
- space_free -> tampoco es una estrategia, pero marca a las cartas que eliminan cartas indeseadas de tu tablero o que se eliminan a si mismas tras jugarse
- high_costs -> estrategía que te recompensa por tener cartas de alto coste
- low_costs -> estrategia que te recompensa por tener cartas de bajo coste
- loose_points -> estrategia que te recompensa por perder puntos, o no ir en cabeza
- cost_reduction -> estrategia que te recompensa por tener cartas que reduzcan el coste de tus otras cartas de forma general o por familia
- multicast -> estrategia que te recompensa por jugar muchas cartas, y hacer que un número alto de criaturas entren al tablero
- tableau_width -> estrategia que te recompensa por tener muchas cartas en tu tablero

##### Representación en pantalla

Las cartas se representan en pantalla por medio de una entidad con un [CardComponent](VOE/Assets/Scripts/CardComponent.cs). En su creación recibe un [CardNameId](VOE/Assets/Scripts/CardsEnum.cs) y en función de este selecciona su imagen.

Además poniendo el ratón sobre una de las cartas una representación de tamaño aumentado de esta se colocará en pantalla para poder leer el texto mejor.

### Apartado B

Los procesos de la ronda de caza se verán en [MarketRound.cs](VOE/Assets/Scripts/MarketRound.cs).

La parte importante de esta fase es que los jugadores escojan las cartas más valiosas para ellos. Esto lo hacen en la función choose_card_on_market de [Player.cs](VOE/Assets/Scripts/Player.cs).

En esta función reciben una lista de cartas posibles entre las que elegir, escogeran una, la quitan de la lista de cartas posibles para el resto de jugadores y a partir de ahora sabran que esa carta es de su propiedad.

#### Escoger cartas

Para llevar a cabo la mayoría de sus acciones los jugadores mirarán su lista de prioridades. Esta se actualiza en cualquier momento en el que las cartas en su mano o en su mesa, o la divisa que poseen cambie.

De forma que escogeran cartas en función de su lista de propiedades.

Algunas prioridades básicas podrían ser:

- obtener puntos
- obtener divisa
- obtener cartas en mano
- jugar su carta favorita

##### Carta Favorita

Al escoger sus prioridades los jugadores tendrán en cuenta sus cartas en mano, en su mesa, poseidas en el mercado, o aún libres en el mercado.

De entre todas estas escogerán la carta que, al jugarla, satisfaga de mejor forma las prioridades que tienen en ese momento. Esa carta será su **carta favorita**.

##### Prioridades

###### Obtener puntos

Para saber que carta pueda obtener una mayor cantidad de puntos se hara una suma ponderada entre las sinergias que permite, y aquellas por las que recompensa. Adiccionalmente cada carta tiene un estimado de puntos al jugarla y por ronda. De forma que tomará esto en cuenta también y el número de rondas que quedan para saber cual es la carta que mayor cantidad de puntos le otorga.

```cpp
int ponderate_points_of_card(CardNameId cni){
  int enabler_ponderation = 2* (flags coincidentes con cartas payoff en la mesa) + 1*(flags coincidentes con cartas payoff en la mano);
  int payoff_ponderation = 2*(flags coincidentes con cartas enabler en la mesa) + 1*(flags coincidentes con cartas enabler en la mano);
  int estimado_puntos_al_jugarla = CardData.getCard(cni).estimado_puntos_al_jugarla;
  int estimado_puntos_por_ronda = rondas_que_faltan_por_jugar * CardData.getCard(cni).estimado_puntos_por_ronda;

  return enabler_ponderation + payoff_ponderation + estimado_puntos_al_jugarla + estimado_puntos_por_ronda;
}
```

##### Obtener divisa

Para esto tendrá en cuenta tanto el valor de divisa de las cartas del mercado, como aquellas cartas que generen monedas en la mano como las sinergias con cada tipo de divisa.

```cpp

int get_stones_ponderated_value(stone_quant sq){
  -- número de piedras generadas de cada tipo multiplicado con la sinergia con ellas
  value += stones_if_sold.s[StoneType.Stones_1] * sinergies_with_1_stones;
  value += stones_if_sold.s[StoneType.Stones_3]* sinergies_with_3_stones;
  value += stones_if_sold.s[StoneType.Stones_6]* sinergies_with_6_stones;
  value += total_stone_value;
}

int ponderate_stone_gain_of_card_in_market(CardNameId cni){
  stone_quant stones_if_sold = CardFamily.stone_value.get_value_per_family(CardData.getCard(cni).family);
  return get_stones_ponderated_value(stone_quant sq);
}

int poderate_stone_gain_of_card_in_hand(CardNameId cni){
  int value = 0;
  
  stone_quant stones_if_played = CardData.getCard(cni).stones_if_played;
  value += get_stones_ponderated_value(stone_quant sq);
  
  stone_quant stones_per_turn = CardData.getCard(cni).stones_per_turn;
  value += get_stones_ponderated_value(stone_quant sq);

  return value;
}
```

##### Jugar carta favorita

Para esto usaremos el siguiente grafo para saber que deberíamos hacer.

```mermaid
---
title: Favourite Card Priority Decision Tree
---
stateDiagram-v2
state if_state <<choice>>
state if_state2 <<choice>>
state if_state3 <<choice>>
state if_state4 <<choice>>
[*] --> IsInHand?
IsInHand? --> if_state
if_state --> CanBePayed?:true
if_state --> IsInMarket?:false
IsInMarket? --> if_state2
if_state2 --> chooseIt:true
if_state2 --> throwError:false
CanBePayed? --> if_state3
if_state3 --> WeNeedMoreMoney?:true
if_state3 --> ChooseWithObtainMoneyPriority:false
WeNeedMoreMoney? --> if_state4
if_state4 --> ChooseWithObtainMoneyPriority:true
if_state4 --> ChooseWithObtainPointsPriority:false
```

### Apartado C

Las acciones durante el turno vienen dadas por las prioridades del [Apartado E](#apartado-e).

Podemos modelar esto como una máquina de estados simple.

Mientras la prioridad sea obtener dinero. Se intentará vender la carta con piedras más valiosas del mercado, o jugar cartas generadoras de piedras/divisa.

Mientras la prioridad sea jugar la carta favorita. Se jugará inmediatamente esta, y se saldrá de este estado.

Mientras la prioridad sea obtener cartas en mano. Se llevarán cartas del mercado a la mano y se jugarán cartas que roben cartas.

Mientras la prioridad sea jugar removal. Se jugarán cartas que eliminen cartas del resto de jugadores sobre aquel con más puntos con algún objetivo valido.

### Apartado D

Durante la fase de activación de relojes sabemos con certeza que cada jugador debe activar todos los efectos de reloj en su mesa. Y de que un jugador ha de activar todos los efectos antes de que lo haga el siguiente.

Por lo que para resolver el problema de en que orden activarlos usaremos el algoritmo de resolución de problemas con incertidumbre **[Monte Carlo Rollout](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search)**.

Para usar este algoritmo usaremos [MonteCarloClockSimulator.cs](VOE/Assets/Scripts/MonteCarloClockSimulator.cs). En este crearemos un DumbPlayer.cs con los parametros del jugador real. Y le dispondremos a hacer la ronda de relojes. Los cambios al estado de juego que pudiese hacer este bot no se ven reflejados en el juego real.

Este algoritmo se trata de un algoritmo recursivo con decisiones en árbol.

```cpp
struct Rewards{
  stone_count sq;
  int points;
  int cards_in_hand;
}
struct ClockActivation{
  int order;
  int card_decision;
}
int ponderate_reward(Priorities player_priorities, Rewards rwrd){
  int value = 0;
  value += get_stones_ponderated_value(rwrd);
  value += points * idx_of_gain_points_in_player_priorities;
  value += card_in_hand * big_hand_sinergy_puntuation
  return value;
}
//value of ponderated + 
static tuple<int,List<ClockActivation>> best_solution;
//card_order inicializado a un array de -1 del mismo tamaño que el número de cartas con relojes en mesa
//cards_to_activate tiene un identificador por cada carta con un reloj en mesa
void MonteCarloClockResolutor(List<ClockActivation> activation_order, Rewards rew, const List<CardNameId> cards_to_activate, int depth){
  if(no_more_cards_to_choose){
    int answer_value = ponderate_answer(rew);
    if(answer_value > best_solution.first) 
      best_solution(answer_value, activation_order.get_deep_copy());
    return;
  }

  for(int i = 0; i < card_order.Count; ++i){
    if(card_order != -1) continue; //Ya ha sido escogido
    if(card_has_decisions){
      Rewards r;
      int decision = 0;
      //Por cada decisión posible que tenga la carta tiramos un algoritmo
      foreach(Decision d in CardData.getCard(cards_to_activate).decisions){
        Rewards rewards_of_this_card = CardData.getCard(cards_to_activate[i]).get_cards_reward(d);
        rew += rewards_of_this_card;
        activation_order[i] = ClockActivation(depth, decision);
        MonteCarloClockResolutor(activation_order, rew, cards_to_activate, depth+1);
        activation_order[i] = ClockActivation(-1,-1);
        rew -= rewards_of_this_card;
        ++decision;
      }
    }else{
        Rewards rewards_of_this_card = CardData.getCard(cards_to_activate[i]).get_cards_reward(0);
        rew += rewards_of_this_card;
        activation_order[i] = ClockActivation(depth, 0);
        MonteCarloClockResolutor(activation_order, rew, cards_to_activate, depth+1);
        activation_order[i] = ClockActivation(-1,-1);
        rew -= rewards_of_this_card;
        ++decision;
    }
  }
}
```

### Incertidumbre

Este algoritmo podría ser uno de vuelta atrás cualquiera. En la parte en la que se adoptará el enfoque probabilista es con aquellas cartas que devuelvan cartas a la mano o que roben. Como es casi imposible saber que carta será más valiosa devolver a la mano o si robar nos dará un mejor resultado que descartar esa carta por otro efecto, usaremos probabilidad y los valores de sinergias y ponderaciones que usamos para el resto de cosas.

#### Robar una carta

El valor de recompensa que dá robar una carta será el siguiente:

Cuanto más dispersas estén nuestras sinergias, es decir, que no haya una predominante querrá decir que es más probable que robemos una carta que nos sirva, ya sea porque esté empezando la partida o porque todavía no hemos encontrado una línea (una estrategia a seguir) para esta partida.

Después tomaremos en cuenta si nuestra estrategia se basa en tener una mano con muchas cartas. Si es así sumaremos un bonus generoso.

```cpp
int ponderate_draw_on_player(Player p){
  //A mayor sea este valor menos útil será robar cartas, pues indicará que ya tenemos un tablero con una estrategia enfocada y funcionando
  float strategy_predominance = p.get_max_sinergie_value()/ p.get_total_sinergie_value();
  //Cuantas cartas tenemos que tengan sinergia con tener una gran mano de cartas
  int big_hand_strategy_value = p.sinergies[BIG_HAND_SINERGIE];
  return Math.Floor((1-strategy_predominance)*100) + big_hand_strategy_value*10;
}
```

#### Devolver a la mano

De una forma parecida a robar cartas hay estrategias que benefician tener muchas cartas en mesa y otras que requieren reusar cartas poniendolas de vuelta a la mano. Por lo que usaremos otra función de ponderación.
Además de que es complicado saber si una de las cartas que pongamos en mano, verá uso facilmente el próximo turno, sin conocer las cartas que saldrán en la próxima ronda de mercado.

Para esto lanzaremos una vez el algoritmo por cada carta que pudiesemos poner en mano, siendo cada una, una decisión distinta de la carta.

Y para cada carta tendremos en cuenta las sinergias y payoffs que satisface según sus flags o etiquetas. De forma que el valor ponderado de cada una escale en función de estas y las sinergias predominantes en el jugador.

También tendremos en cuenta si hacer que devolver esa carta a su mano impedirá que se haga su efecto de reloj. Lo que a veces podría convenirnos.

### Apartado E

Usaremos las mismas prioridades que en el [apartado B](#escoger-cartas):

- obtener puntos
- obtener divisa
- obtener cartas en mano
- jugar su carta favorita

#### Elección de prioridades

Tendrá en cuenta su carta favorita, sus puntos y los del resto de jugadores, su divisa, el número de espacios libres en el tablero, sus cartas en mano y en el tablero.

Con un árbol de decisiones muy similar al mencionado anteriormente:

```mermaid
---
title: Priority Decision Tree
---
stateDiagram-v2
state if_state <<choice>>
state if_state2 <<choice>>
state if_state3 <<choice>>
state if_state4 <<choice>>
[*] --> ChooseFavouriteCard
ChooseFavouriteCard --> FavouriteCardCanBePlayed?
FavouriteCardCanBePlayed? --> if_state
if_state --> SetPlayFavouriteCardAsPriority: True
if_state --> AreWeLoosingByALot?: False
AreWeLoosingByALot? --> if_state2
if_state2 --> DoWeHaveCardsThatGenerateMoney?:True
DoWeHaveCardsThatGenerateMoney? --> if_state3
if_state3 --> IsItEnoughMoney?:True
IsItEnoughMoney? --> if_state4
if_state4 --> PrioritizeGettingCardsInHand:True 
if_state4 --> SetGainMoneyAsPriority:False
if_state3 --> SetGainMoneyAsPriority:False
if_state2 --> SetRemovalAsPriority: False
```

Con esto elegiremos nuestra prioridad principal. El resto de decisiones en el juego la tendrán en cuenta a la hora de escoger el mejor resultado posible.

## Plan de pruebas

1. Lanzar el ejecutable o ejecutar en el editor de unity el proyecto
2. Pulsar el botón comenzar partida

La partida comenzará a llevarse a cabo por su cuenta a partir de este momento.

### A

#### Prueba 1: Sobre las manos de cada jugador

1. Pulsar el botón de siguiente jugador. Esto debería cambiar las cartas en mano que vemos y poner el icono del ojo sobre el jugador correspondiente. Repetir este proceso varias veces.
2. Pulsar los números de jugador. El ojo debería colocarse sobre ese número y mostrarnos sus cartas en mano.

#### Prueba 2: Sobre las representaciones de cartas

1. Situar el ratón sobre una de las cartas en el tablero o en el mercado. Debería aparecer una representación de mayor tamaño.
2. Comprobar que se puede leer el texto y apreciar los simbolos de la carta.
3. Comprobar que al mover el ratón fuera de la carta desaparece esta versión ampliada.

#### Prueba 3: Sobre la divisa

1. En la zona de cada jugador aparece varias imagenes de piedras con los valores 1, 3 y 6.
2. Asegurarse de que el número máximo de piedras en esta zona es 4.
  En caso de encontrar más piedras asegurarse de que poseen la carta Hestia. Esta carta aumenta el límite a 6 piedras, asegurarse de que este límite se sigue cumpliendo.

#### Prueba 4: Sobre los puntos de victoria

1. En la zona de cada jugador aparece un número que cambia a lo largo de la partida, con el título "puntos" adyacente.
2. Asegurarse de que este número está siempre entre los valores comprendidos (0-60).
3. Si algún número sobrepasa 60 las condiciones de final de partida se cumplen. Por lo tanto el juego debería detenerse al final de la ronda.

### B

#### Prueba 1: Sobre el correcto orden de escoger en el mercado

1. En la zona de mercado se colocan 2 marcadores por jugador
2. En la zona de mercado se colocan los marcadores en el orden correcto. Esto debería ser: 1 >> 2 >> 3 >> 4 >> 4 >> 3 >> 2 >> 1. Siendo el primero el jugador con el marcador de primer jugador.

#### Prueba 2: Sobre las cartas escogidas

1. Asegurarse de que las cartas escogidas cumplían la prioridad de ese jugador en el momento de escogerlas. Esta prioridad se puede ver bajo el número del jugador.
  Como reglas generales:

- prioridad "ganar divisa" eligirá con esta prioridad:
    dragones >> rosa >> verde >> azul >> rojo. Pues así se ordenan de mayor a menor valor.
    Este orden fluctuará si tienen sinergias con piedras de algún color.
- prioridad "ganar puntos" eligirá la carta con mayor sinergia con las suyas. Para esto habrá que ver el tablero y la mano de ese jugador y asegurarnos de que tiene sentido.

### C

#### Prueba 1: Cartas jugables

1. Asegurarse de que el jugador solo juega cartas que puede pagar. En caso contrario el juego se detendrá y se lanzará una excepción. Cosa que no debería ocurrir en la versión final.
2. Asegurarse de que al jugarlas desaparecen de su mano

#### Prueba 2: Cartas jugadas

1. Asegurarse de que las cartas jugadas, en la mesa de ese jugador, tienen cierta sinergia entre sí. esto debería ser claro leyendo los efectos de las cartas. En una mala partida esto se podría incumplir. Pero al menos dos de los 4 jugadores deberían tener cartas en mesa con cierto grado de sinergia entre sí en todo momento (pasadas las 3 primeras rondas).

### D

#### Prueba 1: Sobre el orden de los relojes

1. Asegurarnos de que todas las cartas con relojes se activan en la ventana de mensajes para cada jugador.

#### Prueba 2: Asegurarnos de que el orden de los relojes tiene sentido

1. En más de un caso el orden en el que se activen los relojes no afecta al resultado. Pero en la mayoría podremos maximizar el número de monedas/piedras obtenidas, el número de puntos, el número de cartas en mano o una combinación de todos estos.
2. El orden en el que se activan debería ser sencillo de seguir con las pausas entre girar las distintas cartas.

### E

#### Prueba 1: Prioridad visible

1. La prioridad actual de cada jugador será escrita en el panel de texto.
2. La prioridad tiene sentido con sus recursos. No será obtener dinero si las reservas de monedas están llenas. No será jugar cartas si no tenemos dinero. No será usar removal si vamos en cabeza.


### Resultados

Para cada prueba obtendremos los resultados de las 10 primeras partidas. Después cambiaremos la parametrización de las decisiones para intentar obtener un mejor resultado.

#### Primera parametrización

Esta será la función de evaluación que usaremos para esta primera vez.
```cpp
if (payoffs_value == 0 || enabler_value == 0) return 0;
return Mathf.CeilToInt(Mathf.Pow(payoffs_value, enabler_value));
```

|Número de Partida|---|P1|P2|P3|P4|Media|
|---|---|---|---|---|---|---|
|1|--|22|22|4|6|13.5|
|2|--|33|25|13|9|20|
|3|--|2|24|13|24|15.75|
|4|--|24|0|17|25|16.5|
|5|--|12|30|31|48|30.25|
|6|--|19|24|6|36|21.25|
|7|--|36|7|14|20|19.25|
|8|--|24|37|31|26|29.5|
|9|--|21|32|40|19|28|
|10|--|5|8|9|13|8.75|

__Media Final: 20.275__

#### Segunda parametrización

Aqui hemos reevaluado las flags de sinergias de la mayoría de cartas y cambiado la fórmula de puntuación de cartas a una que beneficie obtener cartas que recompensan sinergias después de tener varias cartas que facilitan dicha sinergia.

Esta es la función de evaluación: 

```cpp
return enabler_value + payoffs_value * (enabler_value);
```

|Número de Partida|---|P1|P2|P3|P4|Media|
|---|---|---|---|---|---|---|
|1|--|38|16|36|18|27|
|2|--|10|30|7|2|12.25|
|3|--|1|9|25|15|12.5|
|4|--|0|28|28|0|14|
|5|--|24|10|36|7|19.25|
|6|--|10|24|24|35|23.25|
|7|--|36|17|8|3|16|
|8|--|10|9|11|16|11.5|
|9|--|9|12|15|13|12.25|
|10|--|22|21|0|5|12|

__Media Final: 16__

#### Tercera parametrización

Mezclaremos la función de evaluación de la segunda parametrización con las flags de la primera.

|Número de Partida|---|P1|P2|P3|P4|Media|
|---|---|---|---|---|---|---|
|1|--|22|24|4|7|25|
|2|--|20|21|1|3|11.25|
|3|--|36|29|24|9|24.5|
|4|--|23|21|22|34|25|
|5|--|32|1|19|32|21|
|6|--|24|49|1|1|18.75|
|7|--|10|4|11|16|10.25|
|8|--|17|14|16|13|15|
|9|--|20|10|1|45|19|
|10|--|17|30|4|23|18.5|

__Media Final: 16.746__

#### Cuarta parametrización

Ahora lo haremos al contrario, conservaremos la función de evaluación de la primera con las flags de la segunda.

|Número de Partida|---|P1|P2|P3|P4|Media|
|---|---|---|---|---|---|---|
|1|--|41|39|15|27|30.5|
|2|--|24|15|3|22|16|
|3|--|23|11|0|16|12.5|
|4|--|32|6|23|4|16.25|
|5|--|19|16|35|26|24|
|6|--|11|39|3|31|21|
|7|--|21|30|26|30|26.75|
|8|--|22|0|14|12|12|
|9|--|0|6|30|26|15.5|
|10|--|22|12|0|20|13.5|

__Media Final: 18.8__

#### Quinta parametrización

Se observo que a veces los jugadores se quedaban bloqueados cuando tenían piedras de bajo coste. Así que se aumentó la probabilidad de escoger cartas que se pudiesen jugar como cartas favoritas.

|Número de Partida|---|P1|P2|P3|P4|Media|
|---|---|---|---|---|---|---|
|1|--|28|50|0|13|22.75|
|2|--|20|48|40|35|35.75|
|3|--|55|13|32|30|32.5|
|4|--|19|35|9|16|19.75|
|5|--|10|35|9|6|15|
|6|--|23|34|25|6|22|
|7|--|57|20|26|0|25.75|
|8|--|23|24|17|15|19.75|
|9|--|19|56|0|11|21.5|
|10|--|20|22|45|30|29.25|

__Media Final: 24.3__

Esta parametrización tiene las puntaciones más altas con diferencia y una mejor media. Aunque se observo que algunos players se seguían quedando bloqueados. Posiblemente por vender las cartas más baratas que podrían jugar en vez de ponerlas en su mano.

#### Sexta parametrización

Para evitar el problema observado en la segunda, se decidió aplicar la bonificación por ser jugable, solo si la carta no estaba en el mercado.

|Número de Partida|---|P1|P2|P3|P4|Media|
|---|---|---|---|---|---|---|
|1|--|22|24|28|44|29.5|
|2|--|27|20|18|1|16.5|
|3|--|32|24|12|32|25|
|4|--|21|26|21|25|23.25|
|5|--|36|2|20|15|18.25|
|6|--|37|23|21|35|29|
|7|--|13|3|26|15|14.25|
|8|--|1|10|25|30|16.5|
|9|--|13|17|56|4|22.5|
|10|--|29|12|8|30|19.75|

__Media Final: 21.45__

#### Conclusión 

La quinta fue la que mejor resultados obtuvo tanto en absoluto como en promedio, así que será la que permanecerá.

### Final de las pruebas

1. Salir de la aplicación o parar la ejecución en el entorno de unity.

### Promedio de resultados

## Recuento de Tareas

Se pueden encontrar en el [proyects](https://github.com/users/IzanDeVegaLopez/projects/7/views/1). Todas han sido realizadas por el único alumno responsable del proyecto.

## Conclusiones

La IA que se ha usado en este proyecto está lejos de obtener resultados óptimos. Pero con un sistema simple obtiene resultados aceptables, muy posiblemente mejorables con una mejor parametrización del proyecto.

Durante la última fase de prueba y parametrización obtuve varios comportamientos distintos de esta IA.
En el primer caso la mayoría de jugadores obtenían puntos bajos (5-10), pero en algunas ocasiones alguno conseguía obtener más de 40 puntos.
En el segundo la puntuación media subío hasta más de veinte puntos, pero la más alta también disminuyo a poco menos de cuarenta.

Un jugador humano experimentado es capaz de obtener 60 puntos en la mayoría de partidas de dos jugadores al final de la ronda 10. En las partidas de 4 jugadores es normal que alguno de ellos siga llegando a 60 puntos, pero también de que la media de puntuaciones disminuya hasta encontrarse respecto a los 50. Los datos de partidas de 4 personas se han obtenido de varias partidas jugadas en persona durante la elaboración de este proyecto. Mientras que los de partidas de dos jugadores estan respaldados por repeticiones de partidas en board game arena y los datos de la wiki.

## Agradecimientos Especiales

Quiero agradecer a mi padre por su aportación en el apartado visual al proyecto. Que convirtió mi boceto chuchurrió del fondo del menú no solo en algo que no te diese un sarpullido en los ojos si no en algo verdaderamente genial a partir del arte original del juego de mesa.

## Referencias

- [https://www.geeksforgeeks.org/c-sharp/text-textmeshpro-in-unity/](https://www.geeksforgeeks.org/c-sharp/text-textmeshpro-in-unity/)
- [https://stackoverflow.com/questions/44390454/what-is-the-use-of-region-and-endregion-in-c](https://stackoverflow.com/questions/44390454/what-is-the-use-of-region-and-endregion-in-c)
- [https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Image.html](https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Image.html)
- [https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject.AddComponent.html](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject.AddComponent.html)
- [https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject-ctor.html](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/GameObject-ctor.html)
- [https://discussions.unity.com/t/destroy-all-children-of-object/92016](https://discussions.unity.com/t/destroy-all-children-of-object/92016)
- [https://discussions.unity.com/t/how-to-get-list-of-child-game-objects/34696](https://discussions.unity.com/t/how-to-get-list-of-child-game-objects/34696)
- [https://www.geeksforgeeks.org/c-sharp/lambda-expressions-in-c-sharp/](https://www.geeksforgeeks.org/c-sharp/lambda-expressions-in-c-sharp/)
- [https://valeofeternity.wiki.gg](https://valeofeternity.wiki.gg)
- [https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Events.UnityEvent.html](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Events.UnityEvent.html)
- [https://docs.unity3d.com/6000.0/Documentation/ScriptReference/30_search.html?q=unityAction](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/30_search.html?q=unityAction)
- [https://boardgamegeek.com/filepage/262447/english-rules](https://boardgamegeek.com/filepage/262447/english-rules)
- [Vale of Eternity Rulebook](VOE_RULEBOOK_EN.pdf)
- [https://www.geeksforgeeks.org/artificial-intelligence/mini-max-algorithm-in-artificial-intelligence/](https://www.geeksforgeeks.org/artificial-intelligence/mini-max-algorithm-in-artificial-intelligence/)
- [https://www.whisthub.com/blog/how-to-write-an-ai-for-a-card-game](https://www.whisthub.com/blog/how-to-write-an-ai-for-a-card-game)
- Monte Carlo Rollout como estrategia útil para resolver problemas en juegps de cartas [https://www.youtube.com/watch?v=lmSRnG4eaKs&t=635s](https://www.youtube.com/watch?v=lmSRnG4eaKs&t=635s)
- Investigación Monte Carlo Rollout [https://en.wikipedia.org/wiki/Monte_Carlo_tree_search](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search)
- [https://mermaid.ai/open-source/syntax/stateDiagram.html](https://mermaid.ai/open-source/syntax/stateDiagram.html)
- [https://narratech.com/es/inteligencia-artificial-para-videojuegos/decision/probabilidad-y-utilidad/](https://narratech.com/es/inteligencia-artificial-para-videojuegos/decision/probabilidad-y-utilidad/)
- [https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Debug.LogWarning.html](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Debug.LogWarning.html)
- [https://stackoverflow.com/questions/289/how-do-you-sort-a-dictionary-by-value](https://stackoverflow.com/questions/289/how-do-you-sort-a-dictionary-by-value)
- [https://stackoverflow.com/questions/141088/how-to-iterate-over-a-dictionary](https://stackoverflow.com/questions/141088/how-to-iterate-over-a-dictionary)
- [https://discussions.unity.com/t/when-does-yield-startcoroutine-actually-execute/869077/3](https://discussions.unity.com/t/when-does-yield-startcoroutine-actually-execute/869077/3)
- [https://docs.unity3d.com/6000.3/Documentation/Manual/Coroutines.html](https://docs.unity3d.com/6000.3/Documentation/Manual/Coroutines.html)
- [https://www.emergentmind.com/topics/monte-carlo-rollouts](https://www.emergentmind.com/topics/monte-carlo-rollouts)
- [https://www.emergentmind.com/topics/monte-carlo-tree-search](https://www.emergentmind.com/topics/monte-carlo-tree-search)
- [https://www.emergentmind.com/topics/real-time-audio-variational-autoencoder-rave](https://www.emergentmind.com/topics/real-time-audio-variational-autoencoder-rave)
- [https://www.youtube.com/watch?v=TR_XbJBwjvw](https://www.youtube.com/watch?v=TR_XbJBwjvw)
- [https://www.youtube.com/watch?v=gayEHaol4Lc](https://www.youtube.com/watch?v=gayEHaol4Lc)
- [https://stackoverflow.com/questions/3143657/truncate-two-decimal-places-without-rounding](https://stackoverflow.com/questions/3143657/truncate-two-decimal-places-without-rounding)
- [https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics2D.OverlapPointAll.html](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics2D.OverlapPointAll.html)