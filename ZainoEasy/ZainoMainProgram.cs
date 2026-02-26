using System;

namespace ZainoEasy
{
    class ZainoMainProgram
    {
        // --- DATI DI INPUT ---
        static int[] pesi = [11, 27, 13, 7, 40]; // Array dei pesi generati
        static int[] valori = [10, 28, 10, 9, 50];

        // private static int[] pesi;
        static int capacita = 50; // Capacità massima dello zaino
        
        // --- STATO DELL'ALGORITMO (Record) ---
        static int spazioOccupatoOttimo = 0;    // Il miglior peso totale trovato finora
        static int valoreOttimo = 0;
        static bool[] risultatoOttimo;// Array per memorizzare la combinazione vincente

        // --- VARIABILI DI ANALISI (Per la spiegazione ai ragazzi) ---
        static long nodiVisitati = 0; // Conta quante volte entriamo nella funzione ricorsiva

        static void Main(string[] args)
        {
            Console.Title = "Analizzatore Algoritmico - Subset Sum Problem";
            Console.WriteLine("=== SISTEMA DI OTTIMIZZAZIONE CARICO (Backtracking + B&B) ===");

            // Prepariamo gli array per la ricerca
            risultatoOttimo = new bool[pesi.Length];
            bool[] sceltaCorrente = new bool[pesi.Length];

            Console.WriteLine("\nElaborazione in corso... L'intelligenza artificiale sta potando l'albero.");

            // Registriamo il tempo di calcolo
            DateTime inizio = DateTime.Now;

            // 4. AVVIO RICERCA RICORSIVA
            // Partiamo dall'indice 0, con peso 0 e l'array di scelte vuoto
            // EseguiRicerca(0, 0, sceltaCorrente);
            EseguiRicercaBase(0, 0, 0, sceltaCorrente); // Per confronto con l'algoritmo puro

            TimeSpan fineProcesso = DateTime.Now - inizio;
            for (int i = 0; i < pesi.Length; i++)
            {
                Console.WriteLine(risultatoOttimo[i] ? $"{pesi[i]} {valori[i]} [X] " : $"{pesi[i]} {valori[i]} [ ] ");
            }
            Console.WriteLine($"Tempo di calcolo:{fineProcesso:g}");
        }

        // --- ALGORITMO PURO (SENZA BRANCH & BOUND) ---
        static void EseguiRicercaBase(int indice, int pesoCorrente, int valoreCorrente, bool[] risultato)
        {
            nodiVisitati++; // Contiamo ogni singolo tentativo

            // CASO BASE: Siamo arrivati in fondo all'array (Foglia)
            if (indice == pesi.Length)
            {
                // Abbiamo trovato una combinazione valida finale?
                // if (pesoCorrente > spazioOccupatoOttimo)
                if (valoreCorrente > valoreOttimo)
                {
                    spazioOccupatoOttimo = pesoCorrente;
                    valoreOttimo = valoreCorrente;
                    Array.Copy(risultato, risultatoOttimo, risultato.Length); // Foto della soluzione
                }
                return;
            }

            // RAMO 1: PROVO A PRENDERE L'OGGETTO (Se ci sta)
            if (pesoCorrente + pesi[indice] <= capacita)
            {
                risultato[indice] = true;
                EseguiRicercaBase(indice + 1, pesoCorrente + pesi[indice], valoreCorrente + valori[indice], risultato);
            }

            // RAMO 2: NON PRENDO L'OGGETTO
            risultato[indice] = false;
            EseguiRicercaBase(indice + 1, pesoCorrente, valoreCorrente, risultato);
        }
    }
}