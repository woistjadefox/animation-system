# AnimationSystem
This little framework is visual animation system that can manipulate Transforms and other components. Essentially, level designers can take a specific animation class and define states and customize settings. These MonoBehaviour scripts, also provide public methods to start, to stop or to transit into a specific defined state. 
 
### Example AnimationFixedPosition
![alt text](http://www.unity-glue.com/r/animationfixedposition.png "AnimationFixedPosition.cs")

The figure above shows the AnimationFixedPosition component of the animation system. The first property gives the possibility to address a target Transform which should become animated. Then the initial state of the animation can be set between “Idle” and “Running”. We can also define here if we want to loop through all defined states. The next field is an array of possible states for the animation.

Each state of the component can be defined by the following settings:
* “Time”: The time it takes to perform the animation.
* “Delay”: If the animation should be delayed with a certain time.
* "Loop": If the animation state should loop until we stop the animation or transit into another state. 
* “Trigger Once”: An option if the state should only be triggerable once.
* “Curve”: An AnimationCurve which forms the interpolation of the lerp. 
* “Enter Event”: UnityEvent for actions that will be invoked when the animation starts.
* “Exit Event”: UnityEvent for actions that will be invoked when the animation ends. 
 
The section after the states form the settings of the animation in general:
* “Local Position”: An option to define if the target should be manipulated in local or world space. 
* “Reset Start Pos On Play”: An option to define if the start position of the animation should be reset every time the animation gets played. 
* “Target Pos”: The desired target position. 
 
It’s possible to test the animation within the scene view by choosing first the state array number in the “Player” section and then hitting the “Play” button. 
 
Besides this simple animation which moves a Transform to a fixed position, there are other helpful components such as animating a Transform’s rotation, or fading the colors of a material, etc. The animation system offers an “AnimationBase” class with all of the important main functions. New components with additional functionality can easily be implemented by deriving from that base class.
