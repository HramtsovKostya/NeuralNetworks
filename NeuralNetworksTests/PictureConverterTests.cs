using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeuralNetworks.Model.Tests
{
    [TestClass()]
    public class PictureConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var converter = new PictureConverter();
            var inputs = converter.Convert(@"Images\Parasitized.png");
            converter.Save(@"Images\image1.png", inputs);

            inputs = converter.Convert(@"Images\Uninfected.png");
            converter.Save(@"Images\image2.png", inputs);
        }
    }
}