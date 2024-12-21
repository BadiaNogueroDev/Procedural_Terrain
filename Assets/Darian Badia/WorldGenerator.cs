using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private List<ChunkGenerator> _chunks = new List<ChunkGenerator>();
    [SerializeField] private ChunkGenerator _chunkGenerator;
    
    [SerializeField] private int _chunkAmount = 3;
    [SerializeField] private int _chunkSizeX = 13;
    [SerializeField] private int _chunkSizeZ = 13;
    
    public void SetChunkSize(int X, int Z)
    {
        _chunkSizeX = X;
        _chunkSizeZ = Z;
    }

    public void SetChunkAmount(int amount)
    {
        _chunkAmount = amount;
    }
    
    public void GenerateWorld()
    {
        int endPathEdge = Random.Range(0,4);
        Vector2Int instantiatePosition = new Vector2Int(0,0);
        Vector2Int startPathPosition = new Vector2Int(_chunkSizeX/2, _chunkSizeZ/2);
        Vector2Int endPathPosition = SetNewEndPathPosition(endPathEdge);
        
        for (int i = 0; i < _chunkAmount; i++)
        {
            //Generate chunk
            ChunkGenerator chunk = Instantiate(_chunkGenerator, new Vector3(instantiatePosition.x, 0, instantiatePosition.y), Quaternion.identity, transform);
            chunk.Initialize(_chunkSizeX, _chunkSizeZ, instantiatePosition, startPathPosition, endPathPosition);
            _chunks.Add(chunk);
            
            //Assign new values for next chunk
            instantiatePosition = SetNewInstantiatePositions(endPathEdge, instantiatePosition);
            startPathPosition = SetNewStartPathPositions(endPathEdge, endPathPosition);
            endPathEdge = SetNewEndPathEdge(endPathEdge);
            endPathPosition = SetNewEndPathPosition(endPathEdge);
        }
    }
    
    public void DeleteWorld()
    {
        if (_chunks != null)
        {
            foreach (var chunk in _chunks)
            {
                DestroyImmediate(chunk.gameObject);
            }
            _chunks.Clear();
        }
    }

    int SetNewEndPathEdge(int previousEdge)
    {
        int edge = Random.Range(0,4);
        switch (edge)
        {
            case 0:
                if (previousEdge == 1) edge = SetNewEndPathEdge(previousEdge);
                break;
            case 1:
                if (previousEdge == 0) edge = SetNewEndPathEdge(previousEdge);
                break;
            case 2:
                if (previousEdge == 3) edge = SetNewEndPathEdge(previousEdge);
                break;
            case 3:
                if (previousEdge == 2) edge = SetNewEndPathEdge(previousEdge);
                break;
        }

        return edge;
    }
    
    Vector2Int SetNewInstantiatePositions(int edge, Vector2Int instantiatePosition)
    {
        switch (edge)
        {
            case 0: return new Vector2Int(instantiatePosition.x - _chunkSizeX, instantiatePosition.y); //Left
            case 1: return new Vector2Int(instantiatePosition.x + _chunkSizeX, instantiatePosition.y); //Right
            case 2: return new Vector2Int(instantiatePosition.x, instantiatePosition.y + _chunkSizeZ); //Back
            case 3: return new Vector2Int(instantiatePosition.x, instantiatePosition.y - _chunkSizeZ); //Front
            default: return Vector2Int.zero;
        }
    }
    
    Vector2Int SetNewStartPathPositions(int edge, Vector2Int endPathPosition)
    {
        switch (edge)
        {
            case 0: return new Vector2Int(endPathPosition.x + _chunkSizeX - 1, endPathPosition.y); //Left
            case 1: return new Vector2Int(endPathPosition.x - _chunkSizeX + 1, endPathPosition.y); //Right
            case 2: return new Vector2Int(endPathPosition.x, endPathPosition.y - _chunkSizeZ + 1); //Back
            case 3: return new Vector2Int(endPathPosition.x, endPathPosition.y + _chunkSizeZ - 1); //Front
            default: return Vector2Int.zero;
        }
    }
    
    Vector2Int SetNewEndPathPosition(int edge)
    {
        switch (edge)
        {
            case 0: return new Vector2Int(0, Random.Range(1, _chunkSizeZ - 1)); //Left
            case 1: return new Vector2Int(_chunkSizeX - 1, Random.Range(1, _chunkSizeZ - 1)); //Right
            case 2: return new Vector2Int(Random.Range(1, _chunkSizeX - 1), _chunkSizeZ - 1); //Back
            case 3: return new Vector2Int(Random.Range(1, _chunkSizeX - 1), 0); //Front
            default: return Vector2Int.zero;
        }
    }
}