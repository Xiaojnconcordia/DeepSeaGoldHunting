Introduction:

The game level is a cube full of water with sharks and octupus spawning repeatedly. 
The player’s goal is to dive to the very bottem and carry 3 kinds of gold and take it back to the ship at the surface to score. 
Sharks and octupuses will attack the player in the water. 
The game is over when player loses all the lives and health points. 
The level is complete when player scores 100 gold points.

Control:
1. W: move forward.
A: move leftward.
S: move backward.
D: move rightward.
2. Q: rise
E: dive
3. Mouse left button: shoot figuring.
Move Mouse: rotate the camera and the player.
Left Shift + Move Mouse: rotate the camera only
4. ESC: Pause the game and open the pause menu, press again to resume, or player can click resume in the menu. 

Mechanism
Player:
1. Player can move to any direction horizontally by pressing W, A, S, D or up, left, down, right arrow key and can rotate by moving the mouse.
2. Player can rotate the camera by moving the mouse. The camera follows the character with a fixed distance, and will get nearer of there’s obstacle (e.g. wall) between the camera and the character. 
3. If Player goes beyond a border, the character will be at the oppisite side of the border. For example, if the character goes accoss the left-front corner, the character will be at the right-back corner. Thus, player has more way to escape from enemy’s attack. 
4. Player can move vertically, rising with Q key and diving with E key. In order to make the control more challenging, there is a small delay between two moves so that the player can only move up or down once in a little every small amount of time. 
5. Player can carry gold by toughing it. When carrying some gold before, the character the character swim and rises up in water more slowly. If the player successfully takes the gold back to the boat at the surface, some scores are earned. When the score reaches 100, the level is complete. 
6. The player’s oxygen is consumed overtime in the water and can be restored above the surface. If the oxygen runs out, the character dies. 
8. Player can be damaged if toughed by sharks, octopuses and the poison shot by octupuses. After taking damage, player is invincible for a fixed amount of time. If the player loses all the health points, the character dies and will respawn at the surface in a fixed amout of time. If the player loses all the lives and health, game is over.
7. The player can shoot figuring. The projectile can lure the shark, making it chase the projectile. If a shark is chasing the player, the shark will chase the figuring nearby as priority.
9. Player is animated when idle, swimming, invincible, and dead.

Enemies:
1. The shark wanders randomly horzontally and will chase the player when the character carries some gold for more than a fixed time.
2. The shark will chase the figurine shot out by the player prioring to chasing the player. 
3. The Octopus moves vertically and will shoot poison to the player.
4. The moving speed of both kinds of enemies will increase repeatedly after a fixed time. 
5. Enemies have a certain lifetime and will vanish. New enemies may spawn at a random position repeatedly. The vanishment rate is slower than the spwaning rate, so the more time passed, the more enemies there will be in the level. 
6. Sharks may have random sizes.

Level:
1. Three kinds of gold with which the player can earn 1 point, 2 points, 10 points respectively. The more points it has, the heavier it is (player swim more slowly when carrying it)
2. The boat follows Achimedes’ Law.
3. Water with fog effect. 
3. Caustic effect on the seafloor. 
4. Dirt effect on the seafloor.
5. Bubbles effect (Particle emitter). 
6. Sound effects: shooting, taking damage, catching gold, scoring, bubble

UI:
1. Opening Scene: Play, Quit
2. Gameover screen: Try Again, Main Menu. 
3. Level completion screen: Play Again, Main Menu.
4. Pause screen: Resume, Main Menu
5. Icons, bars and text indicating life, health, score, and oxygen. 
6. A minimap. 
