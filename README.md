# Platformer
by Shane O' Malley

This is a proof-of-concept basis for a platforming game. It uses a tile based engine which supports slopes, and animated tiles. It comes complete with a barebones, yet functional level editor.

The game's source can be found simply in the `Platformer` directory, and the Windows x86 (for 32-bit) binary can be found in the `Platformer\bin\Windows\x86\Debug` directory

It is written in C# and uses the [MonoGame](http://www.monogame.net/) framework. There are many tutorials out there for setting up MonoGame with Visual Studio if you want to modify the project and build it for yourself.

I have included two batch files to make it less awkward to run the game and editor. They are `Play Game.bat` and `Launch Editor.bat` respectively, and are in the root folder. All they do is launch the appropriate `.exe` file from within the folders.

## Playing The Game

When you open the game, you will need to move up and down with the arrow keys or d-pad, and select a level to play with `Enter` or the 'A' button on controller.

The controls as they are at the moment are pretty simple: `Left` and `Right` Keys to move left and right, and `Space` to jump. You can also use the d-pad and 'A' button on an XBox controller to move and jump also.

You can also shoot with `Right Bumper` or `Enter`, and aim with the right analog stick (Note that you must use an Xbox controller if you want to aim)

Bullets bounce off walls and sloped floors accurately, and will explode after a certain number of collisions.

I also left in some debugging stuff, If you hold `R` while playing, the game will 'pause', and If you press `E` while holding `R`, the game will logic will advance to the next frame. If you press `Q` the player will return to the spawn point. If you press `T` "debug drawing mode" will be toggle on and off. When debug drawing mode is on: 

* The collision box used for checking collisions with walls will be drawn in red (or blue if the player is colliding with something)

* A yellow box will be drawn to represent the collision box's projected position based on the player's current speed

* a purple square will be drawn at the player's spawn point 

* tiles which are used in collision detection for a bullet will be highlighted

* white dots will be drawn at the points where ceiling and ground collision is checked

## Making Levels
The Level Editor is pretty basic (and not pretty), but it does the job.

Use `WASD` to move the camera, `left mouse` to place an entity/tile, and `right mouse` to remove an entity/tile.

To select an entity/tile for placing, select it from one of the two ListView boxes on the left.

The only entity available for placing at the moment is the player's spawn point.

You can also start a new level resize the grid of the current level, or 'shift' the tiles and entities of the current level by a certain amount

Levels are stored in the `Platformer\bin\Windows\x86\Debug\Data\levels` and the Editor navigates into this folder automatically when loading or saving levels
