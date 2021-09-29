using API.Cores;
using AutoAnswer.command;
using AutoAnswer.Model;
using AutoAnswer.Services.Interfaces;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace AutoAnswer.Services
{
    public class AtomService : IAtomService
    {
        public bool IsUse { get; set; }
        private readonly SimpleTcpClient _midClient;
        private readonly TcpClient _tcpClient;
        private List<Atoms> Atoms { get; set; } = new List<Atoms>();
        private IUnitOfWork UnitOfWork { get; set; }
        private byte[] ParternAddination { get; set; } = new byte[] { };
        public AtomService(SimpleTcpClient midClient, TcpClient game, IUnitOfWork unitOfWork)
        {
            this._midClient = midClient;
            this._tcpClient = game;
            this.UnitOfWork = unitOfWork;
        }
        public void UseAtom()
        {
            if (!this.ParternAddination.Any()) return;
            foreach (var atom in Atoms)
            {
                try
                {
                    var itemId = atom.Packet.Skip(4).Take(3).ToArray();
                    var packet = this.ParternAddination;
                    packet[8] = itemId[0];
                    packet[9] = itemId[1];
                    packet[10] = itemId[2];
                    this._midClient.Write(packet);
                    atom.IsUse = true;
                }
                catch (Exception ex)
                {

                }
                
            }
            Atoms = Atoms.Where(x => !x.IsUse).ToList();
           
            
        }
        public void SetPartern(byte[] packet)
        {
            if(packet.vnToStringHex().StartsWith(PacketContants.ADDINATION.vnToStringHex()))
            {
                UnitOfWork.Logger.Info("Set partern atom");
                var pkgs = packet.ToList();
                var length = PacketContants.ADDINATION.FirstOrDefault();
                this.ParternAddination = pkgs.Take(length).ToArray();
            }
        }
        public bool PacketHasAtom(byte[] packet)
        {
            return packet.vnToStringHex().Contains(PacketContants.PACKET_HAS_ATOM.vnToStringHex());
        }

        public bool IsBuyAtom(byte[] packet)
        {
            if (!packet.Any())
            {
                return false;
            }
            try
            {
                var pkgs = packet.ToList();
                var pkg = pkgs.Take(4).ToArray().vnToStringHex();
                var item = PacketContants.BUY_ATOM.vnToStringHex();
                var result = pkg.StartsWith(item);
                if (result)
                {
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public void SetAtoms(List<Atoms> atoms)
        {
            this.Atoms.AddRange(atoms);
        }

        public List<Atoms> GetAtoms(byte[] packet)
        {
            var pkgs = packet.ToList();
            var atoms = new List<Atoms>();
            while (pkgs.Any())
            {
                try
                {
                    var length = pkgs.FirstOrDefault();
                    if (length == 0) break;
                    var pkg = pkgs.Take(length).ToArray();
                    if (IsBuyAtom(pkg.vnClone()))
                    {
                        atoms.Add(new Atoms
                        {
                            Packet = pkg,
                            IsUse = false
                        });
                    }
                    pkgs = pkgs.Skip(length).ToList();
                }
                catch (Exception ex)
                {
                    UnitOfWork.Logger.Error(ex.Message);
                    break;
                }
            }
            return atoms;
        }
    }
}
