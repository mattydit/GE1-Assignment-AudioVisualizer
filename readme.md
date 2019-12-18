# Games Engines 1 Assignment
## Mateusz Harcej, DT211C

### **Description:**
The assignment will be an audio responsive particle system in the Unity engine. The audio will impact particles contained inside an area by using the information extracted from the audio track.

The resources I have researched are:

- https://forum.unity.com/threads/audio-visualization-tutorial-unity-c-q-a.432461/ 
- http://www.41post.com/4776/programming/unity-making-a-simple-audio-visualization
- https://www.youtube.com/watch?v=GjHeMZp2Yas - particle spawning.


[![YouTube](http://img.youtube.com/vi/w59gI8NH0lc/0.jpg)](https://www.youtube.com/watch?v=w59gI8NH0lc)

Tutorial for creating a simplex noise flowfield has been used. Which can be used to create a grid of noise based on the given dimensions. Particles are instantiated based on a random position in the noise and direction is applied for the movement. The list of particles has a boundry of X, Y, Z edges to reposition the particle on the other side of the field when it has reached the edge. Particles are assigned an audio band from 8 possible bands by iterating over the list of particles.

The audio data is extracted using the unity GetSpectrumData function, then the frequencies are divided into 8 bands by grouping together low frequencies then high frequencies. These grouped ranges allow for a more engaging visual as the highest frequencies from a sample would not be visualised. Rather portraying the usually active bands in the visual. A buffer variable is used on the bands to allow for a smooth transition between the visualised data so a jerky animation is avoided.

Applying the colour gradients and audio responsiveness to the colours gave the visual a nice finish along with generating a transluscent cube around the grid axis.