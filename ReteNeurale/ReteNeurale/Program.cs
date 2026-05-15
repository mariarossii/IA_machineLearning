//Rossi Maria 5H
//Progetto sull'intelligenza artificiale
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace ReteNeurale
{
    class Program
    {
        const int   FEATURES  = 5;  //il percettone ha 5 input
        const float THRESHOLD = 5f; //soglia usata nella funzione di attivazione

        //funzione di attivazione (step)
        static int Activation(float x)
        {
            if (x > THRESHOLD)
                return 1;
            else
                return 0;
        }

        //funzione con cui carico il peso dei campi dal file txt presente nella cartella
        static int CaricaPesi(string filename, float[] weights, out float bias)
        {
            bias = 0f;

            //errore per file non trovato (deve essere nella cartella)
            if (!File.Exists(filename))
            {
                Console.WriteLine($"Errore: file {filename} non trovato");
                return 0;
            }

            try
            {
                //streamreader per la lettura del file
                using (var reader = new StreamReader(filename))
                {
                    for (int i = 0; i < FEATURES; i++)
                    {
                        string? line = reader.ReadLine();
                        
                        //se la riga non riguarda il peso di un campo
                        if (line == null || !line.Contains("Peso"))
                        {
                            Console.WriteLine($"Errore nella lettura del peso {i}");
                            return 0;
                        }

                        //estrae il valore float della riga
                        if (!float.TryParse(line.Split(':')[1], out weights[i]))
                        {
                            Console.WriteLine($"Errore nella conversione del peso {i}");
                            return 0;
                        }
                    }

                    string? biasLine = reader.ReadLine();
                    if (biasLine == null || !biasLine.Contains("Bias"))
                    {
                        Console.WriteLine("Errore nella lettura del bias");
                        return 0;
                    }

                    //lettura del bias che nel mio caso è 0
                    if (!float.TryParse(biasLine.Split(':')[1], out bias))
                    {
                        Console.WriteLine("Errore nella conversione del bias");
                        return 0;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la lettura del file: {ex.Message}");
                return 0;
            }

            return 1;
        }

        //calcolo dell'esito usando la formula che tiene conto dei pesi per campo e del valore estratto in input
        static int Prevedi(float[] weights, float bias, int[] input)
        {
            float somma = bias;

            for (int i = 0; i < FEATURES; i++)
            {
                somma += input[i] * weights[i];
            }

            return Activation(somma);
        }

        static void Main()
        {
            float[] weights = new float[FEATURES];
            float bias;

            if (CaricaPesi("pesi.txt", weights, out bias) == 0)
            {
                return;
            }

            Console.WriteLine("Inserisci i dati:");

            //array che contiene le risposte dell'utente
            int[] input = new int[FEATURES];

            Console.Write("Artista famoso? (1=Sì, 0=No): ");
            input[0] = int.Parse(Console.ReadLine());

            Console.Write("Bel meteo? (1=Sì, 0=No): ");
            input[1] = int.Parse(Console.ReadLine());

            Console.Write("Amici presenti? (1=Sì, 0=No): ");
            input[2] = int.Parse(Console.ReadLine());

            Console.Write("Cibo buono? (1=Sì, 0=No): ");
            input[3] = int.Parse(Console.ReadLine());

            Console.Write("Alcool disponibile? (1=Sì, 0=No): ");
            input[4] = int.Parse(Console.ReadLine());

            //l'esito dipende dal calcolo che si effettua con la funzione Prevedi
            int decisione = Prevedi(weights, bias, input);

            if (decisione == 1) //esito positivo
            {
                Console.WriteLine("\nVAI AL CONCERTO!!!!");
            }
            else
            {
                Console.WriteLine("\nResta a casa valà");
            }
        }
    }
}

