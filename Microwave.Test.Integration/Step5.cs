using System;
using System.Threading;
using System.Threading.Tasks;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NuGet.Frameworks;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;


namespace Microwave.Test.Integration
{
    public class Step5
    {
        private Door door;
        private UserInterface userInterface;
        private Button powerButton;
        private Button cancelStartButton;
        private Button timeButton;
        private CookController cookController;
        private Display display;
        private Light light;
        private Timer uutTimer;
        private IPowerTube fakePowerTube;
        private IOutput fakeOutput;

        [SetUp]
        public void Setup()
        {
            door = new Door();
            powerButton = new Button();
            cancelStartButton = new Button();
            timeButton = new Button();
            uutTimer = new Timer();
            fakePowerTube = Substitute.For<IPowerTube>();
            fakeOutput = Substitute.For<IOutput>();
            cookController = new CookController(uutTimer,display,fakePowerTube);
            display = new Display(fakeOutput);
            light = new Light(fakeOutput);
            userInterface = new UserInterface(powerButton,
                timeButton,
                cancelStartButton,
                door,
                display,
                light,
                cookController);
            cookController.UI = userInterface;
        }

        [Test]
        public void start_setPowerandSetTimeTo3Min_TimeIsSetCorrect()
        {
            //arrange
            powerButton.Press();
            timeButton.Press();
            timeButton.Press();
            timeButton.Press();

            //act
            cancelStartButton.Press();
            //assert
            Assert.AreEqual(180, uutTimer.TimeRemaining);
        }

        [Test]
        public void onTimeEvent_getCalled60Times_timeRemainingIsZero()
        {
            //arrange
            powerButton.Press();
            timeButton.Press();

            //act
            cancelStartButton.Press();
            Thread.Sleep(60000);

            //assert
            Assert.AreEqual(0,uutTimer.TimeRemaining,1);
        }

        [Test]
        public void timerExpired_InvokesEvent_EventInvoked()
        {
            //arrange
            var eventInvoked = new ManualResetEvent(false);

            uutTimer.Expired += (sender, args) => eventInvoked.Set();

            powerButton.Press();
            timeButton.Press();

            //act
            cancelStartButton.Press();
            
            //assert
            Assert.IsTrue(eventInvoked.WaitOne(61000));
        }

        [Test]
        public void DoorOpen_WhileCookingTimerStops_TimeRemainingEqual55()
        {
            //arrange
            powerButton.Press();
            timeButton.Press();

            //act
            cancelStartButton.Press();
            Thread.Sleep(5000);
            door.Open();
            Thread.Sleep(5000);

            //assert
            Assert.AreEqual(55, uutTimer.TimeRemaining);
        }
    }
}