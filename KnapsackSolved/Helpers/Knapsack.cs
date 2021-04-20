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
        public int generationCounter { get; set; }
        public int Generation { get; set; }
        public List<Item> Items { get; set; }


        public Knapsack(int maxCost, int popSize, double crossoverProb, double mutationProb, int terminate, int generation, List<Item> items)
        {
            MaxCost = maxCost;
            PopSize = popSize;
            CrossoverProb = crossoverProb;
            MutationProb = mutationProb;
            generationCounter = terminate;
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
            string individuo;
            string outroIndividuo;
            char[] splitStr1;
            char[] splitStr2;
            int[] selectedItemsArray;                          
            int totalValue;                                 
            int totalCost;

            //Continue fazendo o loop até que o valor de término seja igual ao valor de geração
            while (generationCounter < Generation)
            {
                //Gerar os primeiros cromossomos da população aleatória
                if (generationCounter == 1)
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

                //Calcula o custo total e o valor total de cada cromossomo
                for (int i = 0; i < PopSize; i++)
                {
                    individuo = population[i];//string
                    splitStr1 = individuo.ToCharArray();//chararray
                    selectedItemsArray = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));//intarray
                    totalValue = 0;
                    totalCost = 0;

                    //Verifica o valor e o custo do Item com base na posição do 1 dentro do array
                    for (int m = 0; m < selectedItemsArray.Length; m++)
                    {
                        if (selectedItemsArray[m] == 1)
                        {
                            totalCost += Items[m].Cost;
                            totalValue += Items[m].Value;
                        }
                    }
                    //Adiciona nos arrays de custo e valor
                    chromosomeCost[i] = totalCost;
                    chromosomeValue[i] = totalValue;
                }

                int sumCost = chromosomeCost.Sum();
                int sumValue = chromosomeValue.Sum();

                //Roda de roleta para selecionar 2 cromossomos da população
                int[] roletaArray = new int[110];
                float percent;
                int totalPercent = 0;
                int positionOfChromossome = 0;
                int lastPosition = 0;

                //Cria um valor porcentual para cada individuo e um array que representa todos esses porcentuais juntos
                for (int i = 0; i < population.Length; i++)
                {
                    percent = ((float)chromosomeCost[i] / (float)sumCost) * 100;
                    percent = Convert.ToInt64(percent);
                    for (int j = 0; j < percent; j++)
                    {
                        roletaArray[lastPosition] = positionOfChromossome;
                        lastPosition++;
                    }
                    positionOfChromossome++;
                    totalPercent += Convert.ToInt32(percent);
                }

                //Pega dois individuos aleatórios dentro do array com base nas porcentagens
                int randomNumber1 = random.Next(0, totalPercent);
                int randomNumber2 = random.Next(0, totalPercent);
                string[] roleta = new string[2];
                int individuo1 = roletaArray[randomNumber1];
                int individuo2 = roletaArray[randomNumber2];
                roleta[0] = population[individuo1];
                roleta[1] = population[individuo2];

                //Mutação de cromossomos
                double randomDoubleNumber1 = random.NextDouble() * (1.000 - 0.000) + 0.000;
                double mutationOccurs = Math.Round(randomDoubleNumber1, 3);
                int[] genome1;
                int[] genome2;
                if (mutationOccurs <= MutationProb)
                {
                    //escolhe um dos dois
                    int choosenChromossome = random.Next(0, 2);
                    individuo = roleta[choosenChromossome];
                    splitStr1 = individuo.ToCharArray();
                    genome1 = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
                    int mutationAt = random.Next(0, 3);

                    if (genome1[mutationAt] == 0)
                    {
                        genome1[mutationAt] = 1;
                    }
                    else if (genome1[mutationAt] == 1)
                    {
                        genome1[mutationAt] = 0;
                    }

                    //Recalcula o valor e o custo daquele individuo (genoma)
                    totalValue = 0;
                    totalCost = 0;
                    for (int m = 0; m < genome1.Length; m++)
                    {
                        if (genome1[m] == 1)
                        {
                            totalCost += Items[m].Cost;
                            totalValue += Items[m].Value;
                        }
                    }
                    //Seta dentro dos arrays o custo e o valor que foram calculados
                    if (choosenChromossome == 0)
                    {
                        chromosomeCost[individuo1] = totalCost;
                        chromosomeValue[individuo1] = totalValue;
                    }
                    else if (choosenChromossome == 1)
                    {
                        chromosomeCost[individuo2] = totalCost;
                        chromosomeValue[individuo2] = totalValue;
                    }
                    //Seta no array de roleta o novo individuo com o genoma mutado
                    individuo = string.Join("", genome1);
                    roleta[choosenChromossome] = individuo;
                }

                //Ocorre crossover
                double randomDoubleNumber2 = random.NextDouble() * (1.00 - 0.00) + 0.00;
                double crossoverOccurs = Math.Round(randomDoubleNumber2, 2);
                if (crossoverOccurs <= CrossoverProb)
                {
                    individuo = roleta[0];
                    outroIndividuo = roleta[1];
                    splitStr1 = individuo.ToCharArray();
                    splitStr2 = outroIndividuo.ToCharArray();
                    genome1 = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
                    genome2 = Array.ConvertAll(splitStr2, c => (int)Char.GetNumericValue(c));
                    int crossoverEnd = random.Next(0, 3);

                    for (int i = 0; i <= crossoverEnd; i++)
                    {
                        //Inverte os genes até o final do crossover entre os dois selecionados pela roleta
                        int getGene1 = genome1[i];
                        int getGene2 = genome2[i];
                        genome1[i] = getGene2;
                        genome2[i] = getGene1;
                    }

                    //Seta o valor e o custo dos novos individuos que sofreram o crossover
                    totalValue = 0;
                    totalCost = 0;
                    for (int m = 0; m < genome1.Length; m++)
                    {
                        if (genome1[m] == 1)
                        {
                            totalCost += Items[m].Cost;
                            totalValue += Items[m].Value;
                        }
                        chromosomeCost[individuo1] = totalCost;
                        chromosomeValue[individuo1] = totalValue;
                    }
                    totalValue = 0;
                    totalCost = 0;
                    for (int m = 0; m < genome1.Length; m++)
                    {
                        if (genome2[m] == 1)
                        {
                            totalCost += Items[m].Cost;
                            totalValue += Items[m].Value;
                        }
                        chromosomeCost[individuo2] = totalCost;
                        chromosomeValue[individuo2] = totalValue;
                    }

                    individuo = string.Join("", genome1);
                    outroIndividuo = string.Join("", genome2);
                    roleta[0] = individuo;
                    roleta[1] = outroIndividuo;
                }

                FitnessCalculations(population, chromosomeCost, chromosomeValue, roleta, individuo1, individuo2);

                generationCounter++;
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
            individuo = population[maxIndex];
            splitStr1 = individuo.ToCharArray();
            selectedItemsArray = Array.ConvertAll(splitStr1, c => (int)Char.GetNumericValue(c));
            totalValue = 0;
            totalCost = 0;
            List<Item> selectedItems = new List<Item>();

            for (int m = 0; m < selectedItemsArray.Length; m++)
            {
                if (selectedItemsArray[m] == 1)
                {
                    selectedItems.Add(Items[m]);
                    totalCost = totalCost + Items[m].Cost;
                    totalValue = totalValue + Items[m].Value;
                }
            }

            return new Result()
            {
                Generation = generationCounter,
                Items = selectedItems,
                TotalCost = totalCost,
                TotalValue = totalValue,
            };
        }

        private void FitnessCalculations(string[] population, int[] chromosomeCost, int[] chromosomeValue, string[] roleta, int individuo1, int individuo2)
        {
            //Função de fitness. Selecione o cromossomo de maior valor enquanto for inferior ao Custo Máximo da mochila.
            if (chromosomeCost[individuo1] > MaxCost)
            {
                if (chromosomeCost[individuo2] <= MaxCost)
                {
                    for (int i = 0; i < population.Length; i++)
                    {
                        if (chromosomeValue[i] < chromosomeValue[individuo2])
                        {
                            population[i] = roleta[1];
                            chromosomeCost[i] = chromosomeCost[individuo2];
                            chromosomeValue[i] = chromosomeValue[individuo2];
                            break;
                        }
                        else if (chromosomeCost[i] > MaxCost)
                        {
                            population[i] = roleta[1];
                            chromosomeCost[i] = chromosomeCost[individuo2];
                            chromosomeValue[i] = chromosomeValue[individuo2];
                            break;
                        }
                    }
                }
            }
            else if (chromosomeCost[individuo2] > MaxCost)
            {
                if (chromosomeCost[individuo1] <= MaxCost)
                {
                    for (int i = 0; i < population.Length; i++)
                    {
                        if (chromosomeValue[i] < chromosomeValue[individuo1])
                        {
                            population[i] = roleta[0];
                            chromosomeCost[i] = chromosomeCost[individuo1];
                            chromosomeValue[i] = chromosomeValue[individuo1];
                            break;
                        }
                        else if (chromosomeCost[i] > MaxCost)
                        {
                            population[i] = roleta[0];
                            chromosomeCost[i] = chromosomeCost[individuo1];
                            chromosomeValue[i] = chromosomeValue[individuo1];
                            break;
                        }
                    }
                }
            }
            else if (chromosomeValue[individuo1] >= chromosomeValue[individuo2])
            {
                for (int i = 0; i < population.Length; i++)
                {
                    if (chromosomeValue[i] < chromosomeValue[individuo1])
                    {
                        population[i] = roleta[0];
                        chromosomeCost[i] = chromosomeCost[individuo1];
                        chromosomeValue[i] = chromosomeValue[individuo1];
                        break;
                    }
                    else if (chromosomeCost[i] > MaxCost)
                    {
                        population[i] = roleta[0];
                        chromosomeCost[i] = chromosomeCost[individuo1];
                        chromosomeValue[i] = chromosomeValue[individuo1];
                        break;
                    }
                }
            }
            else if (chromosomeValue[individuo2] >= chromosomeValue[individuo1])
            {
                for (int i = 0; i < population.Length; i++)
                {
                    if (chromosomeValue[i] < chromosomeValue[individuo2])
                    {
                        population[i] = roleta[1];
                        chromosomeCost[i] = chromosomeCost[individuo2];
                        chromosomeValue[i] = chromosomeValue[individuo2];
                        break;
                    }
                    else if (chromosomeCost[i] > MaxCost)
                    {
                        population[i] = roleta[1];
                        chromosomeCost[i] = chromosomeCost[individuo2];
                        chromosomeValue[i] = chromosomeValue[individuo2];
                        break;
                    }
                }
            }
        }
    }
}
