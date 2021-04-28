using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
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
        }
        [Test]

    }
}