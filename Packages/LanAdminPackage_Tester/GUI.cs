﻿using LanAdmin;
using System.Net;
namespace LanAdminPackage.Tester;

public class GUI : IGUI_responder
{
    public string Name => "template";

    public void ResponderAction(HttpListenerContext _context)
    {

    }
}
