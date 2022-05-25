using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour
{
    public int seed = 1337;


    private List<GameObject> stars = new();
    private List<Planet> planets = new();
    private List<Planet> moons = new();


    public List<GameObject> StarPrefabs;
    public List<ColorSettings> planetColorOptions;
    public List<ShapeSettings> planetShapeOptions;

    private System.Random random;

    private void CleanUp()
    {
        if(stars != null)
        {
            foreach (GameObject star in stars)
            {
                Destroy(star);
            }
            stars.Clear();
        }
        if(planets != null)
        {
            foreach (Planet planet in planets)
            {
                Destroy(planet.gameObject);
            }
            planets.Clear();
        }
        if(moons != null)
        {
            foreach (Planet moon in moons)
            {
                Destroy(moon.gameObject);
            }
            moons.Clear();
        }
        
    }
    
    public void Generate()
    {
        //CleanUp();

        if(random == null)
        {
            random = new(seed);
        }
        // always do 1 star for now
        var numberOfStars = 1;

        for(int i = 0; i < numberOfStars; i++)
        {
            // get a random star prefab from the list of prefabs
            GameObject newStar = Instantiate(StarPrefabs[random.Next(StarPrefabs.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            stars.Add(newStar);
        }

        var numberOfPlanets = random.Next(2, 6);

        var lastOrbit = 0f;
        for (int planetIndex = 0; planetIndex < numberOfPlanets; planetIndex++)
        {
            GameObject newPlanet = new GameObject();
            newPlanet.name = "Planet " +(planetIndex + 1);
            var planet = newPlanet.AddComponent<Planet>();
            // random colour settings
            planet.colourSettings = planetColorOptions[random.Next(planetColorOptions.Count)];
            // random shape settings
            planet.shapeSettings = planetShapeOptions[random.Next(planetShapeOptions.Count)];
            planet.shapeSettings.planetRadius = random.Next(10, 30);

            var orbit = newPlanet.AddComponent<CircularOrbit>();
            orbit.radius = lastOrbit + random.Next(300, 1000);
            orbit.center = stars[0].transform;
            orbit.speed = .01f;

            // save orbit each time so the planets are further and further apart
            lastOrbit = lastOrbit + orbit.radius;

            // between 0 and 2 moons per planet
            var numberOfMoons = random.Next(0, 3);

            var lastLunarOrbit = planet.shapeSettings.planetRadius + 10;
            for (int moonIndex = 0; moonIndex < numberOfMoons; moonIndex++)
            {
                GameObject newMoon = new GameObject();
                newMoon.name = newPlanet.name + " Moon " + (moonIndex + 1);

                var moon = newMoon.AddComponent<Planet>();
                // random colour settings
                moon.colourSettings = planetColorOptions[random.Next(planetColorOptions.Count)];
                // random shape settings
                moon.shapeSettings = planetShapeOptions[random.Next(planetShapeOptions.Count)];
                moon.shapeSettings.planetRadius = random.Next(10, 30);

                var moonOrbit = newMoon.AddComponent<CircularOrbit>();
                moonOrbit.radius = lastLunarOrbit + random.Next(100, 1000);
                moonOrbit.center = newPlanet.transform;
                moonOrbit.speed = .01f;

                // save orbit each time so the moons are further and further apart
                lastLunarOrbit = lastLunarOrbit + moonOrbit.radius;

                // add moon to planet
                moon.transform.parent = newPlanet.transform;

                moons.Add(moon);
            }

            planets.Add(planet);
        }



    }
}
