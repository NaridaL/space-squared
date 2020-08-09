using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class generateTerrain : MonoBehaviour
{
    //Config
    public Tilemap lunar;
    public Tile ground;


    public const int cube_side_length = 500;
    public const int circle_radius = 200;


    // Start is called before the first frame update
    void Start()
    {

        //GenerateMap_Crater_Cube_Tiled(cube_side_length);
        GenerateMap_Crater_Circle_Tiled(circle_radius);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generates the planet as a cube with craters on the surface
    void GenerateMap_Crater_Circle_Tiled(int radius)
    {

        //Position and Tile Array for later TileMap Population
        Vector3Int[] positions = new Vector3Int[radius * 2 * radius * 2];
        TileBase[] tileArray = new TileBase[positions.Length];
        int tbi = 0;

        int i = 0;

        int bounding_box = circle_radius;
        for (int x = -bounding_box; x < bounding_box; x++)
        {
            for (int y = -bounding_box; y < bounding_box; y++)
            {
                float dist_to_core = Mathf.Pow(x,2) + Mathf.Pow(y,2);
                if (dist_to_core <= Mathf.Pow(circle_radius,2))
                {
                    positions[tbi] = new Vector3Int(x, y, 0);
                    tileArray[tbi] = ground;
                    tbi++;
                }
            }
            i++;
        }
        lunar.SetTiles(positions, tileArray);

    }


    //Generates the planet as a cube with craters on the surface
    void GenerateMap_Crater_Cube_Tiled(int side_length)
    {

        //Position and Tile Array for later TileMap Population
        Vector3Int[] positions = new Vector3Int[side_length * side_length];
        TileBase[] tileArray = new TileBase[positions.Length];
        int tbi = 0;

        //How deep is the crater on the current position
        int[] crater_depth_map = new int[side_length];

        int i = 0;
        for (int x = -side_length / 2; x < side_length / 2; x++)
        {
            
            int crater_rand = UnityEngine.Random.Range(0, 1000);

            // Crater Size 5 on 5% of surface
            if ((crater_rand > 950) && (crater_rand <= 1000))
            {
                int[] crater_gen = { 1, 2, 3, 3, 2, 1 };
                int j = 0;
                foreach (int cg in crater_gen)
                {
                    crater_depth_map[i + j] = crater_depth_map[i + j] + cg;
                    j++;
                }

            // Crater Size 5 on 1% of surface
            }
            else if ((crater_rand > 940) && (crater_rand <= 950))
            {
                int[] crater_gen = { 1, 2, 3, 5, 5, 3, 2, 1 };
                int j = 0;
                foreach (int cg in crater_gen)
                {
                    crater_depth_map[i + j] = crater_depth_map[i + j] + cg;
                    j++;
                }
            }
            // PPT Exampel Crater on 1% of surface
            else if ((crater_rand > 930) && (crater_rand <= 940))
            {
                int[] crater_gen = { 1, 3, 4, 5, 5, 6, 6, 6, 6, 5, 4, 4, 2 };
                int j = 0;
                foreach (int cg in crater_gen)
                {
                    crater_depth_map[i + j] = crater_depth_map[i + j] + cg;
                    j++;
                }
            }


            for (int y = -side_length / 2; y < side_length / 2 - crater_depth_map[i]; y++)
            {
                if (((-side_length / 2 + 100) >= x) || ((side_length / 2 - 100) <= x) &&
                    ((-side_length / 2 + 100) >= y) || ((side_length / 2 - 100) <= y))
                {
                    positions[tbi] = new Vector3Int(x, y, 0);
                    tileArray[tbi] = ground;
                    tbi++;
                }
            }
            i++;
        }
        lunar.SetTiles(positions, tileArray);



    }

}
