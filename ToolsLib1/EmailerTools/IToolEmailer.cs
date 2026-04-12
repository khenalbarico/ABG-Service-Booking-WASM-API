﻿namespace ToolsLib1.EmailerTools;

public interface IToolEmailer
{
    Task SendAsync(
      string recepient,
      string subject   = "",
      string body      = "");
}