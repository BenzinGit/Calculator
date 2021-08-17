using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Andreas Klæstad, 9806030699
namespace Calculator
{
    public partial class Form1 : Form
    {

        int prevNumber = 0; 
        string nextOperator = "";
        
        bool isNewNumber = true;
        bool waitingForNextNumber = true;
        bool maximumCharactersReached = false;
       
        //Dessa används vid "=" flera gånger
        int continued;
        string continuedOp; 
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeNumber("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            writeNumber("2"); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            writeNumber("3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            writeNumber("4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            writeNumber("5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            writeNumber("6");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            writeNumber("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            writeNumber("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            writeNumber("9");
        }

        private void button0_Click(object sender, EventArgs e)
        {
            writeNumber("0");
        }

        //Skriver ut numret i 
        private void writeNumber(string number)
        {
            if (!maximumCharactersReached)
            {
                if (currentNumberBox.Text == "0" || isNewNumber)
                {
                    currentNumberBox.Text = number;
                    isNewNumber = false;

                }
                else
                {
                    currentNumberBox.Text += number;

                }
                waitingForNextNumber = false;
                if (currentNumberBox.Text.Length > 8)
                {
                    maximumCharactersReached = true;
                }
            }
        }

        //Sätter programmet tillbaka till ursprungstillståndet.
        private void resetAll()
        {
            isNewNumber = true;
            prevNumber = 0;
            historyBox.Text = ""; 
            nextOperator = "";
            currentNumberBox.Text = "0";
            waitingForNextNumber = true;
            maximumCharactersReached = false;
             continued = 0;
            continuedOp = "";
        }

      
        //Kallas när ett värde går över/under int-gränsen. 
        private void ShowNumericError()
        {
            currentNumberBox.Text = "Numeric Limit"; 
            MessageBox.Show($"Värdet på ditt inskrivna tal eller uträkningen var " +
                $"för högt eller för lågt. Ett värde på ett nummer kan endast " +
                $"vara mellan {int.MinValue} och {int.MaxValue}.",
                                    "För höga/låga tal",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            resetAll();
        }


        private void equalsButton_Click(object sender, EventArgs e)
        {
          if (!waitingForNextNumber)
            {
                if (historyBox.Text.Length != 0)
                    if (historyBox.Text.Substring(historyBox.Text.Length - 2) == "= ")
                    {
                        continuedCount();
                    }


                if (historyBox.Text.Length != 0)
                    if (historyBox.Text.Substring(historyBox.Text.Length - 2) != "= ")
                    {
                        historyBox.Text += currentNumberBox.Text + " = ";
                        continued = int.Parse(currentNumberBox.Text);
                        continuedOp = nextOperator; 
                        count();

                        nextOperator = ""; 
                        prevNumber = int.Parse(currentNumberBox.Text);
                    }
            }
        }

        //Används vid upprepade uträkningar när användaren trycker på "=" flera gånger
        private void continuedCount()
        {
            try
            {
                if (continuedOp == "+")
                {
                    historyBox.Text = currentNumberBox.Text + " + " + continued + " = ";

                    currentNumberBox.Text = (int.Parse(currentNumberBox.Text) + continued).ToString();
                    prevNumber = 0;
                }

                if (continuedOp == "-")
                {
                    historyBox.Text = currentNumberBox.Text + " - " + continued + " = ";

                    currentNumberBox.Text = (int.Parse(currentNumberBox.Text) - continued).ToString();
                    prevNumber = int.Parse(currentNumberBox.Text);


                }
                if (continuedOp == "x")
                {
                    historyBox.Text = currentNumberBox.Text + " x " + continued + " = ";

                    currentNumberBox.Text = (continued * int.Parse(currentNumberBox.Text)).ToString();
                    prevNumber = 0;
                }
                if (continuedOp == "/")
                {
                    if (continued != 0)
                    {
                        historyBox.Text = currentNumberBox.Text + " / " + continued + " = ";

                        currentNumberBox.Text = (int.Parse(currentNumberBox.Text) / continued).ToString();
                        prevNumber = int.Parse(currentNumberBox.Text);
                    }
                    else
                    {
                        MessageBox.Show($"Kan inte dela tal med noll!",
                        "Odelbar nämnare",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                        resetAll();

                    }
                }
            }
            catch (Exception exp)
            {
                ShowNumericError();

            }

        }
        //Kallas när användaren trycker på en av operatorerna
        private void writeOperator(string op)
        {
            if (!waitingForNextNumber)
            {
                if (historyBox.Text.Length != 0)
                    if (historyBox.Text.Substring(historyBox.Text.Length - 2) == "= ")
                    {
                        historyBox.Text = "";
                    }

                waitingForNextNumber = true;
                maximumCharactersReached = false;
                historyBox.Text += currentNumberBox.Text + " "+ op + " ";

                count();

                nextOperator = op;
                prevNumber = int.Parse(currentNumberBox.Text);
            }
            else
            {
                if (historyBox.Text.Length != 0)
                {
                    nextOperator = op;
                    historyBox.Text = historyBox.Text.Remove(historyBox.Text.Length - 2) + op +" ";
                }
            }


        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            writeOperator("+"); 
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            writeOperator("-");
        }
        private void multiButton_Click(object sender, EventArgs e)
        {
            writeOperator("x");
        }

        private void divideButton_Click(object sender, EventArgs e)
        {
            writeOperator("/");
        }

        //Räknar ut en beräkningen utifrån vilken av operatornerna användaren valt
        private void count()
        {
            try
            {
                //Testar om talet går över int limit, för att kunna skapa exception. 
                // "limitTest" används alltså inte i någon uträkning.
                int limitTest = int.Parse(currentNumberBox.Text);
                isNewNumber = true;
                if (nextOperator == "+")
                {
                    currentNumberBox.Text = (int.Parse(currentNumberBox.Text) + prevNumber).ToString();
                }

                if (nextOperator == "-" )
                {
                    currentNumberBox.Text = (prevNumber - int.Parse(currentNumberBox.Text)).ToString();
                }
                if (nextOperator == "x")
                {
                    currentNumberBox.Text = (prevNumber * int.Parse(currentNumberBox.Text)).ToString();
                }
                if (nextOperator == "/")
                {
                    if (currentNumberBox.Text != "0")
                    {
                        currentNumberBox.Text = (prevNumber / int.Parse(currentNumberBox.Text)).ToString();

                    }
                    else
                    {
                        MessageBox.Show($"Kan inte dela tal med noll!",
                        "Odelbar nämnare",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                        resetAll();

                    }
                }

            }
            catch (Exception exp)
            {
                ShowNumericError();

            }
        }

        private void cButton_Click(object sender, EventArgs e)
        {
            resetAll();
        }

        private void keyListener(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.NumPad0)
                button0.PerformClick();

            if (e.KeyCode == Keys.NumPad1)
                button1.PerformClick();

            if (e.KeyCode == Keys.NumPad2)
                button2.PerformClick();

            if (e.KeyCode == Keys.NumPad3)
                button3.PerformClick();

            if (e.KeyCode == Keys.NumPad4)
                button4.PerformClick();

            if (e.KeyCode == Keys.NumPad5)
                button5.PerformClick();

            if (e.KeyCode == Keys.NumPad6)
                button6.PerformClick();

            if (e.KeyCode == Keys.NumPad7)
                button7.PerformClick();

            if (e.KeyCode == Keys.NumPad8)
                button8.PerformClick();

            if (e.KeyCode == Keys.NumPad9)
                button9.PerformClick();

            if (e.KeyCode == Keys.Add)
                plusButton.PerformClick();

            if (e.KeyCode == Keys.Subtract)
                minusButton.PerformClick();

            if (e.KeyCode == Keys.Divide)
                divideButton.PerformClick();

            if (e.KeyCode == Keys.Multiply)
                multiButton.PerformClick(); 

            if (e.KeyCode == Keys.Enter)
                equalsButton.PerformClick(); 
        }
    }
}
