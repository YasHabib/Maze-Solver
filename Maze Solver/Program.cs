// See https://aka.ms/new-console-template for more information


using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

try
{
    //File is located in Maze-Solver\Maze Solver\bin\Debug\net6.0\maze.txt
    //Please type in your file path along with the name of the text file between ""
    var fileName = @"maze.txt";
    Maze(fileName);

    //char[,] a = Maze(fileName);
    //for (int i = 0; i < a.GetLength(0); i++)
    //{
    //    for (int j = 0; j < a.GetLength(1); j++)
    //    {
    //        Console.Write(a[i, j].ToString());

    //    }
    //    Console.WriteLine();
    //}

}
catch (Exception ex)
{
    Console.WriteLine("Exception" + ex.Message);
}



//Method to validate the text file and returning the maze.
static char[,] Maze(string fileName)
{
    string[] lines = File.ReadAllLines(fileName);

    var height = lines.Length;
    var width = lines[0].Length;

    //Creating a 2D array using the file where index i represents each element in row of the matrix and index j represent each element in column

    //Initializing the 2D array
    char[,] mazeMatrix = new char[height, width];

    //Checking how many S and E are in the maze
    int totalS = 0;
    int totalE = 0;


    //Adding data to the 2D array from the lines array
    for (int i = 0; i < lines.Length; i++) //Rows
    {
        for (int j = 0; j < lines[0].Length; j++) //Column
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


    //checking data
    //Console.WriteLine("Width: " + width);
    //Console.WriteLine("Height: " + height);
    //Console.WriteLine("S: " + totalS);
    //Console.WriteLine("E: " + totalE);

    //Console.WriteLine("Matrix: ");
    //for (int i = 0; i < mazeMatrix.GetLength(0); i++)
    //{
    //    for (int j = 0; j < mazeMatrix.GetLength(1); j++)
    //    {
    //        Console.Write(mazeMatrix[i, j].ToString());

    //    }
    //    Console.WriteLine();
    //}

    //Console.WriteLine("Top has X: " + firstLineHasAllX);
    //Console.WriteLine("Bottom has X: " + lastLineHasAllX);

    //Console.WriteLine("left side: " + leftSideHasX);
    //Console.WriteLine("right side: " + rightSideHasX);

    //Validating the mazeMatrix
    if (
            lines != null &&
            (width >= 3 && width <= 255) && //Checking if width and height is between 3 and 255 and not the same
            (height >= 3 && height <= 255) &&
            (width != height) &&
            (leftSideHasX == true && rightSideHasX == true) && //Checking if right and left side of the maze has all X or wall
            (firstLineHasAllX == true && lastLineHasAllX == true) &&//Checking if first row and last row has all X or wall 
            (totalE == 1 && totalS == 1) //Checking if there is exactly one S and one E
        )
    {
        return mazeMatrix;
    }
    else
    {
        Console.WriteLine("Please make sure the maze's line format is correct: " +
        "\n 1) Width has to be between 3 and 255, yours is " + width +
        "\n 2) Height has to be between 3 and 255, yours is " + height +
        "\n 3) Height and Width can not have the same length" +
        "\n 4) The maze has to be surrounded by X, no other characters." +
        "\n 5) There should be exactly one S (start point) and one E (end point) in each maze, you have " + totalS +" S and " + totalE + " E.");
        char[,] matrix = new char[0, 0];
        return matrix;
    }

}




    //foreach (string line in lines)
    //{
    //    //Getting the width of each line
    //    width = line.Length;

    //    //Getting the first and the last element of each line to see if they all start with X and ends with X as well
    //    var firstElementHasX = line.StartsWith('X');
    //    var lastElementHasX = line.EndsWith('X');


    //    //Checking if there is S and E in the maze, if so, is there exact one of each.
    //    var charLine = line.ToCharArray();
    //    var s = charLine.Count(i => i == 'S');
    //    totalS += s;
    //    //Console.WriteLine(totalS);

    //    var e = charLine.Count(i => i == 'E');
    //    totalE += e;
    //    //Console.WriteLine(totalE);


    //    var S = line.IndexOf('S');
    //    var E = line.IndexOf('E');



    //    //Doing validations
    //    if (

    //            (width >= 3 && width <= 255) && //Checking if width and height is between 3 and 255 and not the same
    //            (height >= 3 && height <= 255) &&
    //            (width != height) &&
    //            (firstElementHasX == true && lastElementHasX == true) && //Checking if right and left side of the maze has all X or wall
    //            (firstLineHasAllX == true && lastLineHasAllX == true) &&//Checking if first row and last row has all X or wall 
    //            (totalE == 1 && totalS == 1)
    //        )
    //    {
    //        //Console.WriteLine(line);
    //        //Console.WriteLine(firstElementHasX);
    //        //Console.WriteLine(lastElementHasX);
    //        //Console.WriteLine(S);
    //        //Console.WriteLine(E);
    //    }
    //    else
    //    {
    //        Console.WriteLine("Please make sure the maze's line format is correct: " +
    //        "\n 1) Width has to be between 3 and 255, yours is " + width +
    //        "\n 2) Height has to be between 3 and 255, yours is " + height +
    //        "\n 3) Height and Width can not have the same length" +
    //        "\n 4) The maze has to be surrounded by X, no other characters." +
    //        "\n 5) There should be exactly one S (start point) and one E (end point) in each maze");
    //    }

    //}


    //StreamReader file = new StreamReader("maze.txt");
    //var line  = file.ReadLine();  
    //Console.WriteLine(line);
    //while(line != null)
    //{
    //    Console.WriteLine(line);
    //    line = file.ReadLine();
    //}

    //file.Close();
