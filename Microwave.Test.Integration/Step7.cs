using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    public class Step7
    {
        private Door door;
        private UserInterface userInterface;
        private Button powerButton;
        private Button startCancelButton;
        private Button timeButton;
        private CookController cookController;
        private Display uutDisplay;
        private Light light;
        private Timer timer;
        private PowerTube powerTube;
        private IOutput fakeOutput;

        [SetUp]
        public void Setup()
        {
            door = new Door();
            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();
            timer = new Timer();
            fakeOutput = Substitute.For<IOutput>();
            powerTube = new PowerTube(fakeOutput);
            uutDisplay = new Display(fakeOutput);
            light = new Light(fakeOutput);
            cookController = new CookController(timer, uutDisplay, powerTube);
            userInterface = new UserInterface(powerButton,
                timeButton,
                startCancelButton,
                door,
                uutDisplay,
                light,
                cookController);
            cookController.UI = userInterface;

        }
        [Test]
        public void OnCancelStartPressed_SetTimeState_DisplayReceivedShowTime()
        {
            //!!!!!Virkede ikke inden fejl i timeren blev rettet!!!!!

            //Arrange
            powerButton.Press();
            timeButton.Press();
            //Act
            startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:00")));

        }
        [Test]
        public void Waiting60Seconds_CookingState_DisplayReceivedClear()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            //Act
            Thread.Sleep(61000);
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("cleared")));

        }

        [Test]
        public void OnCancelStartPressed_CookingState_DisplayReceivedClear()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            //Act
            startCancelButton.Press();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }
        [Test]
        public void OnDoorOpened_CookingState_DisplayReceivedClear()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            //Act
            door.Open();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }
    }
}