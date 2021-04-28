using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step1
    {
        private Door uutDoor;
        private UserInterface userInterface;
        private IButton fakePowerButton;
        private IButton fakeCancelStartButton;
        private IButton fakeTimeButton;
        private ICookController fakeCookController;
        private IDisplay fakeDisplay;
        private ILight fakeLight;

        [SetUp]
        public void Setup()
        {
            uutDoor = new Door();
            fakePowerButton = Substitute.For<IButton>();
            fakeCancelStartButton = Substitute.For<IButton>();
            fakeTimeButton = Substitute.For<IButton>();
            fakeCookController = Substitute.For<ICookController>();
            fakeDisplay = Substitute.For<IDisplay>();
            fakeLight = Substitute.For<ILight>();
            userInterface = new UserInterface(fakePowerButton,
                fakeTimeButton, 
                fakeCancelStartButton,
                uutDoor, 
                fakeDisplay, 
                fakeLight,
                fakeCookController);
        }

        [Test]
        [Category("Main Scenario")]
        public void Open_DoorOpensStateReady_lightOnReceivedOne()
        {
            //arrange
            //act
            uutDoor.Open();
            //assert
            fakeLight.Received(1).TurnOn();
        }

        [Test]
        [Category("Main Scenario")]
        public void Open_DoorCloseWhenDoorOpen_lightOFFReceivedOne()
        {
            //arrange
            uutDoor.Open();
            //act
            uutDoor.Close();
            //assert
            fakeLight.Received(1).TurnOff();
        }

        [Test]
        [Category("Extension 2")]
        public void Open_DoorOpenDuringPowerSetup_displayIsCleared()
        {
            //Arrange
            fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //Act
            uutDoor.Open();
            //Assert
            fakeDisplay.Received(1).Clear();
        }

        [Test]
        [Category("Extension 2")]
        public void Open_DoorOpenDuringTimerSetup_displayIsCleared()
        {
            //Arrange
            fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //Act
            uutDoor.Open();
            //Assert
            fakeDisplay.Received(1).Clear();
        }

        [Test]
        [Category("Extension 4")]
        public void Open_DoorOpenDuringCooking_DisplayIsCleared()
        {
            //Arrange
            fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            fakeCancelStartButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //Act
            uutDoor.Open();
            //Assert
            fakeDisplay.Received(1).Clear();
        }
    }
}