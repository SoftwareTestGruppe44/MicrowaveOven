using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step8
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
        private Output myOutput;
        private PowerTube uutPowerTube;

        [SetUp]
        public void Setup()
        {
            myDoor = new Door();
            myPowerButton = new Button();
            myCancelStartButton = new Button();
            myTimeButton = new Button();
            myTimer = new Timer();
            myOutput = new Output();
            myDisplay = new Display(myOutput);
            myLight = new Light(myOutput);
            uutPowerTube = new PowerTube(myOutput);
            myCookController = new CookController(myTimer, myDisplay, uutPowerTube);
            userInterface = new UserInterface(myPowerButton,
                myTimeButton,
                myCancelStartButton,
                myDoor,
                myDisplay,
                myLight,
                myCookController);
        }

    }
}