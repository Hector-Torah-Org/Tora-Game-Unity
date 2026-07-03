using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class PlayerCreationDTO
{
    public string firstName;
    public string lastName;
    public string userName;
}

public class LoginResponseDTO
{
    public string sessionUUID;
    public string gameState;
}

public class PlayerResponseDTO
{
    public string id;
    public string firstName;
    public string lastName;
    public string userName;
    public string gamestate;
}

[Serializable]
public class ImageResponseListDTO
{
    public List<ImageResponseDTO> images;

    [Serializable]
    public class ImageResponseDTO
    {
        public string id;
        public string link;
        public string character;
    }
}

[Serializable]
public class Classification
{
    public string imageId;
    public bool isDecorated;
    public bool isDatasetError;
}

[Serializable]
public class ClassificationSendDTO
{
    public List<Classification> classifications;
}

[Serializable]
public class StatisticsDTO
{
    public int year;
    public double[] values; 
}

[Serializable]
public class LeaderboardDTO
{
    public int page;
    public LeaderboardElementDTO[] leaderboardElementDTOS;

    [Serializable]
    public class LeaderboardElementDTO
    {
        public string username;
        public int place;
        public string score;
    }
}