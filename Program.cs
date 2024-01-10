int gridSize = 0;

do
{
    Console.WriteLine("Enter the size of the grid (e.g. 4 for a 4x4 grid): ");

    var size = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(size) && int.TryParse(size, out int sizeValue))
    {
        if (sizeValue < 2)
            Console.WriteLine("Minimum size of grid is 2.");
        else if (sizeValue > 10)
            Console.WriteLine("Maximum size of grid is 10.");
        else
            gridSize = sizeValue;
    }
    else
    {
        Console.WriteLine("Invalid Input");
    }

} while (gridSize < 2);

int numberOfMines = 0;
var totalSquares = gridSize * gridSize;
do
{
    Console.WriteLine("Enter the number of mines to place on the grid (maximum is 35% of the total squares): ");

    var mines = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(mines) && int.TryParse(mines, out int minesValue))
    {
        if (minesValue <= 0)
            Console.WriteLine("There must be at least 1 mine.");
        else if ((double)minesValue / (double)totalSquares > 0.35)
            Console.WriteLine("Maximum number is 35% of total sqaures.");
        else
            numberOfMines = minesValue;
    }
    else
    {
        Console.WriteLine("Invalid Input");
    }

} while (numberOfMines <= 0);

int rows = gridSize, columns = gridSize, mineCount = numberOfMines;
int[,] minefield = new int[rows, columns];

// Random placing of mines
for (int k = 0; k < mineCount; k++)
{
    int y = Random.Shared.Next(rows);
    int x = Random.Shared.Next(columns);
    while (minefield[y, x] == 9)
        (y, x) = (Random.Shared.Next(rows), Random.Shared.Next(columns));
    minefield[y, x] = 9;

    for (int i = Math.Max(y - 1, 0); i <= Math.Min(y + 1, rows - 1); i++)
        for (int j = Math.Max(x - 1, 0); j <= Math.Min(x + 1, columns - 1); j++)
            if (minefield[i, j] != 9)
                minefield[i, j]++;
}

// Covered Fields :
// 0 = empty
// 1 to 8 = number of mines adjacent
// 9 = mine placed

// Uncovered Fields:
// -10 = empty
// -1 to -8 = number of mines adjacent
// -9 = mine placed

DrawMinefield(minefield, null);

// Game status indicator
GameResult result = GameResult.Continue;
while (result == GameResult.Continue)
{
    // Get input
    int x, y, adjMines = 0;
    while (true)
        try
        {
            Console.Write("Select a square to reveal (e.g. A1): ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 3 || !ValidInput(totalSquares, input, out y, out x))
            {
                Console.WriteLine("Incorrect input.");
            }
            else
            {
                if (x >= 0 && y >= 0 && x < columns && y < rows) // Minefield limits
                    if (minefield[y, x] >= 0)                    // Covered field
                        break;                                   // OK
                    else
                        Console.WriteLine("This field is already uncovered");
                else
                    Console.WriteLine("Invalid coordinates");
            }
        }
        catch
        {
            Console.WriteLine("Coordinates must be a valid integer");
        }

    // Uncover the field by value
    if (minefield[y, x] == 9) // Mine placed
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                if (minefield[i, j] >= 1 && minefield[i, j] <= 9)
                    minefield[i, j] *= -1;
                else if (minefield[i, j] == 0)
                    minefield[i, j] = -10;
        result = GameResult.Loss;
    }
    else if (minefield[y, x] >= 0) // 0 - 8
    {
        adjMines = minefield[y, x];
        UncoverField(y, x, minefield);
    }

    // Verify if is a Win
    if (result == GameResult.Continue)
    {
        result = GameResult.Win;
        foreach (int field in minefield)
            if (field >= 0 && field != 9)
            {
                result = GameResult.Continue;
                break;
            }
    }

    DrawMinefield(minefield, adjMines);
}

// Game result output
if (result == GameResult.Loss)
    Console.WriteLine("You lose!");
else
    Console.WriteLine("You win!");

static void UncoverField(int i, int j, int[,] minefield)
{
    if (i >= 0 && j >= 0 && i < minefield.GetLength(0) && j < minefield.GetLength(1))
    {
        if (minefield[i, j] == 0)
        {
            minefield[i, j] = -10;
            // Uncover all nearby fields
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                    UncoverField(i + y, j + x, minefield);
        }
        else if (minefield[i, j] > 0 && minefield[i, j] < 9) // 1-8
            minefield[i, j] *= -1;
    }
}

static void DrawMinefield(int[,] minefield, int? adjMines)
{
    Console.Clear();

    if (adjMines.HasValue)
    {
        Console.WriteLine($"This square contains {adjMines} adjacent mines.");
        Console.WriteLine();
    }

    Console.Write("   | ");
    for (int j = 0; j < minefield.GetLength(1); j++)
        Console.Write("{0,2} ", GetColumnInteger(j));
    Console.WriteLine();
    Console.WriteLine("---+-".PadRight(minefield.GetLength(1) * 3 + 5, '-'));

    for (int i = 0; i < minefield.GetLength(0); i++)
    {
        Console.Write("{0,2} | ", GetRowAlphabet(i));
        for (int j = 0; j < minefield.GetLength(1); j++)
        {
            if (minefield[i, j] >= 0) // Covered field
                WriteColorizedChar('#', ConsoleColor.Black);
            else if (minefield[i, j] == -10) // Uncovered empty field
                WriteColorizedChar('0', ConsoleColor.DarkGray);
            else if (minefield[i, j] == -9) // Uncovered mine
                WriteColorizedChar('X', ConsoleColor.Red);
            else // Uncovered number
                WriteColorizedChar((-minefield[i, j]).ToString()[0],
                    ConsoleColor.DarkCyan);

        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

static void WriteColorizedChar(char letter, ConsoleColor color)
{
    var originalColor = Console.BackgroundColor;
    Console.BackgroundColor = color;
    Console.Write(" {0} ", letter);
    Console.BackgroundColor = originalColor;
}

static bool ValidInput(int totalSquares, string input, out int finalRow, out int finalColumn)
{
    finalRow = 0; finalColumn = 0;

    var rowString = input.Substring(0, 1);
    var columnString = input.Substring(1, input.Length - 1);

    var row = GetRowInteger(rowString);
    if (!row.HasValue)
        return false;

    finalRow = row.Value;

    if (!int.TryParse(columnString, out int parseColumn))
        return false;

    if (parseColumn < 1 || parseColumn > totalSquares)
        return false;

    finalColumn = parseColumn - 1;
    return true;
}

static int GetColumnInteger(int column)
{
    return column + 1;
}

static string GetRowAlphabet(int row)
{
    switch (row)
    {
        case 0: return "A";
        case 1: return "B";
        case 2: return "C";
        case 3: return "D";
        case 4: return "E";
        case 5: return "F";
        case 6: return "G";
        case 7: return "H";
        case 8: return "I";
        case 9: return "J";
        default:
            break;
    }

    return "";
}

static int? GetRowInteger(string row)
{
    row = row.ToUpper();

    int? result = null;
    switch (row)
    {
        case "A": result = 0; break;
        case "B": result = 1; break;
        case "C": result = 2; break;
        case "D": result = 3; break;
        case "E": result = 4; break;
        case "F": result = 5; break;
        case "G": result = 6; break;
        case "H": result = 7; break;
        case "I": result = 8; break;
        case "J": result = 9; break;
        default: break;
    }

    return result;
}

enum GameResult { Continue, Win, Loss }
