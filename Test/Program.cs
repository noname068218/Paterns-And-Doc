public class Configurazione {
    public static string Versione;

    // Costruttore statico
    static Configurazione() {
        Versione = "1.0.0";
        Console.WriteLine("Configurazione inizializzata");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Versione attuale: " + Configurazione.Versione);
    }
}