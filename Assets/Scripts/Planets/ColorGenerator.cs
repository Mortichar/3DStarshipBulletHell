﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{

    ColorSettings settings;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColourSettings.biomes.Length;
        float blendRange = settings.biomeColourSettings.blendAmount / 2f + .001f;

        for (int i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public Texture2D UpdateColours()
    {
        var texture = new Texture2D(textureResolution * 2, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32, false);
        Color[] colours = new Color[texture.width * texture.height];
        int colourIndex = 0;
        foreach (var biome in settings.biomeColourSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                Color gradientCol;
                if (i < textureResolution) {
                    gradientCol = settings.oceanColour.Evaluate(i / (textureResolution - 1f));
                }
                else {
                    gradientCol = biome.gradient.Evaluate((i-textureResolution) / (textureResolution - 1f));
                }
                Color tintCol = biome.tint;
                // mix based on tint
                colours[colourIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                colourIndex++;
            }
        }
        texture.SetPixels(colours);
        texture.Apply();
        return texture;
    }
}
