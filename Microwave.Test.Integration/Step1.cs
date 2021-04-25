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
        public void Open_DoorOpensStateReady_lightOnReceivedOne()
        {
            //arrange

            //act
            uutDoor.Open();
            //assert
            fakeLight.Received(1).TurnOn();
        }

        [Test]
        public void Open_DoorCloseWhenDoorOpen_lightOFReceivedOne()
        {
            //arrange
            uutDoor.Open();
            //act
            uutDoor.Close();
            //assert
            fakeLight.Received(1).TurnOff();
        }

    }
}