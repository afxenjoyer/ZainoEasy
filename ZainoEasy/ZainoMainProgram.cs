using System;

namespace ZainoEasy
{
    class ZainoMainProgram
    {
        // --- DATI DI INPUT ---
        static int[] pesi = [ 11, 27, 13, 7, 40 ]; // Array dei pesi generati

        // private static int[] pesi;
        static int capacita = 50; // Capacità massima dello zaino
        
        // --- STATO DELL'ALGORITMO (Record) ---
        static int spazioOccupatoOttimo = 0;    // Il miglior peso totale trovato finora
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
            EseguiRicercaBase(0, 0, sceltaCorrente); // Per confronto con l'algoritmo puro

            for (int i = 0; i < pesi.Length; i++)
            {
                Console.WriteLine(risultatoOttimo[i] ? $"{pesi[i]} [X] " : $"{pesi[i]} [ ] ");
            }

        }

        // --- ALGORITMO PURO (SENZA BRANCH & BOUND) ---
        static void EseguiRicercaBase(int indice, int pesoCorrente, bool[] risultato)
        {
            nodiVisitati++; // Contiamo ogni singolo tentativo

            // CASO BASE: Siamo arrivati in fondo all'array (Foglia)
            if (indice == pesi.Length)
            {
                // Abbiamo trovato una combinazione valida finale?
                if (pesoCorrente > spazioOccupatoOttimo)
                {
                    spazioOccupatoOttimo = pesoCorrente;
                    Array.Copy(risultato, risultatoOttimo, risultato.Length); // Foto della soluzione
                }
                return;
            }

            // RAMO 1: PROVO A PRENDERE L'OGGETTO (Se ci sta)
            if (pesoCorrente + pesi[indice] <= capacita)
            {
                risultato[indice] = true;
                EseguiRicercaBase(indice + 1, pesoCorrente + pesi[indice], risultato);
            }

            // RAMO 2: NON PRENDO L'OGGETTO
            risultato[indice] = false;
            EseguiRicercaBase(indice + 1, pesoCorrente, risultato);
        }

        static void MostraReportEfficienza(TimeSpan durata)
        {
            // Calcolo combinazioni teoriche totali (2^(n+1)-1)
            double nodiTeorici = Math.Pow(2, pesi.Length + 1) - 1;
            double risparmioPerc = (1 - nodiVisitati / nodiTeorici) * 100;
            long nodiRisparmiati = (long)nodiTeorici - nodiVisitati;

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("REPORT DI OTTIMIZZAZIONE");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Tempo di calcolo          : {durata.TotalMilliseconds} ms");
            Console.WriteLine($"Peso Ottimo trovato       : {spazioOccupatoOttimo} / {capacita}");
            Console.WriteLine($"Nodi visitati (Reali)     : {nodiVisitati:N0}");
            Console.WriteLine($"Nodi teorici (Senza B&B)  : {nodiTeorici:N0}");
            Console.WriteLine($"Nodi non esplorati (Poto) : {nodiRisparmiati:N0}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"EFFICIENZA BRANCH & BOUND : {risparmioPerc:F8}%");
            Console.ResetColor();
            Console.WriteLine(new string('=', 60));

            Console.WriteLine("\nVISUALIZZAZIONE SCELTE (Verde = Nello zaino):");
            for (int i = 0; i < pesi.Length; i++)
            {
                if (risultatoOttimo[i])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" [X] Elemento {i:D2}: peso {pesi[i]}");
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine($" [ ] Elemento {i:D2}: peso {pesi[i]}");
                }
            }
            Console.ResetColor();
            Console.WriteLine("\nPremi un tasto per uscire...");
            Console.ReadKey();
        }
    }
}