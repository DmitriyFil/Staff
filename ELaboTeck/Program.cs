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

      
    }
}
