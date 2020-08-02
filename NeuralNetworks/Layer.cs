using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworks
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }
        public int Count => Neurons?.Count ?? 0;

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
            if (neurons == null)
            {
                throw new ArgumentNullException("Список нейронов" +
                    " не может быть равным null!", nameof(neurons));
            }

            if (!neurons.All(neuron => neuron.NeuronType == type))
            {
                throw new ArgumentException("Тип каждого нейрона должен" +
                    " соответствовать указанному типу!", nameof(type));
            }

            Neurons = neurons;
        }

        public List<double> GetSignals()
        {
            return Neurons.Select(neuron => neuron.Output).ToList();
        }
    }
}