using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // COSTANTI
        int N = 500;
        int EPOCHS = 100;
        double LEARNING_RATE = 0.1;

        // Funzione di attivazione (definita come funzione locale)
        int Activation(double x)
        {
            return x > 0.5 ? 1 : 0;
        }

        // Dati di addestramento
        List<int[]> inputData = new List<int[]>();
        List<int> expected = new List<int>();

        // Lettura da file
        try
        {
            string[] lines = File.ReadAllLines("combinazioni.txt");
            for (int i = 0; i < N; i++)
            {
                string[] valori = lines[i].Split(' ');

                // Carica i 6 input
                int[] rigaInput = new int[6];
                for (int j = 0; j < 6; j++)
                {
                    rigaInput[j] = int.Parse(valori[j]);
                }
                inputData.Add(rigaInput);

                // Carica il risultato atteso (settimo valore)
                expected.Add(int.Parse(valori[6]));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore nella lettura del file: " + ex.Message);
            return;
        }

        // Pesi iniziali e bias
        double[] weights = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
        double bias = 0.0;

        // Addestramento
        for (int epoch = 0; epoch < EPOCHS; epoch++)
        {
            for (int i = 0; i < N; i++)
            {
                double sumVal = bias;
                for (int j = 0; j < 6; j++)
                {
                    sumVal += weights[j] * inputData[i][j];
                }

                int output = Activation(sumVal);
                int error = expected[i] - output;

                // Aggiornamento pesi e bias
                for (int j = 0; j < 6; j++)
                {
                    weights[j] += LEARNING_RATE * error * inputData[i][j];
                }
                bias += LEARNING_RATE * error;
            }
        }

        // Output dei pesi finali
        Console.WriteLine("Pesi allenati:");
        for (int i = 0; i < 6; i++)
        {
            Console.WriteLine($"Peso {i}: {weights[i]:F6}");
        }
        Console.WriteLine($"Bias: {bias:F6}");

        // Test finale
        Console.WriteLine("\nTest del percettrone:");
        for (int i = 0; i < N; i++)
        {
            double sumVal = bias;
            for (int j = 0; j < 6; j++)
            {
                sumVal += weights[j] * inputData[i][j];
            }

            int output = Activation(sumVal);
            Console.WriteLine($"Input [{string.Join(", ", inputData[i])}] => Concerto: {output} (Atteso: {expected[i]})");
        }
    }
}