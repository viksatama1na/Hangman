using System;
using System.Collections.Generic;
using System.IO;

class HangmanGame
{
    static string[] DeathAnimationFrames =
    {
        
        @"  +---+
      |   |
          |
          |
          |
          |
    =========",
        @"  +---+
      |   |
      O   |
          |
          |
          |
    =========",
        @"  +---+
      |   |
      O   |
      |   |
          |
          |
    =========",
        @"  +---+
      |   |
      O   |
     /|   |
          |
          |
    =========",
        @"  +---+
      |   |
      O   |
     /|\  |
          |
          |
    =========",
        @"  +---+
      |   |
      O   |
     /|\  |
     /    |
          |
    =========",
        @"  +---+
      |   |
      O   |
     /|\  |
     / \  |
          |
    ========="
    };

    static Random random = new Random();
    static string[] words;

    static void Main()
    {
        InitializeGame();
        string wordToGuess = GetRandomWord(words);
        string guessedWord = new string('_', wordToGuess.Length);
        int incorrectGuessCount = 0;
        List<char> playerUsedLetters = new List<char>();

        while (true)
        {
            DrawCurrentGameState(false, incorrectGuessCount, guessedWord, playerUsedLetters);
            char playerGuess = GetPlayerGuess(playerUsedLetters);

            if (CheckIfSymbolIsContained(wordToGuess, playerGuess))
            {
                guessedWord = AddLetterToGuessWord(wordToGuess, playerGuess, guessedWord);
                if (CheckIfPlayerWins(guessedWord))
                {
                    Console.WriteLine("Congratulations! You won!");
                    break;
                }
            }
            else
            {
                incorrectGuessCount++;
                if (CheckIfPlayerLoses(incorrectGuessCount))
                {
                    DrawDeathAnimation();
                    Console.WriteLine("Sorry, you lost! The word was: " + wordToGuess);
                    break;
                }
            }

            playerUsedLetters.Add(playerGuess);
        }
    }

    static void InitializeGame()
    {
        string filePath = "words.txt";

        Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());

        if (File.Exists(filePath))
        {
            words = File.ReadAllLines(filePath);
            Console.WriteLine("File 'words.txt' found and loaded.");
        }
        else
        {
            Console.WriteLine("Error: The file 'words.txt' does not exist.");
            Environment.Exit(1); 
        }
    }

    static char GetPlayerGuess(List<char> usedLetters)
    {
        char guess;
        do
        {
            Console.Write("Enter a letter: ");
            string input = Console.ReadLine().ToLower();
            if (input.Length == 1 && char.IsLetter(input[0]))
            {
                guess = input[0];
                if (!usedLetters.Contains(guess))
                    break;
                else
                    Console.WriteLine("You already guessed that letter. Try again.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a single letter.");
            }
        } while (true);

        return guess;
    }

    static void DrawCurrentGameState(bool inputIsInvalid, int incorrectGuess, string guessedWord, List<char> playerUsedLetters)
    {
        Console.Clear();
        if (inputIsInvalid)
            Console.WriteLine("Invalid input. Please enter a single letter.");

        Console.WriteLine($"Incorrect guesses: {incorrectGuess}/{DeathAnimationFrames.Length - 1}");
        Console.WriteLine("Used letters: " + string.Join(", ", playerUsedLetters));
        Console.WriteLine("Guessed word: " + guessedWord);
    }

    static bool CheckIfSymbolIsContained(string word, char playerLetter)
    {
        return word.Contains(playerLetter);
    }

    static string AddLetterToGuessWord(string word, char playerLetter, string currentGuess)
    {
        char[] guessArray = currentGuess.ToCharArray();
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == playerLetter)
            {
                guessArray[i] = playerLetter;
            }
        }
        return new string(guessArray);
    }

    static bool CheckIfPlayerWins(string wordToGuessChar)
    {
        return !wordToGuessChar.Contains('_');
    }

    static bool CheckIfPlayerLoses(int incorrectGuessCount)
    {
        return incorrectGuessCount == DeathAnimationFrames.Length - 1;
    }

    static void DrawDeathAnimation()
    {
        foreach (var frame in DeathAnimationFrames)
        {
            Console.Clear();
            Console.WriteLine(frame);

            
            System.Threading.Thread.Sleep(200);
        }
    }

    static string GetRandomWord(string[] wordsArray)
    {
        return wordsArray[random.Next(wordsArray.Length)];
    }
}
