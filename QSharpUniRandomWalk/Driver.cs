﻿//Copyright (c) Daniel Strano 2017. All rights reserved.
//Licensed under the GNU General Public License V3.
//See LICENSE.md in the project root or https://www.gnu.org/licenses/gpl-3.0.en.html for details.


using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using System;

namespace Quantum.QSharpUniRandomWalk
{
    ////////////////////////////////////////////////////////////////////////////
    // Introduction ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    //  This is a simple example of quantum mechanics simulation in quantum
    //computational logic. It is essentially a unidirectional binary quantum
    //random walk algorithm, from a positive starting point, heading toward zero.

    //  We assume a fixed length time step. During each time step, we step through
    //an equal superposition of either standing still or taking one fixed length
    //step from our current position toward our fixed destination.

    //  This is equivalent to a physical body having a 50% chance of emitting a
    //fixed unit of energy in a fixed time, in a pure quantum state. Hence, it
    //might be considered a simple quantum mechanics simulation.

    class Driver
    {
        static void Main(string[] args)
        {
            const int maxTrials = 1000;
            const int planckTimes = 100;
            const int planckMassPowerOfTwo = 8;
            double[] masses = new double[maxTrials];
            double totalMass = 0.0;
            

			//We instantiate a quantum simulator, and require that qubits are "cleaned" before release:
            using (var sim = new QuantumSimulator(throwOnReleasingQubitsNotInZeroState: true))
            {
				//Our ultimate result is probabilistic, so we repeat the simulation many times and
				// average the result. We also want the standard deviation, so we save all the results.
                for (int trial = 0; trial < maxTrials; trial++)
                {
                    masses[trial] = SimulateWalk.Run(sim, planckMassPowerOfTwo, planckTimes).Result;
                    totalMass += masses[trial];
                }
            }

            double averageMass = totalMass / maxTrials;
            double sqrDiff = 0.0;
            double diff;
			//Calculate the standard deviation of the simulation trials:
            for (int trial = 0; trial < maxTrials; trial++)
            {
                diff = masses[trial] - averageMass;
                sqrDiff += diff * diff;
            }
            double stdDev = Math.Sqrt(sqrDiff / (maxTrials - 1));

			//Output the results:
            Console.WriteLine($"Trials:{maxTrials}");
            Console.WriteLine($"Starting Point:{Math.Pow(2, planckMassPowerOfTwo) - 1}");
            Console.WriteLine($"Time units passed:{planckTimes}");
            Console.WriteLine($"Average distance left:{averageMass}");
            Console.WriteLine($"Distance left std. dev.:{stdDev}");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}