using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DemoTSP.Common;
using Encog.ML.EA.Population;
using Encog.Neural.Networks.Training;
using Encog.ML.EA.Train;
using Encog.MathUtil;
using Encog.ML.Genetic.Genome;
using Encog.ML.EA.Species;
using Encog.ML.Genetic.Crossover;
using Encog.ML.Genetic.Mutate;

namespace DemoTSP.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        City[] cities;
        IPopulation population;
        ICalculateScore score;
        TrainEA train;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region "Load Citites"
        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            LoadCities();
            txt_status.Text = Config.CITIES.ToString() + " Cities loaded on the map";
        }

        private void LoadCities()
        {
            citiesLine.Points.Clear();
            cities = new City[Config.CITIES];
            for (int i = 0; i < cities.Length; i++)
            {
                var xPos = (int)(ThreadSafeRandom.NextDouble() * Config.MAP_SIZE);
                var yPos = (int)(ThreadSafeRandom.NextDouble() * Config.MAP_SIZE);
                cities[i] = new City(xPos, yPos);
                citiesLine.Points.Add(new Point(xPos, yPos));
            }
        }

        #endregion

        #region "GA Optimizer"
        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            Step1();    // Create Population
            Step2();    // Create Fitness Function
            Step3();    // Create Genetic algorithm trainer
            Step4();    // Training Process
        }

        #endregion

        #region "Step 1: Create Population"

        private void Step1()
        {
            var genomeFactory = new IntegerArrayGenomeFactory(cities.Length);
            population = new BasicPopulation(Config.POPULATION_SIZE, genomeFactory);

            var defaultSpecies = new BasicSpecies();

            for (int i = 0; i < Config.POPULATION_SIZE; i++)
            {
                IntegerArrayGenome genome = RandomGenome();
                defaultSpecies.Members.Add(genome);
            }
            population.Species.Add(defaultSpecies);
        }

        public void Shuffle<T>(T[] array)
        {
            var random = new Random();
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }

        private IntegerArrayGenome RandomGenome()
        {
            int[] citiesNumbers = new int[cities.Length];
            for (int i = 0; i < cities.Length; i++)
            {
                citiesNumbers[i] = i;
            }
            Shuffle(citiesNumbers);

            var result = new IntegerArrayGenome(cities.Length);

            for (int i = 0; i < cities.Length; i++)
            {
                result.Data[i] = citiesNumbers[i];
            }

            return result;
        }

        #endregion

        #region "Step 2: Create Fitness Function"

        private void Step2()
        {
            score = new TSPScore(cities);
        }

        #endregion

        #region "Step 3: Create Genetic Algorithm Trainer

        private void Step3()
        {
            train = new TrainEA(population, score);
            train.AddOperation(Config.CROSSOVER_PROBABILITY, new SpliceNoRepeat(Config.CITIES / 3));
            train.AddOperation(Config.MUTATION_PROBABILITY, new MutateShuffle());
        }

        #endregion

        #region Step 4: Train Using Genetic Algorithm Trainer"

        private void Step4()
        {
            int sameSolutionCount = 0;
            double lastSolution = Double.MaxValue;
            while (sameSolutionCount < Config.MAX_SAME_SOLUTION)
            {
                train.Iteration();
                double currentSolution = train.Error;

                if (Math.Abs(lastSolution - currentSolution) < 1)
                {
                    sameSolutionCount++;
                }
                else
                {
                    sameSolutionCount = 0;
                }

                lastSolution = currentSolution;
            }

            train.FinishTraining();
            txt_status.Text = "Good Solution found...";
            displaySolution();
        }

        private void displaySolution()
        {
            citiesLine.Points.Clear();
            var bestGenome = (IntegerArrayGenome)train.Population.BestGenome;

            foreach (int gene in bestGenome.Data)
            {
                citiesLine.Points.Add(new Point(cities[gene].X, cities[gene].Y));
            }
        }

        #endregion
    }
}
