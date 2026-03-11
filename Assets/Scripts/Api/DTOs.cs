using JetBrains.Annotations;
using System;

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
