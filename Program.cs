int WindowWidth;
int WindowHeight;

string playerChar = "🐥";
string playerChar2 = "🐣";
string playerShoot = "🥚";
bool shotFired = false;
int shotPos = 0;
int toShootScoreAmount = 1;

string enemyChar = "👾";
int enemySpawnX = 0;
float enemySpawnY = 0;
float enemySpeed = 0.01f;

string[] deathMessages = [
    "Enemy Has Landed. YOU DIED!",
    "You were made into fried Chicken.",
    "You were made slave. You now produce eggs for them",
    "YOU WERE KILLED.",
    "You Died Loser",
    "Delicious soup of chicken was made.",
    "You were stripped and sold dead."
    ];

string foodChar = "🫐🫐";
bool playState = true;
int playerX = 40, playerY = 40;
// float speed = 1f;

bool foodInScreen = false;
int foodX = 0;
int foodY = 0;

Random rand = new Random();
int score = 0;
int barHeight;


ConsoleColor originalForegroundColor = Console.ForegroundColor;
ConsoleColor originalBackgroundColor = Console.BackgroundColor;

Start();

void Start()
{
    String[] texts = new String[2];
    texts[0] = "Press Enter to Play";
    texts[1] = "Rearrange terminal now if you want";

    while (true)
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.SetCursorPosition(Console.WindowWidth / 2 - texts[0].Length / 2, Console.WindowHeight / 2);
        Console.Write(texts[0]);
        Console.SetCursorPosition(Console.WindowWidth / 2 - texts[1].Length / 2, Console.WindowHeight / 2 - 2);
        Console.Write(texts[1]);
        if (Console.ReadKey().Key == ConsoleKey.Enter)
        {
            // Console.BackgroundColor = ConsoleColor.Green;
            Console.Clear();
            WindowWidth = Console.WindowWidth;
            WindowHeight = Console.WindowHeight;
            barHeight = WindowHeight - WindowHeight / 5 - 1;
            playerX = WindowWidth / 3;
            playerY = WindowHeight / 3;
            setBoundary();
            foodSpawn();
            enemySpawn();
            Console.SetCursorPosition(playerX, playerY);
            Console.Write(playerChar);
            scoreSystem();
            Update();
        }
    }

}

void Update()
{
    do
    {
        food();
        gravity('F');
        player();
        gravity('P');
        enemy();
        scoreSystem();
        if (shotFired)
        {
            shotUP(playerX);
        }
        changeInTerminalBoundaryError();
        if (score >= toShootScoreAmount)
        {
            String temp = playerChar;
            if (temp == "🐣")
            {
                playerChar = playerChar2;
                playerChar2 = temp;
            }
        }
        enemySpeed += 0.0001f;
        System.Threading.Thread.Sleep(35); // if 100, 0.1s delay everyupdate ie. fps =  10. Do the math
    } while (playState);
    quitGame("How was this escape sequence achieved. Not Possible. Send GamePlay");
}

void shotUP(int x)
{
    if (shotPos == 2)
    {
        shotFired = false;
    }
    Console.SetCursorPosition(x, shotPos);
    Console.Write("  ");
    shotPos -= 1;
    Console.SetCursorPosition(x, shotPos);
    Console.Write(playerShoot);
}

void player()
{
    int trailX = playerX, trailY = playerY;
    if (Console.KeyAvailable)
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
                if (playerY == barHeight)
                {
                    playerY -= 10;
                }
                break;

            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
                if (playerX > 2)
                {
                    playerX--;
                }   
                break;

            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                if  (playerX < WindowWidth - 3)
                {
                    playerX++;
                }
                break;

            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
                if (playerY < barHeight)
                {
                    playerY++;
                }
                break;

            case ConsoleKey.Escape:
                quitGame("Escape Button Pressed");
                break;
            
            case ConsoleKey.Spacebar:
                if (score >= toShootScoreAmount && shotFired == false)
                {
                    score -= toShootScoreAmount;
                    shotFired = true;
                    shotPos = playerY - 1;
                }
                break;
                
        }
    }

    if (playerX != trailX || playerY != trailY)
    {
        Console.SetCursorPosition(trailX, trailY);
        Console.Write(" ");
    }

    Console.SetCursorPosition(playerX, playerY);
    Console.Write(playerChar);
}

void enemy()
{
    // enemySpawn();
    if (((int) enemySpawnX == playerX || (int) enemySpawnX == playerX - 1 || (int) enemySpawnX == playerX + 1) && (int) enemySpawnY == shotPos)
    {
        enemySpawn();
        enemySpeed -= 0.01f;
        score++;
    }
    if ((int)enemySpawnY >= barHeight)
    {
        int i = rand.Next(0, deathMessages.Length);
        quitGame(deathMessages[i]);
    }
    gravity('E');

}

void enemySpawn()
{
    enemySpawnX = rand.Next(10, WindowWidth - 10);
    enemySpawnY = 2;
    Console.SetCursorPosition(enemySpawnX, (int)enemySpawnY);
    Console.Write(enemyChar);
}

void gravity(char character)
{
    if (character == 'P')
    {
        if (playerY < barHeight)
        {
            Console.SetCursorPosition(playerX, playerY);
            Console.Write(" ");
            playerY += 1;
            Console.SetCursorPosition(playerX, playerY);
            Console.Write(playerChar);
        }
    }
    else if (character == 'F')
    {
        if (foodY < barHeight)
        {
            Console.SetCursorPosition(foodX, foodY);
            Console.Write("  ");
            foodY += 1;
            Console.SetCursorPosition(foodX, foodY);
            Console.Write(foodChar);
        }
    }
    else if (character == 'E')
    {
        int spawnY = (int)enemySpawnY;
        Console.SetCursorPosition(enemySpawnX, spawnY);
        Console.Write(" ");
        enemySpawnY += enemySpeed;
        spawnY = (int)enemySpawnY;
        Console.SetCursorPosition(enemySpawnX, spawnY);
        Console.Write(enemyChar);
    }
    else
    {
        return;
    }
}

void food()
{
    if (foodX == playerX && foodY == playerY)
    {
        score += 2 * rand.Next(1, 3);
        scoreSystem();
        Console.SetCursorPosition(foodX, foodY);
        Console.Write(" ");
        foodInScreen = false;
        Console.SetCursorPosition(foodX, foodY);
        Console.Write(playerChar);
    }
}

void scoreSystem()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(WindowWidth / 2, 0);
    Console.Write("Score:   ");
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

void quitGame(string text)
{
    String[] textsDefault = new String[2];
    textsDefault[0] = $"You Scored {score}";
    textsDefault[1] = "Press Enter to Exit";
    while (true)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.SetCursorPosition(WindowWidth / 2 - text.Length / 2, WindowHeight / 2 - 2);
        Console.Write(text);
        Console.ForegroundColor = originalForegroundColor;

        Console.SetCursorPosition(WindowWidth / 2 - textsDefault[0].Length / 2, WindowHeight / 2);
        Console.Write(textsDefault[0]);
        Console.SetCursorPosition(WindowWidth / 2 - textsDefault[1].Length / 2, WindowHeight / 2 + 2);
        Console.Write(textsDefault[1]);
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

    // Top boundary and Bottom Boudary
    for (int i = 0; i < windowWidth; i++)
    {
        Console.SetCursorPosition(i, 0);
        Console.Write('-');
        Console.SetCursorPosition(i, windowHeight - 1);
        Console.Write('-');
    }

    // Left boundary and right border
    for (int j = 0; j < windowHeight; j++)
    {
        Console.SetCursorPosition(0, j);
        Console.Write('|');
        Console.SetCursorPosition(windowWidth - 1, j);
        Console.Write('|');
    }

    // horizontal land
    for (int i = 1; i < windowWidth - 1; i++)
    {
        Console.SetCursorPosition(i, barHeight + 1);
        Console.Write("T");
    }
}

void changeInTerminalBoundaryError()
{
    // Console.Write(WindowWidth + WindowHeight);
    if (Console.WindowWidth != WindowWidth || Console.WindowHeight != WindowHeight)
    {
        String text = "You have changed the Terminal size or minimum terminal size wasn't achieved";
        Console.SetCursorPosition(Console.WindowWidth / 2 - text.Length / 2, Console.WindowHeight / 2);
        Console.Write(text);
        System.Threading.Thread.Sleep(1000);
        quitGame(text);
    }
}