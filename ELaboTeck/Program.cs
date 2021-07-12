using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELaboTeck {
    class Program {

        static void Main(string[] args) {

            Console.WriteLine("Test 1:");
            int[] chips = { 1, 5, 9, 10, 5 };

            MinimumOfChipsMovesToEquilibrium m = new MinimumOfChipsMovesToEquilibrium();
            m.SetChips(chips);

            Console.WriteLine("chips: [1, 5, 9, 10, 5]");
            Console.WriteLine($"Output: {m.MinimumNumberOfChipMovesToEquilibrium()}");
            Console.WriteLine("Expected output: 12");
            Console.WriteLine();

            Console.WriteLine("Test 2:");
            chips = new int[] { 1, 2, 3 };

            m.SetChips(chips);

            Console.WriteLine("chips: [1, 2, 3]");
            Console.WriteLine($"Output: {m.MinimumNumberOfChipMovesToEquilibrium()}");
            Console.WriteLine("Expected output: 1");
            Console.WriteLine();

            Console.WriteLine("Test 3:");

            chips = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 };
            m.SetChips(chips);

            Console.WriteLine("chips: [0, 1, 1, 1, 1, 1, 1, 1, 1, 2]");
            Console.WriteLine($"Output: {m.MinimumNumberOfChipMovesToEquilibrium()}");
            Console.WriteLine("Expected output: 1");

            Random r = new Random();
            int[] test = new int[20];
            for(int i = 0; i < 20; i++) {
                for(int j = 0; j < 20; j++) {
                    test[j] = 3;
                }
                for(int j = 0; j < i; j++) {
                    test[r.Next(0, 20)]++;
                    test[r.Next(0, 20)]--;
                }
                Console.WriteLine();

                for (int j = 0; j< 20; j++) {
                    Console.Write($"{test[j],4}");
                }
                Console.WriteLine();
                m.SetChips(test);
                Console.WriteLine($"Output: {m.MinimumNumberOfChipMovesToEquilibrium()}");


            }


        }

        static int MinimumNumberOfChipMovesToEquilibrium(int[] chips) {

            int average = 0;                                        // находим среднее значение в клетке
            for (int i = 0; i < chips.Length; i++) {
                average += chips[i];
            }
            average /= chips.Length;

            List<int> excess = new List<int>();
            List<int> excessIndexes = new List<int>();

            List<int> deficit = new List<int>();
            List<int> deficitIndexes = new List<int>();

            for (int i = 0; i < chips.Length; i++) {            // выписываем избытки и недостатки с их индексами
                if (chips[i] < average) {
                    deficit.Add(chips[i]);
                    deficitIndexes.Add(i);
                } else if (chips[i] > average) {
                    excess.Add(chips[i]);
                    excessIndexes.Add(i);
                }
            }

            if (excess.Count == 0) {
                return 0;
            }

            int n = 0;
            for (int i = 0; i < excess.Count; i++) {
                n += excess[i] - average;
            }

            int[,] taskMatrix = new int[n, n];
            int[,] matrix = new int[n, n];

            // заполняем матрицу расстояний
            for (int row = 0, i = 0; i < deficit.Count; i++) {
                for (int i_0 = 0; i_0 < average - deficit[i]; i_0++, row++) {
                    for (int column = 0, j = 0; j < excess.Count; j++) {
                        for (int j_0 = 0; j_0 < excess[j] - average; j_0++, column++) {
                            int cost = deficitIndexes[i] - excessIndexes[j];
                            if (cost < 0) {
                                cost *= -1;
                            }
                            if (cost > chips.Length / 2) {
                                cost = chips.Length - cost;
                            }
                            taskMatrix[row, column] = cost;
                            matrix[row, column] = cost;
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++) {
                int min = matrix[i, 0];
                for (int j = 0; j < n; j++) {
                    if (matrix[i, j] < min) {
                        min = matrix[i, j];
                    }
                }
                for (int j = 0; j < n; j++) {
                    matrix[i, j] -= min;
                }
            }

            for (int j = 0; j < n; j++) {
                int min = matrix[0, j];
                for (int i = 0; i < n; i++) {
                    if (matrix[i, j] < min) {
                        min = matrix[i, j];
                    }
                }
                if (min > 0) {
                    for (int i = 0; i < n; i++) {
                        matrix[i, j] -= min;
                    }
                }
            }

            HashSet<int> rows = new HashSet<int>();
            HashSet<int> columns = new HashSet<int>();
            List<int>[] g = new List<int>[n];
            for (int i = 0; i < n; i++) {
                g[i] = new List<int>();
            }

            int[] mt = new int[n];
            bool[] used = new bool[n];
            bool[] used1 = new bool[n];

            int[,] stack = new int[n, 2];
            int pairs = 0;

            while (pairs != n) {
                pairs = 0;
                for (int i = 0; i < n; i++) {
                    g[i].Clear();
                }
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        if (matrix[i, j] == 0) {
                            g[i].Add(j);
                        }
                    }
                }

                for (int i = 0; i < mt.Length; i++) {
                    mt[i] = -1;
                    used1[i] = false;
                }

                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < g[i].Count; j++)
                        if (mt[g[i][j]] == -1) {
                            mt[g[i][j]] = i;
                            pairs++;
                            used1[i] = true;
                            break;
                        }
                }
                if (pairs == n) {
                    continue;
                }

                rows.Clear();
                columns.Clear();
                bool[] selectedRows = new bool[n];
                bool[] selectedColumns = new bool[n];

                for (int i = 0; i < n; i++) { // Ищем наибольшее паросочетание алгоритмом Куна
                    if (used1[i]) continue;
                    for (int j = 0; j < used.Length; j++) {
                        used[j] = false;
                        selectedRows[j] = false;
                        selectedColumns[j] = false;
                    }

                    int v = i;
                    int step = 0;

                    selectedRows[v] = true;
                    while (step >= 0) {
                        stack[step, 0] = v;
                        used[v] = true;
                        int newItem = -1; ;
                        while (stack[step, 1] < g[v].Count) {
                            if (mt[g[v][stack[step, 1]]] == -1) {
                                break;
                            }
                            if (!used[mt[g[v][stack[step, 1]]]]) {
                                newItem = mt[g[v][stack[step, 1]]];
                                selectedRows[newItem] = true;
                                selectedColumns[g[v][stack[step, 1]]] = true;
                                break;
                            }
                            stack[step, 1]++;
                        }
                        if (stack[step, 1] == g[v].Count) {
                            stack[step, 1] = 0;
                            if (step != 0) {
                                v = stack[step - 1, 0];
                            }
                            step--;
                            continue;
                        }
                        if (newItem != -1) {
                            v = newItem;
                        }
                        if (stack[step, 0] == v) {
                            pairs++;
                            break;
                        } else {
                            step++;
                        }

                    }
                    for (int j = 0; j < step + 1; j++) {
                        mt[g[stack[j, 0]][stack[j, 1]]] = stack[j, 0];
                        stack[j, 1] = 0;
                    }

                    if (step == -1) {
                        for (int j = 0; j < n; j++) {
                            if (selectedRows[j]) {
                                rows.Add(j);
                            }
                            if (selectedColumns[j]) {
                                columns.Add(j);
                            }
                        }
                    }

                }
                if (pairs == n) {
                    continue;
                }

                int min = int.MaxValue;
                for (int i = 0; i < n; i++) {          // пересчитываем матрицу
                    for (int j = 0; j < n; j++) {
                        if (rows.Contains(i) && !columns.Contains(j)) {
                            if (matrix[i, j] < min) {
                                min = matrix[i, j];
                            }
                        }
                    }
                }

                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        if (rows.Contains(i)) {
                            matrix[i, j] -= min;
                        }
                        if (columns.Contains(j)) {
                            matrix[i, j] += min;
                        }
                    }
                }

            }

            int steps = 0; // - ответ

            for (int i = 0; i < mt.Length; i++) {
                steps += taskMatrix[mt[i], i];
            }

            return steps;
        }
    }
}
