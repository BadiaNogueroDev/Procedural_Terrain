using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _brownTilePrefab;
    [SerializeField] private GameObject _greenTilePrefab;
    private GameObject[,] ground;
    private GameObject[,] path;
    
    private int _chunkSizeX;
    private int _chunkSizeZ;
    private Vector2Int _instantiatePosition;
    private Vector2Int _startPathPosition;
    private Vector2Int _endPathPosition;
    
    public void Initialize(int chunkSizeX, int chunkSizeZ, Vector2Int instantiatePosition, Vector2Int startPathPosition, Vector2Int endPathPosition)
    {
        _chunkSizeX = chunkSizeX;
        _chunkSizeZ = chunkSizeZ;
        _instantiatePosition = instantiatePosition;
        _startPathPosition = startPathPosition;
        _endPathPosition = endPathPosition;
        
        GenerateChunk();
        GeneratePath();
    }
    
    public void GenerateChunk()
    {
        ground = new GameObject[_chunkSizeX, _chunkSizeZ];
        path = new GameObject[_chunkSizeX, _chunkSizeZ];

        for (int x = 0; x < _chunkSizeX; x++)
        {
            for (int z = 0; z < _chunkSizeZ; z++)
            {
                //Generate lower ground
                GameObject groundTile = Instantiate(_brownTilePrefab, new Vector3(_instantiatePosition.x + x, 0, _instantiatePosition.y + z), Quaternion.identity, transform);
                ground[x, z] = groundTile;
                
                //Generate upper path
                GameObject pathTile = Instantiate(_greenTilePrefab, new Vector3(_instantiatePosition.x + x, 1, _instantiatePosition.y + z), Quaternion.identity, transform);
                path[x, z] = pathTile;
            }
        }
    }

    public void GeneratePath()
    {
        List<Vector2Int> pathPositions = new List<Vector2Int>();
        Vector2Int currentPosition = _startPathPosition;
        bool generatingPath = true;
        
        pathPositions.Add(currentPosition);
        
        while (generatingPath)
        {
            path[currentPosition.x, currentPosition.y].SetActive(false);
            
            //If the path hasn't reached the end
            if (currentPosition != _endPathPosition)
            {
                //Get the possible directions from the current position
                List<Vector2Int> possibleMoves = new List<Vector2Int>
                {
                    new Vector2Int(currentPosition.x + 1, currentPosition.y),
                    new Vector2Int(currentPosition.x - 1, currentPosition.y),
                    new Vector2Int(currentPosition.x, currentPosition.y + 1),
                    new Vector2Int(currentPosition.x, currentPosition.y - 1)
                };

                //Remove non-valid positions: Edges and already visited positions
                possibleMoves.RemoveAll(pos => pos.x < 0 || pos.x >= _chunkSizeX || pos.y < 0 || pos.y >= _chunkSizeZ || pathPositions.Contains(pos));

                //If there are valid positions, keep creating path
                if (possibleMoves.Count != 0)
                {
                    currentPosition = NextStepTowardsTarget(possibleMoves, _endPathPosition);
                    pathPositions.Add(currentPosition);
                }
                else
                {
                    generatingPath = false;
                }
            }
            else
            {
                generatingPath = false;
            }
        }
    }
    
    Vector2Int NextStepTowardsTarget(List<Vector2Int> possibleMoves, Vector2Int targetPosition)
    {
        //Sort all possible moves by proximity to the target
        possibleMoves.Sort((a, b) => Vector2Int.Distance(a, targetPosition).CompareTo(Vector2Int.Distance(b, targetPosition)));

        //Return the closest position to the target
        return possibleMoves[0];
    }
}
