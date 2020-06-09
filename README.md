# USRP-2020
OSCAR USRP Summer 2020 - "Integrating Procedurally Generated Planets and Ecosystems"

You can find the entirety of the research proposal in the GitHub folder. It outlines my inspiration, process, timeline, and expected outcomes of this project.

### Week 1 - Research and Planning: Procedurally Generated Planets
During this week, I researched the possible resources that would aid in helping me create procedurally generated planets.
I'm currently looking at two game engines: Unity and Unreal Engine 4.
The engine which has more resources for this research project will likely be the engine I will use for the remainder of the project.

#### Resources for Unity
* [Here](https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) is a youtube playlist by Sebastian Lague. It outlines some of the core elements I would like in a procedurally generated planet such as biomes and oceans.
  * Creation of the planet: 3 methods: a sphere with triangles around the axis, a sphere with triangles the same shape, and an inflated cube.
  * Use a script to generate the mesh of a side of the cube or the "terrain face," then use another script to create six of these to create a cube. Then make sure each triangle is equadistant from the center of the sphere.
* [Here](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) is another youtube playlist by Sebastian Lague that walks viewers through procedural land-mass generation.
It could be integrated in this project to have more depth to the landscape.
* [Here](https://medium.com/@peter_winslow/creating-procedural-planets-in-unity-part-1-df83ecb12e91) is an article by Peter Winslow which walks an individual through the creation of a low-poly procedural planet.
The article only has a total of three parts; meaning as whole it will be unfinished.

#### Resources for Unreal Engine 4
* [Here](https://www.reddit.com/r/unrealengine/comments/b2n3c8/procedural_planet/) is a reddit post which shows images of a low-poly planet and it cites the resources that were used (many of them were actually unity resources mentioned above).
* [Here](https://www.reddit.com/r/unrealengine/comments/b2n3c8/procedural_planet/) is the first part of a two part series on creating the basic shape of a planet with a mesh.
* [Here](https://www.youtube.com/playlist?list=PLgwhqR8QlpRVKQ5JEWcdjV77lex0q-Oth) is a youtube playlist by Tefel who goes into depth with Unreal to recreate a Minecraft like procedural generation.

#### Conclusion
Because the present research that has been conducted on procedurally generated planets, especially on the low-poly style that I desire, is mostly found utilizing the Unity engine, I plan to use Unity in the remainder of the project.
By making this decision, I am trying to streamline the process. I am trying to make the deadline of the project along with ease the learning curve that comes with game development.
I created the Unity project (on version 2019.3.15f) and I chose to the Universal Render Pipeline because I wanted to the project to be viewable by a large audience.
I plan on deploying to both web and desktop so by keeping that in mind, I knew the extra features of the High Definition Render Pipeline were unnecessary.
I have also watched and outlined the keypoints and concepts from the videos. I will review these during Week 2 when implementing the procedurally generated planets.
