using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step2
    {
        private Door door;
        private UserInterface userInterface;
        private Button uutPowerButton;
        private Button uutCancelStartButton;
        private Button uutTimeButton;
        private ICookController fakeCookController;
        private IDisplay fakeDisplay;
        private ILight fakeLight;

        [SetUp]
        public void Setup()
        {
            door = new Door(); 
            uutPowerButton = new Button();
            uutCancelStartButton = new Button();
            uutTimeButton = new Button();
            fakeCookController = Substitute.For<ICookController>();
            fakeDisplay = Substitute.For<IDisplay>();
            fakeLight = Substitute.For<ILight>();
            userInterface = new UserInterface(uutPowerButton,
                uutTimeButton,
                uutCancelStartButton,
                door,
                fakeDisplay,
                fakeLight,
                fakeCookController);
        }

        [Test]
        [Category("Main Scenario")]
        public void Press_PowerButtonPress_UIReceivedOnPowerPressedEvent()
        {
            //Arrange
            //Act
            uutPowerButton.Press();
            //Assert
            fakeDisplay.Received(1).ShowPower(Arg.Any<int>());
        }

        [Test]
        [Category("Main Scenario")]
        public void Press_PowerButtonPressedTwice_UIReceivedOnPowerPressedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            //Act
            uutPowerButton.Press();
            //Assert
            fakeDisplay.Received(2).ShowPower(Arg.Any<int>());
        }

        [Test]
        [Category("Main Scenario")]
        public void Press_TimeButtonPress_UIReceivedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            //Act
            uutTimeButton.Press();
            //Assert
            //Decided that the second parameter should be equal to zero because it always receives zero no matter what. 
            fakeDisplay.Received(1).ShowTime(Arg.Any<int>(), Arg.Is<int>(x => x == 0));
        }

        [Test]
        [Category("Main Scenario")]
        public void Press_TimeButtonPressedTwice_UIReceivedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            uutTimeButton.Press();
            //Act
            uutTimeButton.Press();
            //Assert
            //Decided that the second parameter should be equal to zero because it always receives zero no matter what. 
            fakeDisplay.Received(2).ShowTime(Arg.Any<int>(), Arg.Is<int>(x => x == 0));
        }

        [Test]
        [Category("Main Scenario")]
        public void Press_StartCancelButtonPress_UIReceivedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            uutTimeButton.Press();
            //Act
            uutCancelStartButton.Press();
            //Assert
            //Asserting that both the cookController and Light received correct method calls.
            fakeCookController.Received(1).StartCooking(Arg.Any<int>(), Arg.Any<int>());
            fakeLight.Received(1).TurnOn();
        }

        [Test]
        [Category("Extension 3")]
        public void Press_StartCancelButtonPressWhileCooking_UIReceivedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            uutTimeButton.Press();
            uutCancelStartButton.Press();
            //Act
            uutCancelStartButton.Press();
            //Assert
            fakeDisplay.Received(1).Clear();
        }

        [Test]
        [Category("Extension 1")]
        public void Press_StartCancelButtonPressDuringSetup_UIReceivedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            //Act
            uutCancelStartButton.Press();
            //Assert
            fakeDisplay.Received(1).Clear();
        }
    }
}
