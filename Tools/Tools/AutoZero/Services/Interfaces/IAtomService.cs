using AutoAnswer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAnswer.Services.Interfaces
{
    public interface IAtomService
    {
        bool IsBuyAtom(byte[] packet);
        void UseAtom();
        List<Atoms> GetAtoms(byte[] packet);
        void SetAtoms(List<Atoms> atoms);
        bool PacketHasAtom(byte[] packet);
        void SetPartern(byte[] packet);
    }
}
