using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Block
{
    Air,
    Dirt,
    Grass,
    Stone,
    Log,
    Leaves
}

public enum FaceOrientation
{
    Up,
    Down,
    Left,
    Right,
    Front,
    Back
}

public class BlockData
{
    private const int N_SPRITES = 4;
    private const float EPSILON = 0.0001f;
    private const float STEP = 1 / (float)N_SPRITES;

    public static Dictionary<Block, Dictionary<FaceOrientation, Vector2>> spritePositions = new Dictionary<Block, Dictionary<FaceOrientation, Vector2>>()
    {
        {
            Block.Dirt,
            new Dictionary<FaceOrientation, Vector2>()
            {
                { FaceOrientation.Up, new Vector2(0, 0) },
                { FaceOrientation.Down, new Vector2(0, 0) },
                { FaceOrientation.Left, new Vector2(0, 0) },
                { FaceOrientation.Right, new Vector2(0, 0) },
                { FaceOrientation.Front, new Vector2(0, 0) },
                { FaceOrientation.Back, new Vector2(0, 0) },
            }
        },
        {
            Block.Grass,
            new Dictionary<FaceOrientation, Vector2>()
            {
                { FaceOrientation.Up, new Vector2(1, 0) },
                { FaceOrientation.Down, new Vector2(0, 0) },
                { FaceOrientation.Left, new Vector2(2, 0) },
                { FaceOrientation.Right, new Vector2(2, 0) },
                { FaceOrientation.Front, new Vector2(2, 0) },
                { FaceOrientation.Back, new Vector2(2, 0) },
            }
        },
        {
            Block.Stone,
            new Dictionary<FaceOrientation, Vector2>()
            {
                { FaceOrientation.Up, new Vector2(3, 0) },
                { FaceOrientation.Down, new Vector2(3, 0) },
                { FaceOrientation.Left, new Vector2(3, 0) },
                { FaceOrientation.Right, new Vector2(3, 0) },
                { FaceOrientation.Front, new Vector2(3, 0) },
                { FaceOrientation.Back, new Vector2(3, 0) },
            }
        },
        {
            Block.Log,
            new Dictionary<FaceOrientation, Vector2>()
            {
                { FaceOrientation.Up, new Vector2(1, 1) },
                { FaceOrientation.Down, new Vector2(1, 1) },
                { FaceOrientation.Left, new Vector2(0, 1) },
                { FaceOrientation.Right, new Vector2(0, 1) },
                { FaceOrientation.Front, new Vector2(0, 1) },
                { FaceOrientation.Back, new Vector2(0, 1) },
            }
        },
        {
            Block.Leaves,
            new Dictionary<FaceOrientation, Vector2>()
            {
                { FaceOrientation.Up, new Vector2(2, 1) },
                { FaceOrientation.Down, new Vector2(2, 1) },
                { FaceOrientation.Left, new Vector2(2, 1) },
                { FaceOrientation.Right, new Vector2(2, 1) },
                { FaceOrientation.Front, new Vector2(2, 1) },
                { FaceOrientation.Back, new Vector2(2, 1) },
            }
        },
    };

    public static Vector2[] GetUVCoordinates(Vector2 SpritePosition)
    {
        SpritePosition /= (float)N_SPRITES;
        Vector2[] uvs = new Vector2[] 
        {
            SpritePosition + new Vector2(EPSILON, EPSILON),
            SpritePosition + new Vector2(EPSILON, STEP - EPSILON),
            SpritePosition + new Vector2(STEP - EPSILON, STEP - EPSILON),
            SpritePosition + new Vector2(STEP - EPSILON, EPSILON),
        };

        return uvs;
    }

}