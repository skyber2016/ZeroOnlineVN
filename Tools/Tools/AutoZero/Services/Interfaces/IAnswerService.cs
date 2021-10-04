using AutoAnswer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAnswer.Services.Interfaces
{
    public interface IAnswerService
    {
        bool IsQuestion(byte[] packet);
        Question GetQuest(byte[] packet);
        bool IsAnswer(byte[] packet);
    }
}
