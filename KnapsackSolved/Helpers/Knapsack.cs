using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnapsackSolved.Helpers
{
    public class Knapsack
    {
        public int MaxCost { get; set; }
        public int PopSize { get; set; }
        public double CrossoverProb { get; set; }
        public double MutationProb { get; set; }
        public int Terminate { get; set; }
        public int Generation { get; set; }
        public List<Item> Items { get; set; }

        public Knapsack(int maxCost, int popSize, double crossoverProb, double mutationProb, int terminate, int generation, List<Item> items)
        {
            MaxCost = maxCost;
            PopSize = popSize;
            CrossoverProb = crossoverProb;
            MutationProb = mutationProb;
            Terminate = terminate;
            Generation = generation;
            Items = items;
        }

        public int Solver()
        {
            return MaxCost;
        }
    }
}
