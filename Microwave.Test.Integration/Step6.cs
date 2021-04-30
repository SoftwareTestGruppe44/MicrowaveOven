using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step6
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
        private IOutput fakeOutput;
        private PowerTube uutPowerTube;

        [SetUp]
        public void Setup()
        {
            myDoor = new Door();
            myPowerButton = new Button();
            myCancelStartButton = new Button();
            myTimeButton = new Button();
            myTimer = new Timer();
            fakeOutput = Substitute.For<IOutput>();
            myDisplay = new Display(fakeOutput);
            myLight = new Light(fakeOutput);
            uutPowerTube = new PowerTube(fakeOutput);
            myCookController = new CookController(myTimer, myDisplay, uutPowerTube);
            userInterface = new UserInterface(myPowerButton, 
                myTimeButton, 
                myCancelStartButton, 
                myDoor, 
                myDisplay,
                myLight, 
                myCookController);

            myCookController.UI = userInterface;
        }

        [TestCase(1, 50)]
        [TestCase(2, 100)]
        [TestCase(12, 600)]
        [TestCase(14, 700)]
        [TestCase(15, 50)]
        public void TurnOn_StartCancelPressedXTimes_OutputIsCalledWithCorrectValue(int numberOfButtonPress, int power)
        {
             //Arrange
             for (int i = 0; i < numberOfButtonPress; i++)
             {
                myPowerButton.Press();
             }
             myTimeButton.Press();

             //Act
             myCancelStartButton.Press();

             //Assert
             fakeOutput.Received(1).OutputLine($"PowerTube works with {power}");
        }

        [Test]
        public void TurnOff_StartCancelPressed_OutputIsCalledWithCorrectOutputAfter61Seconds()
        {
            //Arrange
            myPowerButton.Press();
            myTimeButton.Press();

            //Act
            myCancelStartButton.Press();
            Thread.Sleep(61000);

            //Assert
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void TurnOff_StartCancelPressedDuringCooking_OutputCalledWithCorrectValue()
        {
            //Arrange
            myPowerButton.Press();
            myTimeButton.Press();
            myCancelStartButton.Press();

            //Act
            myCancelStartButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void TurnOff_DoorOpenedDuringCooking_OutputCalledWithCorrectValue()
        {
            //Arrange
            myPowerButton.Press();
            myTimeButton.Press();
            myCancelStartButton.Press();

            //Act
            myDoor.Open();

            //Assert
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }
    }
}