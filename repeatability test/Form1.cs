using System;
using System.Windows.Forms;
using System.Device.Gpio;
using System.Diagnostics;

namespace repeatability_test
{
    public partial class Form1 : Form
    {
        public Stopwatch Totalwatch;
        public Stopwatch Cyclewatch;
        public int greenPresses = 0;
        public int redPresses = 0;  
        public GpioController rpi = new GpioController();
        public const int GREENPIN = 7;
        public const int REDPIN = 8;
        public const int CAMPIN = 7;
        public Form1()
        {
            InitializeComponent();
            rpi.OpenPin(GREENPIN, PinMode.InputPullUp);
            rpi.OpenPin(REDPIN, PinMode.InputPullUp);
            Cyclewatch = Stopwatch.StartNew();
            Totalwatch = Stopwatch.StartNew();
            while (true)
            {
                textBox1.Invoke(new MethodInvoker (delegate{ textBox1.Text = (greenPresses + redPresses).ToString();}));
                textBox2.Invoke(new MethodInvoker( delegate {textBox2.Text = greenPresses.ToString();}));
                textBox3.Invoke(new MethodInvoker( delegate {textBox3.Text = redPresses.ToString();}));  
                textBox4.Invoke(new MethodInvoker( delegate {textBox4.Text = Cyclewatch.Elapsed.ToString();}));
                textBox5.Invoke(new MethodInvoker( 
                delegate {textBox5.Text = (Totalwatch.Elapsed.TotalSeconds / (greenPresses + redPresses)).ToString();}));
                rpi.RegisterCallbackForPinValueChangedEvent(GREENPIN, PinEventTypes.Falling | PinEventTypes.Rising, GreenButtonPress);
                rpi.RegisterCallbackForPinValueChangedEvent(REDPIN, PinEventTypes.Falling | PinEventTypes.Rising, RedButtonPress);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
    
        }
        private void GreenButtonPress(object sender, PinValueChangedEventArgs e)
        {
            greenPresses++;
            Cyclewatch.Restart();
        }
        private void RedButtonPress(object sender, PinValueChangedEventArgs e)
        {
            redPresses++;
            Cyclewatch.Restart();
        }
    }
}
