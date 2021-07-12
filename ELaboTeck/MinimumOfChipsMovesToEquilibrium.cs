using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELaboTeck {
    class MinimumOfChipsMovesToEquilibrium {

        int[] chips;
        int average;
        int[,] taskMatrix;
        int[,] matrix;
        int n = 0;

        HashSet<int> rows = new HashSet<int>();
        HashSet<int> columns = new HashSet<int>();
        List<int>[] g;
        int[] mt;
        bool[] used;
        bool[] used1;

        int[,] stack;
        int pairs = 0;

        public void SetChips(int[] chips) {
            this.chips = chips;
        }

        public MinimumOfChipsMovesToEquilibrium() {
            chips = null;
            average = 0;
            taskMatrix = null;
        }

        public MinimumOfChipsMovesToEquilibrium(int[] chips) {
            this.chips = chips;
            average = 0;
            taskMatrix = null;
        }

        void CalculateAverage() {
            for (int i = 0; i < chips.Length; i++) {
                average += chips[i];
            }
            average /= chips.Length;
        }

        void CreateMatrices() {
            List<int> excess = new List<int>();
            List<int> excessIndexes = new List<int>();

            List<int> deficit = new List<int>();
            List<int> deficitIndexes = new List<int>();

            for (int i = 0; i < chips.Length; i++) {
                if (chips[i] < average) {
                    deficit.Add(chips[i]);
                    deficitIndexes.Add(i);
                } else if (chips[i] > average) {
                    excess.Add(chips[i]);
                    excessIndexes.Add(i);
                }
            }

            if (excess.Count == 0) {
                return;
            }

            n = 0;
            for (int i = 0; i < excess.Count; i++) {
                n += excess[i] - average;
            }

            taskMatrix = new int[n, n];
            matrix = new int[n, n];

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
        }

        void InitializeFields() {
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
        }

        void KuhnAlg() {
            rows.Clear();
            columns.Clear();
            bool[] selectedRows = new bool[n];
            bool[] selectedColumns = new bool[n];

            for (int i = 0; i < n; i++) {
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
        }

        void RecalculateMatrix() {
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

        public int MinimumNumberOfChipMovesToEquilibrium() {

            if (chips == null) {
                return 0;
            }

            CalculateAverage();

            CreateMatrices();

            if (n == 0) {
                return 0;
            }

            rows = new HashSet<int>();
            columns = new HashSet<int>();
            g = new List<int>[n];
            for (int i = 0; i < n; i++) {
                g[i] = new List<int>();
            }

            mt = new int[n];
            used = new bool[n];
            used1 = new bool[n];

            stack = new int[n, 2];
            pairs = 0;

            while (pairs != n) {
                pairs = 0;

                InitializeFields();

                if (pairs == n) {
                    continue;
                }

                KuhnAlg();

                if (pairs == n) {
                    continue;
                }

                RecalculateMatrix();

            }

            int steps = 0;

            for (int i = 0; i < mt.Length; i++) {
                steps += taskMatrix[mt[i], i];
            }

            return steps;
        }

    }
}
