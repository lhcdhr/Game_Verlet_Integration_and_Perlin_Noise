Haochen Liu
260917834
COMP521 A2

Since I set the game space with size of index 40*20, the game scene should be set with 2:1 for better visual performance.
1.
Terrain is generated randomized for each playthrough, though the shape might be similar. 
The height of mound is randomized, and the texture of the plains and the mound are modelled with 1D perlin noise.
2.
The cannon's angle can be modified within the range of 0 to 90 degree, with up arrow increasing it and down arrow for decreasing.
The muzzle speed can be increased up tp 50 with left arrow, and can be reduced down to 5 with right arrow.
The cannon is fired when space bar is down. Note that the next shot can only be fired after 0.5s of the previous shot.
3.
The invisible wall on the right is set to coordinate x = 10.
The collision detections and resolutions between cannonball and terrain, cannonball and the left wall, cannonball and other cannonballs are implemented.
Cannonballs out of bound will be destoryed.
4.
Insects are implemented base on Verlet integration. For detailed explaination, please refer to Verlet.pdf and scrpits for details.
5.
The collision between insects and left wall, insects and terrain are implemented. I failed to implement the collision between insect and cannonballs.
There is a chunk of commented-out code of collision detection and resolution between insects and cannonballs.
The reason I leave that out is that when applying this collision, the collision between insect and left wall/terrain will be disfunctioned and I have no about idea how to solve this.
So there will be intepeneration of cannonballs and insects. I apologize for the inconvenience.
6.
There are 3 sources of rising air. No. 1 is between x-coord -16 and -15, No. 2 is between -13 and -12, and No.3 is between -9 and -8.
Air will be randomly generated every 2 seconds.
Each has 50% chance of not generating air, and 50% chance of generating air based on the random value it gets.

Thank you!



