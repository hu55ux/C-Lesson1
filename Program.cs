
using System;
using System.Threading;

class QuizGame
{
    private static bool timeUp = false;
    private static int timeLeft = 5 * 60; // 5 minutes in seconds

    static void UpdateTimerDisplay()
    {
        // Save current cursor position
        int savedLeft = Console.CursorLeft;
        int savedTop = Console.CursorTop;

        // Update timer in fixed position (top right)
        Console.SetCursorPosition(Console.WindowWidth - 20, 0);
        int minutes = timeLeft / 60;
        int seconds = timeLeft % 60;
        Console.Write($"Time: {minutes}:{seconds:D2}   ");

        // Restore cursor position
        Console.SetCursorPosition(savedLeft, savedTop);
    }

    static void StartFiveMinuteTimer()
    {
        while (timeLeft > 0 && !timeUp)
        {
            UpdateTimerDisplay();
            Thread.Sleep(1000);
            timeLeft--;

            if (timeLeft == 0)
            {
                timeUp = true;
                UpdateTimerDisplay();
                Console.WriteLine("\n\nTime's up!");
            }
        }
    }

    static void Main()
    {
        Console.Clear();
        Console.WriteLine("Quiz Game - Press any key to start!");
        Console.ReadKey();

        string[] questions =
        {
            "1. What's the capital of France?\nA) Paris\nB) London\nC) Berlin\nD) Madrid",
            "2. What's the capital of Germany?\nA) Paris\nB) London\nC) Berlin\nD) Madrid",
            "3. What's the capital of Spain?\nA) Paris\nB) London\nC) Berlin\nD) Madrid",
            "4. What's the capital of Italy?\nA) Paris\nB) London\nC) Berlin\nD) Rome",
            "5. What's the capital of USA?\nA) New York\nB) Los Angeles\nC) Chicago\nD) Washington"
        };

        string[] answers = { "A", "C", "D", "D", "D" };
        int score = 0;

        // Start timer thread
        Thread timerThread = new Thread(StartFiveMinuteTimer);
        timerThread.Start();

        for (int i = 0; i < questions.Length && !timeUp; i++)
        {
            // Clear only the question area (below timer)
            Console.SetCursorPosition(0, 2);
            for (int j = 2; j < Console.WindowHeight; j++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 2);

            // Display question
            Console.WriteLine(questions[i]);
            Console.Write("\nYour answer: ");

            string userAnswer = Console.ReadLine()?.Trim().ToUpper();

            if (timeUp) break;

            // Show feedback
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            if (userAnswer == answers[i])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Correct!");
                score++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Wrong! Correct answer was {answers[i]}");
            }
            Console.ResetColor();

            if (i < questions.Length - 1 && !timeUp)
            {
                Console.WriteLine("\nPress any key for next question...");
                Console.ReadKey();
            }
        }

        // Game over screen
        Console.Clear();
        Console.WriteLine("Quiz Finished!");
        Console.WriteLine($"Your score: {score}/{questions.Length}");

        if (timeUp)
        {
            Console.WriteLine("(Time ran out)");
        }

        timeUp = true;
        timerThread.Join();
    }
}
