using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;


public class Controller : MonoBehaviour
{

     SerialPort stream;
     string receivedData = "EMPTY";


     int[] Buttons = { 0x0040, 0x0080, 0x0100, 0x0200, 0x0400, 0x0800 };
     bool[] ButtonPressed = { false, false, false, false, false, false };

     int buttonVal;
     string[] SliderTurner = new string[4];

     bool[] LEDsOn = { false, false, false, false};
     int MotorStatus = 0;
     string receive;
    public  float xAcc,yAcc,zAcc;

    // Use this for initialization
    public void Init(string Port)
    {
        stream = new SerialPort(Port, 115200);
        stream.Open();
        Debug.Log("Serial port opened");
        
    }


    public  void CheckButtons()
    {
        stream.Write("1");
        receivedData = stream.ReadLine();
        buttonVal = System.Convert.ToInt32(receivedData, 16);
    }

    public  bool GetButton(int buttonNumber)
    {
        buttonNumber = (buttonNumber-1) % 6;
        if ((buttonVal & Buttons[buttonNumber]) != 0)
            return true;
        else
            return false;

    }
    public  bool GetButtonDown(int buttonNumber)
    {
        buttonNumber = (buttonNumber - 1) % 6;

        if (((buttonVal & Buttons[buttonNumber]) != 0) && !ButtonPressed[buttonNumber])
        {
            ButtonPressed[buttonNumber] = true;
            return true;
        }
        else if ((buttonVal & Buttons[buttonNumber]) == 0)
        {
            ButtonPressed[buttonNumber] = false;
            return false;
        }
        else
            return false;



    }

    public  bool GetButtonUp(int buttonNumber)
    {
        buttonNumber = (buttonNumber - 1) % 6;

        if (((buttonVal & Buttons[buttonNumber]) != 0) && !ButtonPressed[buttonNumber])
        {
            ButtonPressed[buttonNumber] = true;
            return false;
        }
        else if (((buttonVal & Buttons[buttonNumber]) == 0) && ButtonPressed[buttonNumber])
        {
            ButtonPressed[buttonNumber] = false;
            return true;
        }
        else
            return false;

    }

    //Sliders
    //0 left, 1 right

    public  void CheckSlidersTurners()
    {
        stream.Write("4");
        receivedData = stream.ReadLine();
        SliderTurner = receivedData.Split(' ');
    }

    public  float GetSlider(int i)
    {
        i %= 2;
        if (i == 0)
        {
            return ((float)(System.Convert.ToInt32(SliderTurner[4], 16)) / 4095.0f) ;
        }
        else
        {
            return ((float)(System.Convert.ToInt32(SliderTurner[3], 16)) / 4095.0f) ;
        }
    }

    public  float GetTurner(int i)
    {
        i %= 2;
        if (i == 0)
        {
            return ((float)(System.Convert.ToInt32(SliderTurner[2], 16)) / 4095.0f);
        }
        else
        {
            return ((float)(System.Convert.ToInt32(SliderTurner[1], 16)) / 4095.0f);
        }
    }

    //Motor LEDs
    // send new motor speed
    public  void setMotor(int motorValue)
    {
        Mathf.Clamp(motorValue, 0f, 1000f);
        // we just linearly map the heat value to a motor speed between 0 and 1000

        // if the change is too small, we ignore it
        if (Math.Abs(motorValue - MotorStatus) < 50)
            return;

        // otherwise store the new value and talk to the device
        // importnat: do not forget to read back reply
        MotorStatus = motorValue;
        stream.Write("m " + motorValue + "\r\n");
        receive = stream.ReadLine();
    }

    public  void LEDOn(int i)
    {
        i %= 4;
        if (LEDsOn[i])
        {
            return;
        }

        LEDsOn[i] = true;
        stream.Write("l " + i + " 1\r\n");
        receive = stream.ReadLine();
    }

    public  void LEDOff(int i)
    {
        i %= 4;
        if (!LEDsOn[i])
        {
            return;
        }

        
        stream.Write("l " + i + " 0\r\n");
        LEDsOn[i] = false;
        receive = stream.ReadLine();
    }

    public  void LEDToggle(int i)
    {
        i %= 4;

        LEDsOn[i] = !LEDsOn[i];
        stream.Write("l " + i + " 2\r\n");
        receive = stream.ReadLine();
    }

    public  void TurnOffMotor()
    {
        setMotor(0);
    }

    public  void TurnOffLEDs()
    {
        LEDOff(0);
        LEDOff(1);
        LEDOff(2);
        LEDOff(3);
    }

    public void TurnOff()
    {
        TurnOffMotor();
        TurnOffLEDs();
        stream.Close();
    }

    void OnApplicationQuit()
    {

        TurnOffMotor();
        TurnOffLEDs();
        stream.Close();
    }

    //Accelerator
    public  void CheckAccelerator()
    {
        stream.Write("a");
        // read a line
        receivedData = stream.ReadLine();
        string[] parts = receivedData.Split(' ');

        // convert x and y values into integers
        xAcc = System.Convert.ToInt32(parts[1], 16);
        yAcc = System.Convert.ToInt32(parts[2], 16);
        zAcc = System.Convert.ToInt32(parts[3], 16);

        if (xAcc > 127)
        {
            xAcc -= 256;
            xAcc /= -64;
        }
        else
            xAcc /= -127;
        if (yAcc > 63.5f)
        {
            yAcc -= 256;
            yAcc /= 64;
        }
        else
            yAcc /= 63.5f;
        if (zAcc > 127)
        {
            zAcc -= 256;
            zAcc /= 64;
        }
        else
            zAcc /= 63.5f;
            
    }

}





