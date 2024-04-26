using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] 
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth; //number of cells in x direction
    [SerializeField]
    private int _mazeDepth; //number of cells in z direction

    private MazeCell[,] _mazeGrid; 

    //TODO: make the width and depth of the maze configurable by user
    //TODO: Start the maze generation process when a button is pressed
    //TODO: Add a camera to the scene and move it to a position where the whole maze is visible
    //TODO: speed of generation can be controlled by user
    //TODO: Add a way to reset the maze and generate a new one
    //TODO: uneven maze width and depth
    IEnumerator Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity); //specifies no rotation, places cell next to each other 
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0, 0]);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        Clearwalls(previousCell, currentCell);

        yield return new WaitForSeconds(0.05f);
        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedeCell(currentCell);
            // recursive call, if next cell is not null, generate maze again
            if(nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedeCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        return unvisitedCells.OrderBy(c => Random.Range(0, 10)).FirstOrDefault();
        // if(unvisitedCells.Count == 0)
        // {
        //     return null;
        // }
        // int randomIndex = Random.Range(0, unvisitedCells.Count);
        // return unvisitedCells[randomIndex];
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if(x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if(!cellToRight.IsVisited)
            {
                yield return cellToRight;
                //adds it to return collection but not exits the method
            }
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if(!cellToLeft.IsVisited)
            {
                yield return cellToLeft;
            }
        }
        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if(!cellToFront.IsVisited)
            {
                yield return cellToFront;
            }
        }
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if(!cellToBack.IsVisited)
            {
                yield return cellToBack;
            }
        }
    }
   
    private void Clearwalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }
        //check if position of previous cell is left of current cell and clear relevant walls
        if(previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        //check if position of previous cell is right of current cell and clear relevant walls
        if(previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        //back to front
        //check if position of previous cell is below current cell and clear relevant walls
        if(previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        //front to back
        //check if position of previous cell is above current cell and clear relevant walls
        if(previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
