using System;

namespace ZainoEasy
{
    class ZainoMainProgram
    {
        // --- DATI DI INPUT ---
        static int n;                 // Numero di elementi (pesi)
        static int[] pesi;            // Array dei pesi generati
        static int capacitaMax;       // Capacità massima dello zaino

        // --- STATO DELL'ALGORITMO (Record) ---
        static int pesoOttimo = 0;    // Il miglior peso totale trovato finora
        static bool[] soluzioneOttima;// Array per memorizzare la combinazione vincente

        // --- VARIABILI DI ANALISI (Per la spiegazione ai ragazzi) ---
        static long nodiVisitati = 0; // Conta quante volte entriamo nella funzione ricorsiva

        static void Main(string[] args)
        {
            Console.Title = "Analizzatore Algoritmico - Subset Sum Problem";
            Console.WriteLine("=== SISTEMA DI OTTIMIZZAZIONE CARICO (Backtracking + B&B) ===");

            // 1. VALIDAZIONE INPUT: Numero di oggetti
            // Usiamo do-while per impedire input nulli o negativi
            do
            {
                Console.Write("\nQuanti elementi vuoi generare? (es. 25-30): ");
                int.TryParse(Console.ReadLine(), out n);
                if (n <= 0) Console.WriteLine("ERRORE: Inserire un numero intero positivo!");
            } while (n <= 0);

            // 2. VALIDAZIONE INPUT: Capacità massima
            do
            {
                Console.Write("Capacita' massima dello zaino: ");
                int.TryParse(Console.ReadLine(), out capacitaMax);
                if (capacitaMax <= 0) Console.WriteLine("ERRORE: La capacita' deve essere maggiore di zero!");
            } while (capacitaMax <= 0);

            // 3. GENERAZIONE AUTOMATICA E ORDINAMENTO
            GeneraDatiEOrdina();

            // Prepariamo gli array per la ricerca
            soluzioneOttima = new bool[n];
            bool[] sceltaCorrente = new bool[n];

            Console.WriteLine("\nElaborazione in corso... L'intelligenza artificiale sta potando l'albero.");

            // Registriamo il tempo di calcolo
            DateTime inizio = DateTime.Now;

            // 4. AVVIO RICERCA RICORSIVA
            // Partiamo dall'indice 0, con peso 0 e l'array di scelte vuoto
            // EseguiRicerca(0, 0, sceltaCorrente);
            EseguiRicercaBase(0, 0, sceltaCorrente); // Per confronto con l'algoritmo puro

            DateTime fine = DateTime.Now;
            TimeSpan tempoImpiegato = fine - inizio;

            // 5. REPORT FINALE COMPLETO
            MostraReportEfficienza(tempoImpiegato);
        }

        /// <summary>
        /// Funzione ricorsiva che esplora lo spazio degli stati (Albero Decisionale)
        /// </summary>
        static void EseguiRicerca(int indice, int pesoCorrente, bool[] scelta)
        {
            nodiVisitati++; // Conteggio nodi per il report di efficienza

            // --- CASO BASE (Foglia raggiunto) ---
            if (indice == n)
            {
                // Se abbiamo trovato una combinazione migliore del record attuale
                if (pesoCorrente > pesoOttimo)
                {
                    pesoOttimo = pesoCorrente;
                    Array.Copy(scelta, soluzioneOttima, n); // Salviamo il percorso vincente
                }
                return;
            }

            // --- BRANCH AND BOUND (Il cuore dell'efficienza) ---
            // Se il peso attuale + la somma di TUTTI i pesi rimanenti non batte il record,
            // non ha senso continuare l'esplorazione. TAGLIAMO IL RAMO (Pruning).
            if (pesoCorrente + SommaRimanente(indice) <= pesoOttimo)
            {
                return;
            }

            // --- BACKTRACKING: RAMO SI (Includo il peso corrente) ---
            if (pesoCorrente + pesi[indice] <= capacitaMax)
            {
                scelta[indice] = true;
                EseguiRicerca(indice + 1, pesoCorrente + pesi[indice], scelta);

                // Ottimizzazione "Scacco Matto": se ho riempito lo zaino al 100%, esco subito
                if (pesoOttimo == capacitaMax) return;
            }

            // --- BACKTRACKING: RAMO NO (Escludo il peso corrente) ---
            scelta[indice] = false;
            EseguiRicerca(indice + 1, pesoCorrente, scelta);
        }
        // --- ALGORITMO PURO (SENZA BRANCH & BOUND) ---
        static void EseguiRicercaBase(int indice, int pesoCorrente, bool[] scelta)
        {
            nodiVisitati++; // Contiamo ogni singolo tentativo

            // CASO BASE: Siamo arrivati in fondo all'array (Foglia)
            if (indice == n)
            {
                // Abbiamo trovato una combinazione valida finale?
                if (pesoCorrente > pesoOttimo)
                {
                    pesoOttimo = pesoCorrente;
                    Array.Copy(scelta, soluzioneOttima, n); // Foto della soluzione
                }
                return;
            }

            // QUI MANCA IL "BOUND": Non controlliamo se ha senso continuare.
            // Andiamo avanti alla cieca.

            // RAMO 1: PROVO A PRENDERE L'OGGETTO (Se ci sta)
            if (pesoCorrente + pesi[indice] <= capacitaMax)
            {
                scelta[indice] = true;
                EseguiRicercaBase(indice + 1, pesoCorrente + pesi[indice], scelta);
            }

            // RAMO 2: NON PRENDO L'OGGETTO
            scelta[indice] = false;
            EseguiRicercaBase(indice + 1, pesoCorrente, scelta);
        }

        // Calcola la somma di tutti i pesi non ancora valutati (stima ottimistica)
        static int SommaRimanente(int start)
        {
            int s = 0;
            for (int j = start; j < n; j++) s += pesi[j];
            return s;
        }

        static void GeneraDatiEOrdina()
        {
            pesi = new int[n];
            Random rnd = new Random();
            // Ogni peso sarà tra 1 e il 40% della capacità per rendere il problema "denso"
            int maxPesoOggetto = (int)(capacitaMax * 0.4) + 2; // Il +2 (e non il +1) è per evitare di generare pesi troppo piccoli che non sfruttano bene la capacità

            for (int i = 0; i < n; i++) pesi[i] = rnd.Next(1, maxPesoOggetto);

            // ORDINAMENTO DECRESCENTE: Fondamentale per far funzionare bene il Bound.
            // Pesi grandi all'inizio = record alto subito = più rami tagliati.
            // Array.Sort(pesi);
            // Array.Reverse(pesi);

            // Console.WriteLine("\nPesi generati (ordinati per efficienza):");
            Console.WriteLine("\nPesi generati :");
            foreach (int p in pesi) Console.Write($"[{p}] ");
            Console.WriteLine("\n");
        }

        static void MostraReportEfficienza(TimeSpan durata)
        {
            // Calcolo combinazioni teoriche totali (2^(n+1)-1)
            double nodiTeorici = Math.Pow(2, n + 1) - 1;
            double risparmioPerc = (1 - nodiVisitati / nodiTeorici) * 100;
            long nodiRisparmiati = (long)nodiTeorici - nodiVisitati;

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("REPORT DI OTTIMIZZAZIONE");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Tempo di calcolo          : {durata.TotalMilliseconds} ms");
            Console.WriteLine($"Peso Ottimo trovato       : {pesoOttimo} / {capacitaMax}");
            Console.WriteLine($"Nodi visitati (Reali)     : {nodiVisitati:N0}");
            Console.WriteLine($"Nodi teorici (Senza B&B)  : {nodiTeorici:N0}");
            Console.WriteLine($"Nodi non esplorati (Poto) : {nodiRisparmiati:N0}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"EFFICIENZA BRANCH & BOUND : {risparmioPerc:F8}%");
            Console.ResetColor();
            Console.WriteLine(new string('=', 60));

            Console.WriteLine("\nVISUALIZZAZIONE SCELTE (Verde = Nello zaino):");
            for (int i = 0; i < n; i++)
            {
                if (soluzioneOttima[i])
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