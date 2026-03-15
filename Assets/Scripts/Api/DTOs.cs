using JetBrains.Annotations;
using System;
using System.Collections.Generic;

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

public class ImageResponseDTO
{
    public string id;
    public string link;
    public string character;
}

[Serializable]
public class Classification
{
    public string imageId;
    public bool isDecorated;
}

[Serializable]
public class ClassificationSendDTO
{
    public List<Classification> classifications;
}
