using System;
using System.Collections.Generic;

namespace NeuralNetworks
{
    public class Neuron
    {
        public List<double> Weigths { get; }
        public NeuronType NeuronType { get; }
        public double Output { get; private set; }

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
        {
            NeuronType = type;
            Weigths = new List<double>();

            for (int i = 0; i < inputCount; i++)
                Weigths.Add(1);
        }

        public double FeedForward(List<double> inputs)
        {
            double sum = 0.0;

            for (int i = 0; i < inputs.Count; i++)
                sum += inputs[i] * Weigths[i];

            Output = Sigmoid(sum);
            return Output;
        }

        private double Sigmoid(double valueX)
        {
            var result = 1.0 / (1.0 + Math.Exp(-valueX));
            return result;
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}