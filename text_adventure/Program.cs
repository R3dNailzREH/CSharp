using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace HW2_TextAdventure
{
    /** Basic Text Adventure Engine
     * Intent of the program is to show an object oriented design 
     * for the engine. Very simple
     */
    class Program
    {
        static void Main(string[] args)
        {
            // Objects used by engine
            String lcTempInput = "";

            // =============== INSTANTIATE ASSETS ======================== //

            // Everything related to the character
            Character mainCharacter = new Character();

            // Everything related to the map including descriptions, directions
            // and player/item locations
            WorldMap worldMap = new WorldMap();

            // Input processing class
            GetUserInput getUserInput = new GetUserInput();

            // The Game Handler actually runs the game.
            GameHandler gameHandler = new GameHandler(ref mainCharacter, ref worldMap, ref getUserInput);

            // ================ GET PLAYER INFO ========================== //

            Console.Write("Welcome brave adventurer!\n\nWhat is thy name?\n> ");
            lcTempInput = getUserInput.getStringFromConsole();

            Console.WriteLine("Pleased to make your acquiantance " + lcTempInput + "\n");
            mainCharacter.SetPlayerName(lcTempInput);
            Console.WriteLine("If need assistance, you may enter HELP at any time.\n");


            // Intro location and Character info
            Console.WriteLine(worldMap.GetDescription());
            Console.WriteLine(mainCharacter.GetPlayerStatusFull());

            // ===============  MAIN LOOP ===================== //

            // This routine will run the game. When it exits, we are done.
            gameHandler.RunGame();

            // =============  END MAIN LOOP =================== //

            Console.WriteLine("Thank you for playing " + mainCharacter.GetPlayerName());

            // Clean exit
            System.Environment.Exit(0);
        }
    }// class Program

    /** This class perfoms the main processing loop for the game. It will ask for a direction and provide
     * a descripion of the current location. It will then tell the player which directions are valid.
     * It will continue to allow the player to move through the map until they type EXIT
     */
    public class GameHandler
    {
        // References to the main assets
        Character mainCharacter;
        WorldMap worldMap;
        GetUserInput getUserInput;

        // local variables I don't want to reinstantiate every time I call a method
        String lcTempInput = "";
        bool lbContinuePlaying;
        bool lbInputValid;
        bool lbDirectionValid;

        // Store the references passed in on start
        public GameHandler(ref Character acMainCharacter, ref WorldMap acWorldMap, ref GetUserInput acGetUserInput)
        {
            mainCharacter = acMainCharacter;
            worldMap = acWorldMap;
            getUserInput = acGetUserInput;
        }

        // Main Player Loop.
        // This method is passive. It tells the player where they can go, delegates player actions,
        // then tells the player where they are. Stay in this until the user inputs EXIT
        public void RunGame()
        {
            // Loop until the player types EXIT
            lbContinuePlaying = true;

            while( true == lbContinuePlaying )
            {
                // Ask player what they would like to do
                Console.WriteLine("\nWhat is thy bidding " + mainCharacter.GetPlayerName() + "?");
                Console.WriteLine(worldMap.GetValidDirections());

                // Process commands until the player types EXIT
                if( false == ProcessPlayerInput() )
                {
                    lbContinuePlaying = false;
                    break; // This is only false if the user asked to exit
                }

                // Tell them where they are now
                Console.WriteLine( "\n" + worldMap.GetDescription() );                
            }
        }

        /** This is our player input parser. 
         * It takes all player input and compares it against valid commands. If the command 
         * is a direction it delegates it to the WorldMap for updating the player location.
         */
        private bool ProcessPlayerInput()
        {
            lbContinuePlaying = true;
            lbInputValid = true; 
            lbDirectionValid = true;

            // Get input and keep trying to update user location until it is valid
            Console.Write("> ");
            lcTempInput = getUserInput.getUpperCaseStringFromConsole();

            // Run the entered tex through the standardize routine. This allows shorthand
            // as well as synonyms
            lcTempInput = StandardizeInput(lcTempInput);

            // MINOR DEFECT: Check the input to be sure you only get one word.
            switch (lcTempInput)
            {
                case "NORTH":
                case "SOUTH":
                case "EAST":
                case "WEST":
                case "UP":
                case "DOWN":
                    lbInputValid = true;
                    lbDirectionValid = worldMap.UpdatePlayerLocation(lcTempInput);
                    break;

                case "LOOK":
                    lbInputValid = true;
                    worldMap.GetDescription();
                    break;

                case "STATUS":
                    lbInputValid = true;
                    Console.WriteLine(mainCharacter.GetPlayerStatusFull());
                    break;

                case "INVENTORY":
                    lbInputValid = true;
                    Console.WriteLine(mainCharacter.GetPlayerInventory());
                    break;

                case "HELP":
                    lbInputValid = true;
                    Console.WriteLine(ValidCommands());
                    break;

                case "EXIT":
                    lbInputValid = true;
                    lbContinuePlaying = false;
                    break;

                default:
                    lbInputValid = false;
                    break;
            }

            // If the command was invalid tell the user why. 
            //
            // Direction is a special case of invalid. You can enter a valid INPUT
            // (direction) but not have it be a valid DIRECTION on the map. 
            if (false == lbDirectionValid)
            {
                Console.WriteLine("Invalid direction. Try again.");
            }
            if (false == lbInputValid)
            {
                Console.WriteLine("Invalid command. Try again.");
            }

            return lbContinuePlaying;
        }
        // Returns the list of commands that can be issued
        private String ValidCommands()
        {
            String lcReturnString = "Valid Directions: NORTH, SOUTH, EAST, WEST, UP, DOWN\n";
            lcReturnString = lcReturnString + "Valid Commands: HELP, LOOK, STATUS, INVENTORY, EXIT\n";
            return lcReturnString;
        }

        // Checks the player input against synonyms and contractions of commands
        // Useful to allow several words to map to one command
        private String StandardizeInput(String acInputString)
        {
            switch (acInputString)
            {
                case "NORTH":
                case "N":
                    return "NORTH";
                case "SOUTH":
                case "S":
                    return "SOUTH";
                case "EAST":
                case "E":
                    return "EAST";
                case "WEST":
                case "W":
                    return "WEST";
                case "UP":
                case "U":
                    return "UP";
                case "DOWN":
                case "D":
                    return "DOWN";
                case "LOOK":
                case "L":
                    return "LOOK";
                case "STATUS":
                case "HEALTH":
                    return "STATUS";
                case "INVENTORY":
                case "INV":
                case "I":
                case "ITEMS":
                    return "INVENTORY";
                // NOT YET SUPPORTED
                case "FIGHT":
                case "HIT":
                case "ATTACK":
                case "KILL":
                    return "FIGHT";
                case "TAKE":
                case "STEAL":
                    return "TAKE";
                case "DROP":
                case "LEAVE":
                case "DISCARD":
                    return "DROP";
                case "BUY":
                case "PURCHASE":
                    return "BUY";
                case "SELL":
                    return "SELL";
                // End Not supported
                case "HELP":
                case "H":
                    return "HELP";
                case "EXIT":
                case "QUIT":
                    return "EXIT";
                // No match, return whatever we got
                default:
                    return acInputString;
            }
        }
    }


    /**
     * CHARACTER 
     * 
     * Handles information about our player. 
     * 
     * NOTES:
     * Currently hardwired to have three items and default values on creation.
     * 
     * TODO:
     * - Improve inventory to be a linked list of items we can add or remove
     * - Make starting HP and Loot variable
     * - Add a STATUS enum for player alive, change to player status (ALIVE, DEAD, ASLEEP, PARALYZED, etc)
     * - Could add string returns for each player element vs. just getting all via getPlayerStatusFull()
     */
    public class Character
    {
        String mcPlayerName { get; set; }
        bool mbPlayerAlive { get; set; }
        int mnPlayerHP { get; set; }
        int mnPlayerLoot { get; set; }
        bool mbHasSword { get; set; }
        bool mbHasAlly { get; set; }

        // Player starts with 100 HP and gold, and no items.
        public Character()
        {
            mcPlayerName = "";
            mbPlayerAlive = true;
            mnPlayerHP = 100;
            mnPlayerLoot = 100;
            mbHasSword = false;
            mbHasAlly = false;
        }

        // Set the Player Name, in Leading uppercase like a name should be
        public void SetPlayerName(String acNewName)
        {
            acNewName.ToUpperInvariant();
            mcPlayerName = acNewName;
        }
        
        public String GetPlayerName()
        {
            return mcPlayerName;
        }
        
        // Used to set the player to a specific value
        //
        // SIDE EFFECTS:
        // - Changes Player status if health = 0
        public void SetPlayerHP(int anNewTotal)
        {
            mnPlayerHP = anNewTotal;           
            if(0 > mnPlayerHP)
            {
                mnPlayerHP = 0; // health cannot be negative
            }
            if(0 == mnPlayerHP)
            {
                mbPlayerAlive = false; // if your health is 0 you die
            }
        }

        // Used to increase or decrease health by a specific amount.
        // This can be negative. If the player was at 0 and this 
        // increases their health to > 0 their status will be 
        // updated to Alive.
        //
        // SIDE EFFECTS:
        // - Changes Player status to dead if health = 0
        // - Changes Player status to alive if health > 0
        public void ModifyPlayerHP(int anChangeHP)
        {
            mnPlayerHP = mnPlayerHP + anChangeHP;
            if (0 > mnPlayerHP)
            {
                mnPlayerHP = 0; // health cannot be negative
            }
            if (0 == mnPlayerHP)
            {
                mbPlayerAlive = false; // if your health is 0 you die
            }
            else if(0 < mnPlayerHP)
            {
                mbPlayerAlive = true; // We don't care what it was before
            }
        }

        // Used to set the player loot to a specific value
        public void SetPlayerLoot(int anNewTotal)
        {
            mnPlayerLoot = anNewTotal;
            if (0 > mnPlayerLoot)
            {
                mnPlayerLoot = 0; // loot cannot be negative
            }
        }

        // Used to increase or decrease loot by a specific amount.
        public void ModifyPlayerLoot(int anChangeLoot)
        {
            mnPlayerLoot = mnPlayerLoot + anChangeLoot;
            if (0 > mnPlayerLoot)
            {
                mnPlayerLoot = 0; // loot cannot be negative
            }
        }

        /**
         * Get a string with all the info about the player
         * This combines the other string methods for convenience
         * It has a newline after each value
         */
        public String GetPlayerStatusFull()
        {
            String lcReturnString = "";
            // Build a small block of player information with newlines
            lcReturnString += mcPlayerName + " has:\n";
            lcReturnString += "Health: " + mnPlayerHP + " HP\n";
            lcReturnString += "Wealth: " + mnPlayerLoot + " Doubloons\n";
            lcReturnString += GetPlayerInventory();

            return lcReturnString;
        }

        /**
         * Get a string with player inventory
         * It will tell the player what they have (if anything)
         * but will not reveal what they COULD have.
         * String has newline after each item.
         * 
         * IMPROVEMENTS
         * - Support other languages
         */
        public String GetPlayerInventory()
        {
            String lcReturnString="";

            // If the player has nothing, just say so
            if( false == (mbHasAlly && mbHasSword))
            {
                lcReturnString = "You have no possessions.\n";
            }

            if(mbHasSword)
            {
                lcReturnString += "You have a sword.\n";
            }

            if (mbHasAlly)
            {
                lcReturnString += "You have an ally.\n";
            }

            return lcReturnString;
        }

    } // Character class

    
    // GetUserInput
    // Gets input from the console. Continues to ask for the input if the incorrect type is entered. 
    // Limitations: It does not work for floating point numbers entered for Integer
    public class GetUserInput
    {
        // Get Uppercase string from console
        public String getUpperCaseStringFromConsole()
        {
            String lcTempString = getStringFromConsole();
            return lcTempString.ToUpper();
        }

        // Get Lowercase string from console
        public String getLowerCaseStringFromConsole()
        {
            String lcTempString = getStringFromConsole();
            return lcTempString.ToLower();
        }

        // Get a string from console. Do not return until you have one
        public String getStringFromConsole()
        {
            String lcReturnString = System.String.Empty;
            String lcTempString = System.String.Empty;
            // Required by TryParse but not used
            int lnUnusedInt;
            float lnUnusedFloat;
            bool lbValid = false;

            do
            {
                lcTempString = System.Console.ReadLine();
                lcTempString = lcTempString.Trim(); // remove leading/trailing white space

                // If it's at least one character long
                if (0 < lcTempString.Length)
                {
                    // and does NOT return an INT or FLOAT
                    if (false == int.TryParse(lcTempString, out lnUnusedInt) &&
                       (false == float.TryParse(lcTempString, out lnUnusedFloat)))
                    {
                        // We'll assume it's text and return it
                        lbValid = true;
                        lcReturnString = lcTempString;
                    }
                }
                if (false == lbValid)
                {
                    Console.Write("Invalid value! Try Again: ");
                }
            } while (false == lbValid);

            return lcReturnString;
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
    }// GetUserInput

    /**
     * WORLD MAP CLASS
     * This map contains a 2D array of coordinates for moving from location to location.
     * Each row is a location. Each column is the location you go to if you move in that
     * direction (North, South, East, Up, Down). If a cell value is 0 you cannot move in 
     * that direction. 
     * 
     * Row 0 is a special location that represents your starting location. You cannot return
     * to this location, only leave from it.
     * 
     * Future Features
     * - Add bonus columns are to allow up to two items to be left in a location
     * - Support a variable sub-class where each is a different language
     *   specified on start to allow for different translations
     * - Allow alternate text based on items on character
     */
    public class WorldMap
    {
        // The map keeps track of the player's location
        int mnPlayerLocation;

        // Array is as follows:
        // Row = location 
        // Col =  North, South, East, West, Up, Down,
        // Future expansion:                         ITEM1, ITEM2, CREATURE
        int[,] msLocationArray =
               // N, S, E, W, U, D, I1, I2, CR
                {{1, 2, 0, 0, 0, 0,  0,  0,  0}, // Starting point
                 {0, 2, 3, 0, 0, 0,  0,  0,  0}, // 1) Forest
                 {1, 0, 4, 0, 0, 0,  0,  0,  0}, // 2) Field
                 {0, 5, 0, 1, 7, 0,  0,  0,  0}, // 3) Forest, climb tree 7
                 {5, 0, 0, 2, 0, 0,  0,  0,  0}, // 4) Stream
                 {3, 4, 6, 0, 0, 0,  0,  0,  0}, // 5) Town Entrance
                 {0, 0, 0, 5, 0, 0,  0,  0,  0}, // 6) In Town
                 {0, 0, 0, 0, 0, 3,  0,  0,  (int)eCreatures.eeBIRD}  // 7) Up the tree from 3
                };

        // The descriptive text that accompanies each location
        String[] mcLocationTextArray;

        // The only thing we need to allocate are the descriptions that accompany our map
        // and the initial position of the player (always 0)
        public WorldMap()
        {
            mnPlayerLocation = 0; // Always the same starting point. 

            /** Allocate our descriptions. Each description is the text for that row in the msLocationArray
            * The player starts at 0, which cannot be returned to as 0 values for location are invalid. 
            * This means the text for location[0] can be the "welcome text" without worry that you might
            * display it more than once. 
            */
            mcLocationTextArray = new string[8];
                mcLocationTextArray[0] = "You stand at the crossroads of adventure!  To your north is a forest. A field lay to the south.";
                mcLocationTextArray[1] = "You are in a forest. There are tall trees as far as the eye can see...";
                mcLocationTextArray[2] = "You are in a lush green field. The breeze gently sways the tall grass like an emerald ocean.";
                mcLocationTextArray[3] = "You are in a forest. There is an extra tall tree here. It looks very old.";
                mcLocationTextArray[4] = "You come upon a brisk stream. It looks easy to cross.";
                mcLocationTextArray[5] = "You are at the gates of a bustling town. An Inn lies to the East";
                mcLocationTextArray[6] = "You find yourself in an Inn. It is thick with travelers like yourself.";
                mcLocationTextArray[7] = "You climb the tree and look around. You can see for some distance.\nThere is a town to the south.";
        }

        // Returns the description text associated with the player's location
        // This does not need to be error checked because the location is private to
        // the class and cannot be changed except through valid movement.
        public String GetDescription()
        {
            return mcLocationTextArray[mnPlayerLocation];
        }

        // Returns a String of the directions a player may go.
        public String GetValidDirections( )
        {
            String lcReturnString = "You may go: ";
            if (0 != msLocationArray[mnPlayerLocation, 0]) { lcReturnString += "NORTH, "; }
            if (0 != msLocationArray[mnPlayerLocation, 1]) { lcReturnString += "SOUTH, "; }
            if (0 != msLocationArray[mnPlayerLocation, 2]) { lcReturnString += "EAST, "; }
            if (0 != msLocationArray[mnPlayerLocation, 3]) { lcReturnString += "WEST, "; }
            if (0 != msLocationArray[mnPlayerLocation, 4]) { lcReturnString += "UP, "; }
            if (0 != msLocationArray[mnPlayerLocation, 5]) { lcReturnString += "DOWN, "; }

            // This removes the trailing ", " from the string and adds newline before returning.
            lcReturnString = lcReturnString.Remove(lcReturnString.Length - 2);
            lcReturnString += "\n";
            return lcReturnString;
        }

        // We take the input from the player on what direction to go
        // It comes as a String because this will be expanded into our command processing engine
        // and will eventually take ALL text entered by the player.
        //
        // If the direction leads to a valid destination, player location will be updated, return TRUE
        // If it is not valid, it will return FALSE.
        //
        // NOTES:
        // We do not have to check the input to see if the string is valid. The User input routine
        // will handle that for us and convert to UpperCase.
        public bool UpdatePlayerLocation(String acPlayerDirection)
        {
            bool lbDirectionValid = true;

            // We do not have to check the input to see if the string is valid. The User input routine
            // will handle that for us and convert to UpperCase.

            // The player's current location is the row. If the entered direction is != 0 the value 
            // at that location becomes the player's new location. If the location doesn't have a 
            // destination, it's not valid
            switch (acPlayerDirection)
            {
                case "NORTH":
                    if (0 != msLocationArray[mnPlayerLocation, 0])
                    { mnPlayerLocation = msLocationArray[mnPlayerLocation, 0]; }
                    else { lbDirectionValid = false; }
                    break;
                case "SOUTH":
                    if (0 != msLocationArray[mnPlayerLocation, 1])
                    { mnPlayerLocation = msLocationArray[mnPlayerLocation, 1]; }
                    else { lbDirectionValid = false; }
                    break;
                case "EAST":
                    if (0 != msLocationArray[mnPlayerLocation, 2])
                    { mnPlayerLocation = msLocationArray[mnPlayerLocation, 2]; }
                    else { lbDirectionValid = false; }
                    break;
                case "WEST":
                    if (0 != msLocationArray[mnPlayerLocation, 3])
                    { mnPlayerLocation = msLocationArray[mnPlayerLocation, 3]; }
                    else { lbDirectionValid = false; }
                    break;
                case "UP":
                    if (0 != msLocationArray[mnPlayerLocation, 4])
                    { mnPlayerLocation = msLocationArray[mnPlayerLocation, 4]; }
                    else { lbDirectionValid = false; }
                    break;
                case "DOWN":
                    if (0 != msLocationArray[mnPlayerLocation, 5])
                    { mnPlayerLocation = msLocationArray[mnPlayerLocation, 5]; }
                    else { lbDirectionValid = false; }
                    break;
                default:
                    // Should never see this
                    lbDirectionValid = false;
                    break;
            }
            return lbDirectionValid;
        }
    }// class WorldMap

    enum eWeapons { eeSTICK, eeFRYINGPAN, eeSWORD, eeBOW, eeSPEAR, eeTRIDENT, eeDAGGER };
    public class Weapons
    {
        
    }

    enum eArmor { eeDEFAULT, eeLEATHER, eeCHAINMAIL, eePLATEMAIL };
    public class Armor
    {
        
    }

    enum eCreatures { eeRAT, eeBEAR, eeGIANTSPIDER, eeBIRD };
    public class Creatures
    {
       
    }
}
