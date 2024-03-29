﻿using Core.NewModels;
using System.Collections.Generic;

namespace Core.Models
{
    public class User : IdKey
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string Record { get; set; }

        public int CoinsCount = 0;

        public string Skin;

        public List<BaseElement> Inventory { get; set; }
    }
}
