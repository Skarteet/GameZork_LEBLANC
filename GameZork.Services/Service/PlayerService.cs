﻿using GameZork.DataAccessLayer.AccessLayer;
using GameZork.DataAccessLayer.Models;
using GameZork.Services.Dto;
using GameZork.Services.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZork.Services.Service
{
    public class PlayerService
    {
        private readonly PlayerAccessLayer players;
        private readonly CellAccessLayer cells;
        private readonly MapAccessLayer maps;
        private readonly WeaponsAccessLayer weapons;
        private readonly ItemAccessLayer items;

        public PlayerService(PlayerAccessLayer playerAccessLayer, CellAccessLayer cellAccessLayer, MapAccessLayer mapAccessLayer,WeaponsAccessLayer weaponsAccessLayer,ItemAccessLayer itemAccessLayer)
        {
            this.players = playerAccessLayer;
            this.cells = cellAccessLayer;
            this.maps = mapAccessLayer;
            this.weapons = weaponsAccessLayer;
            this.items = itemAccessLayer;
        }

        public PlayerDto CreatePlayer(string name)
        {
            var player = new Player()
            {
                Name = name,
                XP = 0,
                HP = 100,
                MaxHP = 100,
                Level = 1,
                NextLevelXpRequired = 500,
                Power = 10,
                Defense = 0
            };
            player.Map = GenerateMap();
            player.Cell = player.Map.Cells.ElementAt(new Random().Next(1, player.Map.Cells.Count));
            player.Weapons = new List<Weapon> { weapons.GetSingle(w => w.Name == "Poing")};
            player.Items = items.GetCollection().ToList();
            
            //player = this.players.CreatePlayer(player);

            return new PlayerDto(player);
        }
        public void Save(PlayerDto playerDto)
        {
            var player = MapperExtension.Mapper.Map<Player>(playerDto);
            players.Save(player);
        }

        public void DeletePlayer(int playerId)
        {
            this.players.Remove(playerId);
        }

        private static Map GenerateMap()
        {
            var map = new Map();
            map.Cells = new List<Cell>();
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    map.Cells.Add(new Cell() { canMoveTo = true, Description = "test cell", PosX = x, PosY = y });
                }
            }
            return map;
        }

        public List<PlayerDto> GetAll()
        {
            return this.players.GetCollection().Select(p => new PlayerDto(p)).ToList();
        }

        public PlayerDto Get(int id)
        {
            return new PlayerDto(this.players.GetPlayer(id));
        }

        public void AddWeapon(int idPlayer, int idWeapon)
        {
            this.players.AddWeapon(idPlayer, idWeapon);
        }

        public void AddItem(int idPlayer, int idItem)
        {
            this.players.AddItem(idPlayer, idItem);
        }
    }
}
