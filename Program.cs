using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
class Program
{
    // Definiamo i dati a livello di classe affinché tutti i metodi possano vederli
    static string filePath = "todo.txt";
    static List<TaskItem> tasks = new List<TaskItem>();

    static void Main(string[] args)
    {
        Load(); // Recuperiamo i dati prima di iniziare
        
        bool running = true;
        while (running)
        {
            PrintMenu();
            string choice = Console.ReadLine();
            
            // Qui richiamiamo la funzione che contiene lo switch
            running = Actions(choice); 
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("\n--- TODO LIST ---");
        Console.WriteLine("1. Aggiungi | 2. Visualizza | 3. Cancella | 4. Salva ed esci");
        Console.Write("Scegli: ");
    }

    // Questa funzione gestisce lo switch e restituisce 'false' solo quando usciamo
    static bool Actions(string choice)
    {
        switch (choice)
        {
            case "1": 
                AddTask(); 
                return true;

            case "2": 
                ViewTasks(); 
                return true;

            case "3": 
                DeleteTask(); 
                return true;

            case "4": 
                Save();
                return false; // Restituiamo false per fermare il while nel Main

            default: 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opzione non valida."); 
                Console.ResetColor();
                return true;
        }
    }

    // --- Metodi di supporto (la logica "sporca" sta qui) ---

    static void AddTask()
    {
        Console.Write("Cosa devi fare? ");
        string text = Console.ReadLine();
        tasks.Add(new TaskItem(text));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Compito aggiunto!");
        Console.ResetColor();
    }

    static void ViewTasks()
    {
        Console.WriteLine("\nI tuoi compiti:");
        for (int i = 0; i < tasks.Count; i++) Console.WriteLine($"{i}. {tasks[i]}");
    }

    static void DeleteTask()
    {
        ViewTasks(); // Riutilizziamo il metodo di visualizzazione
        Console.Write("Numero da cancellare: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < tasks.Count)
        {
            tasks.RemoveAt(index);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Compito rimosso.");
            Console.ResetColor();
        }
        else 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Errore: indice non valido.");
            Console.ResetColor();
        }
    }

    static void Save()
    {
        var lines = tasks.Select(t =>  $"{t.CreationDate:yyyy-MM-dd HH:mm} | {t.Description}").ToArray();
        File.WriteAllLines(filePath, lines);
    } 
    static void Load()
    { 
        if (File.Exists(filePath)) 
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    var task = new TaskItem(parts[1]);
                    task.CreationDate = DateTime.Parse(parts[0]);
                    tasks.Add(task);
                }
            }
        }
    }

    
}
