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
            cookController = new CookController(fakeTimer,uutDisplay,fakePowerTube,userInterface);
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
            //Sætter timeren til 1 min:
            timeButton.Press();

            //Assert
            fakeOutput.Received().OutputLine(Arg.Is<string>(str => str.Contains("01:00")));
        }

        [Test]
        public void OnsPowerPressed_DisplayReceivedShowPower()
        {
            //Arrange
            //Act
            //Power trykkes på 2 gange(50W) så den burde give 100
            powerButton.Press();
            powerButton.Press();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("100 W")));

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
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:00")));

        }

        [Test]
        public void OnCancelStartPressed_CookingState_DisplayReceivedClear()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            cancelStartButton.Press();
            //Act
            cancelStartButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str =>str.Contains("cleared")));
        }

        [Test]
        public void OnDoorOpened_TurnOnLight()
        {
            //Arrange
            //Act 
            door.Open();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorClosed_TurnOffLight()
        {
            //Arrange
            door.Open();
            //Act 
            door.Close();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }


        [Test]
        public void OnStartCancelButtonPressed_ReadyState_TurnOnLight()
        {
            //Arrange
            door.Open();
            door.Close();
            //Act 
            cancelStartButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void CookingIsDone_CookingState_TurnOffLight()
        {
            //Arrange
            door.Open();
            door.Close();

            //Act 
            //starter og venter på den er færdig, så light slukker
            cancelStartButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void OnStartCancelButtonPressed_CookingState_TurnOffLight()
        {
            //Arrange
            door.Open();
            door.Close();
            cancelStartButton.Press();
            //Act 
            cancelStartButton.Press();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }
}