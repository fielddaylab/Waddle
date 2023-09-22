using System;


static public class LayerMasks {
    
    // Layer 0: Default
    public const int Default_Index = 0;
    public const int Default_Mask = 1;
    // Layer 1: TransparentFX
    public const int TransparentFX_Index = 1;
    public const int TransparentFX_Mask = 2;
    // Layer 2: Ignore Raycast
    public const int IgnoreRaycast_Index = 2;
    public const int IgnoreRaycast_Mask = 4;
    // Layer 3: NavigationTrigger
    public const int NavigationTrigger_Index = 3;
    public const int NavigationTrigger_Mask = 8;
    // Layer 4: Water
    public const int Water_Index = 4;
    public const int Water_Mask = 16;
    // Layer 5: UI
    public const int UI_Index = 5;
    public const int UI_Mask = 32;
    // Layer 6: Beak
    public const int Beak_Index = 6;
    public const int Beak_Mask = 64;
    // Layer 7: Rock
    public const int Rock_Index = 7;
    public const int Rock_Mask = 128;
    // Layer 8: Nest
    public const int Nest_Index = 8;
    public const int Nest_Mask = 256;
    // Layer 9: Flippers
    public const int Flippers_Index = 9;
    public const int Flippers_Mask = 512;
    // Layer 10: Skua
    public const int Skua_Index = 10;
    public const int Skua_Mask = 1024;
    // Layer 11: Terrain
    public const int Terrain_Index = 11;
    public const int Terrain_Mask = 2048;
    // Layer 12: Bubble
    public const int Bubble_Index = 12;
    public const int Bubble_Mask = 4096;
    // Layer 13: Jenga
    public const int Jenga_Index = 13;
    public const int Jenga_Mask = 8192;
    // Layer 14: StartGame
    public const int StartGame_Index = 14;
    public const int StartGame_Mask = 16384;
    // Layer 15: PenguinPlayer
    public const int PenguinPlayer_Index = 15;
    public const int PenguinPlayer_Mask = 32768;
    // Layer 16: MirrorPlayer
    public const int MirrorPlayer_Index = 16;
    public const int MirrorPlayer_Mask = 65536;
    // Layer 17: ThreeDUI
    public const int ThreeDUI_Index = 17;
    public const int ThreeDUI_Mask = 131072;
    // Layer 18: ThreeDUIHands
    public const int ThreeDUIHands_Index = 18;
    public const int ThreeDUIHands_Mask = 262144;
    // Layer 19: NPCPenguin
    public const int NPCPenguin_Index = 19;
    public const int NPCPenguin_Mask = 524288;
    // Layer 20: PlayerPhysics
    public const int PlayerPhysics_Index = 20;
    public const int PlayerPhysics_Mask = 1048576;
    // Layer 21: WorldCollision
    public const int WorldCollision_Index = 21;
    public const int WorldCollision_Mask = 2097152;
    // Layer 22: Region
    public const int Region_Index = 22;
    public const int Region_Mask = 4194304;
    // Layer 23: PenguinSensor
    public const int PenguinSensor_Index = 23;
    public const int PenguinSensor_Mask = 8388608;
    // Layer 24: Wind
    public const int Wind_Index = 24;
    public const int Wind_Mask = 16777216;
    // Layer 31: CullingRegion
    public const int CullingRegion_Index = 31;
    public const int CullingRegion_Mask = -2147483648;
}
static public class SortingLayers {
    
    // Layer Default
    public const int Default = 0;
}
static public class UnityTags {
    
    // Tag Untagged
    public const string Untagged = "Untagged";
    // Tag Respawn
    public const string Respawn = "Respawn";
    // Tag Finish
    public const string Finish = "Finish";
    // Tag EditorOnly
    public const string EditorOnly = "EditorOnly";
    // Tag MainCamera
    public const string MainCamera = "MainCamera";
    // Tag Player
    public const string Player = "Player";
    // Tag GameController
    public const string GameController = "GameController";
    // Tag NestRock
    public const string NestRock = "NestRock";
    // Tag BowlingBall
    public const string BowlingBall = "BowlingBall";
    // Tag Egg
    public const string Egg = "Egg";
    // Tag BowlingPenguin
    public const string BowlingPenguin = "BowlingPenguin";
    // Tag PTNPenguin
    public const string PTNPenguin = "PTNPenguin";
    // Tag MainMenu
    public const string MainMenu = "MainMenu";
    // Tag MatingDancePenguin
    public const string MatingDancePenguin = "MatingDancePenguin";
    // Tag WorldCollision
    public const string WorldCollision = "WorldCollision";
}