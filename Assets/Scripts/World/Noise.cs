using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
    static float maxHeight = 32;
    static float minHeight = 32;
    static float scale = 0.01f;
    static float seed = 2;

    public static int Perlin(float x, float z) {
        float y = (Mathf.PerlinNoise((x * scale) + seed, (z * scale) + seed) * maxHeight) + minHeight;

        return (int)y;
    }

    public static int PerlinStone(float x, float z) {
        float y = (Mathf.PerlinNoise((x * scale * 2) + seed, (z * scale * 2) + seed) * maxHeight - 4) + minHeight;

        return (int)y;
    }

    public static int Perlin3D(float x, float y, float z) {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);

        float yx = Mathf.PerlinNoise(y, x);
        float yz = Mathf.PerlinNoise(y, z);

        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        float xyz = xy + xz + yx + yz + zx + zy;
        
        return (int)(xyz / 6.0f);
    }
}
