using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NeuralNetworks.Model.Tests
{
    [TestClass()]
    public class PictureConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var converter = new PictureConverter();
            const string PATH = @"..\..\Images\";

            var inputs = converter.Convert(PATH + "Parasitized.png");
            converter.Save(PATH + "image1.png", inputs);

            inputs = converter.Convert(PATH + "Uninfected.png");
            converter.Save(PATH + "image2.png", inputs);
        }
    }
}