// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

string fileName = "";

try
{
    //File is located in Maze-Solver\Maze Solver\bin\Debug\net6.0\*fileName.txt*
    Console.Write("Enter your filename with the extention: ");
    fileName = Console.ReadLine();

}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}
catch (IOException ex)
{
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
}

//string[] lines = File.ReadAllLines(fileName);
char[,] maze = Maze(fileName);

//Getting the start and end indexes
int startRow = -1;
int startCol = -1;

//A list of the path which have already been taken
bool[,] pathsAlreadyTaken = new bool[maze.GetLength(0), maze.GetLength(1)];

//A list of path (row, column) to write to file
List<int[]> paths = new List<int[]>();

//Checking at which index we are starting at.
for (int i = 0; i < maze.GetLength(0); i++)
{
    for (int j = 0; j < maze.GetLength(1); j++)
    {
        if (maze[i, j] == 'S')
        {
            startRow = i;
            startCol = j;
        }
    }
}
int[] start = { startRow, startCol };

//If the recursion algorithm solves the maze, it'll write "." into a seperate text file
if (SolveMaze(start[0], start[1], pathsAlreadyTaken, paths, maze))
{
    Console.WriteLine("\nPath found!");

    //Skpiing the coordinates of S so it will be visiable in the saved doc
    List<int[]> newPaths = paths.Skip(1).ToList();
    WriteToFile(newPaths, fileName);
}
else Console.WriteLine("\nPath not found");



//Method to validate the text file and returning the maze.
static char[,]? Maze(string fileName)
{
    string[] lines = File.ReadAllLines(fileName);
    var height = lines.Length;
    var width = lines[0].Length;
    bool isRectengle = false;

    //Checking if all line's width are the same
    for(int i = 0; i < height; i++)
    {
        if(lines[i].Length == width && height != width)
        {
            isRectengle = true; 
        }
    }

    //Creating a 2D array using the file where index i represents each element in row of the matrix and index j represent each element in column

    //Initializing the 2D array
    char[,] mazeMatrix = new char[height, width];

    //Checking how many S and E are in the maze
    int totalS = 0;
    int totalE = 0;

    //Adding data to the 2D array from the lines array while checking total number of S and E
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            mazeMatrix[i, j] = lines[i][j];
            if (mazeMatrix[i, j] == 'S')
            {
                totalS++;
            }
            if (mazeMatrix[i, j] == 'E')
            {
                totalE++;
            }
        }
    }

    //Validating to see if the first line(top) and the last line(bottom) has all X values
    var firstLine = lines[0].ToCharArray();
    var lastLine = lines[height - 1].ToCharArray();
    bool firstLineHasAllX = false;
    bool lastLineHasAllX = false;

    foreach (char element in firstLine)
    {
        if (element == 'X')
        {
            firstLineHasAllX = true;
        }
    }
    foreach (char element in lastLine)
    {
        if (element == 'X')
        {
            lastLineHasAllX = true;
        }
    }

    //Checking if the left and right side of the matrix has all X as well
    var leftSideHasX = false;
    var rightSideHasX = false;

    foreach (var line in lines)
    {
        leftSideHasX = line.StartsWith('X');
        rightSideHasX = line.EndsWith('X');
    }

    //Validating the mazeMatrix
    if (
            lines != null &&
            (width >= 3 && width <= 255) && //Checking if width and height is between 3 and 255 and is a rectengle
            (height >= 3 && height <= 255) &&
            isRectengle &&
            (leftSideHasX && rightSideHasX) && //Checking if right and left side of the maze has all X or wall
            (firstLineHasAllX && lastLineHasAllX) &&//Checking if first row and last row has all X or wall 
            (totalE == 1 && totalS == 1) //Checking if there is exactly one S and one E
        )
    {
        return mazeMatrix;
    }
    else
    {
        Console.WriteLine("Please make sure the maze's line format is correct: " +
        "\n 1) Ensure that the file is not blank and the maze is a rectengle" +
        "\n 1) Width has to be between 3 and 255, yours is " + width +
        "\n 2) Height has to be between 3 and 255, yours is " + height +
        "\n 3) Height and Width can not have the same length" + 
        "\n 4) The maze has to be surrounded by X, no other characters." +
        "\n 5) There should be exactly one S (start point) and one E (end point) in each maze, you have " + totalS +" S and " + totalE + " E." + 
        "\n \n This is the outline of your maze, please fix the necessary errors. \n");
        for (int i = 0; i < mazeMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < mazeMatrix.GetLength(1); j++)
            {
                Console.Write(mazeMatrix[i, j].ToString());
            }
            Console.WriteLine();
        }
        char[,] emptyMatrix = new char[0, 0];
        return emptyMatrix;
    }
}

//Method will return if the recursion algorithm was successful or not
static bool SolveMaze(int rowIndex, int colIndex, bool[,] pathsAlreadyTaken, List<int[]> paths, char[,] maze)
{
    bool shouldCheck = true;
    bool correctPath = false;

    //Checking if it is out of bound
    if (rowIndex >= maze.GetLength(0) || rowIndex < 0 || colIndex >= maze.GetLength(1) || colIndex < 0)
    {
        shouldCheck = false;
    }
    else
    {
        if (maze[rowIndex, colIndex] == 'E')
        {
            shouldCheck = false;
            correctPath = true;
        }
        //If it hits a wall, or goes back to an already explored path
        if (maze[rowIndex, colIndex] == 'X')
        {
            shouldCheck = false;
        }
        if (pathsAlreadyTaken[rowIndex, colIndex])
        {
            shouldCheck = false;
        }

    }

    if (shouldCheck)
    {
        paths.Add(new int[] { rowIndex, colIndex });
        pathsAlreadyTaken[rowIndex, colIndex] = true;

        //Check right tile
        correctPath = correctPath || SolveMaze(rowIndex +1, colIndex, pathsAlreadyTaken, paths, maze);
        //Check down tile
        correctPath = correctPath || SolveMaze(rowIndex, colIndex - 1, pathsAlreadyTaken, paths, maze);
        //check left tile
        correctPath = correctPath || SolveMaze(rowIndex - 1, colIndex, pathsAlreadyTaken, paths, maze);
        //check up tile
        correctPath = correctPath || SolveMaze(rowIndex, colIndex + 1, pathsAlreadyTaken, paths, maze);
    }

    return correctPath;
}

static bool Solve(int rowIndex, int colIndex, bool[,] pathsAlreadyTaken, List<int[]> paths, char[,] maze)
{
    if (maze[rowIndex, colIndex] == 'E')
    {
        return true;
    }

    if (maze[rowIndex, colIndex] == 'X' || pathsAlreadyTaken[rowIndex, colIndex])
    {
        return false;
    }

    pathsAlreadyTaken[rowIndex, colIndex] = true;
    paths.Add(new int[] { rowIndex, colIndex });

    //Checking if the points are within boundary and left, right, top and bottom of the maze, respectively.
    if (rowIndex > 0 && Solve(rowIndex - 1, colIndex, pathsAlreadyTaken, paths, maze))
    {
        return true;
    }
    if (rowIndex < maze.GetLength(0) - 1 && Solve(rowIndex + 1, colIndex, pathsAlreadyTaken, paths, maze))
    {
        return true;
    }
    if (rowIndex > 0 && Solve(rowIndex, colIndex - 1, pathsAlreadyTaken, paths, maze))
    {
        return true;
    }
    if (colIndex < maze.GetLength(1) - 1 && Solve(rowIndex, colIndex + 1, pathsAlreadyTaken, paths, maze))
    {
        return true;
    }

    return false;
}



//Method to write '.' to each of the coordinates of the path which have been taken
static void WriteToFile(List<int[]> paths, string fileName)
{
    //Taking the original file, and creating a copy with a different name
    var fileNameWOext = Path.GetFileNameWithoutExtension(fileName);
    var savedFileName = fileNameWOext + "-solution.txt";
    File.Copy(fileName, savedFileName, true);

    //Creating a matrix/2d array
    string[] lines = File.ReadAllLines(savedFileName);
    var height = lines.Length;
    var width = lines[0].Length;
    char[,] solutionMatrix = new char[height, width];

    //Viewing the solution to the user
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            solutionMatrix[i, j] = lines[i][j];
            foreach (var path in paths)
            {
                solutionMatrix[path[0], path[1]] = '.';
            }
            Console.Write(solutionMatrix[i, j].ToString());

        }
        Console.WriteLine();
    }

    //Writing the file
    using (StreamWriter writer = new StreamWriter(savedFileName))
    {

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                writer.Write(solutionMatrix[i, j]);
            }
            writer.WriteLine();
        }
    }
}
