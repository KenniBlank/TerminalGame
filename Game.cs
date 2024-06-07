string playerChar = "🐣";
string foodChar = "🫛";
bool playState = true;
int playerX = 10, playerY = 10;
// float speed = 1f;

bool foodInScreen = false;
int foodX = 0;
int foodY = 0;

Random rand = new Random();

int score = 0;

int WindowWidth = 0;
int WindowHeight = 0;

ConsoleColor originalForegroundColor = Console.ForegroundColor;
ConsoleColor originalBackgroundColor = Console.BackgroundColor;


onStart();

void onStart()
{
    Console.Clear();
    Console.CursorVisible = false;
    Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
    Console.Write("Press Enter to Play");
    Console.SetCursorPosition(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2 - 1);
    Console.Write("Rearrange terminal now if you want");
    if (Console.ReadKey().Key == ConsoleKey.Enter)
    {
        // Console.BackgroundColor = ConsoleColor.Green;
        Console.Clear();
        WindowWidth = Console.WindowWidth;
        WindowHeight = Console.WindowHeight;
        setBoundary();
        foodSpawn();
        Console.SetCursorPosition(playerX, playerY);
        Console.Write(playerChar);
        scoreSystem();
        Update();
    }
}

void Update()
{
    do
    {
        food();
        player();
    } while (playState);
    // quit();
}

void player()
{
    int trailX = playerX, trailY = playerY;
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
        case ConsoleKey.W:
            trailY = playerY;
            playerY--;
            break;

        case ConsoleKey.LeftArrow:
        case ConsoleKey.A:
            trailX = playerX;
            playerX--;
            break;

        case ConsoleKey.RightArrow:
        case ConsoleKey.D:
            trailX = playerX;
            playerX++;
            break;

        case ConsoleKey.DownArrow:
        case ConsoleKey.S:
            trailY = playerY;
            playerY++;
            break;

        case ConsoleKey.Escape:
            quitGame();
            break;
    }

    checkForPlayerOutOfBound();
    if (playerX != trailX || playerY != trailY)
    {
        Console.SetCursorPosition(trailX, trailY);
        Console.Write(" ");
    }

    Console.SetCursorPosition(playerX, playerY);
    Console.Write(playerChar);
}

void food()
{
    if (foodX == playerX && foodY == playerY)
    {
        score++;
        scoreSystem();
        Console.SetCursorPosition(foodX, foodY);
        Console.Write(" ");
        foodInScreen = false;
        // playerChar = playerChar + "o";
        Console.SetCursorPosition(foodX, foodY);
        Console.Write(playerChar);
        foodSpawn();
    }
}

void scoreSystem()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(WindowWidth / 2, 0);
    Console.Write($"Score: {score}");
    Console.ForegroundColor = originalForegroundColor;
}

void foodSpawn()
{
    if (!foodInScreen)
    {
        foodX = rand.Next(10, WindowWidth - 10);
        foodY = rand.Next(10, WindowHeight - 10);
        Console.SetCursorPosition(foodX, foodY);
        Console.Write(foodChar);
        Console.SetCursorPosition(playerX, playerY);
        foodInScreen = true;
    }
}

void checkForPlayerOutOfBound()
{
    if (playerY == 1 || playerX == 1 || playerX == (WindowWidth - 1) || playerY == (WindowHeight - 1))
    {
        quitGame();
    }
}


void quitGame()
{
    while (true)
    {
        Console.Clear();
        Console.SetCursorPosition(WindowWidth / 2, WindowHeight / 2);
        Console.Write($"You Scored {score}");
        Console.SetCursorPosition(WindowWidth / 2 - 3, WindowHeight / 2 + 1);
        Console.Write("Press Enter to Exit");
        if (Console.ReadKey().Key == ConsoleKey.Enter)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Environment.Exit(0);
        }
    }
}


void setBoundary()
{
    int windowWidth = Console.WindowWidth;
    int windowHeight = Console.WindowHeight;

    // Top boundary
    for (int i = 0; i < windowWidth; i++)
    {
        Console.SetCursorPosition(i, 0);
        Console.Write("+");
    }

    // Bottom boundary
    for (int i = 0; i < windowWidth; i++)
    {
        Console.SetCursorPosition(i, windowHeight - 1);
        Console.Write("+");
    }

    // Left boundary
    for (int j = 0; j < windowHeight; j++)
    {
        Console.SetCursorPosition(0, j);
        Console.Write("+");
    }

    // Right boundary
    for (int j = 0; j < windowHeight; j++)
    {
        Console.SetCursorPosition(windowWidth - 1, j);
        Console.Write("+");
    }
}




// void changeInTerminalBoundaryError()
// {
//     Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2 - 1);
//     Console.Write("You have changed the Terminal size");
//     quitGame();
// }