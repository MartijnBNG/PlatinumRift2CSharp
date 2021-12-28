using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Zone
{
    static Random r = new Random();

    public int zoneId;          // id of this zone
    public int platinumSource;  // the amount of Platinum this zone can provide (0 if hidden by fog)
    public int ownerId = 0;     // the player who owns this zone (-1 otherwise)
    public int myPods = 0;      // my PODs on this zone
    public int enemyPods = 0;   // enemy PODs on this zone
    public int visible = 0;     // 1 if one of your units can see this tile, else 0

    public List<Zone> linkedZones;  //List of bordering zones

    public Zone(int zoneId, int platinumSource)
    {
        this.zoneId = zoneId;
        this.platinumSource = platinumSource;
        linkedZones = new List<Zone>();
    }

    public void MoveMyPods()
    {
        for ( int pod = 0 ; pod < myPods ; pod++ )
        {
            int goZone = linkedZones[r.Next(linkedZones.Count)].zoneId;
            Console.Write("1" + " " + zoneId + " " + goZone + " "); //Command to move one pod from zoneId (this zone) to goZone (a random linked zone)
        }
    }
}

class Player
{
    static int myId;            // my player ID (0 or 1)
    static List<Zone> zones;    // the "map" of zone objects

    //Returns zone object given an id
    static Zone findZoneById(int zoneId)
    {
        return zones.Find(item => item.zoneId == zoneId);
    }

    static void GameSetup()
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int playerCount = int.Parse(inputs[0]); // the amount of players (always 2)
        myId = int.Parse(inputs[1]);        // my player ID (0 or 1)
        int zoneCount = int.Parse(inputs[2]);   // the amount of zones on the map
        int linkCount = int.Parse(inputs[3]);   // the amount of links between all zones

        zones = new List<Zone>();
        for ( int i = 0 ; i < zoneCount ; i++ )
        {
            inputs = Console.ReadLine().Split(' ');
            int zoneId = int.Parse(inputs[0]);                  // this zone's ID (between 0 and zoneCount-1)
            int platinumSource = int.Parse(inputs[1]);          // Because of the fog, will always be 0
            Zone newZone = new Zone(zoneId, platinumSource);    //Create zone
            zones.Add(newZone);                                 //And add to list
        }
        for ( int i = 0 ; i < linkCount ; i++ )
        {
            //Create links between bordering zones
            inputs = Console.ReadLine().Split(' ');
            int zone1Id = int.Parse(inputs[0]);
            int zone2Id = int.Parse(inputs[1]);
            Zone zone1 = findZoneById(zone1Id);
            Zone zone2 = findZoneById(zone2Id);
            zone1.linkedZones.Add(zone2);
            zone2.linkedZones.Add(zone1);
        }
    }

    static void ProcessTurnInfo()
    {
        string[] inputs;

        int myPlatinum = int.Parse(Console.ReadLine());         // your available platinum. Probably useless, all platinum is automatically used to buy new pods at your base

        for ( int i = 0 ; i < zones.Count ; i++ )
        {
            inputs = Console.ReadLine().Split(' ');
            int zId = int.Parse(inputs[0]);                     // this zone's ID
            Zone currentZone = findZoneById(zId);               // get zone object
            currentZone.ownerId = int.Parse(inputs[1]);         // the player who owns this zone (-1 if not owned)
            int podsP0 = int.Parse(inputs[2]);                  // player 0's PODs on this zone
            int podsP1 = int.Parse(inputs[3]);                  // player 1's PODs on this zone
            currentZone.visible = int.Parse(inputs[4]);         // 1 if one of your units can see this tile, else 0
            currentZone.platinumSource = int.Parse(inputs[5]);  // the amount of Platinum this zone can provide (0 if hidden by fog)

            //Set pods as mine or enemy
            if ( myId == 0 )
            {
                currentZone.myPods = podsP0;
                currentZone.enemyPods = podsP1;
            }
            else
            {
                currentZone.myPods = podsP1;
                currentZone.enemyPods = podsP0;
            }
        }
    }

    static void AllZonesMovePods()
    {
        foreach ( Zone zone in zones )
        {
            zone.MoveMyPods();
        }
    }

    static void Main(string[] args)
    {
        GameSetup();

        // game loop
        while ( true )
        {
            Console.Error.WriteLine("Debug messages... Good luck everyone!");    // To debug: Console.Error.WriteLine("Debug messages...");

            ProcessTurnInfo();  // interpret new turn info
            AllZonesMovePods(); // iterate over zones and tell them to move their pods

            //End turn
            Console.WriteLine("");          // first line for movement commands (gets filled in in Zone.MoveMyPods. Just writing a newline here to end turn)
            Console.WriteLine("END TURN");  // second line no longer used (bought pods is PR 1) (see the protocol in the statement for details)
        }
    }
}
