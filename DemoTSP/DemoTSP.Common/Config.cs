using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTSP.Common
{
    public static class Config
    {
        public const int CITIES = 20;
        public const int POPULATION_SIZE = 1000;
        public const int MAP_SIZE = 256;
        public const double CROSSOVER_PROBABILITY = 0.9;
        public const double MUTATION_PROBABILITY = 0.1;
        public const int MAX_SAME_SOLUTION = 25;
    }
}
