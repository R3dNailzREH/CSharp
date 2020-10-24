/*
 *  HOMEWORK 1
 * For this assignment, you’ll be implementing an interactive C# console application that analyzes the player’s
 * name and age and then plays a game of Mad Libs with them.
 * 
 * Activity 1
 * REQ_1-1: Prompt the player for their name and age
 * REQ_1-2: Use the given info to create a nickname for the player and do some calculations based on their age.

 * Activity 2
 * REQ_2-1: Prompt the player for a series of strings and numbers to fill in the blanks of a Mad Libs story of your own design.
 * REQ_2-2: Use some of the numbers given to calculate other fields in the story
 * REQ_2-3: Print the story with all user-inputted strings in ALL CAPS and well-formatted by using escape characters as needed.

 */

using System;

namespace HW1_MadLibs
{
    class Program
    {
        // Member Variables
        GetUserInput tcUserInput;
        String mcPlayerFirstName;
        String mcPlayerLastName;
        String mcPlayerNickname;
        int mnPlayerAge;

        static void Main(string[] args)
        {

            bool lbDebug = true;
            MadLibInfo lcMadLibInfo = new MadLibInfo();

            // Create an instance of this class
            Program _Program = new Program();
            _Program.tcUserInput = new GetUserInput();

            // GET THE USER NAME AND AGE //
            Console.WriteLine("Welcome User!");

            // REQ_1-1: get name
            Console.Write("Enter your first name: ");
            _Program.mcPlayerFirstName = _Program.tcUserInput.getStringFromConsole();
            Console.Write("Enter your last name: ");
            _Program.mcPlayerLastName = _Program.tcUserInput.getStringFromConsole();
            Console.WriteLine("Hello {0} {1}!", _Program.mcPlayerFirstName, _Program.mcPlayerLastName);

            // REQ_1-2: Create a nickname for the player and do some calculations based on their age
            _Program.mcPlayerNickname = _Program.CreateNickname(_Program.mcPlayerFirstName, _Program.mcPlayerLastName);
            Console.WriteLine("Can I call you {0}?", _Program.mcPlayerNickname);

            // REQ_1-1: get age
            Console.Write("Enter your age: ");
            _Program.mnPlayerAge = _Program.tcUserInput.getPositiveIntegerFromConsole();
            Console.WriteLine("So you're {0} old", _Program.mnPlayerAge);

            // Get the user inputs for the Madlib
            String lcTempString; 
            for (int i = 0; i < lcMadLibInfo.GetNumberOfInputsRequired(); i++)
            {
                Console.WriteLine("Please enter a {0}: ", lcMadLibInfo.GetNextPrompt());
                lcTempString = _Program.tcUserInput.getStringFromConsole();
                lcMadLibInfo.AddUserInput(lcTempString);
            }

            Console.WriteLine("\nThank you!  Let's see what you created... \n");
            lcMadLibInfo.PrintMadLib();
        }

        private String CreateNickname(String acFirstName, String acLastName)
        {
            String lcTempNicknameString = new String("");
            String lcTempFirstNameString = new String("");
            String lcTempLastNameString = new String("");

            // We can assume the name is at least one character long based on GetName.
            // But we cannot assume it's three letters long.
            // Pad the name with three underbars to ensure the next call succeeds.
            lcTempFirstNameString = acFirstName + "___";
            lcTempNicknameString = lcTempFirstNameString.Substring(0, 3);

            //Same for the last name
            lcTempLastNameString = acLastName + "___";
            // Add the last name to the working Nickname string
            lcTempNicknameString += lcTempLastNameString.Substring(0, 3);

            return lcTempNicknameString;
        }

        // This class contains a mad lib and the number of inputs to ask the user for. Currently 
        // it holds a single MadLib for now but enhancements could include:
        //   1) Support multiple MadLibs and pick one randomly
        public class MadLibInfo
        {
            String mcMadLibText;
            int mnNumberOfUserInputs;
            int mnNumberOfCurrentInput; 
            String[] masUserInputs;// Hold the responses
            String[] masUserPrompts; // Tell the user what you want them to input

            public MadLibInfo()
            {
                // https://www.madlibs.com/wp-content/uploads/2016/04/VacationFun_ML_2009_pg15.pdf
                mcMadLibText =
                    "A vacation is when you take a trip to some {0} place with your {1} " +
                    "family.Usually you go to some place that is near a/an {2} or up on a/an {3}. " +
                    "A good vacation place is one where you can ride {4} " +
                    "or play {5} or go hunting for {6}.I like " +
                    "to spend my time {7} or {8}. " +
                    "When parents go on a vacation, they spend their time eating " +
                    "three {9} a day, and fathers play golf, and mothers " +
                    "sit around {9}.Last summer, my little brother " +
                    "fell in a/an {10} and got poison {11} all " +
                    "over his {12}.My family is going to go to (the) " +
                    "{13}, and I will practice {14}.Parents " +
                    "need vacations more than kids because parents are always very " +
                    "{15} and because they have to work {16} " +
                    "hours every day all year making enough {17} to pay " +
                    "for the vacation.";

                mnNumberOfUserInputs = 18;
                mnNumberOfCurrentInput = 0; // points to the first unprovided input

                masUserInputs = new String[mnNumberOfUserInputs];

                masUserPrompts = new String[] { "ADJECTIVE", "ADJECTIVE", "NOUN", 
                        "NOUN", "PLURAL NOUN", "GAME", "PLURAL NOUN", "VERB ENDING IN \"ING\"",
                        "VERB ENDING IN \"ING\"", "PLURAL NOUN", "VERB ENDING IN \"ING\"", 
                        "NOUN", "PLANT", "PART OF THE BODY", "A PLACE", "VERB ENDING IN \"ING\"",
                        "ADJECTIVE", "NUMBER", "PLURAL NOUN"};
            }

            public int GetNumberOfInputsRequired()
            {
                return mnNumberOfUserInputs;
            }

            // Add a user string to the list of values to be substituted into the madlibs output
            public void AddUserInput(String acUserString)
            {
                if(mnNumberOfCurrentInput == mnNumberOfUserInputs)
                {
                    Console.WriteLine("ERROR! We already have all our inputs!");
                    return;
                }
                masUserInputs[mnNumberOfCurrentInput] = acUserString;
                mnNumberOfCurrentInput++;
            }

            // Returns the tupe of string we want from the user
            public String GetNextPrompt()
            {
                return masUserPrompts[mnNumberOfCurrentInput];
            }

            // Prints the MadLib using the inputs. 
            // IMPROVMENTS: Currently hardwired because I don't know how to use the strings, but I can probably 
            //              do one per line and one value per line, iterate through the MadLib as an array.
            public void PrintMadLib()
            {
                Console.WriteLine(mcMadLibText, masUserInputs[0], masUserInputs[1], masUserInputs[2], masUserInputs[3],
                    masUserInputs[4], masUserInputs[5], masUserInputs[6], masUserInputs[7], masUserInputs[8], masUserInputs[9],
                    masUserInputs[10], masUserInputs[11], masUserInputs[12], masUserInputs[13], masUserInputs[14], masUserInputs[15],
                    masUserInputs[16], masUserInputs[17]);
            }
        }

        // GetUserInput
        // Gets input from the console. Continues to ask for the input if the incorrect type is entered. 
        // Limitations: It does not work for floating point numbers entered for Integer
        public class GetUserInput
        {
            // Get a string from console. Do not return until you have one
            public String getStringFromConsole()
            {
                String lcReturn = System.String.Empty;
                String lcTempString = System.String.Empty;
                // Required by TryParse but not used
                int lnUnusedInt;
                float lnUnusedFloat;
                bool lbValid = false;

                do
                {
                    lcTempString = System.Console.ReadLine();

                    // If it's at least one character long
                    if (0 < lcTempString.Length)
                    {
                        // and does NOT return an INT or FLOAT
                        if (false ==   int.TryParse(lcTempString, out lnUnusedInt) &&
                           (false == float.TryParse(lcTempString, out lnUnusedFloat)))
                        {
                            // We'll assume it's text and return it
                            lbValid = true;
                            lcReturn = lcTempString;
                        }
                    }
                    if (false == lbValid)
                    {
                        Console.Write("Invalid value! Try Again: ");
                    }
                } while (false == lbValid);

                return lcReturn;
            }

            // Get an integer from console. Do not return until you have an int
            public int getIntegerFromConsole()
            {
                int lcReturn = 0;
                String lcTempString = System.String.Empty;
                bool lbValid = false;
                bool lbIsInteger = false;

                do
                {
                    lcTempString = System.Console.ReadLine();

                    // If it's at least one character long
                    if (0 < lcTempString.Length)
                    {
                        // And can be converted to a number
                        lbIsInteger = int.TryParse(lcTempString, out lcReturn);
                        lbValid = true;
                    }
                    if (false == lbValid)
                    {
                        Console.Write("Invalid value! Try Again: ");
                    }
                } while (false == lbValid);

                return lcReturn;
            }

            // Get a positive integer from console. Do not return until you have an int
            public int getPositiveIntegerFromConsole()
            {
                int lcReturn = 0;
                String lcTempString = System.String.Empty;
                bool lbValid = false;
                bool lbIsInteger = false;

                do
                {
                    lcTempString = System.Console.ReadLine();

                    // If it's at least one character long
                    if (0 < lcTempString.Length)
                    {
                        // And can be converted to a number
                        lbIsInteger = int.TryParse(lcTempString, out lcReturn);
                        // And is greater than 0
                        if (0 < lcReturn)
                        {
                            lbValid = true;
                        }
                    }
                    if (false == lbValid)
                    {
                        Console.Write("Invalid value! Try Again: ");
                    }
                } while (false == lbValid);

                return lcReturn;
            }
        }


    }
}
