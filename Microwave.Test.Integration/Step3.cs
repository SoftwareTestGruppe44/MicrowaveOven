using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step3
    {
        private Door door;
        private UserInterface userInterface;
        private Button powerButton;
        private Button startCancelButton;
        private Button timeButton;
        private CookController uutCookController;
        private IDisplay fakeDisplay;
        private ILight fakeLight;
        private ITimer fakeTimer;
        private IPowerTube fakePowerTube;

        [SetUp]
        public void Setup()
        {
            door = new Door();
            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();
            fakeTimer = Substitute.For<ITimer>();
            fakePowerTube = Substitute.For<IPowerTube>();
            fakeDisplay = Substitute.For<IDisplay>();
            fakeLight = Substitute.For<ILight>();
            uutCookController = new CookController(fakeTimer, fakeDisplay, fakePowerTube);
            userInterface = new UserInterface(powerButton,
                timeButton,
                startCancelButton,
                door,
                fakeDisplay,
                fakeLight,
                uutCookController);

        }

        [Test]
        public void StartCooking_StartCancelButtonPressed_CookingStarts()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            //Act
            startCancelButton.Press();
            //Assert
            fakeTimer.Received(1).Start(Arg.Any<int>());

        }
        [Test]
        public void Stop_StartCancelButtonPressed_CookingStops()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            //Act
            startCancelButton.Press();
            //Assert
            fakeTimer.Received(1).Stop();

        }
        [Test]
        public void Stop_DoorOpened_CookingStops()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            //Act
            door.Open();
            //Assert
            fakeTimer.Received(1).Stop();

        }
        [Test]
        public void OnTimerExpired_startCancelButtonPressed_()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            //Act
            fakeTimer.Expired += Raise.EventWith(this, EventArgs.Empty);
            //Assert
            fakePowerTube.Received(1).TurnOff();
        }
        [Test]
        public void OnTimerTick_StartCancelButtonPressed_ShowsRemainingCookingTime()
        {
            //Arrange
            powerButton.Press();
            timeButton.Press();
            //Act
            startCancelButton.Press();
            //Assert
            fakeDisplay.Received(1).ShowTime(Arg.Any<int>(),Arg.Any<int>());

        }
    }
}