# IK Arm

## Authors

-   Madhava Raveendra

## Description

This project includes a robot arm generated in Untiy with Forward and Inverse Kinematics implemented. The use will be able to interact with the Arms to view the motion of the Arms based on their Inverse Kinematics Implementation.

## Features

-   Dual Robot Arms with Forward Kinematics - Default <br />
-   Inverse Kinematics using FABRIK <br />
-   User can move the camera freely around the scene <br />
-   Manual and automatic Mode <br />
-   User can allow the arm to interact with an Objec in scene <br />

## Code

I am using Unity3D Game Engine for the implementation of this project. Using spheres as Joints and Cubes as the arm segments, I am constructing a bi-joint robot arm by joining them using basic transformations and forward kinematics. Using Unity's Collider System, we are implementing collisions on the lifters of the arms towards the object in the scene. The user is allowed to interact with the motion of the arms using Targets represented by small spheres in Manual Mode of the scene. The arms have inverse kinematics implemented using the FABRIK algorithm. You can switch from manual to automatic mode using the button given in the game. At first switch to automatic, one of the arms navigates to the object and picks it up. The user can move the arms to see its interaction with the arms and click on the object to drop it in Manual Mode.

## Execution

Use the button below to navigate to the source code.
Find the 'Project Build' folder and run the executable inside.

Controls : <br />
ASWD/Arrow Keys : Move Around <br />
q/e : Camera Up/Down <br />
z/x : Camera Look-Left/Look-Right <br />
Right-Click : Select Targets and Obstacle to Move <br />
SPACE : Quit Application <br />

## Difficulties

One of the main difficulties of the project was the implementation of Inverse Kinematics using FABRIK algorithm which included manually adjusting the rotation and positions of the child components of each segment of the arm (Arm segment and Joint).

## Video

## Images

## References/Links

-   World Materials Asset: https://assetstore.unity.com/packages/2d/textures-materials/world-materials-free-150182
-   Skybox Series Asset: https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633
