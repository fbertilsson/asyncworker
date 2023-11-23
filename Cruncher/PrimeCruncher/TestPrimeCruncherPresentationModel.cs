using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CruncherModule.PrimeCruncher;
using Moq;
using NUnit.Framework;

namespace PrimeCruncher
{
    // TODO fber: Move this class to a the test assembly for the project. Create one if necessary.
    [TestFixture]
    public class TestPrimeCruncherPresentationModel
    {
        private PrimeCruncherPresentationModel m_Model;
        private Mock<IPrimeCruncherView> m_PrimeCruncherViewMock;

        [SetUp]
        public void SetUp()
        {
            m_PrimeCruncherViewMock = new Mock<IPrimeCruncherView>();
            m_Model = new PrimeCruncherPresentationModel(m_PrimeCruncherViewMock.Object);
        }

        [Test]
        public void AMethodName_WhenSomething_DoesSomething()
        {
            // Arrange
            m_PrimeCruncherViewMock.Setup(view => view.GetBananaColor(It.IsAny<string>)).Returns("Yellow"); // TODO fber:Remove this example line

            // Act
            var result = m_Model.AMethodName(); // TODO fber: Remove this example line

            // Assert
            Assert.AreEqual("LightYellow", result);
            // TODO fber: Remove this example line:
            // m_DialogServiceMock.Verify(
            //    x => x.ShowQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<DialogResults, string>>()));
        }

    }
}
