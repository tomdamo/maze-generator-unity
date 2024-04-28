using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] 
    private MazeCell _mazeCellPrefab;

    // [SerializeField]
    private int _mazeWidth; //number of cells in x direction
    // [SerializeField]
    private int _mazeHeight; //number of cells in z direction
    // [SerializeField]
    private float _generationSpeed; //speed of maze generation
    private MazeCell[,] _mazeGrid; 
    private bool _isGenerating = false;
    // fixes indexoutofrange exception and null reference error by resetting the maze
    private int _currentMazeWidth;
    private int _currentMazeHeight;
    private Coroutine _mazeGenerationCoroutine;


    //speed of generation can be controlled by user
    public void SetTimeValue(int timeValue)
    {
        _generationSpeed = timeValue;
    }
    //make the width and Height of the maze configurable by user
    public void SetWidthValue(int widthValue)
    {
        _mazeWidth = widthValue;
    }
    public void SetHeightValue(int heightValue)
    {
        _mazeHeight = heightValue;
    }
    //TODO: Add a camera to the scene and move it to a position where the whole maze is visible
    //TODO: uneven maze width and Height

    //Start the maze generation process when a button is pressed
    public void StartMazeGeneration()
    {
        if (!_isGenerating)
        {
            _isGenerating = true;
            _mazeGenerationCoroutine = StartCoroutine(Starting());
        }
    }
    public void StopMazeGeneration()
    {
        if (_mazeGenerationCoroutine != null)
        {
            StopCoroutine(_mazeGenerationCoroutine);
            _mazeGenerationCoroutine = null;
        }
        _isGenerating = false;
    }
    //TODO: Add a way to reset the maze and generate a new one
    public void ResetMazeGeneration()
    {
        StopMazeGeneration();
        StartCoroutine(ResetAndStartMaze());
    }
    IEnumerator Starting()
    {
        ResetMaze();
        _mazeGrid = new MazeCell[_mazeWidth, _mazeHeight];
        _currentMazeWidth = _mazeWidth;
        _currentMazeHeight = _mazeHeight;

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeHeight; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity); //specifies no rotation, places cell next to each other 
            }
        }
        _mazeGenerationCoroutine = StartCoroutine(GenerateMaze(null, _mazeGrid[0, 0]));
        yield return _mazeGenerationCoroutine;
    }
    //eventually hits unity's overstack..TODO: fix 
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
        if (unvisitedCells.Count > 0)
        {
            return unvisitedCells.OrderBy(c => Random.Range(0, 10)).FirstOrDefault(c => c != null);
        }
        else
        {
            return null;
        }
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
        if (z + 1 < _mazeHeight)
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
    public void ResetMaze()
    {
        // Check if _mazeGrid is not null
        if (_mazeGrid != null)
        {
            // Destroy all the existing maze cells
            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeHeight; z++)
                {
                    if (_mazeGrid[x, z] != null)
                    {
                        Destroy(_mazeGrid[x, z].gameObject);
                        _mazeGrid[x, z] = null;
                    }
                }
            }
        }
    }
    private IEnumerator ResetAndStartMaze()
    {
        ResetMaze();
        // Wait until the end of frame to ensure all MazeCells are destroyed
        yield return new WaitForEndOfFrame();

        _mazeGenerationCoroutine = StartCoroutine(Starting());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
