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
        public void Press_PowerButtonPress_UIReceivedOnPowerPressedEvent()
        {
            //Arrange
            //Act
            uutPowerButton.Press();
            //Assert
            fakeDisplay.Received(1).ShowPower(Arg.Any<int>());
        }

        [Test]
        public void Press_TimeButtonPress_UIReceivedOnTimerPressedEvent()
        {
            //Arrange
            uutPowerButton.Press();
            //Act
            uutTimeButton.Press();
            //Assert
            fakeDisplay.Received(1).ShowTime(Arg.Any<int>(), Arg.Is<int>(x => x == 0));
        }
    }
}
