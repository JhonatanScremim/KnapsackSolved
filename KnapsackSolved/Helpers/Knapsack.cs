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

        public Result Solver()
        {
            string[] population = new string[PopSize];  
            var chars = "01";
            var stringChars = new char[Items.Count];
            var random = new Random();
            int[] chromosomeCost = new int[PopSize];        
            int[] chromosomeValue = new int[PopSize];       
            string str1;
            string str2;
            char[] splitStr1;
            char[] splitStr2;
            int[] splitChromosome;                          
            int totalValue;                                 
            int totalCost;                                  

                   
            while (Terminate <= Generation)
            {
                if (Terminate == 1)
                {
                    for (int j = 0; j < PopSize; j++)
                    {
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }
                        population[j] = new String(stringChars);
                    }
                }

                for (int i = 0; i < PopSize; i++)
                {
                    str1 = population[i];
                    splitStr1 = str1.ToCharArray();
                    splitChromosome = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
                    totalValue = 0;
                    totalCost = 0;

                    for (int m = 0; m < splitChromosome.Length; m++)
                    {
                        if (splitChromosome[m] == 1)
                        {
                            totalCost = totalCost + Items[m].Cost;
                            totalValue = totalValue + Items[m].Value;
                        }
                    }
                    chromosomeCost[i] = totalCost;
                    chromosomeValue[i] = totalValue;
                }

                int sumCost = chromosomeCost.Sum();
                int sumValue = chromosomeValue.Sum();

                int[] array = new int[110];
                float percent;                          
                int totalPercent = 0;                   
                int q = 0;
                int s = 0;
                for (int c = 0; c < population.Length; c++)
                {
                    percent = ((float)chromosomeCost[c] / (float)sumCost) * 100;
                    percent = Convert.ToInt64(percent);
                    for (int r = 0; r < percent; r++)
                    {
                        array[s] = q;
                        s++;
                    }
                    q++;
                    totalPercent = totalPercent + Convert.ToInt32(percent);
                }

                int randomNumber1 = random.Next(0, totalPercent);                       
                int randomNumber2 = random.Next(0, totalPercent);                       
                                                                                                                                                                       
                string[] roulette = new string[2];                                      

                int Chromosome1 = array[randomNumber1];                                 
                int Chromosome2 = array[randomNumber2];                                 
                roulette[0] = population[Chromosome1];                                  
                roulette[1] = population[Chromosome2];                                  

                double randomDoubleNumber1 = random.NextDouble() * (1.000 - 0.000) + 0.000;
                double mutationOccurs = Math.Round(randomDoubleNumber1, 3);                    
                int[] genome1;
                int[] genome2;
                if (mutationOccurs <= MutationProb)
                {
                    int ranChooseChromosome = random.Next(0, 2);

                    str1 = roulette[ranChooseChromosome];
                    splitStr1 = str1.ToCharArray();
                    genome1 = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
                    int ranMutateAt = random.Next(0, 3);

                    if (genome1[ranMutateAt] == 0)
                    {
                        genome1[ranMutateAt] = 1;
                    }
                    else if (genome1[ranMutateAt] == 1)
                    {
                        genome1[ranMutateAt] = 0;
                    }

                    totalValue = 0;
                    totalCost = 0;
                    for (int m = 0; m < genome1.Length; m++)
                    {
                        if (genome1[m] == 1)
                        {
                            totalCost = totalCost + Items[m].Cost;
                            totalValue = totalValue + Items[m].Value;
                        }
                    }
                    if (ranChooseChromosome == 0)
                    {
                        chromosomeCost[Chromosome1] = totalCost;
                        chromosomeValue[Chromosome1] = totalValue;
                    }
                    else if (ranChooseChromosome == 1)
                    {
                        chromosomeCost[Chromosome2] = totalCost;
                        chromosomeValue[Chromosome2] = totalValue;
                    }

                    str1 = string.Join("", genome1);
                    roulette[ranChooseChromosome] = str1;                         
                }

                double randomDoubleNumber2 = random.NextDouble() * (1.00 - 0.00) + 0.00;
                double crossoverOccurs = Math.Round(randomDoubleNumber2, 2);                   
                if (crossoverOccurs <= CrossoverProb)
                {
                    str1 = roulette[0];
                    str2 = roulette[1];
                    splitStr1 = str1.ToCharArray();
                    splitStr2 = str2.ToCharArray();
                    genome1 = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
                    genome2 = Array.ConvertAll(splitStr2, c => (int)Char.GetNumericValue(c));
                    int ranCrossoverFrom = random.Next(0, 3);

                    for (int i = 0; i <= ranCrossoverFrom; i++)
                    {
                        if(i == 3)
                        {
                            int a = 0;
                        }
                        int getGene1 = genome1[i];
                        int getGene2 = genome2[i];
                        genome1[i] = getGene2;
                        genome2[i] = getGene1;
                    }
                    totalValue = 0;
                    totalCost = 0;
                    for (int m = 0; m < genome1.Length; m++)
                    {
                        if (genome1[m] == 1)
                        {
                            totalCost = totalCost + Items[m].Cost;
                            totalValue = totalValue + Items[m].Value;
                        }
                        chromosomeCost[Chromosome1] = totalCost;
                        chromosomeValue[Chromosome1] = totalValue;
                    }
                    totalValue = 0;
                    totalCost = 0;
                    for (int m = 0; m < genome1.Length; m++)
                    {
                        if (genome2[m] == 1)
                        {
                            totalCost = totalCost + Items[m].Cost;
                            totalValue = totalValue + Items[m].Value;
                        }
                        chromosomeCost[Chromosome2] = totalCost;
                        chromosomeValue[Chromosome2] = totalValue;
                    }

                    str1 = string.Join("", genome1);
                    str2 = string.Join("", genome2);
                    roulette[0] = str1;
                    roulette[1] = str2;
                }

                if (chromosomeCost[Chromosome1] > MaxCost)
                {
                    if (chromosomeCost[Chromosome2] <= MaxCost)
                    {
                        for (int i = 0; i < population.Length; i++)
                        {
                            if (chromosomeValue[i] < chromosomeValue[Chromosome2])
                            {
                                population[i] = roulette[1];
                                chromosomeCost[i] = chromosomeCost[Chromosome2];
                                chromosomeValue[i] = chromosomeValue[Chromosome2];
                                break;
                            }
                            else if (chromosomeCost[i] > MaxCost)
                            {
                                population[i] = roulette[1];
                                chromosomeCost[i] = chromosomeCost[Chromosome2];
                                chromosomeValue[i] = chromosomeValue[Chromosome2];
                                break;
                            }
                        }
                    }
                }
                else if (chromosomeCost[Chromosome2] > MaxCost)
                {
                    if (chromosomeCost[Chromosome1] <= MaxCost)
                    {
                        for (int i = 0; i < population.Length; i++)
                        {
                            if (chromosomeValue[i] < chromosomeValue[Chromosome1])
                            {
                                population[i] = roulette[0];
                                chromosomeCost[i] = chromosomeCost[Chromosome1];
                                chromosomeValue[i] = chromosomeValue[Chromosome1];
                                break;
                            }
                            else if (chromosomeCost[i] > MaxCost)
                            {
                                population[i] = roulette[0];
                                chromosomeCost[i] = chromosomeCost[Chromosome1];
                                chromosomeValue[i] = chromosomeValue[Chromosome1];
                                break;
                            }
                        }
                    }
                }
                else if (chromosomeValue[Chromosome1] >= chromosomeValue[Chromosome2])
                {
                    for (int i = 0; i < population.Length; i++)
                    {
                        if (chromosomeValue[i] < chromosomeValue[Chromosome1])
                        {
                            population[i] = roulette[0];
                            chromosomeCost[i] = chromosomeCost[Chromosome1];
                            chromosomeValue[i] = chromosomeValue[Chromosome1];
                            break;
                        }
                        else if (chromosomeCost[i] > MaxCost)
                        {
                            population[i] = roulette[0];
                            chromosomeCost[i] = chromosomeCost[Chromosome1];
                            chromosomeValue[i] = chromosomeValue[Chromosome1];
                            break;
                        }
                    }
                }
                else if (chromosomeValue[Chromosome2] >= chromosomeValue[Chromosome1])
                {
                    for (int i = 0; i < population.Length; i++)
                    {
                        if (chromosomeValue[i] < chromosomeValue[Chromosome2])
                        {
                            population[i] = roulette[1];
                            chromosomeCost[i] = chromosomeCost[Chromosome2];
                            chromosomeValue[i] = chromosomeValue[Chromosome2];
                            break;
                        }
                        else if (chromosomeCost[i] > MaxCost)
                        {
                            population[i] = roulette[1];
                            chromosomeCost[i] = chromosomeCost[Chromosome2];
                            chromosomeValue[i] = chromosomeValue[Chromosome2];
                            break;
                        }
                    }
                }
                Terminate = Terminate + 1;
            }

            for (int i = 0; i < chromosomeCost.Length; i++)
            {
                if (chromosomeCost[i] > MaxCost)
                {
                    chromosomeValue[i] = 0;
                }
            }
            int maxValue = chromosomeValue.Max();
            int maxIndex = chromosomeValue.ToList().IndexOf(maxValue);
            str1 = population[maxIndex];
            splitStr1 = str1.ToCharArray();
            splitChromosome = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
            totalValue = 0;
            totalCost = 0;
            List<Item> selectedItems = new List<Item>();

            for (int m = 0; m < splitChromosome.Length; m++)
            {
                if (splitChromosome[m] == 1)
                {
                    selectedItems.Add(Items[m]);
                    totalCost = totalCost + Items[m].Cost;
                    totalValue = totalValue + Items[m].Value;
                }
            }



            Terminate = 1;
            return new Result()
            {
                Generation = this.Generation,
                Items = selectedItems,
                TotalCost = totalCost,
                TotalValue = totalValue
            };
        }
    }
}
