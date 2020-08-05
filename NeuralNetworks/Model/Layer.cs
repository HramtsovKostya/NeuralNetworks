using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworks.Model
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }
        public int NeuronCount => Neurons?.Count ?? 0;
        public NeuronType Type { get; set; }

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Hidden)
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
            Type = type;
        }

        public List<double> GetSignals()
        {
            return Neurons.Select(neuron => neuron.Output).ToList();
        }

        public override string ToString() => Type.ToString();
    }
}