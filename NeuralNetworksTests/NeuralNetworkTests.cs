using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks.Model;
using System.Collections.Generic;

namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeedForwardTest()
        {
            var topology = new Topology(4, 1, 2);
            var neuralNetwork = new NeuralNetwork(topology);

            neuralNetwork.Layers[1].Neurons[0].SetWeights(0.5, -0.1, 0.3, -0.1);
            neuralNetwork.Layers[1].Neurons[1].SetWeights(0.1, -0.3, 0.7, -0.3);
            neuralNetwork.Layers[2].Neurons[0].SetWeights(1.2, 0.8);

            var signals = new List<double>() { 1, 0, 0, 0 };
            neuralNetwork.FeedForward(signals);
        }
    }
}