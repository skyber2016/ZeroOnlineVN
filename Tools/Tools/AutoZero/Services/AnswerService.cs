using AutoAnswer.Services.Interfaces;
using AutoAnswer.command;
using System;
using System.Collections.Generic;
using System.Text;
using AutoAnswer.Model;
using System.Linq;
using System.Data;
using API.Cores;

namespace AutoAnswer.Services
{
    public class AnswerService : IAnswerService
    {
        public bool IsQuestion(byte[] packet)
        {
            return packet.Includes(new byte[] { 0x3d, 0x3f }) && packet.Includes(PacketContants.QUESTION_PARTERN);
        }

        public Question GetQuest(byte[] packet)
        {
            try
            {
                var response = new Question();
                var packetString = packet.vnToStringHex();
                var startIndex = packetString.IndexOf(PacketContants.QUESTION_PARTERN.vnToStringHex());
                var packetQuestion = packetString.Substring(startIndex - 6).Split(' ');
                var questionLength = Convert.ToInt32(packetQuestion.Take(1).Join().HexToByte().FirstOrDefault());
                var question = packetQuestion.Take(questionLength).Skip(14).Join();
                var mathByte = question.HexToByte();
                var partern = "0123456789+-";
                response.Math = Encoding.UTF8.GetString(mathByte).Where(x => partern.Contains(x)).Join("");
                response.Answer = new DataTable().Compute(response.Math, null).ToString().GetNumber();
                var aLength = Convert.ToInt32(packetQuestion.Skip(questionLength).Take(1).Join().HexToByte().FirstOrDefault());
                response.A = packetQuestion.Skip(questionLength).Take(aLength).Skip(14).Join().HexToByte().GetString().GetNumber();
                var bLength = Convert.ToInt32(packetQuestion.Skip(questionLength + aLength).Take(1).Join().HexToByte().FirstOrDefault());
                response.B = packetQuestion.Skip(questionLength + aLength).Take(bLength).Skip(14).Join().HexToByte().GetString().GetNumber();
                var cLength = Convert.ToInt32(packetQuestion.Skip(questionLength + aLength + bLength).Take(1).Join().HexToByte().FirstOrDefault());
                response.C = packetQuestion.Skip(questionLength + aLength + bLength).Take(cLength).Skip(14).Join().HexToByte().GetString().GetNumber();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-------------- ANSWER THE QUESTION --------------");
                Console.WriteLine($"Math: {response.Math}");
                Console.WriteLine($"A: {response.A} \t\t B: {response.B}");
                Console.WriteLine($"C: {response.C}");
                Console.WriteLine($"Final: {response.Answer}");
                Console.ForegroundColor = ConsoleColor.White;
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsAnswer(byte[] packet)
        {
            return packet.Includes(PacketContants.ANSWER_SEND_PARTERN);
        }
    }
}
