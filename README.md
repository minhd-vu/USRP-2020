# USRP-2020
OSCAR USRP Summer 2020 - "Integrating Procedurally Generated Planets and Ecosystems"

**Demo the project [here](https://minhd-vu.github.io/USRP-2020/)**.

You can find the entirety of the [research proposal](https://github.com/minhd-vu/USRP-2020/blob/master/Research%20Proposal.pdf) in the GitHub folder. It outlines my inspiration, process, timeline, and expected outcomes of this project.

### Week 1 - Research and Planning: Procedurally Generated Planets
During this week, I researched the possible resources that would aid in helping me create procedurally generated planets.
I'm currently looking at two game engines: Unity and Unreal Engine 4.
The engine which has more resources for this research project will likely be the engine I will use for the remainder of the project.

#### Resources for Unity
* [Here](https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) is a youtube playlist by Sebastian Lague. It outlines some of the core elements I would like in a procedurally generated planet such as biomes and oceans.
  * Creation of the planet: 3 methods: a sphere with triangles around the axis, a sphere with triangles the same shape, and an inflated cube.
  * Use a script to generate the mesh of a side of the cube or the "terrain face," then use another script to create six of these to create a cube. Then make sure each triangle is equadistant from the center of the sphere.
  * Create a scriptable objects to easily modify the size and color of the planets.
  * Use open source Simplex Noise to have smooth noise to deform the sphere to look more like a planet.
  * Combine multiple noise layers to create a sphere that resembles more of a planet.
  * Use a mutitude of noise filters to create varying terrain (i.e. sharp vs round mountains).
  * Use a shader graph to give the planet different colors depending on elevation.
* [Here](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) is another youtube playlist by Sebastian Lague that walks viewers through procedural land-mass generation.
It could be integrated in this project to have more depth to the landscape.
* [Here](https://medium.com/@peter_winslow/creating-procedural-planets-in-unity-part-1-df83ecb12e91) is an article by Peter Winslow which walks an individual through the creation of a low-poly procedural planet.
The article only has a total of three parts; meaning as whole it will be unfinished.
  * Winslow discusses two different type of planets, similar to Lague: one with a cube-based sphere and one with a icosahedron-based sphere.
  He notes the differences are that the cube-based is easier to texture uniformly whereas the icosahedron-based sphere is much harder.
  * Extrude certain parts of the planets so that there is a terrain elevation difference.
  * Use ambient occlusion to make the ocean clear / ocean depth.

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
I hope to combine the work of Lague and Winslow to expand the low-poly planet to have differnt biomes and assign different environmental values to each region.

### Week 2 - Execution: Procedurally Generated Planets

During Week 2 I began implementing the systems for procedurally generated planets. I mainly followed Peter Winslow's tutorial to achieve the look. I fixed some bugs because the tutorial was a little outdated, also meaning that I could use the desired render pipeline.
* Added trees to the planet.
* Added stars/space themed skybox.
* Adjusted the camera controller to zoom in and out based on fov rather than distance; smooth-damp instead of lerp.
* Fixed a bug that didn't properly render the shaders.

Here is a picture of the progress with procedurally generated planets:

![Image of Procedural Planet Progress](https://raw.githubusercontent.com/minhd-vu/USRP-2020/master/images/Week%201%20Planet.png)

However, there is a limitation to Winslow's method. Further subdivisions of the planet consume large amounts of memory, to the point where 4 subdivisions would prevent the project from being run using WebGL.
That means that finer levels of detail would be unachievable because the scale would be too small. I am considering this limitation in the future.
At this point I felt like I had a good basis on having a procedurally generated planet, now I started my research early to find a way to incorporate an ecosystem.

* [Here](https://youtu.be/r_It_X7v-1E) is another video by Sebastian Lague that details a environment with three species: foxes, rabbits, and plants.

#### Conclusion
Lague linked his codebase for the project, but it remains largely unfinished; even less finished than what the video shows. I downloaded it and I have been experimenting with the ecosystem.
It uses a procedurally generated tiled map and a coordinate system in order to map the entities. This is slightly off track with the planets; but I might restructure the project so that the focus of it is on the ecosystem; because that is the bulk of it.
Were I to try to combine the two; I would have to find a way to map a coordinate system to a sphere and that might prove challenging. I believe that making the ecosystem very depthful and extending the terrain procedural generation may be the best way to take this research project.

Here are the most significant changes I made:
* Made behaviour more "animal-like," animals no longer only ate and drank and did nothing else.
* Added two different cameras to view the simulation.
* Fixed perspective rendering bug with the terrain material.
* Fixed fox eating bug not killing the rabbit.
* Added animal reproduction.
* Added death by old age and exhaustion.
* Varying animal movement speeds.
* Added plant reproduction.

Here is a snapshot of my progress so far:

![Image of Ecosystem Progress](https://raw.githubusercontent.com/minhd-vu/USRP-2020/master/images/Week%202.png)

You can also play the [release 0.6](https://github.com/minhd-vu/USRP-2020/releases/tag/0.6) to see it in action.

### Week 3 - 