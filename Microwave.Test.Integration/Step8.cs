using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step8
    {
        private Door myDoor;
        private UserInterface userInterface;
        private Button myPowerButton;
        private Button myCancelStartButton;
        private Button myTimeButton;
        private Timer myTimer;
        private CookController myCookController;
        private Display myDisplay;
        private Light myLight;
        private Output myOutput;
        private PowerTube uutPowerTube;
        private StringWriter stringWriter;

        private void clearStringWriter()
        {
            StringBuilder sb = stringWriter.GetStringBuilder();
            sb.Remove(0, sb.Length);
        }

        [SetUp]
        public void Setup()
        {
            myDoor = new Door();
            myPowerButton = new Button();
            myCancelStartButton = new Button();
            myTimeButton = new Button();
            myTimer = new Timer();
            myOutput = new Output();
            myDisplay = new Display(myOutput);
            myLight = new Light(myOutput);
            uutPowerTube = new PowerTube(myOutput);
            myCookController = new CookController(myTimer, myDisplay, uutPowerTube);
            userInterface = new UserInterface(myPowerButton,
                myTimeButton,
                myCancelStartButton,
                myDoor,
                myDisplay,
                myLight,
                myCookController);

            stringWriter = new StringWriter();
            System.Console.SetOut(stringWriter);

        }

        [Test]
        public void OutputLine_OnDoorOpened_OutputIsCorrect()
        {
            //Arrange
            string expected = "Light is turned on\r\n";
            //Act 
            myDoor.Open();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OutputLine_OnDoorClosed_OutputIsCorrect()
        {
            //Arrange
            string expected = "Light is turned off\r\n";
            myDoor.Open();
            clearStringWriter();
            //Act 
            myDoor.Close();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected,actual);
        }

        [Test]
        public void OutputLine_OnPowerPressed_OutputIsCorrect()
        {
            //Arrange
            string expected = "Display shows: 50 W\r\n";
            //Act
            myPowerButton.Press();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OutputLine_OnTimePressed_OutputIsCorrect()
        {
            //Arrange
            string expected = "Display shows: 01:00\r\n";
            myPowerButton.Press();
            clearStringWriter();
            //Act
            myTimeButton.Press();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void OutputLine_OnStartCancelPressed_PowerTubeTurnOn_OutputIsCorrect()
        {
            //Arrange
            string expected = "Light is turned on\r\nPowerTube works with 50\r\n";
            myPowerButton.Press(); //power 50W
            myTimeButton.Press();
            clearStringWriter();
            //Act
            myCancelStartButton.Press();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OutputLine_OnStartCancelPressed_PowerTubeOff_OutputIsCorrect()
        {
            //Arrange
            string expected = "PowerTube turned off\r\nDisplay cleared\r\n";
            myPowerButton.Press();
            myTimeButton.Press();
            myCancelStartButton.Press();
            clearStringWriter();
            //Act
            myDoor.Open();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OutputLine_OnStartCancelPressed_Clear_OutputIsCorrect()
        {
            //Arrange
            string expected = "PowerTube turned off\r\nLight is turned off\r\nDisplay cleared\r\n";
            myPowerButton.Press(); 
            myTimeButton.Press();
            myCancelStartButton.Press();
            clearStringWriter();
            //Act
            myCancelStartButton.Press();
            //Assert
            string actual = stringWriter.ToString();
            Assert.AreEqual(expected, actual);
        }

    }
}