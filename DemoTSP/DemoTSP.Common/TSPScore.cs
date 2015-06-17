using Encog.ML;
using Encog.ML.Genetic.Genome;
using Encog.Neural.Networks.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTSP.Common
{
    public class TSPScore : ICalculateScore
    {
        private readonly City[] cities;

        public TSPScore(City[] cities)
        {
            this.cities = cities;
        }

        #region ICalculateScore Members

        public double CalculateScore(IMLMethod phenotype)
        {
            // path length
            double result = 0.0;
            IntegerArrayGenome genome = (IntegerArrayGenome)phenotype;
            int[] path = genome.Data;

            for (int i = 0; i < cities.Length - 1; i++)
            {
                City city1 = cities[path[i]];
                City city2 = cities[path[i + 1]];
                double dist = city1.Proximity(city2);
                result += dist;
            }

            return result;
        }

        public bool RequireSingleThreaded
        {
            get { return false; }
        }

        public bool ShouldMinimize
        {
            get { return true; }
        }

        #endregion
    }
}
