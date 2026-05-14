using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace ReteNeurale
{



class Program
    {
        const int FEATURES = 5;
        const float THRESHOLD = 5f;

        //funzione di attivazione (step)
        int Activation(float x)
        {
            if (x > THRESHOLD)
                return 1;
            else
                return 0;
        }

        int CaricaPesi(string filename, float[] weights, out float bias)
        {
            bias = 0f;
            if (!File.Exists(filename))
            {
                Console.WriteLine($"Errore: file {filename} non trovato");
                return 0;
            }

            try
            {
                using (var reader = new StreamReader(filename))
                {
                    for (int i = 0; i < FEATURES; i++)
                    {
                        string? line = reader.ReadLine();
                        if (line == null || !line.Contains("Peso"))
                        {
                            Console.WriteLine($"Errore nella lettura del peso {i}")
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

        int Prevedi(float[] weights, float bias, int[] input)
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

            if (CaricaPesi("pesi.txt",  weights, out bias) == 0)
            {
                return;
            }

            Console.WriteLine("Inserisci i dati:");

            int[] input = new int[FEATURES];

            Console.WriteLine("Artista famoso? (1=Sì, 0=No): ");
            input[0] = int.Parse(Console.ReadLine());

            Console.WriteLine("Bel meteo? (1=Sì, 0=No): ");
            input[1] = int.Parse(Console.ReadLine());

            Console.WriteLine("Amici presenti? (1=Sì, 0=No): ");
            input[2] = int.Parse(Console.ReadLine());

            Console.WriteLine("Cibo buono? (1=Sì, 0=No): ");
            input[3] = int.Parse(Console.ReadLine());

            Console.WriteLine("Alcool disponibile? (1=Sì, 0=No): ");
            input[4] = int.Parse(Console.ReadLine());

            int decisione = Prevedi(weights, bias, input);

            if (decisione == 1)
            {
                Console.WriteLine("VAI AL CONCERTO!!!!");
            }
            else
            {
                Console.WriteLine("Resta a casa valà");
            }
        }
    }

}