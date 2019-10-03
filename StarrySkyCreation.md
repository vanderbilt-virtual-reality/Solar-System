# Starry Sky Creation: Working with Unity3D Particle Effect System

## Method of Simulation

### Star Particle

<b>2 Oct 2019:</b> Setting up a Particle Effect that has a very large number for duration, speed of 0, rate of emission is as fast as possible, and we can create stationary white particles that, from the player's POV, look like stars. This is however, still random procedurally created. Need to amend to take in real star data.

### Second camera addition
Since we don't want the background to have to move with the player, and since our "background" is based on real particles in 3D space, we are faking the view by adding a second camera from now on referred to as StarBackgroundCamera.

First is to create a new layer, the furthest one (currently at 31), and name it like StarBackground. In this layer will be our StarBackgroundCamera and our StarParticleSystem. By setting up the "Mask culling" appropriately for the character camera and the StarBackgroundCamera, we can limit them to only the layers we want them to see. Also take note of the Depth number, the higher the number, the higher the "priority" and "closer" to draw. From my understanding, the lower one is drawn first and the higher one will be drawn on top of it. As such, StarBackgroundCamera should be the lowest.

## Benefits and Drawbacks
+ (+) Star background does not move with player
+ (+) Can render any number of stars
- (-) Make use of a second camera
- (-) Eat up memory for possible draw call?

## Scripts

1. <a href="Solar-System-Simulation/Assets/Scripts/MatchCameraRotation.cs">MatchCameraRotation</a> is a script to match the rotation of a camera to a target. It does not match any other degree of movement.

2. ~~<a href="Solar-System-Simulation/Assets/Scripts/StarParticleSystem.cs">StarParticleSystem</a> is a script to parse through modified HYG data and generate particles accordingly.~~

## Particle Effect

To initiate a Particle Effect object in Unity Editor:

> GameObject -> Effect -> Particle System


## Links and Tutorials

A simple Google search will probably lead you to a tutorial by Thomas Kole from 2017, but it is now out of date. However, it is still of good starting value. <a href="https://thomaskole.wordpress.com/portfolio/how-to-generate-a-physically-accurate-star-field-in-unity-using-real-world-data/">Link</a>.

A more in depth search with the word "skybox" will lead you to a YouTube video by Mike Dolan Games, this is clearly based on Thomas Kole, without the real star data. Early version of the Starry Sky is based on this video. <a href="https://www.youtube.com/watch?v=Kx-RAJ_7HTE">Link</a>.
