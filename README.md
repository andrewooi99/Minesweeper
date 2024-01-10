# Instructions
- Please use C#, Java, Kotlin to solve the problem  stated below.

## Here are a few tips:
- Include unit/end-to-end tests in your solution. We recommend using TDD to solve these problems, where possible.
- Keep your functions/classes small.
- Demonstrate a good understanding of clean code, object oriented skills, and SOLID principles.
- Submit production-ready code that is clean and easy to understand with appropriate documentation.

## Rules:
- You may NOT use any external libraries that directly solve this problem, but you may use external libraries or tools for building or testing purposes. Specifically, you may use unit-testing libraries or build tools available for your chosen language (e.g., JUnit, NUnit, etc.).
- You should NOT include any executable attachments, including those with .exe or .lib extensions. System security is very important to us and certain file extensions will be blocked for security purposes, resulting in delays to your application. We need to be able to run and build your code ourselves, so please submit your code as a zipped file of source code and supporting files, without any compiled code. If you're submitting in C#, please do not submit your code as a .msi file.
- Please include a brief explanation of your design and assumptions, along with your code, as well as detailed instructions to run your application. Also include the environment required to run the application, eg. Windows, Linux.
- We assess a number of things including your application code design, your tests and general readability. While these are relatively small problems, we expect you to submit what you believe is production-quality code; code that you'd be able to run, maintain, and evolve. You don't need to gold plate your solution, however we are looking for something more than a bare-bones algorithm.

# Problem statement: MineSweeper App
Write a program that simulates a Minesweeper game on a square grid. 

- The game should begin by prompting the user for the grid size and the number of mines to be randomly placed on the grid. 

- The program should then generate the grid and randomly place the specified number of mines on the grid. 

- The user should then be prompted to select a square on the grid to uncover. 
  - If the selected square contains a mine, the game is over and the user loses. 
  - Otherwise, the selected square is uncovered and reveals a number indicating how many of its adjacent squares contain mines. 
  - If an uncovered square has no adjacent mines, the program should automatically uncover all adjacent squares until it reaches squares that do have adjacent mines. 

- The game is won when all non-mine squares have been uncovered.

- The program should display the game grid and allow the user to input their choices through a command line interface. 

- Additionally, the program should track the user's progress throughout the game, displaying the minefield after each user input.

## Game play

### Sucess example
```
Welcome to Minesweeper!

Enter the size of the grid (e.g. 4 for a 4x4 grid): 
4
Enter the number of mines to place on the grid (maximum is 35% of the total squares): 
3

Here is your minefield:
  1 2 3 4
A _ _ _ _
B _ _ _ _
C _ _ _ _
D _ _ _ _

Select a square to reveal (e.g. A1): D4
This square contains 0 adjacent mines. 

Here is your updated minefield:
  1 2 3 4
A _ _ 2 0
B _ _ 2 0
C _ 2 1 0
D _ 1 0 0

Select a square to reveal (e.g. A1): B1
This square contains 3 adjacent mines. 

Here is your updated minefield:
  1 2 3 4
A _ _ 2 0
B 3 _ 2 0
C _ 2 1 0
D _ 1 0 0

Select a square to reveal (e.g. A1): A1
This square contains 2 adjacent mines. 

Here is your updated minefield:
  1 2 3 4
A 2 _ 2 0
B 3 _ 2 0
C _ 2 1 0
D _ 1 0 0

Select a square to reveal (e.g. A1): D1
This square contains 1 adjacent mines. 

Here is your updated minefield:
  1 2 3 4
A 2 _ 2 0
B 3 _ 2 0
C _ 2 1 0
D 1 1 0 0

Congratulations, you have won the game!
Press any key to play again...
```
Failure example
```
Welcome to Minesweeper!

Enter the size of the grid (e.g. 4 for a 4x4 grid): 
3
Enter the number of mines to place on the grid (maximum is 35% of the total squares): 
3

Here is your minefield:
  1 2 3
A _ _ _
B _ _ _
C _ _ _

Select a square to reveal (e.g. A1): C3
Oh no, you detonated a mine! Game over.
Press any key to play again...
```

Incorect input example
Failure example
```
Welcome to Minesweeper!

Enter the size of the grid (e.g. 4 for a 4x4 grid): 
2
Minimum size of grid is 2.
Enter the size of the grid (e.g. 4 for a 4x4 grid): 
11
Maximum size of grid is 10.
Enter the size of the grid (e.g. 4 for a 4x4 grid): 
A
Incorect input.
Enter the size of the grid (e.g. 4 for a 4x4 grid): 
3
Enter the number of mines to place on the grid (maximum is 35% of the total squares):
4
Maximum number is 35% of total sqaures.
Enter the number of mines to place on the grid (maximum is 35% of the total squares):
0
There must be at least 1 mine.
Enter the number of mines to place on the grid (maximum is 35% of the total squares):
A
Incorrect input.
Enter the number of mines to place on the grid (maximum is 35% of the total squares):
3
Here is your minefield:
  1 2 3
A _ _ _
B _ _ _
C _ _ _

Select a square to reveal (e.g. A1): A
Incorrect input.
Select a square to reveal (e.g. A1):
```