using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step4
    {
        private Door door;
        private UserInterface userInterface;
        private Button powerButton;
        private Button cancelStartButton;
        private Button timeButton;
        private ICookController cookController;
        private IPowerTube fakePowerTube;
        private ITimer fakeTimer;
        private IOutput fakeOutput;
        private IDisplay uutDisplay;
        private ILight uutLight;

        [SetUp]
        public void Setup()
        {
            door = new Door();
            powerButton = new Button();
            cancelStartButton = new Button();
            timeButton = new Button();
            fakePowerTube = Substitute.For<IPowerTube>();
            fakeTimer = Substitute.For<ITimer>();
            fakeOutput = Substitute.For<IOutput>();
            uutDisplay = new Display(fakeOutput);
            uutLight = new Light(fakeOutput); 
            cookController = new CookController(fakeTimer,uutDisplay,fakePowerTube);
            userInterface = new UserInterface(powerButton,
                timeButton,
                cancelStartButton,
                door,
                uutDisplay,
                uutLight,
                cookController);
           
        }

        [Test]
        public void OnTimePressed_DisplayReceivedShowTime()
        {
            //Arrange
            powerButton.Press();
            
            //Act
            timeButton.Press();

            //Assert
            fakeOutput.Received(2).OutputLine(Arg.Any<string>());
            
        }

        [Test]
        public void OnsPowerPressed_DisplayReceivedShowPower()
        {
            //Arrange
            //Act
            powerButton.Press();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Any<string>());

        }
        [Test]
        public void OnCancelStartPressed_DisplayReceivedShowTime()
        {
            //!!!!!Virkede ikke inden fejl i timeren blev rettet!!!!!

            //Arrange
            powerButton.Press();
            timeButton.Press();
            //Act
            cancelStartButton.Press();

            //Assert
            fakeOutput.Received().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void Clear()
        {
            //Arrange
            //Act 
            //Assert
        }

        [Test]
        void TurnOn()
        {
            //Arrange
            cancelStartButton.Press();
            //Act 
            //Assert
        }

        [Test]
        void TurnOff()
        {
            //Arrange
            //Act 
            //Assert
        }
    }
}